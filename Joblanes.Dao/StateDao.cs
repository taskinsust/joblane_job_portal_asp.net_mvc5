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
    
    public interface IStateDao : IBaseDao<State, long>
    {
        #region Operational Function

        #endregion

        #region Single Instances Loading Function

        #endregion

        #region List Loading Function
        List<State> LoadState(int start, int length, string orderBy, string orderDir, string name, string shortName, long region, long country, int status);
        List<State> LoadState(long region, long country, int status);       
        
        #endregion

        #region Others Function
        int StateRowCount(string name, string shortName, long region, long country, int status);
        bool CheckDuplicateFields(long id, string name);
        #endregion
    }
    public class StateDao : BaseDao<State, long>, IStateDao
    {
        #region Propertise & Object Initialization

        #endregion

        #region Operational Function

        #endregion

        #region Single Instances Loading Function

        #endregion

        #region List Loading Function
        public List<State> LoadState(int start, int length, string orderBy, string orderDir, string name, string shortName, long region, long country, int status)
        {
            ICriteria criteria = StateCriteria(name,   shortName,  region,  country, status);
            if (!String.IsNullOrEmpty(orderBy))
            {
                criteria.AddOrder(orderDir == "ASC" ? Order.Asc(orderBy.Trim()) : Order.Desc(orderBy.Trim()));
            }

            return (List<State>)criteria.SetFirstResult(start).SetMaxResults(length).List<State>();
        }

        public List<State> LoadState(long region, long country, int status)
        {
            ICriteria criteria = Session.CreateCriteria<State>().Add(Restrictions.Not(Restrictions.Eq("Status", State.EntityStatus.Delete)));

            criteria.CreateAlias("Country", "c", JoinType.InnerJoin, Restrictions.Eq("c.Status", Country.EntityStatus.Active));
            criteria.CreateAlias("c.Region", "r", JoinType.InnerJoin, Restrictions.Eq("r.Status", Region.EntityStatus.Active));
            if (status != 0)
                criteria.Add(Restrictions.Eq("Status", status));

            if (region > 0)
                criteria.Add(Restrictions.Eq("r.Id", region));
            if (country > 0)
                criteria.Add(Restrictions.Eq("c.Id", country));

            return criteria.List<State>().OrderBy(x => x.Name).ToList();
        }

        #endregion

        #region Others Function
        public int StateRowCount(string name, string shortName, long region, long country, int status)
        {
            ICriteria criteria = StateCriteria(name,   shortName,  region,  country, status);

            criteria.SetProjection(Projections.RowCount());

            return Convert.ToInt32(criteria.UniqueResult());
        }
        public bool CheckDuplicateFields(long id, string name)
        {
            var stateQueryOver = Session.QueryOver<State>().Where(x => x.Status != State.EntityStatus.Delete); ;
            if (id > 0)
            {
                stateQueryOver = stateQueryOver.Where(x => x.Id != id);
            }

            if (!string.IsNullOrEmpty(name.Trim()))
            {
                stateQueryOver = stateQueryOver.Where(x => x.Name == name.ToString());
            }

            var rowcount = stateQueryOver.RowCount();
            return rowcount > 0;
        }
        #endregion

        #region Helper Function
        private ICriteria StateCriteria(string name, string shortName, long region, long country, int status)
        {
            ICriteria criteria = Session.CreateCriteria<State>().Add(Restrictions.Not(Restrictions.Eq("Status", State.EntityStatus.Delete)));

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
            return criteria;
        }
        #endregion
    }  
}
