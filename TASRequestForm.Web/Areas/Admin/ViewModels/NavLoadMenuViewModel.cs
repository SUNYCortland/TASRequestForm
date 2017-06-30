using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TASRequestForm.Core.Data.Entities;

namespace TASRequestForm.Web.Areas.Admin.ViewModels
{
    public class NavLoadMenuViewModel
    {
        public int PendingCount { get; set; }
        public int ApprovedCount { get; set; }
        public int DeclinedCount { get; set; }
        public string Active { get; set; }

        public NavLoadMenuViewModel()
        {

        }
    }
}