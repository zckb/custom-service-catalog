// -----------------------------------------------------------------------
// <copyright file="RestApiClient.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.BusinessLogic.Client
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Common.Constants;
    using Common.DataContracts;
    using Common.Helpers;
    using Infrastructure.Client;
    using Newtonsoft.Json;

    public class RestApiClient : IRestApiClient
    {
        private readonly HttpClient httpClient;

        public RestApiClient()
        {
            this.httpClient = new HttpClient();
        }

        /// <summary>
        /// Calls api via POST method.
        /// </summary>
        /// <typeparam name="TOut">Expected result type.</typeparam>
        /// <typeparam name="TIn">Input request object type.</typeparam>
        /// <param name="requestObject">Posted content.</param>
        /// <param name="endpointUrl">Endpoint url.</param>
        /// <param name="accessToken">Access token.</param>
        /// <returns>Response result.</returns>
        public async Task<CommonResponse<TOut>> CallPostAsync<TOut, TIn>(TIn requestObject, string endpointUrl, string accessToken)
        {
            this.PrepareHttpClient(accessToken);

            var response = await this.httpClient.PostAsync(endpointUrl, RequestHelper.CreateStringContent(requestObject));
            return await CreateResponse<TOut>(response);
        }

        /// <summary>
        /// Calls api via PUT method.
        /// </summary>
        /// <typeparam name="TOut">Expected result type.</typeparam>
        /// <typeparam name="TIn">Input request object type.</typeparam>
        /// <param name="requestObject">Posted content.</param>
        /// <param name="endpointUrl">Endpoint url.</param>
        /// <param name="accessToken">Access token.</param>
        /// <returns>Response result.</returns>
        public async Task<CommonResponse<TOut>> CallPutAsync<TOut, TIn>(TIn requestObject, string endpointUrl, string accessToken)
        {
            this.PrepareHttpClient(accessToken);

            var response = await this.httpClient.PutAsync(endpointUrl, RequestHelper.CreateStringContent(requestObject));
            return await CreateResponse<TOut>(response);
        }

        /// <summary>
        /// Calls api via GET method.
        /// </summary>
        /// <typeparam name="TOut">Expected result type.</typeparam>
        /// <param name="endpointUrl">Endpoint url.</param>
        /// <param name="accessToken">Access token.</param>
        /// <returns>Response result.</returns>
        public async Task<CommonResponse<TOut>> CallGetAsync<TOut>(string endpointUrl, string accessToken)
        {
            this.PrepareHttpClient(accessToken);

            var response = await this.httpClient.GetAsync(endpointUrl);
            return await CreateResponse<TOut>(response);
        }

        /// <summary>
        /// Calls api via GET method.
        /// </summary>
        /// <typeparam name="TOut">Expected result type.</typeparam>
        /// <param name="endpointUrl">Endpoint url.</param>
        /// <param name="accessToken">Access token.</param>
        /// <returns>Response result.</returns>
        public async Task<CommonResponse<IEnumerable<TOut>>> CallGetListAsync<TOut>(string endpointUrl, string accessToken)
        {
            this.PrepareHttpClient(accessToken);

            var response = await this.httpClient.GetAsync(endpointUrl);
            var responseString = await response.Content.ReadAsStringAsync();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                var errorInfo = JsonConvert.DeserializeObject<ErrorInfo>(responseString);
                var result = CommonResponseHelper.CreateFailResponse<IEnumerable<TOut>>(errorInfo);
                return result;
            }
            else
            {
                var result = JsonConvert.DeserializeObject<AzureResponse<TOut>>(responseString);
                return CommonResponseHelper.CreateSuccessResponse(result.Items);
            }
        }

        /// <summary>
        /// Calls api via DELETE method.
        /// </summary>
        /// <typeparam name="TOut">Expected result type.</typeparam>
        /// <param name="endpointUrl">Endpoint url.</param>
        /// <param name="accessToken">Access token.</param>
        /// <returns></returns>
        public async Task<CommonResponse<TOut>> CallDeleteAsync<TOut>(string endpointUrl, string accessToken)
        {
            this.PrepareHttpClient(accessToken);

            var response = await this.httpClient.DeleteAsync(endpointUrl);
            return await CreateResponse<TOut>(response);
        }

        public async Task<AzureAsyncOperationStatusResponse> GetAsyncOperationStatus(
            string endpointUrl,
            string accessToken)
        {
            throw new NotImplementedException();
        }

        public async Task<string> CallGetText(string endpointUrl, string accessToken)
        {
            this.PrepareHttpClient(accessToken);
            var response = await this.httpClient.GetAsync(endpointUrl);

            var responseString = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == HttpStatusCode.OK
                || response.StatusCode == HttpStatusCode.Created
                || response.StatusCode == HttpStatusCode.Accepted)
            {
                return responseString;
            }

            return string.Empty;
        }

        private void PrepareHttpClient(string accessToken)
        {
            this.httpClient.BaseAddress = new Uri("https://management.azure.com");
            this.httpClient.DefaultRequestHeaders.Accept.Clear();
            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(RequestConstants.BearerPrefix, accessToken);
            this.httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(UriConstants.JsonContentType));
        }

        private static async Task<CommonResponse<TOut>> CreateResponse<TOut>(HttpResponseMessage response)
        {
            IEnumerable<string> asyncOpHeaderList;
            var hasAsyncHeader = response.Headers.TryGetValues("Azure-AsyncOperation", out asyncOpHeaderList);
            var isAsyncOp = hasAsyncHeader && asyncOpHeaderList.Any() && !string.IsNullOrWhiteSpace(asyncOpHeaderList.First());

            var responseString = await response.Content.ReadAsStringAsync();
            if (response.StatusCode != HttpStatusCode.OK
                && response.StatusCode != HttpStatusCode.Created
                && response.StatusCode != HttpStatusCode.Accepted)
            {
                var azureErrorResponse = JsonConvert.DeserializeObject<AzureErrorResponse>(responseString);
                var result = CommonResponseHelper.CreateFailResponse<TOut>(azureErrorResponse.Error);
                return result;
            }
            else
            {
                var result = JsonConvert.DeserializeObject<TOut>(responseString);
                if (isAsyncOp)
                {
                    return CommonResponseHelper.CreateSuccessAsyncResponse(result, response.StatusCode, asyncOpHeaderList.First());
                }
                else
                {
                    return CommonResponseHelper.CreateSuccessResponse(result, response.StatusCode);
                }
            }
        }
    }
}
