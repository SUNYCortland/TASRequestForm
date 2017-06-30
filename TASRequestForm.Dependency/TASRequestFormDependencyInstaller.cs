using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using FluentValidation;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TASRequestForm.Core.Services.Impl;
using TASRequestForm.Core.Validators;
using TASRequestForm.Data.Repositories.Nh;
using TASRequestForm.Dependency.UoW;

namespace TASRequestForm.Dependency
{
    public class TASRequestFormDependencyInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Kernel.ComponentRegistered += Kernel_ComponentRegistered;

            container.AddFacility(new PersistenceFacility());

            // Register all controllers
            container.Register(
                // Unitofwork interceptor
                Component.For<NhUnitOfWorkInterceptor>().LifeStyle.PerWebRequest,

                Component.For<ITemplateService>().ImplementedBy<TemplateService>().LifeStyle.Singleton,

                // All validators
                Classes.FromAssembly(Assembly.GetAssembly(typeof(FormEntryValidator))).BasedOn(typeof(IValidator<>)).WithService.Base().LifestyleTransient(),

                // All repositories
                Classes.FromAssembly(Assembly.GetAssembly(typeof(NhFormEntryRepository))).InSameNamespaceAs<NhFormEntryRepository>().WithService.DefaultInterfaces().LifestyleTransient(),

                // All services
                Classes.FromAssembly(Assembly.GetAssembly(typeof(FormEntryService))).InSameNamespaceAs<FormEntryService>().WithService.DefaultInterfaces().LifestyleTransient()

                );
        }

        void Kernel_ComponentRegistered(string key, Castle.MicroKernel.IHandler handler)
        {
            //Intercept all methods of all repositories.
            if (UnitOfWorkHelper.IsRepositoryClass(handler.ComponentModel.Implementation))
            {
                handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(NhUnitOfWorkInterceptor)));
            }

            //Intercept all methods of classes those have at least one method that has UnitOfWork attribute.
            foreach (var method in handler.ComponentModel.Implementation.GetMethods())
            {
                if (UnitOfWorkHelper.HasUnitOfWorkAttribute(method))
                {
                    handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(NhUnitOfWorkInterceptor)));
                    return;
                }
            }
        }
    }
}
