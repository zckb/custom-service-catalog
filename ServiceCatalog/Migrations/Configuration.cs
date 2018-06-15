// -----------------------------------------------------------------------
// <copyright file="Configuration.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.Migrations
{
    using System;
    using Models;
    using System.Data.Entity.Migrations;
    using Common.Helpers;
    using System.IO;
    using NLog;
    using System.Linq;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    internal sealed partial class Configuration : DbMigrationsConfiguration<Context.WebAppContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "ServiceCatalog.Context.WebAppContext";
        }

        protected override void Seed(Context.WebAppContext context)
        {
            var log = LogManager.GetLogger(GetType().FullName);
            try
            {
                if (context.TemplateJsons.Any())
                {
                    return;
                }

                context.TemplateJsons.AddOrUpdate(x => x.TemplateId, GetTemplates());
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private static TemplateViewModel[] GetTemplates()
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin", string.Empty) + "\\Templates\\";
            var templateListFile = File.ReadAllText(Path.Combine(baseDir, "template-list.json"));
            var templates = JsonConvert.DeserializeObject<List<Template>>(templateListFile);

            return templates.Select((template, i) => new TemplateViewModel()
            {
                Date = DateTime.Now,
                IsManageTemplate = template.IsManage,
                TemplateId = i,
                TemplateJson = File.ReadAllText(Path.Combine(baseDir, template.Name)),
                TemplateName = template.Name,
                Comment = template.Comment,
                UserName = UserRoleHelper.AdminUserName,
                TemplateJsonVersion = template.Version,
                TemplateUsersGroup = template.UsersGroup
            }).ToArray();
        }
    }
}
