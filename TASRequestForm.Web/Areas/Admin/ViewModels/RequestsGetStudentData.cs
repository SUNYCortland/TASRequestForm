using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TASRequestForm.Core.Data.Entities;

namespace TASRequestForm.Web.Areas.Admin.ViewModels
{
    public class RequestsGetStudentData
    {
        public BannerIdentity Student { get; set; }
        public IList<Course> Courses { get; set; }
    }
}