using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes.Base;
using Model.JobLanes.Dto;
using Model.JobLanes.Entity;
using NHibernate;
using NHibernate.Criterion;

namespace Dao.Joblanes
{
   
    public interface ICompanyTypeDao : IBaseDao<CompanyType, long>
    {
        #region Operational Function

        #endregion

        #region Single Instances Loading Function

        #endregion

        #region List Loading Function 
        List<CompanyType> LoadCompanyType(int start, int length, string orderBy, string orderDir, string name, int status);
        #endregion

        #region Others Function
        int CompanyTypeRowCount(string name, int status); 
        bool CheckDuplicateFields(long id, string name);
        #endregion
    }
    public class CompanyTypeDao : BaseDao<CompanyType, long>, ICompanyTypeDao
    {
        #region Propertise & Object Initialization

        #endregion

        #region Operational Function

        #endregion

        #region Single Instances Loading Function

        #endregion

        #region List Loading Function
        public List<CompanyType> LoadCompanyType(int start, int length, string orderBy, string orderDir, string name, int status)
        {

            ICriteria criteria = CompanyTypeCriteria(name, status);
         
            if (!String.IsNullOrEmpty(orderBy))
            {
                criteria.AddOrder(orderDir == "ASC" ? Order.Asc(orderBy.Trim()) : Order.Desc(orderBy.Trim()));
            }
            return (List<CompanyType>)criteria.SetFirstResult(start).SetMaxResults(length).List<CompanyType>();      
        }



        #endregion

        #region Others Function
        public int CompanyTypeRowCount(string name, int status)
        {
            ICriteria criteria = CompanyTypeCriteria(name, status);

            criteria.SetProjection(Projections.RowCount());

            return Convert.ToInt32(criteria.UniqueResult());
        }

        public bool CheckDuplicateFields(long id, string name)
        {
            var companyTypeQueryOver = Session.QueryOver<CompanyType>().Where(x => x.Status != CompanyType.EntityStatus.Delete); ;
            if (id > 0)
            {
                companyTypeQueryOver = companyTypeQueryOver.Where(x => x.Id != id);
            }

            if (!string.IsNullOrEmpty(name.Trim()))
            {
                companyTypeQueryOver = companyTypeQueryOver.Where(x => x.Name == name.ToString());
            }

            var rowcount = companyTypeQueryOver.RowCount();
            return rowcount > 0;
        }

        #endregion

        #region Helper Function
        private ICriteria CompanyTypeCriteria(string name, int status)
        {
            ICriteria criteria = Session.CreateCriteria<CompanyType>().Add(Restrictions.Not(Restrictions.Eq("Status", CompanyType.EntityStatus.Delete)));

            if (status != 0)
                criteria.Add(Restrictions.Eq("Status", status));

            if (!String.IsNullOrEmpty(name))
                criteria.Add(Restrictions.Like("Name", name, MatchMode.Anywhere));

            return criteria;
        }
        #endregion
    }  
}
