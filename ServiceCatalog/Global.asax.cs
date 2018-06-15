// -----------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog
{
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using System;
    using System.Diagnostics;
    using NLog;
    using System.Data.Entity.Migrations;
    using Migrations;

    public class MvcApplication : System.Web.HttpApplication
    {
        private Logger Log { get; set; }

        public MvcApplication()
        {
            // CODE FIRST MIGRATIONS
            var migrator = new DbMigrator(new Configuration());
            migrator.Update();
            Log = LogManager.GetLogger(GetType().FullName);
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // CODE FIRST MIGRATIONS
            var migrator = new DbMigrator(new Configuration());
            migrator.Update();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();

            if (exception != null)
            {
                Trace.TraceError("Application_Error: Uncaught exception: {0}", exception);
                Log.Trace(exception);
            }
        }
    }
}
