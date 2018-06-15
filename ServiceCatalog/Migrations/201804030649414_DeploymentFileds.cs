// -----------------------------------------------------------------------
// <copyright file="201804030649414_DeploymentFileds.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class DeploymentFileds : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DeploymentViewModels", "SubscriptionId", c => c.String());
            AddColumn("dbo.DeploymentViewModels", "SubscriptionName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DeploymentViewModels", "SubscriptionName");
            DropColumn("dbo.DeploymentViewModels", "SubscriptionId");
        }
    }
}
