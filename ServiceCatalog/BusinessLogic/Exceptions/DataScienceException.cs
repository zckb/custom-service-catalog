// -----------------------------------------------------------------------
// <copyright file="DataScienceException.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.BusinessLogic.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [Serializable]
    public class ServiceCatalogException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCatalogException"/> class.
        /// </summary>
        public ServiceCatalogException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCatalogException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public ServiceCatalogException(string message)
            : this(message, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCatalogException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public ServiceCatalogException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCatalogException"/> class.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public ServiceCatalogException(ErrorCode errorCode, string message = default(string), Exception innerException = null)
            : this(message, innerException)
        {
            this.ErrorCode = errorCode;
        }

        /// <summary>
        /// Gets the error code which specifies what happened in the business area that caused the error.
        /// </summary>
        public ErrorCode ErrorCode { get; private set; } = ErrorCode.ServerError;

        /// <summary>
        /// Gets a dictionary of the error details.
        /// </summary>
        public IDictionary<string, string> Details { get; private set; } = new Dictionary<string, string>();

        /// <summary>
        /// Required override to add in the serialized parameters.
        /// </summary>
        /// <param name="info">Serialization information.</param>
        /// <param name="context">Streaming context.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AssertNotNull(nameof(info));

            info.AddValue("ErrorCode", this.ErrorCode);
            info.AddValue("Details", this.Details);

            base.GetObjectData(info, context);
        }
    }
}