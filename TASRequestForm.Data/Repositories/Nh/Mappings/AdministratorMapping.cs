using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASRequestForm.Core.Data.Entities;

namespace TASRequestForm.Data.Repositories.Nh.Mappings
{
    public class AdministratorMapping : ClassMap<Administrator>
    {
        public AdministratorMapping()
        {
            Table("tas_administrators");
            Id(x => x.Id).Column("administrator_id").GeneratedBy.Sequence("seq_tas_administrators_id");
            Map(x => x.Pidm).Column("administrator_pidm");
            References(x => x.AdministratorIdentity)
                .Column("administrator_pidm")
                .PropertyRef(x => x.Pidm)
                .ReadOnly()
                .Fetch.Join();
        }
    }
}
