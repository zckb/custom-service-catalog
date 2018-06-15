// -----------------------------------------------------------------------
// <copyright file="DnsController.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using BusinessLogic.Client;
    using Common.Constants;
    using Common.DataContracts;
    using Common.Extensions;
    using Common.Helpers;

    /// <summary>
    /// Manage dns
    /// </summary>
    public class DnsController : BaseController
    {
        public DnsController() : base() { }

        /// <summary>
        /// Get DNS prefix
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public async Task<string> GenerateDnsPrefix(string subscriptionId, string location)
        {
            var random = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyz";
            // Generate DNS prefix
            var dnsPrefix = new string(Enumerable.Repeat(chars, 7).Select(s => s[random.Next(s.Length)]).ToArray());
            // Check DNS name availability
            var dnsNameAvailabilityResult = await CheckDnsNameAvailability(subscriptionId, location, dnsPrefix);
            while (dnsNameAvailabilityResult.Available != true)
            {
                dnsPrefix = new string(Enumerable.Repeat(chars, 7).Select(s => s[random.Next(s.Length)]).ToArray());
                dnsNameAvailabilityResult = await CheckDnsNameAvailability(subscriptionId, location, dnsPrefix);
            }

            return dnsPrefix;
        }

        private async Task<DnsNameAvailabilityResult> CheckDnsNameAvailability(string subscriptionId, string location, string dnsPrefix)
        {
            try
            {
                var endpointUrl = string.Format(UriConstants.CheckDnsNameAvailabilityUri, subscriptionId, location, dnsPrefix);
                var restApiClient = new RestApiClient();
                var result = await restApiClient.CallGetAsync<DnsNameAvailabilityResult>(endpointUrl, await ServicePrincipal.GetAccessToken());

                Log.Info(TemplateHelper.ToJson(result));

                if (result.Success)
                {
                    return result.Result;
                }

                Log.Error(result.Message);
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }

            return new DnsNameAvailabilityResult() { Available = false };
        }
    }
}