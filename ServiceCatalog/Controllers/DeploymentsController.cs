// -----------------------------------------------------------------------
// <copyright file="DeploymentsController.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using BusinessLogic.Client;
    using Common.Constants;
    using System.Data.Entity;
    using Context;
    using Models;
    using System;
    using Microsoft.Azure.Management.ResourceManager.Models;
    using System.Security.Claims;
    using Common.Helpers;

    public class DeploymentsController : BaseController
    {
        // GET: Deployments
        public async Task<ActionResult> DeploymentsView()
        {
            try
            {
                // Get all subscriptions for this tenant
                var subscriptions = await new SubscriptionController().GetSubscriptions();
                var subscriptionId = subscriptions.FirstOrDefault()?.SubscriptionId;
                var token = await ServicePrincipal.GetAccessToken();
                var client = new RestApiClient();
                // Get all resource groups
                var resourceGroupUri = string.Format(UriConstants.GetAllResourceGroupsUri, Url.Encode(subscriptionId), "");
                var resourceGroups = await client.CallGetListAsync<ResourceGroup>(resourceGroupUri, token);
                // Get all deployments
                var deployments = new List<DeploymentExtended>();
                foreach (var resourceGroup in resourceGroups.Result)
                {
                    var deploymentsUri = string.Format(UriConstants.GetDeploymentsByResourceGroup, subscriptionId, resourceGroup.Name);
                    client = new RestApiClient();
                    var result = await client.CallGetListAsync<DeploymentExtended>(deploymentsUri, token);
                    var deployment = result.Result;
                    deployments.AddRange(deployment);
                }

                var email = ClaimsPrincipal.Current.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
                var resultDeployments = new List<DeploymentViewModel>();
                using (WebAppContext context = new WebAppContext())
                {
                    var localDeployments = await context.Deployments.ToListAsync();
                    foreach (var localDeployment in localDeployments)
                    {
                        foreach (var deployment in deployments)
                        {
                            if (localDeployment.DeploymentName != deployment?.Name)
                            {
                                continue;
                            }

                            if (UserRoleHelper.IsAdmin(email) || email == localDeployment.Owner)
                            {
                                var newDeployment = new DeploymentViewModel()
                                {
                                    TemplateName = localDeployment.TemplateName,
                                    DeploymentId = localDeployment.DeploymentId,
                                    DeploymentName = localDeployment.DeploymentName,
                                    SubscriptionId = localDeployment.SubscriptionId,
                                    SubscriptionName = localDeployment.SubscriptionName,
                                    Owner = localDeployment.Owner,
                                    TemplateVersion = localDeployment.TemplateVersion,
                                    Timestamp = localDeployment.Timestamp,
                                    ProvisioningState = deployment?.Properties?.ProvisioningState,
                                    Outputs = deployment?.Properties?.Outputs?.ToString()
                                };
                                resultDeployments.Add(newDeployment);
                            }
                        }
                    }
                }
                var deploymentsList = resultDeployments.OrderByDescending(d => d.Timestamp).ToList();

                ViewBag.FileLogName = $"{DateTime.Today:yyyy-MM-dd}.log";

                return View(deploymentsList);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error";
                ViewBag.ErrorDetails = ex.Message;

                return View("Error");
            }
        }
    }
}
