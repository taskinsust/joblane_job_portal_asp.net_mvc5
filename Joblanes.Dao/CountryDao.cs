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

    public interface ICountryDao : IBaseDao<Country, long>
    {
        #region Operational Function

        #endregion

        #region Single Instances Loading Function

        #endregion

        #region List Loading Function
        List<Country> LoadCountry(int start, int length, string orderBy, string orderDir, string name, string shortName, string callingCode, long region, int status);
        List<Country> LoadCountry(int status, long region);
        #endregion

        #region Others Function
        int CountryRowCount(string name, string shortName, string callingCode, long region, int status);
        bool CheckDuplicateFields(long id, string name); 
        #endregion
    }
    public class CountryDao : BaseDao<Country, long>, ICountryDao 
    {
        #region Propertise & Object Initialization

        #endregion

        #region Operational Function

        #endregion

        #region Single Instances Loading Function

        #endregion

        #region List Loading Function
        public List<Country> LoadCountry(int start, int length, string orderBy, string orderDir, string name, string shortName, string callingCode, long region, int status)
        {
            ICriteria criteria = CountryCriteria(name, shortName,  callingCode,  region, status);
            if (!String.IsNullOrEmpty(orderBy))
            {
                criteria.AddOrder(orderDir == "ASC" ? Order.Asc(orderBy.Trim()) : Order.Desc(orderBy.Trim()));
            }
            return criteria.List<Country>() as List<Country>;
            //return (List<Country>)criteria.SetFirstResult(start).SetMaxResults(length).List<Country>();  
        }

        public List<Country> LoadCountry(int status, long region)
        {
            ICriteria criteria = Session.CreateCriteria<Country>().Add(Restrictions.Not(Restrictions.Eq("Status", Country.EntityStatus.Delete)));
            criteria.CreateAlias("Region", "r", JoinType.InnerJoin, Restrictions.Eq("r.Status", Region.EntityStatus.Active));
            if (status != 0)
                criteria.Add(Restrictions.Eq("Status", status));

            if (region > 0)
                criteria.Add(Restrictions.Eq("r.Id", region));

               // criteria.CreateAlias("Region", "rg").Add(Restrictions.Eq("rg.Status", Region.EntityStatus.Active));
               // criteria.Add(Restrictions.In("rg.Id", regionList));
           
            return criteria.List<Country>().OrderBy(x => x.Name).ToList();
             
        }

        #endregion

        #region Others Function
        public int CountryRowCount(string name, string shortName, string callingCode, long region, int status)
        {
            ICriteria criteria = CountryCriteria(name,  shortName,  callingCode,  region,status);

            criteria.SetProjection(Projections.RowCount());

            return Convert.ToInt32(criteria.UniqueResult());
        }
        public bool CheckDuplicateFields(long id, string name)
        {
            var countryQueryOver = Session.QueryOver<Country>().Where(x => x.Status != Country.EntityStatus.Delete); ;
            if (id > 0)
            {
                countryQueryOver = countryQueryOver.Where(x => x.Id != id);
            }

            if (!string.IsNullOrEmpty(name.Trim()))
            {
                countryQueryOver = countryQueryOver.Where(x => x.Name == name.ToString());
            }

            var rowcount = countryQueryOver.RowCount();
            return rowcount > 0;
        }
        #endregion

        #region Helper Function 
        private ICriteria CountryCriteria(string name, string shortName, string callingCode, long region, int status)
        {
            ICriteria criteria = Session.CreateCriteria<Country>().Add(Restrictions.Not(Restrictions.Eq("Status", Country.EntityStatus.Delete)));

            if (status != 0)
                criteria.Add(Restrictions.Eq("Status", status));
            criteria.CreateAlias("Region", "r", JoinType.InnerJoin, Restrictions.Eq("r.Status", Region.EntityStatus.Active));

            if (!String.IsNullOrEmpty(name))
                criteria.Add(Restrictions.Like("Name", name, MatchMode.Anywhere));
            if (!String.IsNullOrEmpty(shortName))
                criteria.Add(Restrictions.Like("ShortName", shortName, MatchMode.Anywhere));
            if (!String.IsNullOrEmpty(callingCode))
                criteria.Add(Restrictions.Like("CallingCode", callingCode, MatchMode.Anywhere));
            if (region>0)
                criteria.Add(Restrictions.Eq("r.Id", region));

            return criteria;
        }
        #endregion
        

        
    }  
}
