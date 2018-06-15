// -----------------------------------------------------------------------
// <copyright file="CommonResponse.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.Common.DataContracts
{
    using System.Net;

    public class CommonResponse<T>
    {
        public string Message { get; internal set; }

        public HttpStatusCode StatusCode { get; internal set; }

        public bool Success { get; internal set; }

        public T Result { get; internal set; }

        public bool IsAsyncOperation { get; internal set; }

        public string AsyncOperationUrl { get; internal set; }
    }
}

