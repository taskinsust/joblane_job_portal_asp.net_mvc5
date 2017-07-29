using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes;
using NHibernate;
using Services.Joblanes.Base;

namespace Services.Joblanes
{
    public interface ICompanyDetailsService : IBaseService
    {
        #region Operational Function
        bool Save();
        #endregion

        #region List of Loading function

        #endregion

        #region Other Function

        #endregion
    }
    public class CompanyDetailsService : BaseService, ICompanyDetailsService
    {
        #region Propertise & Object Initialization

        private readonly ICompanyDetailsDao _companyDetailsDao;
        public CompanyDetailsService(ISession session)
        {
            _companyDetailsDao = new CompanyDetailsDao() { Session = session };
        }
        #endregion

        #region Operational Function
        public bool Save()
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region List of Loading function

        #endregion
        
        #region Other Function

        #endregion
    }
}
