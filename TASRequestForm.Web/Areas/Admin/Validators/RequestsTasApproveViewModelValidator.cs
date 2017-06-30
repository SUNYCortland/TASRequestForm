using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TASRequestForm.Web.Areas.Admin.ViewModels;

namespace TASRequestForm.Web.Areas.Admin.Validators
{
    public class RequestsTasApproveViewModelValidator : AbstractValidator<RequestsTasApproveViewModel>
    {
        public RequestsTasApproveViewModelValidator()
        {
            RuleFor(x => x.Note).NotEmpty()
                .When(x => x.Denied)
                .WithMessage("You must enter a note if you a denying this request.");
        }
    }
}