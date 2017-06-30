using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TASRequestForm.Core.Data.Entities;

namespace TASRequestForm.Web.ViewModels
{
    public class RequestIndexViewModel
    {
        public FormEntry PreviousFormEntry { get; set; }
        public FormEntry FormEntry { get; set; }

        public bool NotifiedProfessor { get; set; }

        public bool MinimalDistractionsExtendedTime { get; set; }
        public bool WordProcessor { get; set; }
        public bool TextToSpeech { get; set; }
        public bool OtherAccomodations { get; set; }
        public string OtherAccomodationsValue { get; set; }
        public bool AccomodationSelected
        {
            get
            {
                return this.MinimalDistractionsExtendedTime || this.WordProcessor || this.TextToSpeech || this.OtherAccomodations;
            }
        }

        public string Note { get; set; }
        
        public string SelectedCourse { get; set; }
        public IEnumerable<SelectListItem> Courses { get; set; }

        public RequestIndexViewModel()
        {
            this.Courses = new List<SelectListItem>();
            this.FormEntry = new FormEntry();
        }
    }
}