// -----------------------------------------------------------------------
// <copyright file="LocationViewModel.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class LocationViewModel
    {
        [Display(Name = "Locaton ID")]
        public string LocationId { get; set; }
        public IEnumerable<SelectListItem> Locations { get; set; }
    }
}