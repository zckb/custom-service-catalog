// -----------------------------------------------------------------------
// <copyright file="201804101215500_DeployManageTemplate.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class DeployManageTemplate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TemplateViewModels", "IsManageTemplate", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TemplateViewModels", "IsManageTemplate");
        }
    }
}
