// -----------------------------------------------------------------------
// <copyright file="UserRoleHelper.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog.Common.Helpers
{
    using System.Web.Configuration;

    public static class UserRoleHelper
    {
        public static string AdminUserName
        {
            get { return WebConfigurationManager.AppSettings[ConfigurationConstants.AdminUserName]; }
        }

        public static bool IsAdmin(string userName)
        {
            return AdminUserName == userName;
        }
    }
}