// -----------------------------------------------------------------------
// <copyright file="DnsNameAvailabilityResult.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.Common.DataContracts
{
    using Newtonsoft.Json;

    /// <summary>
    /// Response for the CheckDnsNameAvailabilityUri API service call.
    /// </summary>
    public partial class DnsNameAvailabilityResult
    {
        /// <summary>
        /// Initializes a new instance of the DnsNameAvailabilityResult class.
        /// </summary>
        public DnsNameAvailabilityResult() { }

        /// <summary>
        /// Initializes a new instance of the DnsNameAvailabilityResult class.
        /// </summary>
        /// <param name="available">Domain availability (True/False).</param>
        public DnsNameAvailabilityResult(bool? available = default(bool?))
        {
            Available = available;
        }

        /// <summary>
        /// Gets or sets domain availability (True/False).
        /// </summary>
        [JsonProperty(PropertyName = "available")]
        public bool? Available { get; set; }
    }
}