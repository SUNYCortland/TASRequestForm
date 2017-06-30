using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASRequestForm.Core.Data.Entities;

namespace TASRequestForm.Core.Validators
{
    public class FormEntryValidator : AbstractValidator<FormEntry>
    {
        public FormEntryValidator()
        {

        }
    }
}
