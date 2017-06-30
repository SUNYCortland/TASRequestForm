using FluentNHibernate.Cfg.Db;
using NHibernate.Dialect;
using NHibernate.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TASRequestForm.Data.Repositories.Nh
{
    public class NhOracleConfiguration : PersistenceConfiguration<NhOracleConfiguration, OdbcConnectionStringBuilder>
    {
        protected NhOracleConfiguration()
        {
            Driver<OdbcDriver>();
        }

        public static NhOracleConfiguration Dialect
        {
            get
            {
                return new NhOracleConfiguration().Dialect<Oracle10gDialect>();
            }
        }
    }
}
