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


    public interface IRegionDao : IBaseDao<Region, long>
    {
        #region Operational Function

        #endregion

        #region Single Instances Loading Function

        #endregion

        #region List Loading Function
        List<Region> LoadRegion(int start, int length, string orderBy, string orderDir, string name, int status);
        #endregion

        #region Others Function
        int RegionRowCount(string name, int status);
        bool CheckDuplicateFields(long id, string name);
        #endregion


    }
    public class RegionDao : BaseDao<Region, long>, IRegionDao
    {
        #region Propertise & Object Initialization

        #endregion

        #region Operational Function

        #endregion

        #region Single Instances Loading Function

        #endregion

        #region List Loading Function
        public List<Region> LoadRegion(int start, int length, string orderBy, string orderDir, string name, int status)
        {                 
            ICriteria criteria = RegionCriteria(name, status);
            if (!String.IsNullOrEmpty(orderBy))
            {
                criteria.AddOrder(orderDir == "ASC" ? Order.Asc(orderBy.Trim()) : Order.Desc(orderBy.Trim()));
            }
          
            return (List<Region>)criteria.SetFirstResult(start).SetMaxResults(length).List<Region>();         
        }



        #endregion

        #region Others Function
        public int RegionRowCount(string name, int status)
        {
            ICriteria criteria = RegionCriteria(name, status);

            criteria.SetProjection(Projections.RowCount());

            return Convert.ToInt32(criteria.UniqueResult());
        }

        public bool CheckDuplicateFields(long id, string name)
        {
            var regionQueryOver = Session.QueryOver<Region>().Where(x => x.Status != Region.EntityStatus.Delete); ;
            if (id > 0)
            {
                regionQueryOver = regionQueryOver.Where(x => x.Id != id);
            }

            if (!string.IsNullOrEmpty(name.Trim()))
            {
                regionQueryOver = regionQueryOver.Where(x => x.Name == name.ToString());
            }

            var rowcount = regionQueryOver.RowCount();
            return rowcount > 0;
        }

        #endregion

        #region Helper Function
        private ICriteria RegionCriteria(string name, int status)
        {
            ICriteria criteria = Session.CreateCriteria<Region>().Add(Restrictions.Not(Restrictions.Eq("Status", Region.EntityStatus.Delete)));

            if (status != 0)
                criteria.Add(Restrictions.Eq("Status", status));

            if (!String.IsNullOrEmpty(name))
                criteria.Add(Restrictions.Like("Name", name, MatchMode.Anywhere));

            return criteria;
        }
        #endregion
    }
}
