// -----------------------------------------------------------------------
// <copyright file="CommonResponseHelper.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.Common.Helpers
{
    using System;
    using System.Net;
    using DataContracts;

    public static class CommonResponseHelper
    {
        public static CommonResponse<T> CreateSuccessResponse<T>(T result)
        {
            return new CommonResponse<T>
            {
                Result = result,
                Message = null,
                Success = true,
                StatusCode = HttpStatusCode.OK
            };
        }

        public static CommonResponse<T> CreateSuccessResponse<T>(T result, CommonResponse<T> commonResponse)
        {
            return new CommonResponse<T>
            {
                Result = result,
                Message = commonResponse.Message,
                Success = commonResponse.Success,
                StatusCode = commonResponse.StatusCode
            };
        }

        public static CommonResponse<T> CreateSuccessResponse<T>(T result, HttpStatusCode statusCode)
        {
            return new CommonResponse<T>
            {
                Result = result,
                Message = null,
                Success = true,
                StatusCode = statusCode
            };
        }

        public static CommonResponse<T> CreateSuccessResponse<T>(T result, string message)
        {
            return new CommonResponse<T>
            {
                Result = result,
                Message = message,
                Success = true,
                StatusCode = HttpStatusCode.OK
            };
        }

        public static CommonResponse<T> CreateSuccessResponse<T>(T result, string message, HttpStatusCode statusCode)
        {
            return new CommonResponse<T>
            {
                Result = result,
                Message = message,
                Success = true,
                StatusCode = statusCode
            };
        }

        public static CommonResponse<T> CreateFailResponse<T>(Exception ex)
        {
            return new CommonResponse<T>
            {
                Result = default(T),
                Message = ex.Message,
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };
        }

        public static CommonResponse<T> CreateFailResponse<T>(Exception ex, HttpStatusCode statusCode)
        {
            return new CommonResponse<T>
            {
                Result = default(T),
                Message = ex.Message,
                Success = false,
                StatusCode = statusCode
            };
        }

        public static CommonResponse<T> CreateFailResponse<T>(string message)
        {
            return new CommonResponse<T>
            {
                Result = default(T),
                Message = message,
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };
        }

        public static CommonResponse<T> CreateFailResponse<T>(string message, HttpStatusCode statusCode)
        {
            return new CommonResponse<T>
            {
                Result = default(T),
                Message = message,
                Success = false,
                StatusCode = statusCode
            };
        }

        public static CommonResponse<T> CreateFailResponse<T>(ErrorInfo errorInfo)
        {
            return new CommonResponse<T>
            {
                Result = default(T),
                Message = errorInfo.Message,
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };
        }

        public static CommonResponse<T> CreateFailResponse<T>(ErrorInfo errorInfo, HttpStatusCode statusCode)
        {
            return new CommonResponse<T>
            {
                Result = default(T),
                Message = errorInfo.Message,
                Success = false,
                StatusCode = HttpStatusCode.BadRequest
            };
        }

        public static CommonResponse<T> CreateSuccessAsyncResponse<T>(T result, HttpStatusCode statusCode, string asyncOpStatusUrl)
        {
            return new CommonResponse<T>
            {
                Result = result,
                Message = null,
                Success = true,
                StatusCode = statusCode,
                IsAsyncOperation = true,
                AsyncOperationUrl = asyncOpStatusUrl,
            };
        }
    }
}
