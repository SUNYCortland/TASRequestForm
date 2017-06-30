using Castle.Core.Logging;
using Castle.MicroKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using TASRequestForm.Core.Services;

namespace TASRequestForm.Web.Attributes
{
    /// <summary>
    /// Authorizes web api calls by reading Authorization HTTP header
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class WebApiAuthorizeAttribute : AuthorizeAttribute
    {
        public ILogger Logger { get; set; }

        // Secret Key
        private const string KEY = "YOUR_KEY";

        // Parameters
        public bool RequireSsl { get; set; }

        // Property inject services
        public IKernel _kernel { get; set; }

        private IAdministratorService _administratorService { get; set; }
        private IBannerIdentityService _bannerIdentityService { get; set; }

        public WebApiAuthorizeAttribute(bool RequireSsl)
        {
            this.RequireSsl = RequireSsl;
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            // If we require ssl then check
            if (RequireSsl)
            {
                if (actionContext.Request.RequestUri.Scheme != Uri.UriSchemeHttps)
                {
                    Logger.Debug("API Error: SSL Required for API call.");
                    HandleRequireSsl(actionContext);
                    return;
                }
            }

            // Get token from authorization header
            var credentials = ParseAuthorizationHeader(actionContext);

            // Make sure token format is correct
            if (credentials == null)
            {
                Logger.Debug("API Error: Credentials are null.");
                HandleUnauthorized(actionContext);
                return;
            }

            // Get pidm and key
            int pidm;

            if (!int.TryParse(credentials[0], out pidm))
            {
                Logger.Debug("API Error: Pidm is not a valid integer.");
                HandleUnauthorized(actionContext);
                return;
            }
                
            var apiKey = credentials[1];

            if (apiKey != KEY)
            {
                Logger.Debug("API Error: Invalid key.");
                HandleUnauthorized(actionContext);
                return;
            }

            // We must do this everytime as these attributes are created only once per application lifetime
            // but this method is called once per web request
            InitializeDependencies();

            // Check if user exists
            var isAdmin = _administratorService.IsAdministratorByPidm(pidm);

            if (!isAdmin)
            {
                Logger.Debug(String.Format("API Error: Pidm ({0}) is not an administrator.", pidm.ToString()));
                HandleUnauthorized(actionContext);
                return;
            }

            var user = _bannerIdentityService.GetBannerIdentityByPidm(pidm);

            if (user == null)
            {
                Logger.Debug(String.Format("API Error: Pidm ({0}) could not be found in banner.", pidm.ToString()));
                HandleUnauthorized(actionContext);
                return;
            }

            // Store username to access in controller
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(user.NetId.Value), null);

            //base.OnAuthorization(actionContext);
        }

        private void InitializeDependencies()
        {
            _administratorService = _kernel.Resolve<IAdministratorService>();
            _bannerIdentityService = _kernel.Resolve<IBannerIdentityService>();
        }

        private string[] ParseAuthorizationHeader(HttpActionContext actionContext)
        {
            var auth = actionContext.Request.Headers.Authorization;
            string authHeader = String.Empty;

            Logger.Debug(String.Format("API Call: auth ({0})", auth));

            if (auth != null && auth.Scheme == "Basic")
                authHeader = auth.Parameter;

            Logger.Debug(String.Format("API Call: authHeader ({0})", authHeader));

            if (string.IsNullOrEmpty(authHeader))
                return null;

            authHeader = Encoding.ASCII.GetString(Convert.FromBase64String(authHeader));

            var tokens = authHeader.Split(':');

            if (tokens.Length < 2)
                return null;

            return tokens;
        }

        // Send back unauthorized 403.4 response
        private void HandleRequireSsl(HttpActionContext actionContext)
        {
            var host = actionContext.Request.RequestUri.DnsSafeHost;
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
            actionContext.Response.Headers.Add("WWW-Authenticate", string.Format("Basic realm=\"{0}\"", host));
        }

        // Send back unauthorized 401 response
        private void HandleUnauthorized(HttpActionContext actionContext)
        {
            var host = actionContext.Request.RequestUri.DnsSafeHost;
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            actionContext.Response.Headers.Add("WWW-Authenticate", string.Format("Basic realm=\"{0}\"", host));
        }
    }
}