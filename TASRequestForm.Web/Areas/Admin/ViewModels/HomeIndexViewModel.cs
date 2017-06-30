using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TASRequestForm.Core.Data.Entities;

namespace TASRequestForm.Web.Areas.Admin.ViewModels
{
    public class HomeIndexViewModel
    {
        public IList<TestTime> Pending { get; set; }
        public IList<TestTime> Approved { get; set; }
        public IList<TestTime> Declined { get; set; }

        public HomeIndexViewModel()
        {
            this.Pending = new List<TestTime>();
            this.Approved = new List<TestTime>();
            this.Declined = new List<TestTime>();
        }
    }
}