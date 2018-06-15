// -----------------------------------------------------------------------
// <copyright file="201802191333283_UpdateTemplateVersionType.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTemplateVersionType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TemplateViewModels", "TemplateJsonVersion", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TemplateViewModels", "TemplateJsonVersion", c => c.Long(nullable: false));
        }
    }
}
