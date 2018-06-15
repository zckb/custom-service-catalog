// -----------------------------------------------------------------------
// <copyright file="ResourceGroupManagementViewModel.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.Models
{
    using System.ComponentModel;
    using Microsoft.Azure.Management.ResourceManager.Models;

    public class ResourceGroupManagementViewModel : ResourceGroup
    {
        [DisplayName("Tag")]
        public string Tag { get; set; }
    }
}