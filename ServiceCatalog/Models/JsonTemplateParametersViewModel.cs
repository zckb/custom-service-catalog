// -----------------------------------------------------------------------
// <copyright file="JsonTemplateParametersViewModel.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.Models
{
    using System.Collections.Generic;
    using Microsoft.Azure.Management.ResourceManager.Models;

    public class JsonTemplateParametersViewModel
    {
        public int TemplateId { get; set; }

        public List<JsonTemplateParameter> Parameters { get; set; }

        public List<Subscription> Subscriptions { get; set; }

        public IEnumerable<ResourceGroup> ResourceGroups { get; set; }

        public bool IsManage { get; set; }
    }

    public class JsonTemplateParameter
    {
        public string Name { get; set; }

        public JsonTemplateParameterType Type { get; set; }

        public List<string> AllowedValues { get; set; }

        public string DefaultValue { get; set; }
    }

    public enum JsonTemplateParameterType
    {
        Unknown = 0, 
        String = 1,
        SecureString = 2,
    }
}