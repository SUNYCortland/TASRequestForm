using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASRequestForm.Core.Data.Entities;

namespace TASRequestForm.Data.Repositories.Nh.Mappings
{
    public class AccomodationMapping : ClassMap<Accomodation>
    {
        public AccomodationMapping()
        {
            Table("tas_accomodations");
            Id(x => x.Id).Column("accomodation_id").GeneratedBy.Sequence("seq_tas_accomodations_id");
            Map(x => x.Type).Column("accomodation_type");
            Map(x => x.Value).Column("accomodation_value");
            References(x => x.FormEntry).Column("accomodation_request_id");
        }
    }
}
