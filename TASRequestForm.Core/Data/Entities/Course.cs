using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TASRequestForm.Core.Data.Entities
{
    public class Course : Entity<int?>
    {
        public string CRN { get; set; }
        public string Subject { get; set; }
        public string Number { get; set; }
        public string Section { get; set; }
        public string Title { get; set; }
        public string ProfessorFirstName { get; set; }
        public string ProfessorLastName { get; set; }
        public int ProfessorPidm { get; set; }
    }
}
