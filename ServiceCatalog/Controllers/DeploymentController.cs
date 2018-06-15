// -----------------------------------------------------------------------
// <copyright file="DeploymentController.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using ServiceCatalog.Common.DataContracts;
using ServiceCatalog.Infrastructure.Client;

namespace ServiceCatalog.Controllers
{
    using BusinessLogic.Client;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Common.Constants;
    using Common.Helpers;
    using Models;
    using Microsoft.Azure.Management.ResourceManager.Models;
    using BusinessLogic.Exceptions;
    using Newtonsoft.Json.Linq;
    using System.Text;
    using Context;

    /// <summary>
    /// Manage deployments
    /// </summary>
    public class DeploymentController : BaseController
    {
        public DeploymentController() : base() { }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> DeployView()
        {
            try
            {
                Log.Info("DeployView");

                var templates = await new TemplateController().GetTemplates();
                if (templates.Count > 0)
                {
                    ViewBag.Templates = templates.OrderBy(s => s.TemplateName).Where(t => !t.IsManageTemplate).ToList();
                    ViewBag.IsAdmin = UserRoleHelper.IsAdmin(System.Web.HttpContext.Current.User.Identity.Name);
                }
                return View();
            }
            catch (Exception exception)
            {
                ViewBag.ErrorMessage = "Error";
                ViewBag.ErrorDetails = exception.Message;

                Log.Error(exception);

                return View("Error");
            }
        }

        [HttpGet]
        public async Task<ActionResult> ManageView()
        {
            try
            {
                Log.Info("ManageView");

                var templates = await new TemplateController().GetTemplates();
                if (templates.Count > 0)
                {
                    ViewBag.Templates = templates.OrderBy(s => s.TemplateName).Where(t => t.IsManageTemplate).ToList();
                }

                ViewBag.IsAdmin = UserRoleHelper.IsAdmin(System.Web.HttpContext.Current.User.Identity.Name);

                return View();
            }
            catch (Exception exception)
            {
                ViewBag.ErrorMessage = "Error";
                ViewBag.ErrorDetails = exception.Message;

                Log.Error(exception);

                return View("Error");
            }
        }

        public async Task<ActionResult> Deletetemplate(long templateId)
        {
            return await new TemplateController().DeleteTemplate(templateId);
        }

        /// <summary>
        /// Create new Deployment.
        /// </summary>
        [Authorize]
        [HttpPost]
        public async Task<RedirectToRouteResult> Deploy()
        {
            try
            {
                Log.Info("Read parameters from request data.");

                var templateIdStr = Request.Form["TemplateId"];
                int templateId;
                if (!int.TryParse(templateIdStr, out templateId))
                {
                    throw new ServiceCatalogException("Couldn't read template ID from request.");
                }
                Log.Info($"Template ID: {templateId}");

                var subscriptionId = Request.Form["SelectedSubscriptionId"];

                var location = WebConfigurationManager.AppSettings[ConfigurationConstants.DefaultLocation];

                var context = new WebAppContext();
                var template = context.TemplateJsons.FirstOrDefault(tj => tj.TemplateId == templateId);
                if (template == null)
                {
                    throw new ServiceCatalogException("Couldn't locate template with specified template ID.");
                }

                Log.Info("Parse parameters from request data");

                var paramsDict = new Dictionary<string, string>();
                var tagsDict = new Dictionary<string, string>()
                {
                    { "Deploy", "ServiceCatalog" },
                };
                FillParametersAndTagsDictionaries(template.TemplateJson, ref paramsDict, ref tagsDict);
                Log.Info($"Is Manage Template: {template.IsManageTemplate}");
                var jobId = Guid.NewGuid().ToString();
                if (template.IsManageTemplate)
                {
                    Log.Info($"Job Id: {jobId}");
                    paramsDict["jobid"] = jobId;
                }
                var deploymentsId = Guid.NewGuid();
                Log.Info("Start deployments - {0}", deploymentsId);

                var resourceGroupName = template.IsManageTemplate
                    ? "automation-account-resource-group"
                    : Request.Form["SelectedResourceGroupName"];
                if (string.IsNullOrWhiteSpace(subscriptionId) || string.IsNullOrWhiteSpace(resourceGroupName))
                {
                    throw new ServiceCatalogException("You should specify both Subscription and Resource Group.");
                }

                Log.Info($"Subscription ID: {subscriptionId}");
                Log.Info($"Resource group name: {resourceGroupName}");

                var resourceGroup = await GetOrCreateResourceGroup(resourceGroupName, subscriptionId, location, tagsDict);
                var parametersObj = TemplateHelper.PrepareTemplateParametersWithValues(template.TemplateJson, paramsDict);
                Log.Info($"Parameters: {parametersObj}");
                var deployment = new Deployment
                {
                    Properties = new DeploymentProperties
                    {
                        Mode = DeploymentMode.Incremental,
                        Template = JObject.Parse(template.TemplateJson),
                        Parameters = parametersObj,
                        DebugSetting = new DebugSetting()
                        {
                            DetailLevel = "requestContent, responseContent"
                        }
                    }
                };

                var deploymentId = Guid.NewGuid().ToString();

                var subscriptions = await new SubscriptionController().GetSubscriptions();
                var subscription = subscriptions.FirstOrDefault(s => s.SubscriptionId == subscriptionId);
                if (template.IsManageTemplate)
                {
                    using (WebAppContext webAppContext = new WebAppContext())
                    {
                        Job job = new Job()
                        {
                            Id = jobId,
                            Owner = System.Web.HttpContext.Current.User.Identity.Name
                        };
                        webAppContext.Jobs.Add(job);
                        webAppContext.SaveChanges();
                    }
                }
                else
                {
                    using (WebAppContext webAppContext = new WebAppContext())
                    {
                        DeploymentViewModel deploymentViewModel = new DeploymentViewModel()
                        {
                            DeploymentName = deploymentId,
                            TemplateVersion = template.TemplateJsonVersion,
                            Owner = System.Web.HttpContext.Current.User.Identity.Name,
                            TemplateName = template.TemplateName,
                            Timestamp = DateTime.Now,
                            SubscriptionId = subscription?.SubscriptionId,
                            SubscriptionName = subscription?.DisplayName
                        };
                        webAppContext.Deployments.Add(deploymentViewModel);
                        webAppContext.SaveChanges();
                    }
                }

                // Preparing endpoint URL for deployment
                var endpointUrl = string.Format(UriConstants.CreateDeploymentsUri, subscriptionId, resourceGroup.Name, deploymentId);
                Log.Info($"Sending request to API: {endpointUrl}");

                // Start deployment call async
                var client = new RestApiClient();
                var result = await client.CallPutAsync<Deployment, Deployment>(deployment, endpointUrl, await ServicePrincipal.GetAccessToken());

                Log.Info(TemplateHelper.ToJson($"Request result: {TemplateHelper.ToJson(result)}"));

                ViewBag.AsyncOperationUrl = result.AsyncOperationUrl;
                ViewBag.OperationResultUrl = endpointUrl;
                ViewBag.FileLogName = $"{DateTime.Today:yyyy-MM-dd}.log";

                return template.IsManageTemplate
                    ? RedirectToAction("RunBooksView", "RunBooks")
                    : RedirectToAction("DeploymentsView", "Deployments");
            }
            catch (Exception exception)
            {
                ViewBag.ErrorMessage = "Error";
                ViewBag.ErrorDetails = exception.Message;

                Log.Error(exception);

                return RedirectToAction("DeploymentsView", "Deployments");
            }
        }

        /// <summary>
        /// Get deployment output file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public FileResult GetOutputFile(string fileName)
        {
            try
            {
                var fileVirtualPath = Path.Combine(Server.MapPath("~/Deployments/"), fileName);

                return File(fileVirtualPath, "text/csv", fileName);
            }
            catch (Exception exception)
            {
                ViewBag.ErrorMessage = "Error";
                ViewBag.ErrorDetails = exception.Message;

                Log.Error(exception);
            }

            return null;
        }

        /// <summary>
        /// Get the template file for import
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public async Task<ActionResult> GetJsonTemplateFile(long templateId)
        {
            try
            {
                var template = await new TemplateController().GetTemplate(templateId);
                return File(Encoding.UTF8.GetBytes(template.TemplateJson), System.Net.Mime.MediaTypeNames.Application.Octet, template.TemplateName);

            }
            catch (Exception exception)
            {
                ViewBag.ErrorMessage = "Error";
                ViewBag.ErrorDetails = exception.Message;

                Log.Error(exception);
            }

            return View("Error");
        }

        /// <summary>
        /// Get the log file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public ActionResult GetLogFile(string fileName)
        {
            try
            {
                var fileVirtualPath = Path.Combine(Server.MapPath("~/Logs/"), fileName);

                return File(fileVirtualPath, "application/force-download", Path.GetFileName(fileVirtualPath));
            }
            catch (Exception exception)
            {
                ViewBag.ErrorMessage = "Error";
                ViewBag.ErrorDetails = exception.Message;

                Log.Error(exception);
            }

            return View("Error");
        }

        [HttpGet, Authorize]
        public async Task<JsonResult> GetAzureAsyncOperationStatus([FromUri] string url)
        {
            Log.Info($"Requesting status for Azure async operation: {url}");
            IRestApiClient restApiClient = new RestApiClient();
            var response = await restApiClient.CallGetAsync<AzureAsyncOperationStatusResponse>(
                url,
                await ServicePrincipal.GetAccessToken());
            Log.Info($"Async operation status response: {TemplateHelper.ToJson(response)}");

            if (!response.Success)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(response.Message, JsonRequestBehavior.AllowGet);
            }

            return Json(response.Result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet, Authorize]
        public async Task<JsonResult> GetDeployOutputParameters([FromUri] string url)
        {
            Log.Info($"Receiving output parameters from deployment: {url}");

            IRestApiClient restApiClient = new RestApiClient();
            var response = await restApiClient.CallGetAsync<DeploymentExtended>(
                url,
                await ServicePrincipal.GetAccessToken());
            Log.Info($"Output parameters response: {TemplateHelper.ToJson(response)}");

            if (!response.Success)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(response.Message, JsonRequestBehavior.AllowGet);
            }

            return Json(TemplateHelper.ToJson(response.Result?.Properties?.Outputs), JsonRequestBehavior.AllowGet);
        }

        private void FillParametersAndTagsDictionaries(
            string templateJson,
            ref Dictionary<string, string> paramsDict,
            ref Dictionary<string, string> tagsDict)
        {
            var parameters = TemplateHelper.ReadParametersFromTemplateJson(templateJson);
            foreach (var param in parameters)
            {
                var paramValue = Request.Form[param.Name];
                if (paramValue == null)
                {
                    throw new ServiceCatalogException($"Couldn't read template parameter '{param.Name}' from request.");
                }

                paramsDict.Add(param.Name, paramValue);
                if (param.Type != JsonTemplateParameterType.SecureString)
                {
                    tagsDict.Add(param.Name, paramValue);
                }
            }
        }

        private async Task<ResourceGroup> GetOrCreateResourceGroup(string resourceGroupName, string subscriptionId, string location, Dictionary<string, string> tagsDict)
        {
            Log.Info($"Trying to get resource group by name: {resourceGroupName}");
            try
            {
                return await new ResourceGroupController().GetResourceGroupByName(subscriptionId, resourceGroupName);
            }
            catch (ServiceCatalogException ex)
            {
                Log.Info(ex, "Failed to get resource group. Trying to create new.");
            }

            Log.Info("Creating resource group");
            var resourceGroup = await new ResourceGroupController().CreateResourceGroup(tagsDict, resourceGroupName, subscriptionId, location);
            Log.Info($"Resource group created. Name: {resourceGroup.Name}");

            return resourceGroup;
        }


        public async Task<RedirectResult> Visualize(long templateId)
        {
            try
            {
                Log.Info($"Visualize templateId: {templateId}");
                var template = await new TemplateController().GetTemplate(templateId);
                var pathToFile = Server.MapPath($"~/Content/Templates/{template.TemplateName}");
                using (StreamWriter fWriter = new StreamWriter(pathToFile, false))
                {
                    await fWriter.WriteAsync(template.TemplateJson);
                }
                var clientUrl = WebConfigurationManager.AppSettings[ConfigurationConstants.ClientUrl];
                var param = HttpUtility.UrlEncode($"{clientUrl}/Content/Templates/{template.TemplateName}");
                var url = string.Format(UriConstants.ArmVizualizeUrl, param);
                Log.Info($"Visualize url: {url}");

                return Redirect(url);
            }
            catch (Exception exception)
            {
                ViewBag.ErrorMessage = "Error";
                ViewBag.ErrorDetails = exception.Message;

                Log.Error(exception);

                return Redirect(UriConstants.ArmVizualizeUrl);
            }
        }
    }
}