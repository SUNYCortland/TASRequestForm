using Castle.MicroKernel.Facilities;
using Castle.MicroKernel.Registration;
using FluentNHibernate.Cfg;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TASRequestForm.Data.Repositories.Nh;
using TASRequestForm.Data.Repositories.Nh.Mappings;

namespace TASRequestForm.Dependency
{
    public class PersistenceFacility : AbstractFacility
    {

        protected override void Init()
        {
            Kernel.Register(
                //Nhibernate session factory
                Component.For<ISessionFactory>().UsingFactoryMethod(CreateNhSessionFactory).LifeStyle.Singleton,

                //Nhibernate session
                Component.For<ISession>().UsingFactoryMethod(kernel => kernel.Resolve<ISessionFactory>().OpenSession()).LifeStyle.PerWebRequest
            );
        }

        /// <summary>
        /// Creates NHibernate Session Factory.
        /// </summary>
        /// <returns>NHibernate Session Factory</returns>
        private static ISessionFactory CreateNhSessionFactory()
        {
            return Fluently.Configure()
                .Database(NhOracleConfiguration.Dialect.ConnectionString(x => x.FromConnectionStringWithKey("DBConnect")))
                .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetAssembly(typeof(FormEntryMapping))))
                .BuildSessionFactory();
        }
    }
}
