using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASRequestForm.Core.Data;
using TASRequestForm.Core.Data.Entities;
using TASRequestForm.Core.Data.Repositories;
using TASRequestForm.Common.Extensions;

namespace TASRequestForm.Core.Services.Impl
{
    public class FormEntryService : IFormEntryService
    {
        private IFormEntryRepository _formEntryRepository;

        public FormEntryService(IFormEntryRepository formEntryRepository)
        {
            _formEntryRepository = formEntryRepository;
        }

        public bool CourseTimeMismatch(FormEntry entry)
        {
            var courseTime = entry.CourseTime.Split(new string[] { "at" }, StringSplitOptions.None)[1].Trim();

            if (courseTime.Split('-').Count() == 0)
                return true;

            var courseTimeStart = DateTime.Parse(courseTime.Split('-')[0]).TimeOfDay;
            var courseTimeEnd = DateTime.Parse(courseTime.Split('-')[1]).TimeOfDay;

            if (entry.QuizTimes.Any(x => x.Date.TimeOfDay > courseTimeEnd || x.Date.TimeOfDay < courseTimeStart))
                return true;

            if (entry.ExamTimes.Any(x => x.Date.TimeOfDay > courseTimeEnd || x.Date.TimeOfDay < courseTimeStart))
                return true;

            // No longer check for final exam times when checking mismatch
            /*if (entry.FinalExamTime != null)
            {
                if (entry.FinalExamTime.Date.TimeOfDay > courseTimeEnd || entry.FinalExamTime.Date.TimeOfDay < courseTimeStart)
                    return true;
            }*/

            return false;
        }

        public IList<FormEntry> GetAllFormEntriesWithConflictingTime(DateTime date, int pidm)
        {
            var formEntries = _formEntryRepository.FilterBy(x => x.TestTimes.Any(y => !y.Canceled) && x.StudentPidm == pidm).ToList();

            formEntries = formEntries.Where(x => x.TestTimes.Any(y => (date > y.Date.AddMinutes(-45) && date < y.Date.AddMinutes(45)))).ToList();

            return formEntries.ToList();
        }

        public IList<FormEntry> GetAllInstructorReminderFormEntries()
        {
            var formEntries = _formEntryRepository.FilterBy(x => !x.ProfessorApprovedDate.HasValue).ToList();

            formEntries = formEntries.Where(x => x.DateSubmitted.BusinessDaysTo(DateTime.Now) >= 2).ToList();

            return formEntries;
        }

        public IQueryable<FormEntry> GetAllStudentFormEntriesByPidm(int Pidm)
        {
            return _formEntryRepository.FilterBy(x => x.StudentPidm == Pidm);
        }

        public IQueryable<FormEntry> GetAllProfessorFormEntriesByPidm(int Pidm)
        {
            return _formEntryRepository.FilterBy(x => x.ProfessorPidm == Pidm);
        }

        public IQueryable<FormEntry> GetAllFormEntries()
        {
            return _formEntryRepository.All();
        }

        public IQueryable<FormEntry> GetAllPendingFormEntries()
        {
            return _formEntryRepository.FilterBy(x => !x.ProfessorApprovedDate.HasValue || !x.ApprovedDate.HasValue);
        }

        public IQueryable<FormEntry> GetAllApprovedFormEntries()
        {
            return _formEntryRepository.FilterBy(x => x.ProfessorApproved && x.TASApproved);
        }

        public IQueryable<FormEntry> GetAllDeclinedFormEntries()
        {
            return _formEntryRepository.FilterBy(x => (!x.ProfessorApproved && x.ProfessorApprovedDate.HasValue) || (!x.TASApproved && x.ApprovedDate.HasValue));
        }

        public FormEntry GetFormEntryById(int id)
        {
            return _formEntryRepository.FindBy(x => x.Id == id);
        }

        [UnitOfWork]
        public FormEntry SaveFormEntry(FormEntry entry)
        {
            return _formEntryRepository.SaveOrUpdate(entry);
        }
    }
}
