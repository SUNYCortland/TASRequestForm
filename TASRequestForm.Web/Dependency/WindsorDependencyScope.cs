using Castle.MicroKernel;
using Castle.MicroKernel.Lifestyle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;

namespace TASRequestForm.Web.Dependency
{
    public class WindsorDependencyScope : IDependencyScope
    {
        private readonly IKernel _kernel;
        private readonly IDisposable _scope;

        public WindsorDependencyScope(IKernel kernel)
        {
            _kernel = kernel;
            _scope = _kernel.BeginScope();
        }

        public object GetService(Type serviceType)
        {
            return _kernel.HasComponent(serviceType) ? _kernel.Resolve(serviceType) : null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.ResolveAll(serviceType).Cast<object>();
        }

        public void Dispose()
        {
            _scope.Dispose();
        }
    }
}