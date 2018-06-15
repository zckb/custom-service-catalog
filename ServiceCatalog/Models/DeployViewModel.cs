// -----------------------------------------------------------------------
// <copyright file="DeployViewModel.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.Models
{
    using System.Web;
    using System.ComponentModel;

    public class DeployViewModel
    {
        [DisplayName("Locations")]
        public string Location { get; set; }

        [DisplayName("VM Type")]
        public string VmType { get; set; }

        [DisplayName("MetaData")]
        public HttpPostedFileBase MetaData { get; set; }
    }
}