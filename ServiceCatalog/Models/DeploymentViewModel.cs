// -----------------------------------------------------------------------
// <copyright file="DeploymentViewModel.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class DeploymentViewModel
    {
        [Key]
        public long DeploymentId { get; set; }

        public string DeploymentName { get; set; }

        public string TemplateVersion { get; set; }

        public string TemplateName { get; set; }

        public string Owner { get; set;}

        public DateTime Timestamp { get; set; }

        public string Outputs { get; set; }

        public string ProvisioningState { get; set; }

        public string SubscriptionId { get; set; }

        public string SubscriptionName { get; set; }
    }
}