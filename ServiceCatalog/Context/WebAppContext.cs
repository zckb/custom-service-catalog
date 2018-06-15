// -----------------------------------------------------------------------
// <copyright file="WebAppContext.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.Context
{
    using System.Data.Entity;
    using System.Web.Configuration;
    using Models;

    public class WebAppContext : DbContext
    {
        private static readonly string ConnectionString = WebConfigurationManager.ConnectionStrings[ConfigurationConstants.ConnectionStringName].ConnectionString;

        public WebAppContext()
            : base(ConnectionString)
        { }

        public DbSet<TemplateViewModel> TemplateJsons { get; set; }

        public DbSet<DeploymentViewModel> Deployments { get; set; }

        public DbSet<Job> Jobs { get; set; }
    }
}