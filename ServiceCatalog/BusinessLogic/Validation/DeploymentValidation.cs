// -----------------------------------------------------------------------
// <copyright file="DeploymentValidation.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.BusinessLogic.Validation
{
    using System;
    using System.Web.Configuration;
    using Infrastructure;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Exceptions;
    using Common.DataContracts;
    using Controllers;

    /// <summary>
    /// The deployment validation class.
    /// </summary>    
    public class DeploymentValidation : IDeploymentValidation
    {
        /// <summary>
        /// Validation deployments. 
        /// </summary>
        /// <param name="inputDictionary"></param>
        /// <returns></returns>
        public async Task<bool> IsValid(Dictionary<string, List<DeploymentInput>> inputDictionary)
        {
            // iteration of templates
            foreach (var inputTemplate in inputDictionary)
            {
                /* TODO vNext check quota
                var vmCount = deploymentInput.Value.Count;
                var quotaAvailable = await CheckUsageInformation(deploymentInput.Key, location, vmCount);
                if (quotaAvailable.Any())
                {
                    string message = string.Empty;
                    foreach (var usageInformationViewModel in quotaAvailable)
                    {
                        message += $"Cannot create more than {usageInformationViewModel.Name} {usageInformationViewModel.Quota} for this subscription in this region\n";
                    }
                    throw new ServiceCatalogException(message);
                }
                */

                var vmCount = inputTemplate.Value.Count;

                var maximumLimit = Convert.ToInt32(WebConfigurationManager.AppSettings["ida:MaxVmCount"]);

                if (vmCount > maximumLimit)
                {
                    throw new ServiceCatalogException($"Virtual machines per cloud service. Maximum Limit {maximumLimit}");
                }

                var subscriptions = await new SubscriptionController().GetSubscriptions();

                foreach (var template in inputTemplate.Value)
                {
                    if (!subscriptions.Select(s => s.Id == template.SubscriptionId).Any())
                    {
                        throw new ServiceCatalogException($"The subscription of '{template.SubscriptionId}' does not exist or disabled");
                    }
                }
            }

            return true;
        }
    }
}