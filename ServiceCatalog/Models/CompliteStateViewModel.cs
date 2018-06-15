// -----------------------------------------------------------------------
// <copyright file="CompliteStateViewModel.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.Models
{
    using System.Security.Policy;

    public class CompliteStateViewModel
    {
        public string Message { get; set; }

        public Url LogFile { get; set; }

        public string OutputFile { get; set; }
    }
}