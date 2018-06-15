// -----------------------------------------------------------------------
// <copyright file="RequestHelper.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.Common.Helpers
{
    using System.Net.Http;
    using System.Text;
    using Constants;
    using Newtonsoft.Json;

    /// <summary>
    /// Helper for request content creation.
    /// </summary>
    public static class RequestHelper
    {
        /// <summary>
        /// Creates StringContent instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestObject"></param>
        /// <returns></returns>
        public static StringContent CreateStringContent<T>(T requestObject)
        {
            return new StringContent(
                JsonConvert.SerializeObject(requestObject),
                Encoding.UTF8,
                UriConstants.JsonContentType);
        }
    }
}
