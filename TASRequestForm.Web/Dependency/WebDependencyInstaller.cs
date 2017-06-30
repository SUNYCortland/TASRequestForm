using Castle.Facilities.Logging;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using TASRequestForm.Web.Validators;

namespace TASRequestForm.Web.Dependency
{
    public class WebDependencyInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.AddFacility<LoggingFacility>(f => f.UseLog4Net().WithConfig("log4net.config"));

            //Register all controllers
            container.Register(
                // All validators
                Classes.FromAssembly(Assembly.GetAssembly(typeof(RequestIndexViewModelValidator))).BasedOn(typeof(IValidator<>)).WithService.Base().LifestyleTransient(),

                Component.For<IKernel>().Instance(container.Kernel),

                Component.For<System.Web.Http.Filters.IFilterProvider>().ImplementedBy<WebApiActionFilterProvider>().LifeStyle.Transient,

                //All MVC controllers
                Classes.FromThisAssembly().BasedOn<System.Web.Mvc.IController>().LifestyleTransient(),

                Classes.FromThisAssembly().BasedOn<ApiController>().LifestyleScoped()
            );
        }
    }
}