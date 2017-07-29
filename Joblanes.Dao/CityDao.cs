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
    
    public interface ICityDao : IBaseDao<City, long>
    {
        #region Operational Function

        #endregion

        #region Single Instances Loading Function

        #endregion

        #region List Loading Function
        List<City> LoadCity(int start, int length, string orderBy, string orderDir, string name, string shortName, long region, long country, long state, int status);

        List<City> LoadCity(long region, long country, long state, int status);         
        #endregion

        #region Others Function
        int CityRowCount(string name,  string shortName, long region, long country, long state, int status);
        bool CheckDuplicateFields(long id, string name);
        #endregion
    }
    public class CityDao : BaseDao<City, long>, ICityDao
    {
        #region Propertise & Object Initialization

        #endregion

        #region Operational Function

        #endregion

        #region Single Instances Loading Function

        #endregion

        #region List Loading Function
        public List<City> LoadCity(int start, int length, string orderBy, string orderDir, string name, string shortName, long region, long country, long state, int status)
        {
            ICriteria criteria = CityCriteria(name, shortName, region, country, state, status);
            if (!String.IsNullOrEmpty(orderBy))
            {
                criteria.AddOrder(orderDir == "ASC" ? Order.Asc(orderBy.Trim()) : Order.Desc(orderBy.Trim()));
            }

            return (List<City>)criteria.SetFirstResult(start).SetMaxResults(length).List<City>();
        }

        public List<City> LoadCity(long region, long country, long state, int status)
        {
            ICriteria criteria = Session.CreateCriteria<City>().Add(Restrictions.Not(Restrictions.Eq("Status", City.EntityStatus.Delete)));

           
            criteria.CreateAlias("Country", "c", JoinType.InnerJoin, Restrictions.Eq("c.Status", Country.EntityStatus.Active));
            criteria.CreateAlias("c.Region", "r", JoinType.InnerJoin, Restrictions.Eq("r.Status", Region.EntityStatus.Active));


            if (status != 0)
                criteria.Add(Restrictions.Eq("Status", status));
            if (region > 0)
                criteria.Add(Restrictions.Eq("r.Id", region));
            if (country > 0)
                criteria.Add(Restrictions.Eq("c.Id", country));
            if (state > 0)
            {
                criteria.CreateAlias("State", "s", JoinType.InnerJoin, Restrictions.Eq("s.Status", State.EntityStatus.Active));
                criteria.Add(Restrictions.Eq("s.Id", state));
            }
                

            return criteria.List<City>().OrderBy(x => x.Name).ToList();
        }

        #endregion

        #region Others Function
        public int CityRowCount(string name,  string shortName, long region, long country, long state, int status)
        {
            ICriteria criteria = CityCriteria(name, shortName, region, country, state, status);

            criteria.SetProjection(Projections.RowCount());

            return Convert.ToInt32(criteria.UniqueResult());
        }
        public bool CheckDuplicateFields(long id, string name)
        {
            var cityQueryOver = Session.QueryOver<City>().Where(x => x.Status != City.EntityStatus.Delete); ;
            if (id > 0)
            {
                cityQueryOver = cityQueryOver.Where(x => x.Id != id);
            }

            if (!string.IsNullOrEmpty(name.Trim()))
            {
                cityQueryOver = cityQueryOver.Where(x => x.Name == name.ToString());
            }

            var rowcount = cityQueryOver.RowCount();
            return rowcount > 0;
        }
        #endregion

        #region Helper Function
        private ICriteria CityCriteria(string name, string shortName, long region, long country, long state, int status)
        {
            ICriteria criteria = Session.CreateCriteria<City>().Add(Restrictions.Not(Restrictions.Eq("Status", City.EntityStatus.Delete)));

            
            criteria.CreateAlias("Country", "c", JoinType.InnerJoin, Restrictions.Eq("c.Status", Country.EntityStatus.Active));
            criteria.CreateAlias("c.Region", "r", JoinType.InnerJoin, Restrictions.Eq("r.Status", Region.EntityStatus.Active));


            if (status != 0)
                criteria.Add(Restrictions.Eq("Status", status));

            if (!String.IsNullOrEmpty(name))
                criteria.Add(Restrictions.Like("Name", name, MatchMode.Anywhere));
            if (!String.IsNullOrEmpty(shortName))
                criteria.Add(Restrictions.Like("ShortName", shortName, MatchMode.Anywhere));
            if (region > 0)
                criteria.Add(Restrictions.Eq("r.Id", region));
            if (country > 0)
                criteria.Add(Restrictions.Eq("c.Id", country));
            if (state > 0)
            {
                criteria.CreateAlias("State", "s", JoinType.InnerJoin, Restrictions.Eq("s.Status", State.EntityStatus.Active));
                criteria.Add(Restrictions.Eq("s.Id", state));
            }
               

            return criteria;
        }
        #endregion
    }  
}
