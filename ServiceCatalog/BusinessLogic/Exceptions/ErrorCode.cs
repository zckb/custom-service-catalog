// -----------------------------------------------------------------------
// <copyright file="ErrorCode.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.BusinessLogic.Exceptions
{
    public enum ErrorCode
    {
        /// <summary>
        /// The server had a failure it can't understand.
        /// </summary>
        ServerError,

        /// <summary>
        /// The resource already exists.
        /// </summary>
        AlreadyExists,

        /// <summary>
        /// An invalid set of inputs was provided.
        /// </summary>
        InvalidInput,

        /// <summary>
        /// A dependent service has failed.
        /// </summary>
        DownstreamServiceError,

        /// <summary>
        /// Failure in accessing persistence.
        /// </summary>
        PersistenceFailure,

        /// <summary>
        /// Unexpected file type.
        /// </summary>
        InvalidFileType,

        /// <summary>
        /// Maximum request size exceeded error.
        /// </summary>
        MaximumRequestSizeExceeded,

        /// <summary>
        /// Invalid address.
        /// </summary>
        InvalidAddress
    }
}