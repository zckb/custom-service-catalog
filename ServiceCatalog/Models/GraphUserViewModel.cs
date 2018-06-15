// -----------------------------------------------------------------------
// <copyright file="GraphUserViewModel.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace ServiceCatalog.Models
{
    public class GraphUserViewModel
    {
        public Guid ObjectId { get; set; }

        public string ObjectType { get; set; }

        public bool AccountEnabled { get; set; }

        public string DisplayName { get; set; }

        public string UserPrincipalName { get; set; }

        public string UserType { get; set; }

        public string EmployeeId { get; set; }

        public string Mail { get; set; }
    }
}