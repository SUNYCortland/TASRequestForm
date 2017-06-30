using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;
using TASRequestForm.Core.Data.Entities;
using TASRequestForm.Core.Services;
using TASRequestForm.Web.Attributes;
using TASRequestForm.Web.ViewModels.Api;
using TASRequestForm.Web.Extensions;

namespace TASRequestForm.Web.Controllers.Api
{
    public class AutomationController : ApiController
    {
        private readonly ITestTimeService _testTimeService;
        private readonly IFormEntryService _formEntryService;
        private readonly IEmailService _emailService;

        public AutomationController(ITestTimeService testTimeService,
                                    IFormEntryService formEntryService,
                                    IEmailService emailService)
        {
            _testTimeService = testTimeService;
            _formEntryService = formEntryService;
            _emailService = emailService;
        }

        [HttpGet]
        [WebApiAuthorize(false)]
        public WebApiResponse RemindInstructors()
        {
            try
            {
                var tests = _testTimeService.GetInstructorReminderTestTimes();

                foreach (var test in tests)
                {
                    _emailService.SendEmail<TestTime>(EmailTemplate.InstructorReminder, test, test.FormEntry.ProfessorIdentity.Email, "TAS Request - Instructor Reminder");
                }
                
            }
            catch (Exception ex)
            {
                return new WebApiResponse
                {
                    Success = false,
                    Messages = new string [] { ex.ToString() }
                };
            }

            return new WebApiResponse
            {
                Success = true
            };
        }

        [HttpGet]
        [WebApiAuthorize(false)]
        public WebApiResponse RemindInstructorsOfApprovals()
        {
            if (DateTime.Now.IsBusinessDay()) 
            { 
                try
                {
                    var formEntries = _formEntryService.GetAllInstructorReminderFormEntries();

                    foreach (var entry in formEntries)
                    {
                        _emailService.SendEmail<FormEntry>(EmailTemplate.InstructorReceiptReminder, entry, entry.ProfessorIdentity.Email, "TAS Request - Instructor Approval Reminder");
                    }

                }
                catch (Exception ex)
                {
                    return new WebApiResponse
                    {
                        Success = false,
                        Messages = new string[] { ex.ToString() }
                    };
                }
            }

            return new WebApiResponse
            {
                Success = true
            };
        }

        [HttpGet]
        [WebApiAuthorize(false)]
        public WebApiResponse RemindStudents()
        {
            try
            {
                var tests = _testTimeService.GetStudentReminderTestTimes();

                foreach (var test in tests)
                {
                    _emailService.SendEmail<TestTime>(EmailTemplate.StudentReminder, test, test.FormEntry.StudentIdentity.Email, "TAS Request - Test Reminder");
                }

            }
            catch (Exception ex)
            {
                return new WebApiResponse
                {
                    Success = false,
                    Messages = new string[] { ex.ToString() }
                };
            }

            return new WebApiResponse
            {
                Success = true
            };
        }
    }
}
