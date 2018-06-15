// -----------------------------------------------------------------------
// <copyright file="HttpContextExtensions.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.Common.Extensions
{
    using System.Linq;
    using System.Security.Claims;
    using System.Web;

    public static class HttpContextExtensions
    {
        public static string GetAuthorizationBearer1(this HttpContext context)
        {
            var claimsIdentity = (ClaimsIdentity)context.GetOwinContext().Authentication.User.Identity;

            var accessToken = claimsIdentity.Claims.First(s => s.Type == ClaimTypes.Authentication).Value;

            return !string.IsNullOrWhiteSpace(accessToken) ? accessToken : null;
        }
    }
}