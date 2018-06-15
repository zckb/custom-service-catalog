// -----------------------------------------------------------------------
// <copyright file="AzureResponse.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.Common.DataContracts
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class AzureResponse<T>
    { 
        [JsonProperty("value")]
        public IEnumerable<T> Items { get; set; }
    }
}
