using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TASRequestForm.Core.Data.Entities;

namespace TASRequestForm.Web.Areas.Admin.ViewModels
{
    public class RequestsTasApproveViewModel
    {
        public bool Denied { get; set; }
        public string Note { get; set; }
        public FormEntry FormEntry { get; set; }
    }
}