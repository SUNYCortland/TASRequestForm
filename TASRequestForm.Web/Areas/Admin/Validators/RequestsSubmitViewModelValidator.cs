using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TASRequestForm.Core.Data.Entities;
using TASRequestForm.Web.Areas.Admin.ViewModels;
using TASRequestForm.Web.Extensions;

namespace TASRequestForm.Web.Areas.Admin.Validators
{
    public class RequestsSubmitViewModelValidator : AbstractValidator<RequestsSubmitViewModel>
    {
        public RequestsSubmitViewModelValidator()
        {
            RuleFor(x => x.SelectedCourse).NotEmpty()
                .WithMessage("You must select a course.");

            RuleFor(x => x.FormEntry.CourseTime).NotEmpty()
                .WithMessage("You must select a course in order to get the course time.");

            RuleFor(x => x.FormEntry.PhoneNumber).NotEmpty()
                .WithMessage("You must enter your phone number.");

            RuleFor(x => x.FormEntry.TestTimes).Must(HaveMoreThanOneTestTime)
                .WithMessage("You must enter at least one test time.<br />Remember to click on the 'Add Date &amp; Time' button.");

            RuleFor(x => x.FormEntry.TestTimes).Must(NotScheduleTestInThePast)
                .WithMessage("You must schedule a test to occur sometime in the future.");

            // Admin need to submit requests within 24 hours
            /*RuleFor(x => x.FormEntry.TestTimes).Must(HaveAmpleTimeBeforeFirstTest)
                .WithMessage("You cannot schedule a test less than one business day in advance.");*/

            RuleFor(x => x.OtherAccomodationsValue).NotEmpty()
                .When(x => x.OtherAccomodations)
                .WithMessage("You must describe your other accomodations.");

            RuleFor(x => x.NotifiedProfessor).Equal(true)
                .WithMessage("You must pick up accomodation letters from Disability Services and give one to this instructor.");
        }

        private bool NotScheduleTestInThePast(IList<TestTime> TestTimes)
        {
            var result = true;

            foreach (var testTime in TestTimes)
            {
                if (testTime.Date < DateTime.Now)
                    result = false;
            }

            return result;
        }

        private bool HaveAmpleTimeBeforeFirstTest(IList<TestTime> TestTimes)
        {
            var result = true;

            foreach (var testTime in TestTimes)
            {
                if (DateTime.Now.BusinessTimeDelta(testTime.Date).TotalHours <= 24)
                {
                    result = false;
                }
            }

            return result;
        }

        private bool HaveMoreThanOneTestTime(IList<TestTime> TestTimes)
        {
            return TestTimes.Count > 0;
        }
    }
}