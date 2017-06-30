using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TASRequestForm.Core.Data.Entities;

namespace TASRequestForm.Web.Areas.Admin.ViewModels
{
    public class RequestsInstructorApproveViewModel
    {
        public FormEntry FormEntry { get; set; }

        public string Note { get; set; }
        public bool Denied { get; set; }

        public bool BookQuizAccomodation { get; set; }
        public bool NotesQuizAccomodation { get; set; }
        public bool CalculatorQuizAccomodation { get; set; }
        public bool FormulaeQuizAccomodation { get; set; }
        public bool OtherQuizAccomodation { get; set; }
        public bool NoQuizAccomodation { get; set; }
        public string OtherQuizAccomodationString { get; set; }

        public bool BookExamAccomodation { get; set; }
        public bool NotesExamAccomodation { get; set; }
        public bool CalculatorExamAccomodation { get; set; }
        public bool FormulaeExamAccomodation { get; set; }
        public bool OtherExamAccomodation { get; set; }
        public bool NoExamAccomodation { get; set; }
        public string OtherExamAccomodationString { get; set; }

        public bool BookFinalExamAccomodation { get; set; }
        public bool NotesFinalExamAccomodation { get; set; }
        public bool CalculatorFinalExamAccomodation { get; set; }
        public bool FormulaeFinalExamAccomodation { get; set; }
        public bool OtherFinalExamAccomodation { get; set; }
        public bool NoFinalExamAccomodation { get; set; }
        public string OtherFinalExamAccomodationString { get; set; }
    }
}