using System.Configuration;
using Model.JobLanes.Extensions;
using Model.JobLanes.Helper;

namespace JobLanes.WcfService
{
    public class ConnectionBase
    {

        public ConnectionBase()
        {
            //for database connection string 
            DbConnectionString.MssqlConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            //DbConnectionString.MyqlConnectionString = ConfigurationManager.ConnectionStrings[""].ConnectionString;            
        }
    }
}