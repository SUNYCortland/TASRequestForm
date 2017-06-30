using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASRequestForm.Core.Data.Entities;

namespace TASRequestForm.Core.Services
{
    public interface ITestTimeService
    {
        TestTime SaveTestTime(TestTime testTime);
        TestTime GetTestTimeById(int id);
        IList<TestTime> GetInstructorReminderTestTimes();
        IList<TestTime> GetStudentReminderTestTimes();
        IQueryable<TestTime> GetAllPendingTestTimes();
        IQueryable<TestTime> GetAllApprovedTestTimes();
        IQueryable<TestTime> GetAllDeclinedTestTimes();
        IQueryable<TestTime> GetAllTestTimesBetween(DateTime StartTime, DateTime EndTime);
        IQueryable<TestTime> GetAllStudentTestTimes(int pidm);
        IQueryable<TestTime> GetAllStudentPendingTestTimes(int pidm);
        IQueryable<TestTime> GetAllStudentApprovedTestTimes(int pidm);
        IQueryable<TestTime> GetAllStudentDeclinedTestTimes(int pidm);
        IQueryable<TestTime> GetAllInstructorTestTimes(int pidm);
        IQueryable<TestTime> GetAllInstructorPendingTestTimes(int pidm);
        IQueryable<TestTime> GetAllInstructorApprovedTestTimes(int pidm);
        IQueryable<TestTime> GetAllInstructorDeclinedTestTimes(int pidm);
        IQueryable<TestTime> Search(string query);
    }
}
