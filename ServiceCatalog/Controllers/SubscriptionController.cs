// -----------------------------------------------------------------------
// <copyright file="SubscriptionController.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BusinessLogic.Client;
    using BusinessLogic.Exceptions;
    using Common.Constants;
    using Microsoft.Azure.Management.ResourceManager.Models;

    /// <summary>
    /// Manage subscriptions
    /// </summary>
    public class SubscriptionController : BaseController
    {
        public SubscriptionController() : base() { }

        /// <summary>
        /// Retrieves the subscriptions
        /// </summary>
        /// <returns>Subscriptions list</returns>
        public async Task<List<Subscription>> GetSubscriptions()
        {
            var endpointUrl = string.Format(UriConstants.GetAllSubscriptionsUri);

            var client = new RestApiClient();
            var result = await client.CallGetListAsync<Subscription>(endpointUrl, await ServicePrincipal.GetAccessToken());

            if (result.Success)
            {
                return result.Result.Where(s => s.State == SubscriptionState.Enabled).ToList();
            }

            throw new ServiceCatalogException(result.Message);
        }
    }
}