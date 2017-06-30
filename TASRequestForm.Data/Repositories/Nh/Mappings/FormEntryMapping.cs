using FluentNHibernate.Mapping;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASRequestForm.Core.Data.Entities;

namespace TASRequestForm.Data.Repositories.Nh.Mappings
{
    public class FormEntryMapping : ClassMap<FormEntry>
    {
        public FormEntryMapping()
        {
            Table("tas_requests");
            Id(x => x.Id).Column("request_id").GeneratedBy.Sequence("seq_tas_requests_id");
            Map(x => x.StudentPidm).Column("request_student_pidm");
            Map(x => x.PhoneNumber).Column("request_phone_number");
            Map(x => x.ProfessorPidm).Column("request_professor_pidm");
            Map(x => x.DeliveryType).Column("request_delivery_type");
            Map(x => x.DateSubmitted).Column("request_date_submitted");
            Map(x => x.ReturnType).Column("request_return_type");
            Map(x => x.Course).Column("request_course");
            Map(x => x.CourseCRN).Column("request_course_crn");
            Map(x => x.CourseTime).Column("request_course_time");
            Map(x => x.ProfessorEmail).Column("request_professor_email");
            Map(x => x.ProfessorCampusAddress).Column("request_professor_campus_addr");
            Map(x => x.ProfessorApprovedDate).Column("request_prof_approved_date");
            Map(x => x.ProfessorReachedBy).Column("request_professor_reached_by");
            Map(x => x.ProfessorApproved).Column("request_professor_approved").CustomType<YesNoType>();
            Map(x => x.TASApproved).Column("request_tas_approved").CustomType<YesNoType>();
            Map(x => x.ApprovedDate).Column("request_approved_date");
            Map(x => x.ApprovedByPidm).Column("request_approved_by_pidm");
            Map(x => x.ReminderDays).Column("request_reminder_days");
            Map(x => x.Archived).Column("request_archived").CustomType<YesNoType>();
            HasMany(x => x.Accomodations)
                .Table("tas_accomodations")
                .KeyColumn("accomodation_request_id")
                .OrderBy("accomodation_type")
                .Cascade.AllDeleteOrphan();
            HasMany(x => x.TestTimes)
                .Table("tas_test_times")
                .KeyColumn("test_time_request_id")
                .OrderBy("test_time_type")
                .Cascade.AllDeleteOrphan();
            HasMany(x => x.Notes)
                .Table("tas_request_notes")
                .KeyColumn("note_request_id")
                .OrderBy("note_type")
                .Cascade.AllDeleteOrphan();
            References(x => x.ProfessorIdentity)
                .Column("request_professor_pidm")
                .PropertyRef(x => x.Pidm)
                .ReadOnly()
                .Fetch.Join();
            References(x => x.StudentIdentity)
                .Column("request_student_pidm")
                .PropertyRef(x => x.Pidm)
                .ReadOnly()
                .Fetch.Join();
            References(x => x.ApprovedByIdentity)
                .Column("request_approved_by_pidm")
                .PropertyRef(x => x.Pidm)
                .ReadOnly()
                .Fetch.Join();
        }
    }
}
