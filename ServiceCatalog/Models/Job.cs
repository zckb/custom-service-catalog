// -----------------------------------------------------------------------
// <copyright file="Job.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Job
    {
        [Key]
        public string Id { get; set; }

        public string Owner { get; set; }
    }
}