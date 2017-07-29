using Model.JobLanes.Entity;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joblanes.Services.Test.ObjectFactory.EmployeeFactory
{
    
    public class CompanyFactory : ObjectFactoryBase<Company>
    {
        #region Object Initialization
        public CompanyFactory(IObjectFactoryBase caller, ISession session)
            : base(caller, session)
        {

        }
        #endregion

        #region Create
        #endregion

        #region Others
        #endregion

        #region CleanUp
        public void Cleanup()
        {
            DeleteAll();

        }
        #endregion
    }
}
