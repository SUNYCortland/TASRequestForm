using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TASRequestForm.Core.Data.Entities
{
    public class FormEntry : Entity<int?>
    {
        public virtual int StudentPidm { get; set; }
        public virtual BannerIdentity StudentIdentity { get; set; }
        [Display(Name = "Phone Number")]
        public virtual string PhoneNumber { get; set; }

        public virtual int ProfessorPidm { get; set; }
        public virtual BannerIdentity ProfessorIdentity { get; set; }
        [Display(Name = "Instructor Email Address")]
        public virtual string ProfessorEmail { get; set; }
        [Display(Name = "Instructor Department and Building")]
        public virtual string ProfessorCampusAddress { get; set; }

        public virtual bool Archived { get; set; }

        public virtual bool ProfessorApproved { get; set; }
        public virtual bool TASApproved { get; set; }
        public virtual DateTime? ProfessorApprovedDate { get; set; }
        public virtual DateTime? ApprovedDate { get; set; }
        public virtual int? ApprovedByPidm { get; set; }
        public virtual BannerIdentity ApprovedByIdentity { get; set; }

        public virtual string Course { get; set; }
        public virtual string CourseCRN { get; set; }
        public virtual string CourseTime { get; set; }
        public virtual string DeliveryType { get; set; }
        public virtual string ReturnType { get; set; }
        public virtual int? ReminderDays { get; set; }
        [Display(Name = "How may we reach you if the student has a question during exam?")]
        public virtual string ProfessorReachedBy { get; set; }
        public virtual DateTime DateSubmitted { get; set; }

        public virtual IList<Note> Notes { get; set; }
        public virtual IList<Note> PublicNotes
        {
            get
            {
                return this.Notes.Where(x => x.Type != "private").ToList();
            }
        }
        public virtual IList<Note> PrivateNotes
        {
            get
            {
                return this.Notes.Where(x => x.Type == "private").ToList();
            }
        }
        public virtual IList<Note> StudentNotes
        {
            get
            {
                return this.Notes.Where(x => x.Type == "student").ToList();
            }
        }
        public virtual IList<Note> ProfessorNotes
        {
            get
            {
                return this.Notes.Where(x => x.Type == "professor").ToList();
            }
        }
        public virtual IList<Note> TASNotes
        {
            get
            {
                return this.Notes.Where(x => x.Type == "tas").ToList();
            }
        }
        public virtual IList<Note> SystemNotes
        {
            get
            {
                return this.Notes.Where(x => x.Type == "system").ToList();
            }
        }

        public virtual IList<TestTime> TestTimes { get; set; }
        public virtual IList<TestTime> QuizTimes 
        {
            get
            {
                return this.TestTimes.Where(x => x.Type == "quiz").ToList();
            }
        }
        public virtual IList<TestTime> ExamTimes
        {
            get
            {
                return this.TestTimes.Where(x => x.Type == "exam").ToList();
            }
        }
        public virtual TestTime FinalExamTime
        {
            get
            {
                return this.TestTimes.Where(x => x.Type == "final").SingleOrDefault();
            }
        }

        public virtual IList<Accomodation> Accomodations { get; set; }
        public virtual IList<Accomodation> GeneralAccomodations
        {
            get
            {
                return this.Accomodations.Where(x => x.Type == "general").ToList();
            }
        }
        public virtual IList<Accomodation> QuizAccomodations
        {
            get
            {
                return this.Accomodations.Where(x => x.Type == "quiz").ToList();
            }
        }
        public virtual IList<Accomodation> ExamAccomodations
        {
            get
            {
                return this.Accomodations.Where(x => x.Type == "exam").ToList();
            }
        }
        public virtual IList<Accomodation> FinalExamAccomodations
        {
            get
            {
                return this.Accomodations.Where(x => x.Type == "final").ToList();
            }
        }

        public FormEntry()
        {
            this.Accomodations = new List<Accomodation>();
            this.TestTimes = new List<TestTime>();
            this.Notes = new List<Note>();
        }
    }
}
