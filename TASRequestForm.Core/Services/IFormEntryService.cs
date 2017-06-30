using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASRequestForm.Core.Data.Entities;

namespace TASRequestForm.Core.Services
{
    public interface IFormEntryService
    {
        bool CourseTimeMismatch(FormEntry entry);

        IList<FormEntry> GetAllFormEntriesWithConflictingTime(DateTime date, int pidm);

        IList<FormEntry> GetAllInstructorReminderFormEntries();

        IQueryable<FormEntry> GetAllFormEntries();

        IQueryable<FormEntry> GetAllStudentFormEntriesByPidm(int Pidm);

        IQueryable<FormEntry> GetAllProfessorFormEntriesByPidm(int Pidm);

        IQueryable<FormEntry> GetAllPendingFormEntries();

        IQueryable<FormEntry> GetAllApprovedFormEntries();

        IQueryable<FormEntry> GetAllDeclinedFormEntries();

        FormEntry GetFormEntryById(int id);

        FormEntry SaveFormEntry(FormEntry entry);
    }
}
