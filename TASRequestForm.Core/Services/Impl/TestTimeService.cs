using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASRequestForm.Core.Data;
using TASRequestForm.Core.Data.Entities;
using TASRequestForm.Core.Data.Repositories;

namespace TASRequestForm.Core.Services.Impl
{
    public class TestTimeService : ITestTimeService
    {
        private readonly ITestTimeRepository _testTimeRepository;

        public TestTimeService(ITestTimeRepository testTimeRepository)
        {
            _testTimeRepository = testTimeRepository;
        }

        [UnitOfWork]
        public TestTime SaveTestTime(TestTime testTime)
        {
            return _testTimeRepository.SaveOrUpdate(testTime);
        }

        public TestTime GetTestTimeById(int id)
        {
            return _testTimeRepository.FindBy(x => x.Id == id);
        }

        public IList<TestTime> GetInstructorReminderTestTimes()
        {
            var tests = _testTimeRepository.FilterBy(x => x.FormEntry.ReminderDays != null && x.FormEntry.TASApproved && x.FormEntry.ProfessorApproved && !x.Canceled && x.Date >= DateTime.Now).ToList();

            tests = tests.Where(x => x.Date.AddDays(-x.FormEntry.ReminderDays.Value).Date == DateTime.Now.Date).ToList();

            return tests;
        }

        public IList<TestTime> GetStudentReminderTestTimes()
        {
            var tests = _testTimeRepository.FilterBy(x => x.FormEntry.TASApproved && x.FormEntry.ProfessorApproved && !x.Canceled && x.Date >= DateTime.Now).ToList();

            tests = tests.Where(x => x.Date.AddDays(-1).Date == DateTime.Now.Date).ToList();

            return tests;
        }

        public IQueryable<TestTime> GetAllPendingTestTimes()
        {
            return _testTimeRepository.FilterBy(x => (!x.FormEntry.ProfessorApprovedDate.HasValue || !x.FormEntry.ApprovedDate.HasValue) && !x.FormEntry.Archived).OrderBy(x => x.Date);
        }

        public IQueryable<TestTime> GetAllApprovedTestTimes()
        {
            return _testTimeRepository.FilterBy(x => x.FormEntry.ProfessorApproved && x.FormEntry.TASApproved && !x.FormEntry.Archived).OrderBy(x => x.Date);
        }

        public IQueryable<TestTime> GetAllDeclinedTestTimes()
        {
            return _testTimeRepository.FilterBy(x => ((!x.FormEntry.ProfessorApproved && x.FormEntry.ProfessorApprovedDate.HasValue) || (!x.FormEntry.TASApproved && x.FormEntry.ApprovedDate.HasValue)) && !x.FormEntry.Archived).OrderBy(x => x.Date);
        }

        public IQueryable<TestTime> GetAllTestTimesBetween(DateTime StartTime, DateTime EndTime)
        {
            return _testTimeRepository.FilterBy(x => x.Date >= StartTime && x.Date <= EndTime && !x.FormEntry.Archived);
        }

        public IQueryable<TestTime> GetAllStudentTestTimes(int pidm)
        {
            return _testTimeRepository.FilterBy(x => x.FormEntry.StudentPidm == pidm && !x.FormEntry.Archived);
        }

        public IQueryable<TestTime> GetAllStudentPendingTestTimes(int pidm)
        {
            return _testTimeRepository.FilterBy(x => ((!x.FormEntry.ProfessorApprovedDate.HasValue || !x.FormEntry.ApprovedDate.HasValue)) && x.FormEntry.StudentPidm == pidm && !x.FormEntry.Archived)
                                        .OrderBy(x => x.Date);
        }

        public IQueryable<TestTime> GetAllStudentApprovedTestTimes(int pidm)
        {
            return _testTimeRepository.FilterBy(x => x.FormEntry.ProfessorApproved && x.FormEntry.TASApproved && x.FormEntry.StudentPidm == pidm && !x.FormEntry.Archived).OrderBy(x => x.Date);   
        }
        
        public IQueryable<TestTime> GetAllStudentDeclinedTestTimes(int pidm)
        {
            return _testTimeRepository.FilterBy(x => ((!x.FormEntry.ProfessorApproved && x.FormEntry.ProfessorApprovedDate.HasValue) || (!x.FormEntry.TASApproved && x.FormEntry.ApprovedDate.HasValue)) && x.FormEntry.StudentPidm == pidm && !x.FormEntry.Archived).OrderBy(x => x.Date);
        }

        public IQueryable<TestTime> GetAllInstructorTestTimes(int pidm)
        {
            return _testTimeRepository.FilterBy(x => x.FormEntry.ProfessorPidm == pidm && !x.FormEntry.Archived);
        }

        public IQueryable<TestTime> GetAllInstructorPendingTestTimes(int pidm)
        {
            return _testTimeRepository.FilterBy(x => ((!x.FormEntry.ProfessorApprovedDate.HasValue || !x.FormEntry.ApprovedDate.HasValue)) && x.FormEntry.ProfessorPidm == pidm && !x.FormEntry.Archived)
                                        .OrderBy(x => x.Date);
        }

        public IQueryable<TestTime> GetAllInstructorApprovedTestTimes(int pidm)
        {
            return _testTimeRepository.FilterBy(x => x.FormEntry.ProfessorApproved && x.FormEntry.TASApproved && x.FormEntry.ProfessorPidm == pidm && !x.FormEntry.Archived).OrderBy(x => x.Date);   
        }

        public IQueryable<TestTime> GetAllInstructorDeclinedTestTimes(int pidm)
        {
            return _testTimeRepository.FilterBy(x => ((!x.FormEntry.ProfessorApproved && x.FormEntry.ProfessorApprovedDate.HasValue) || (!x.FormEntry.TASApproved && x.FormEntry.ApprovedDate.HasValue)) && x.FormEntry.ProfessorPidm == pidm && !x.FormEntry.Archived).OrderBy(x => x.Date);
        }

        public IQueryable<TestTime> Search(string query)
        {
            return _testTimeRepository.FilterBy(x => (x.FormEntry.StudentIdentity.LastName.ToUpper().Contains(query.ToUpper())
                                                    || x.FormEntry.StudentIdentity.FirstName.ToUpper().Contains(query.ToUpper())
                                                    || x.FormEntry.StudentIdentity.CNumber.ToUpper() == query.ToUpper()
                                                    || (x.FormEntry.StudentIdentity.FirstName.ToUpper() + " " + x.FormEntry.StudentIdentity.LastName.ToUpper()).Contains(query.ToUpper())
                                                    || x.FormEntry.ProfessorIdentity.LastName.ToUpper().Contains(query.ToUpper())
                                                    || x.FormEntry.ProfessorIdentity.FirstName.ToUpper().Contains(query.ToUpper())
                                                    || (x.FormEntry.ProfessorIdentity.FirstName.ToUpper() + " " + x.FormEntry.ProfessorIdentity.LastName.ToUpper()).Contains(query.ToUpper()))
                                                    && !x.FormEntry.Archived);
        }
    }
}
