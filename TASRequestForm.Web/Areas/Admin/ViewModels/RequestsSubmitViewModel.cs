using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TASRequestForm.Core.Data.Entities;

namespace TASRequestForm.Web.Areas.Admin.ViewModels
{
    public class RequestsSubmitViewModel
    {
        public FormEntry FormEntry { get; set; }

        public int StudentPidm { get; set; }
        public string StudentCNumber { get; set; }
        public string StudentName { get; set; }

        public bool NotifiedProfessor { get; set; }

        public bool BypassProfessorEmail { get; set; }
        public string ProfessorEmailText { get; set; }

        public bool MinimalDistractionsExtendedTime { get; set; }
        public bool WordProcessor { get; set; }
        public bool TextToSpeech { get; set; }
        public bool OtherAccomodations { get; set; }
        public string OtherAccomodationsValue { get; set; }

        public string Note { get; set; }
        
        public string SelectedCourse { get; set; }
        public IEnumerable<SelectListItem> Courses { get; set; }

        public RequestsSubmitViewModel()
        {
            this.Courses = new List<SelectListItem>();
            this.FormEntry = new FormEntry();
        }
    }
}