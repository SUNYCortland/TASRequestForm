using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASRequestForm.Core.Data.Entities;

namespace TASRequestForm.Data.Repositories.Nh.Mappings
{
    public class NetIdMapping : ClassMap<NetId>
    {
        public NetIdMapping()
        {
            ReadOnly();
            Table("gobtpac");
            Id(x => x.Id).Column("gobtpac_pidm");
            Map(x => x.Pidm).Column("gobtpac_pidm");
            Map(x => x.Value).Column("gobtpac_external_user");
        }
    }
}
