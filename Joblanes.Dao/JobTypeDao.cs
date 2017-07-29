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

    public interface IJobTypeDao : IBaseDao<JobType, long>
    {
        #region Operational Function

        #endregion

        #region Single Instances Loading Function

        #endregion

        #region List Loading Function
        List<JobType> LoadJobType(int start, int length, string orderBy, string orderDir, string name, int status);
        #endregion

        #region Others Function
        int JobTypeRowCount(string name, int status);
        bool CheckDuplicateFields(long id, string name);
        #endregion
    }
    public class JobTypeDao : BaseDao<JobType, long>, IJobTypeDao 
    {
        #region Propertise & Object Initialization

        #endregion

        #region Operational Function

        #endregion

        #region Single Instances Loading Function

        #endregion

        #region List Loading Function
        public List<JobType> LoadJobType(int start, int length, string orderBy, string orderDir, string name, int status)
        {

            ICriteria criteria = JobTypeCriteria(name, status);

            if (!String.IsNullOrEmpty(orderBy))
            {
                criteria.AddOrder(orderDir == "ASC" ? Order.Asc(orderBy.Trim()) : Order.Desc(orderBy.Trim()));
            }

            return (List<JobType>)criteria.SetFirstResult(start).SetMaxResults(length).List<JobType>();         
        }



        #endregion

        #region Others Function
        public int JobTypeRowCount(string name, int status)
        {
            ICriteria criteria = JobTypeCriteria(name, status);

            criteria.SetProjection(Projections.RowCount());

            return Convert.ToInt32(criteria.UniqueResult());
        }

        public bool CheckDuplicateFields(long id, string name)
        {
            var jobTypeQueryOver = Session.QueryOver<JobType>().Where(x => x.Status != JobType.EntityStatus.Delete); ;
            if (id > 0)
            {
                jobTypeQueryOver = jobTypeQueryOver.Where(x => x.Id != id);
            }

            if (!string.IsNullOrEmpty(name.Trim()))
            {
                jobTypeQueryOver = jobTypeQueryOver.Where(x => x.Name == name.ToString());
            }

            var rowcount = jobTypeQueryOver.RowCount();
            return rowcount > 0;
        }

        #endregion

        #region Helper Function
        private ICriteria JobTypeCriteria(string name, int status)
        {
            ICriteria criteria = Session.CreateCriteria<JobType>().Add(Restrictions.Not(Restrictions.Eq("Status", JobType.EntityStatus.Delete)));

            if (status != 0)
                criteria.Add(Restrictions.Eq("Status", status));

            if (!String.IsNullOrEmpty(name))
                criteria.Add(Restrictions.Like("Name", name, MatchMode.Anywhere));

            return criteria;
        }
        #endregion
    }  
}
