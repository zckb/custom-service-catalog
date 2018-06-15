// -----------------------------------------------------------------------
// <copyright file="ErrorInfo.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.Common.DataContracts
{
    public class ErrorInfo
    {
        public string Message { get; set; }

        public ErrorInfo[] Details { get; set; }
    }
}
