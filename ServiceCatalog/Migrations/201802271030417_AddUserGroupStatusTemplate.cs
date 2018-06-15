// -----------------------------------------------------------------------
// <copyright file="201802271030417_AddUserGroupStatusTemplate.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddUserGroupStatusTemplate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TemplateViewModels", "TemplateUsersGroup", c => c.String());
            AddColumn("dbo.TemplateViewModels", "TemplateStatus", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TemplateViewModels", "TemplateStatus");
            DropColumn("dbo.TemplateViewModels", "TemplateUsersGroup");
        }
    }
}
