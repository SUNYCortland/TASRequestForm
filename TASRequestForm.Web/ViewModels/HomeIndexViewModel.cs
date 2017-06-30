using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TASRequestForm.Core.Data.Entities;

namespace TASRequestForm.Web.ViewModels
{
    public class HomeIndexViewModel
    {
        public IList<TestTime> StudentEntries { get; set; }
        public IList<TestTime> ProfessorEntries { get; set; }

        public HomeIndexViewModel()
        {
            this.StudentEntries = new List<TestTime>();
            this.ProfessorEntries = new List<TestTime>();
        }
    }
}