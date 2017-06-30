using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASRequestForm.Core.Data.Entities;
using TASRequestForm.Core.Data.Repositories;

namespace TASRequestForm.Data.Repositories.Nh
{
    public class NhTestTimeRepository : NhRepositoryBase<int?, TestTime>, ITestTimeRepository
    {
        public NhTestTimeRepository(ISession _session)
            : base(_session)
        {

        }
    }
}
