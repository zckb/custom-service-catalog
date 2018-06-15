// -----------------------------------------------------------------------
// <copyright file="201802151345279_InitialCreate.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TemplateViewModels",
                c => new
                    {
                        TemplateId = c.Int(nullable: false, identity: true),
                        TemplateJson = c.String(),
                    })
                .PrimaryKey(t => t.TemplateId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TemplateViewModels");
        }
    }
}
