using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TASRequestForm.Core.Data.Entities;
using TASRequestForm.Web.ViewModels;
using TASRequestForm.Web.Extensions;

namespace TASRequestForm.Web.Validators
{
    public class RequestIndexViewModelValidator : AbstractValidator<RequestIndexViewModel>
    {
        public RequestIndexViewModelValidator()
        {
            RuleFor(x => x.SelectedCourse).NotEmpty()
                .WithMessage("You must select a course.");

            RuleFor(x => x.FormEntry.CourseTime).NotEmpty()
                .WithMessage("You must select a course in order to get the course time.");

            RuleFor(x => x.FormEntry.PhoneNumber).NotEmpty()
                .WithMessage("You must enter your phone number.");

            RuleFor(x => x.FormEntry.TestTimes).Must(HaveMoreThanOneTestTime)
                .WithMessage("You must enter at least one test time.<br />Remember to click on the 'Add Date &amp; Time' button.");

            RuleFor(x => x.FormEntry.TestTimes).Must(HaveAmpleTimeBeforeFirstTest)
                .WithMessage("You cannot schedule a test less than one business day in advance.");

            RuleFor(x => x.FormEntry.TestTimes).Must(BeBetweenMorningAndEvening)
                .WithMessage("You must schedule a test to be taken between 8:00 a.m. and 3:50 p.m.");

            RuleFor(x => x.FormEntry.TestTimes).Must(NotBeInThePast)
                .WithMessage("You cannot schedule a test in the past.");

            RuleFor(x => x.AccomodationSelected).Equal(true)
                .WithMessage("You must select at least one accomodation.");

            RuleFor(x => x.OtherAccomodationsValue).NotEmpty()
                .When(x => x.OtherAccomodations)
                .WithMessage("You must describe your other accomodations.");

            RuleFor(x => x.NotifiedProfessor).Equal(true)
                .WithMessage("You must pick up accomodation letters from Disability Services and give one to this instructor.");
        }

        private bool NotBeInThePast(IList<TestTime> TestTimes)
        {
            foreach (var testTime in TestTimes)
            {
                if (testTime.Date < DateTime.Now)
                    return false;
            }

            return true;
        }

        private bool BeBetweenMorningAndEvening(IList<TestTime> TestTimes)
        {
            var result = true;

            DateTime dtMorning = DateTime.Parse("1988/04/26 08:00:00.000");
            DateTime dtEvening = DateTime.Parse("1988/04/26 15:50:00.000");

            foreach (var testTime in TestTimes)
            {
                if (TimeSpan.Compare(testTime.Date.TimeOfDay, dtMorning.TimeOfDay) < 0
                    || TimeSpan.Compare(testTime.Date.TimeOfDay, dtEvening.TimeOfDay) > 0)
                {
                    result = false;
                    break;
                }
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