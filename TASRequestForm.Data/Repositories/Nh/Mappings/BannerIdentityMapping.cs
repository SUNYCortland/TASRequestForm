using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASRequestForm.Core.Data.Entities;

namespace TASRequestForm.Data.Repositories.Nh.Mappings
{
    public class BannerIdentityMapping : ClassMap<BannerIdentity>
    {
        public BannerIdentityMapping()
        {
            ReadOnly();
            Where("spriden_change_ind IS NULL");
            Table("spriden");
            Id(x => x.Id).Column("spriden_pidm");
            Map(x => x.Pidm).Column("spriden_pidm");
            Map(x => x.CNumber).Column("spriden_id");
            Map(x => x.FirstName).Column("spriden_first_name");
            Map(x => x.LastName).Column("spriden_last_name");
            References(x => x.NetId)
                .Column("spriden_pidm")
                .PropertyRef(x => x.Pidm)
                .Not.LazyLoad()
                .Not.Update()
                .ReadOnly()
                .Fetch.Join();
        }
    }
}
