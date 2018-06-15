// -----------------------------------------------------------------------
// <copyright file="201802160653555_UpdateDatabaseFields.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDatabaseFields : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.TemplateViewModels");
            AddColumn("dbo.TemplateViewModels", "TemplateName", c => c.String());
            AddColumn("dbo.TemplateViewModels", "TemplateJsonVersion", c => c.Long(nullable: false));
            AddColumn("dbo.TemplateViewModels", "Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.TemplateViewModels", "Comment", c => c.String());
            AddColumn("dbo.TemplateViewModels", "UserName", c => c.String());
            AlterColumn("dbo.TemplateViewModels", "TemplateId", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.TemplateViewModels", "TemplateId");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.TemplateViewModels");
            AlterColumn("dbo.TemplateViewModels", "TemplateId", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.TemplateViewModels", "UserName");
            DropColumn("dbo.TemplateViewModels", "Comment");
            DropColumn("dbo.TemplateViewModels", "Date");
            DropColumn("dbo.TemplateViewModels", "TemplateJsonVersion");
            DropColumn("dbo.TemplateViewModels", "TemplateName");
            AddPrimaryKey("dbo.TemplateViewModels", "TemplateId");
        }
    }
}
