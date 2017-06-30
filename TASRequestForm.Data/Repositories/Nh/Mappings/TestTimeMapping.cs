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
    public class TestTimeMapping : ClassMap<TestTime>
    {
        public TestTimeMapping()
        {
            Table("tas_test_times");
            Id(x => x.Id).Column("test_time_id").GeneratedBy.Sequence("seq_tas_test_times_id");
            Map(x => x.Type).Column("test_time_type");
            Map(x => x.Date).Column("test_time_date");
            Map(x => x.Title).Column("test_time_title");
            Map(x => x.NoShow).Column("test_time_no_show").CustomType<YesNoType>();
            Map(x => x.Canceled).Column("test_time_canceled").CustomType<YesNoType>();
            Map(x => x.CanceledDate).Column("test_time_canceled_date");
            Map(x => x.CanceledByPidm).Column("test_time_canceled_by_pidm");
            References(x => x.FormEntry).Column("test_time_request_id");
            References(x => x.CanceledByIdentity)
                .Column("test_time_canceled_by_pidm")
                .PropertyRef(x => x.Pidm)
                .ReadOnly()
                .Fetch.Join();
        }
    }
}
