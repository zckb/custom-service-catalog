// -----------------------------------------------------------------------
// <copyright file="LocationController.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BusinessLogic.Client;
    using Common.Constants;
    using Common.Extensions;
    using Microsoft.Azure.Management.ResourceManager.Models;
    using System.Web.Mvc;
    using BusinessLogic.Exceptions;

    /// <summary>
    /// Manage locations
    /// </summary>
    public class LocationController : BaseController
    {
        public LocationController() : base() { }

        /// <summary>
        /// Retrieves the locations
        /// </summary>
        /// <returns>Locations <see cref="SelectListItem"/> list</returns>
        public async Task<List<SelectListItem>> GetLocations(string subscriptionId)
        {
            var endpointUrl = string.Format(UriConstants.GetAllLocationsUri, subscriptionId);

            var client = new RestApiClient();
            var result = await client.CallGetListAsync<Location>(endpointUrl, await ServicePrincipal.GetAccessToken());
            if (result.Success)
            {
                return result.Result
                    .Select(location => new SelectListItem()
                    {
                        Value = location.Name,
                        Text = location.DisplayName
                    })
                    .ToList();
            }

            throw new ServiceCatalogException(result.Message);
        }
    }
}