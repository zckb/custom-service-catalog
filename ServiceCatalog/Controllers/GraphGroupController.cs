// -----------------------------------------------------------------------
// <copyright file="GraphGroupController.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using BusinessLogic.Client;
    using Common.Constants;
    using Models;
    using System.Collections.Generic;
    using System.Linq;

    public class GraphGroupController : BaseController
    {
        public GraphGroupController() : base()
        {
        }

        public async Task<ActionResult> GetGraphGroupView()
        {


            return View();
        }

        public async Task<List<GraphGroupsViewModel>> GetGraphGroups()
        {
            var client = new RestApiClient();
            var accessToken = await ServicePrincipal.GetGraphAccessToken();
            var graphGroups = await client.CallGetListAsync<GraphGroupsViewModel>(UriConstants.GetGraphGroups, accessToken);

            return graphGroups.Result.ToList();
        }
    }
}
