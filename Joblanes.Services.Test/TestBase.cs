using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.JobLanes.Extensions;
using NHibernate;
using Joblanes.Services;
using Services.Joblanes.Helper;

namespace Joblanes.Services.Test
{
    public class TestBase:IDisposable
    {
        public ISession session;
        public TestBase()

        {
            DbConnectionString.MssqlConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            session = NhSessionFactory.OpenSession();
        }


        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
