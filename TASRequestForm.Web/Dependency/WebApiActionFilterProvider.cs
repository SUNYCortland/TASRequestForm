using Castle.MicroKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using TASRequestForm.Web.Extensions;

namespace TASRequestForm.Web.Dependency
{
    public class WebApiActionFilterProvider : ActionDescriptorFilterProvider, IFilterProvider
    {
        private readonly IKernel _kernel;

        public WebApiActionFilterProvider(IKernel kernel)
        {
            _kernel = kernel;
        }

        public IEnumerable<FilterInfo> GetFilters(HttpConfiguration configuration, HttpActionDescriptor actionDescriptor)
        {
            var filters = base.GetFilters(configuration, actionDescriptor);

            foreach (var filter in filters)
            {
                _kernel.InjectProperties(filter.Instance);
            }

            return filters;
        }
    }
}