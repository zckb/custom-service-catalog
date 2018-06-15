// -----------------------------------------------------------------------
// <copyright file="201802191221516_AddRequaredFieldsTemplateModel.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddRequaredFieldsTemplateModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TemplateViewModels", "Comment", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TemplateViewModels", "Comment", c => c.String());
        }
    }
}
