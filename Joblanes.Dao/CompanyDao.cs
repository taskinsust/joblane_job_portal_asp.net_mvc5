using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes.Base;
using Model.JobLanes.Entity;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;

namespace Dao.Joblanes
{

    public interface ICompanyDao : IBaseDao<Company, long>
    {
        #region Operational Function

        #endregion

        #region Single Instances Loading Function
        Company GetCompany(long userProfileId);
        Company GetCompany(long companyId, int status);
        #endregion

        #region List Loading Function
        IList<Company> LoadCompany(int status, long companyType, string zip, long region, long country, long state, long city);
        IList<Company> LoadOnlyCompanyByStatusAndType(int status, long companyType);
        IList<Company> LoadCompanyForWebAdmin(int start, int length, string orderBy, string orderDir, int status, long companyType, string companyName, string contactMobile, string contactEmail, string zip, long region, long country, long state, long city);
        #endregion

        #region Others Function
        int CompanyRowCount(int status, long companyType, string companyName, string contactMobile, string contactEmail, string zip, long region, long country, long state, long city);

        bool IsDuplicateCompany(out string fieldName, string contactEmail = null, string contactMobile = null, long id = 0);
        #endregion

       
       
    }
    public class CompanyDao : BaseDao<Company, long>, ICompanyDao
    {
        #region Propertise & Object Initialization

        #endregion

        #region Operational Function

        #endregion

        #region Single Instances Loading Function
        public Company GetCompany(long userProfileId)
        {
            return
                Session.QueryOver<Company>()
                    .Where(x => x.UserProfile.Id == userProfileId && x.Status == Company.EntityStatus.Active)
                    .SingleOrDefault<Company>();
        }

        public Company GetCompany(long companyId, int status)
        {
            return
                  Session.QueryOver<Company>()
                      .Where(x => x.Id == companyId && x.Status == status)
                      .SingleOrDefault<Company>();
        }

        #endregion

        #region List Loading Function
        public IList<Company> LoadCompany(int status, long companyType, string zip, long region, long country, long state, long city)
        {
            ICriteria criteria = Session.CreateCriteria<Company>().Add(Restrictions.Not(Restrictions.Eq("Status", Company.EntityStatus.Delete)));
            criteria.CreateAlias("CompanyType", "ct", JoinType.InnerJoin, Restrictions.Eq("ct.Status", State.EntityStatus.Active));
            criteria.CreateAlias("CompanyDetailses", "cd", JoinType.InnerJoin, Restrictions.Eq("cd.Status", State.EntityStatus.Active));
            criteria.CreateAlias("cd.Country", "c", JoinType.InnerJoin, Restrictions.Eq("c.Status", State.EntityStatus.Active));
            criteria.CreateAlias("cd.State", "s", JoinType.InnerJoin, Restrictions.Eq("s.Status", State.EntityStatus.Active));
            criteria.CreateAlias("cd.City", "city", JoinType.InnerJoin, Restrictions.Eq("city.Status", State.EntityStatus.Active));
            criteria.CreateAlias("c.Region", "r", JoinType.InnerJoin, Restrictions.Eq("r.Status", State.EntityStatus.Active));

            if (status != 0)
                criteria.Add(Restrictions.Eq("Status", status));
            if (companyType > 0)
                criteria.Add(Restrictions.Eq("ct.Id", companyType));
            if (region > 0)
                criteria.Add(Restrictions.Eq("r.Id", region));
            if (country > 0)
                criteria.Add(Restrictions.Eq("c.Id", country));
            if (state > 0)
                criteria.Add(Restrictions.Eq("ss.Id", state));
            if (city > 0)
                criteria.Add(Restrictions.Eq("city.Id", city));
            if (!string.IsNullOrEmpty(zip))
                criteria.Add(Restrictions.Eq("cd.zip", zip));

            return criteria.List<Company>().OrderBy(x => x.Name).ToList();
        }

        public IList<Company> LoadOnlyCompanyByStatusAndType(int status, long companyType)
        {
            ICriteria criteria = Session.CreateCriteria<Company>().Add(Restrictions.Not(Restrictions.Eq("Status", Company.EntityStatus.Delete)));

            criteria.CreateAlias("CompanyType", "ct", JoinType.InnerJoin, Restrictions.Eq("ct.Status", State.EntityStatus.Active));
            if (status != 0)
                criteria.Add(Restrictions.Eq("Status", status));
            if (companyType > 0)
                criteria.Add(Restrictions.Eq("ct.Id", companyType));
            return criteria.List<Company>().OrderBy(x => x.Name).ToList();
        }
        public IList<Company> LoadCompanyForWebAdmin(int start, int length, string orderBy, string orderDir, int status, long companyType,
            string companyName, string contactMobile, string contactEmail, string zip, long region, long country, long state,
            long city)
        {
            ICriteria criteria = CompanyCriteria(status, companyType, companyName, contactMobile, contactEmail, zip,
               region, country, state, city);



            if (!String.IsNullOrEmpty(orderBy))
            {
                criteria.AddOrder(orderDir == "ASC" ? Order.Asc(orderBy.Trim()) : Order.Desc(orderBy.Trim()));
            }
            return criteria.List<Company>() as List<Company>;
            // return (List<Company>)criteria.SetFirstResult(start).SetMaxResults(length).List<Company>();  
        }
        #endregion

        #region Others Function
        public bool IsDuplicateCompany(out string fieldName, string contactEmail = null, string contactMobile = null, long id = 0)
        {
            fieldName = "";
            var checkValue = false;
            ICriteria criteria = Session.CreateCriteria<Company>();
            criteria.Add(Restrictions.Not(Restrictions.Eq("Status", Company.EntityStatus.Delete)));
            if (id != 0)
            {
                criteria.Add(Restrictions.Not(Restrictions.Eq("Id", id)));
            }
            if (contactEmail != null)
            {
                criteria.Add(Restrictions.Eq("ContactEmail", contactEmail));
                fieldName = "ContactEmail";
            }
            else if (contactMobile != null)
            {
                criteria.Add(Restrictions.Eq("ContactMobile", contactMobile));
                fieldName = "ContactMobile";
            }
            var branchList = criteria.List<Company>();
            if (branchList != null && branchList.Count > 0)
            {
                checkValue = true;
            }
            return checkValue;

        }
        public int CompanyRowCount(int status, long companyType, string companyName, string contactMobile, string contactEmail,
           string zip, long region, long country, long state, long city)
        {
            ICriteria criteria = CompanyCriteria(status, companyType, companyName, contactMobile, contactEmail, zip,
                region, country, state, city);

            criteria.SetProjection(Projections.RowCount());

            return Convert.ToInt32(criteria.UniqueResult());
        }

       
        #endregion

        #region Helper Function
        private ICriteria CompanyCriteria(int status, long companyType, string companyName, string contactMobile, string contactEmail, string zip, long region, long country, long state, long city)
        {
            ICriteria criteria = Session.CreateCriteria<Company>().Add(Restrictions.Not(Restrictions.Eq("Status", Company.EntityStatus.Delete)));

            if (status != 0)
                criteria.Add(Restrictions.Eq("Status", status));
            if (companyType > 0)
            {
                criteria.CreateAlias("CompanyType", "ct", JoinType.InnerJoin, Restrictions.Eq("ct.Status", State.EntityStatus.Active));
                criteria.Add(Restrictions.Eq("ct.Id", companyType));
            }

            if (region > 0 || country > 0 || state > 0 || city > 0 || !string.IsNullOrEmpty(zip))
            {
                criteria.CreateAlias("CompanyDetailses", "cd", JoinType.InnerJoin, Restrictions.Eq("cd.Status", State.EntityStatus.Active));
                criteria.CreateAlias("cd.Country", "c", JoinType.InnerJoin, Restrictions.Eq("c.Status", State.EntityStatus.Active));
                criteria.CreateAlias("cd.State", "s", JoinType.InnerJoin, Restrictions.Eq("s.Status", State.EntityStatus.Active));
                criteria.CreateAlias("cd.City", "city", JoinType.InnerJoin, Restrictions.Eq("city.Status", State.EntityStatus.Active));
                criteria.CreateAlias("c.Region", "r", JoinType.InnerJoin, Restrictions.Eq("r.Status", State.EntityStatus.Active));
            }

            if (region > 0)
                criteria.Add(Restrictions.Eq("r.Id", region));            
            if (country > 0)
                criteria.Add(Restrictions.Eq("c.Id", country));       
            if (state > 0)
                criteria.Add(Restrictions.Eq("s.Id", state));            
            if (city > 0)
                criteria.Add(Restrictions.Eq("city.Id", city));                          
            if (!string.IsNullOrEmpty(zip))
                criteria.Add(Restrictions.Eq("cd.Zip", zip));
            if (!string.IsNullOrEmpty(contactMobile))
                criteria.Add(Restrictions.Eq("ContactMobile", contactMobile));
            if (!string.IsNullOrEmpty(contactEmail))
                criteria.Add(Restrictions.Eq("ContactEmail", contactEmail));
            if (!string.IsNullOrEmpty(companyName))
                criteria.Add(Restrictions.Eq("Name", companyName));

            return criteria;
        }

        #endregion

        
    }  
}
