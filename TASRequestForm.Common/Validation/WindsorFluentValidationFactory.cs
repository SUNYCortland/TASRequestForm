using Castle.MicroKernel;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TASRequestForm.Common.Validation
{
    public class WindsorFluentValidatorFactory : ValidatorFactoryBase
    {
        private readonly IKernel _kernel;

        public WindsorFluentValidatorFactory(IKernel kernel)
        {
            _kernel = kernel;
        }

        public override IValidator CreateInstance(Type validatorType)
        {
            return _kernel.HasComponent(validatorType)
                   ? _kernel.Resolve(validatorType) as IValidator
                   : null;
        }
    }
}
