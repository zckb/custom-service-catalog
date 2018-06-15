// -----------------------------------------------------------------------
// <copyright file="GraphUserController.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Models;
    using System.Linq;
    using BusinessLogic.Client;
    using Common.Constants;

    public class GraphUserController : BaseController
    {
        public GraphUserController() : base()
        {
        }

        public ActionResult GetGraphUserView()
        {
            return View();
        }

        public async Task<List<GraphUserViewModel>> GetGetGraphUsers()
        {
            var client = new RestApiClient();
            var accessToken = await ServicePrincipal.GetGraphAccessToken();
            var graphUsers = await client.CallGetListAsync<GraphUserViewModel>(UriConstants.GetGraphUsers, accessToken);

            return graphUsers.Result.ToList();
        }
    }
}
