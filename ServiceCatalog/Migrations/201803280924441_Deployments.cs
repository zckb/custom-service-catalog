// -----------------------------------------------------------------------
// <copyright file="201803280924441_Deployments.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Deployments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DeploymentViewModels",
                c => new
                    {
                        DeploymentId = c.Long(nullable: false, identity: true),
                        DeploymentName = c.String(),
                        TemplateVersion = c.String(),
                        TemplateName = c.String(),
                        Owner = c.String(),
                        Timestamp = c.DateTime(nullable: false),
                        Outputs = c.String(),
                        ProvisioningState = c.String(),
                    })
                .PrimaryKey(t => t.DeploymentId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DeploymentViewModels");
        }
    }
}
