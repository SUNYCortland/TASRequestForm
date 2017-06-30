using Castle.Windsor;
using FluentValidation.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;
using TASRequestForm.Common.Validation;
using TASRequestForm.Dependency;
using TASRequestForm.Web.Authentication;
using TASRequestForm.Web.Dependency;

namespace TASRequestForm.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private WindsorContainer _windsorContainer;

        protected void Application_Start()
        {
            InitializeWindsor();
            InitializeValidation();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                UserPrincipalPoco serializeModel = serializer.Deserialize<UserPrincipalPoco>(authTicket.UserData);

                UserPrincipal newUser = new UserPrincipal(authTicket.Name);
                newUser.BannerIdentity = serializeModel.BannerIdentity;
                newUser.IsAdmin = serializeModel.IsAdmin;

                HttpContext.Current.User = newUser;
            }
        }

        private void InitializeValidation()
        {
            FluentValidationModelValidatorProvider.Configure(provider =>
            {
                provider.ValidatorFactory = new WindsorFluentValidatorFactory(_windsorContainer.Kernel);
            });
        }

        private void InitializeWindsor()
        {
            _windsorContainer = new WindsorContainer();
            _windsorContainer.Install(new TASRequestFormDependencyInstaller());
            _windsorContainer.Install(new WebDependencyInstaller());
            GlobalConfiguration.Configuration.DependencyResolver = new WindsorDependencyResolver(_windsorContainer.Kernel);
            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(_windsorContainer.Kernel));
        }
    }
}
