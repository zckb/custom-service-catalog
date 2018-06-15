// -----------------------------------------------------------------------
// <copyright file="InvoicesViewModel.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.Models
{
    using System;
    
    public class InvoicesViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime InvoicePeriodStartDate { get; set; }
        public DateTime InvoicePeriodEndDate { get; set; }
        public string DownloadUrl { get; set; }
        public string DownloadUrlExpiry { get; set; }
    }
}