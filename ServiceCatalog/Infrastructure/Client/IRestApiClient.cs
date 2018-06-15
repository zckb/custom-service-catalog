// -----------------------------------------------------------------------
// <copyright file="IRestApiClient.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.Infrastructure.Client
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common.DataContracts;

    public interface IRestApiClient
    {
        Task<CommonResponse<TOut>> CallPostAsync<TOut, TIn>(TIn requestObject, string endpointUrl, string accessToken);

        Task<CommonResponse<TOut>> CallPutAsync<TOut, TIn>(TIn requestObject, string endpointUrl, string accessToken);

        Task<CommonResponse<TOut>> CallGetAsync<TOut>(string endpointUrl, string accessToken);

        Task<CommonResponse<IEnumerable<TOut>>> CallGetListAsync<TOut>(string endpointUrl, string accessToken);

        Task<AzureAsyncOperationStatusResponse> GetAsyncOperationStatus(string endpointUrl, string accessToken);
    }
}
