// -----------------------------------------------------------------------
// <copyright file="ResourceGroupController.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System.Web.Http;

namespace ServiceCatalog.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Common.Constants;
    using Common.Helpers;
    using Microsoft.Azure.Management.ResourceManager.Models;
    using BusinessLogic.Client;
    using BusinessLogic.Exceptions;

    /// <summary>
    /// Manage resource group
    /// </summary>
    public class ResourceGroupController : BaseController
    {
        public ResourceGroupController() : base() { }

        public ActionResult DeleteView()
        {
            return View();
        }

        /// <summary>
        /// Delete resource group by tag
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public async Task<ActionResult> DeleteResourceGroups(string tag)
        {
            try
            {
                tag.AssertNotEmpty(nameof(tag));

                var subscriptions = await new SubscriptionController().GetSubscriptions();
                var tagName = tag.Split('=')[0];
                var tagValue = tag.Split('=')[1];

                foreach (var subscription in subscriptions)
                {
                    var resourceGroups = await GetResourceGroupsByTag(subscription.SubscriptionId, tagName, tagValue);

                    foreach (var resourceGroup in resourceGroups)
                    {
                        var endpointUrl = string.Format(UriConstants.DeleteResourceGroupUri, subscription.SubscriptionId, resourceGroup.Name);

                        var client = new RestApiClient();
                        await client.CallDeleteAsync<ResourceGroup>(endpointUrl, await ServicePrincipal.GetAccessToken());
                    }
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

        /// <summary>
        /// Create new resource group
        /// </summary>
        /// <param name="parametersDictionary">List of {parameter-value} pairs</param>
        /// <param name="location"></param>
        /// <returns></returns>
        public async Task<ResourceGroup> CreateResourceGroup(Dictionary<string, string> tagsDictionary, string resourceGroupName, string subscriptionId, string location)
        {
            var resourceGroup = new ResourceGroup()
            {
                Location = location,
                Name = resourceGroupName,
                Tags = tagsDictionary,
            };

            var restApiClient = new RestApiClient();
            var endpointUrl = string.Format(UriConstants.CreateResourceGroupUri, subscriptionId, resourceGroup.Name);
            var result = await restApiClient.CallPutAsync<ResourceGroup, ResourceGroup>(resourceGroup, endpointUrl, await ServicePrincipal.GetAccessToken());

            Log.Info(TemplateHelper.ToJson(result));

            if (result.Success)
            {
                return result.Result;
            }

            throw new ServiceCatalogException(result.Message);
        }

        [HttpGet, Authorize]
        public async Task<JsonResult> GetResourceGroupsForSubscription(
            [FromUri] string subscriptionId
        )
        {
            try
            {
                var endpointUrl = string.Format(UriConstants.GetAllResourceGroupsUri, Url.Encode(subscriptionId), "");

                return Json(await GetResourceGroups(endpointUrl), JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }

            return Json(new List<ResourceGroup>(), JsonRequestBehavior.AllowGet);
        }

        public async Task<ResourceGroup> GetResourceGroupByName(
            string subscriptionId,
            string resourceGroupName
        )
        {
            Log.Info($"Getting resource group by name. Subscription ID: {subscriptionId}, Name: {resourceGroupName}");
            var endpointUri = string.Format(UriConstants.GetResourceGroupUri, subscriptionId, resourceGroupName);
            var client = new RestApiClient();
            var result = await client.CallGetAsync<ResourceGroup>(endpointUri, await ServicePrincipal.GetAccessToken());
            Log.Info($"Resouce group result: {TemplateHelper.ToJson(result)}");

            if (!result.Success)
            {
                throw new ServiceCatalogException(result.Message);
            }

            return result.Result;
        }

        private async Task<IEnumerable<ResourceGroup>> GetResourceGroupsByTag(string subscriptionId, string tagName, string tagValue)
        {
            try
            {
                var endpointUrl = string.Format(UriConstants.GetAllResourceGroupsUri, subscriptionId, $"&$filter=tagname eq '{tagName}' and tagvalue eq '{tagValue}'");

                return await GetResourceGroups(endpointUrl);
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }

           return new List<ResourceGroup>();
        }

        public async Task<IEnumerable<ResourceGroup>> GetResourceGroups(string endpointUrl)
        {
            var client = new RestApiClient();
            var result = await client.CallGetListAsync<ResourceGroup>(endpointUrl, await ServicePrincipal.GetAccessToken());

            if (!result.Success)
            {
                return new List<ResourceGroup>();
            }

            return result.Result;
        }

        
    }
}