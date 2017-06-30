using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TASRequestForm.Core.Data.Entities;

namespace TASRequestForm.Web.ViewModels
{
    public class NavLoadMenuViewModel
    {
        public int PendingCount { get; set; }
        public int ApprovedCount { get; set; }
        public int DeclinedCount { get; set; }
        public bool IsStudent { get; set; }
        public bool IsProfessor { get; set; }
        public IList<TestTime> StudentEntries { get; set; }
        public IList<TestTime> ProfessorEntries { get; set; }
        public string Active { get; set; }

        public NavLoadMenuViewModel()
        {
            this.StudentEntries = new List<TestTime>();
            this.ProfessorEntries = new List<TestTime>();
        }
    }
}