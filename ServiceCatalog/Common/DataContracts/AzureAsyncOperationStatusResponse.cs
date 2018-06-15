// -----------------------------------------------------------------------
// <copyright file="AzureAsyncOperationStatusResponse.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.Common.DataContracts
{
    using Newtonsoft.Json;

    public class AzureAsyncOperationStatusResponse
    {
        [JsonProperty("status")]
        public string Status { get; internal set; }

        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        public ErrorInfo Error { get; internal set; } 
    }
}