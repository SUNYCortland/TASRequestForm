using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TASRequestForm.Web.ViewModels;

namespace TASRequestForm.Web.Validators
{
    public class InstructorRequestViewModelValidator : AbstractValidator<InstructorRequestViewModel>
    {
        public InstructorRequestViewModelValidator()
        {
            RuleFor(x => x.FormEntry.ProfessorEmail).NotEmpty()
                .When(x => x.FormEntry.ReturnType == "Scan, email, and campus mail original"
                    || x.FormEntry.ReturnType == "Scan, email, and retain original exam for the semester")
                .WithMessage("You must enter your email address.");

            RuleFor(x => x.FormEntry.ReturnType).NotEmpty()
                .WithMessage("You must select a return type.");

            RuleFor(x => x.FormEntry.ProfessorCampusAddress).NotEmpty()
                .When(x => x.FormEntry.ReturnType == "Scan, email, and campus mail original")
                .WithMessage("You must enter your campus mailing address.");

            RuleFor(x => x.OtherQuizAccomodationString).NotEmpty()
                .When(x => x.OtherQuizAccomodation)
                .WithMessage("You must enter an other quiz accomodation.");

            RuleFor(x => x.OtherExamAccomodationString).NotEmpty()
                .When(x => x.OtherExamAccomodation)
                .WithMessage("You must enter an other exam accomodation.");

            RuleFor(x => x.OtherFinalExamAccomodationString).NotEmpty()
                .When(x => x.OtherFinalExamAccomodation)
                .WithMessage("You must enter an other final exam accomodation.");

            RuleFor(x => x.Note).NotEmpty()
                .When(x => x.Denied)
                .WithMessage("You must enter a note if you a denying this request.");
        }
    }
}