using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASRequestForm.Core.Data.Entities;

namespace TASRequestForm.Data.Repositories.Nh.Mappings
{
    public class NoteMapping : ClassMap<Note>
    {
        public NoteMapping()
        {
            Table("tas_request_notes");
            Id(x => x.Id).Column("note_id").GeneratedBy.Sequence("seq_tas_request_notes_id");
            Map(x => x.Value).Column("note_value");
            Map(x => x.AddedDate).Column("note_added_date");
            Map(x => x.AddedByPidm).Column("note_added_by_pidm");
            Map(x => x.Type).Column("note_type");
            References(x => x.FormEntry).Column("note_request_id");
            References(x => x.AddedByIdentity)
                .Column("note_added_by_pidm")
                .PropertyRef(x => x.Pidm)
                .ReadOnly()
                .Fetch.Join();
        }
    }
}
