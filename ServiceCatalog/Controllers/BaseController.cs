// -----------------------------------------------------------------------
// <copyright file="BaseController.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.Controllers
{
    using System.Web.Mvc;
    using NLog;

    /// <summary>
    /// Base controller
    /// </summary>
    [Authorize]
    public class BaseController : Controller
    {
        /// <summary>
        /// Logger
        /// </summary>
        protected Logger Log { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        protected BaseController()
        {
            // init logger
            Log = LogManager.GetLogger(GetType().FullName);
        }
    }
}