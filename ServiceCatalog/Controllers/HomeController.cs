// -----------------------------------------------------------------------
// <copyright file="HomeController.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.Controllers
{
    using System.Web.Mvc;

    public class HomeController : BaseController
    {
        public HomeController() : base() { }

        public ActionResult Index()
        {
            return RedirectToAction("../Deployment/DeployView");
        }
    }
}