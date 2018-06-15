// -----------------------------------------------------------------------
// <copyright file="Startup.Auth.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace ServiceCatalog
{
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Cookies;
    using Owin;
    using System.Globalization;
    using System.Web.Configuration;
    using System.Threading.Tasks;
    using Microsoft.Owin.Security.OpenIdConnect;

    /// <summary>
    /// Application start up class.
    /// </summary>
    public partial class Startup
    {
        private static readonly string ClientId = WebConfigurationManager.AppSettings[ConfigurationConstants.ClientIdName];
        private static readonly string AadInstance = WebConfigurationManager.AppSettings[ConfigurationConstants.AadInstanceName];
        private static readonly string Tenant = WebConfigurationManager.AppSettings[ConfigurationConstants.TenantName];
        private readonly string authority = string.Format(CultureInfo.InvariantCulture, AadInstance, Tenant);

        /// <summary>
        /// Configures application authentication.
        /// </summary>
        /// <param name="application">The application to configure.<see cref="IAppBuilder"/></param>
        public void ConfigureAuth(IAppBuilder application)
        {
            // Set cookie authentication type
            application.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            application.UseCookieAuthentication(new CookieAuthenticationOptions { });

            // Configure Open ID connection
            application.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    ClientId = ClientId,
                    Authority = authority,
                    TokenValidationParameters = new System.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = false
                    },
                    Notifications = new OpenIdConnectAuthenticationNotifications()
                    {
                        AuthenticationFailed = (context) =>
                        {
                            context.OwinContext.Response.Redirect($"/Home/Error?errorMessage={context.Exception.Message}");
                            context.HandleResponse();

                            return Task.FromResult(0);
                        }
                    }
                });
        }
    }
}