﻿using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TASRequestForm.Core.Data.Entities;
using TASRequestForm.Core.Data.Repositories;

namespace TASRequestForm.Data.Repositories.Nh
{
    public class NhFormEntryRepository : NhRepositoryBase<int?, FormEntry>, IFormEntryRepository
    {
        public NhFormEntryRepository(ISession _session)
            : base(_session)
        {

        }
    }
}
