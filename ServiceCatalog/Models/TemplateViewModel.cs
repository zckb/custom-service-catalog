// -----------------------------------------------------------------------
// <copyright file="TemplateViewModel.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.Models
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web;

    public class TemplateViewModel
    {
        [Key]
        public long TemplateId { get; set; }

        public string TemplateName { get; set; }

        public string TemplateJson { get; set; }

        [Required]
        [DisplayName("Template Version")]
        [RegularExpression(@"^(\d+\.)?(\d+\.)?(\*|\d+)$", ErrorMessage = "Invalid version number.")]
        public string TemplateJsonVersion { get; set; }

        public DateTime Date { get; set; }

        [Required]
        [DisplayName("Comment")]
        public string Comment { get; set; }

        public string UserName { get; set; }

        [DisplayName("Template")]
        [NotMapped]
        public HttpPostedFileBase TemplateData { get; set; }

        [DisplayName("Template User Groups")]
        public string TemplateUsersGroup { get; set; }

        public string TemplateStatus { get; set; }

        [DisplayName("Template Type")]
        public bool IsManageTemplate { get; set; }
    }
}