// -----------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog
{
    using System;
    using System.Globalization;
    using System.Threading;

    public static class Extensions
    {
        /// <summary>
        /// Ensures that a given object is not null. Throws an exception otherwise.
        /// </summary>
        /// <param name="objectToValidate">The object we are validating.</param>
        /// <param name="caption">The name to report in the exception.</param>
        public static void AssertNotNull(this object objectToValidate, string caption)
        {
            if (objectToValidate == null)
            {
                throw new ArgumentNullException(caption);
            }
        }

        /// <summary>
        /// Ensures that a string is not empty. Throws an exception if so.
        /// </summary>
        /// <param name="nonEmptyString">The string to validate.</param>
        /// <param name="caption">The name to report in the exception.</param>
        public static void AssertNotEmpty(this string nonEmptyString, string caption)
        {
            if (string.IsNullOrWhiteSpace(nonEmptyString))
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "{0} is not set", caption ?? "string"));
            }
        }

        /// <summary>
        /// Checks if an exception is fatal.
        /// </summary>
        /// <param name="ex">The exception to check.</param>
        /// <returns>True if Exception is fatal and process should die.</returns>
        public static bool IsFatal(this Exception ex)
        {
            return ex != null && (ex is OutOfMemoryException || ex is AppDomainUnloadedException || ex is BadImageFormatException
                                  || ex is CannotUnloadAppDomainException || ex is InvalidProgramException || ex is ThreadAbortException || ex is StackOverflowException);
        }
    }
}