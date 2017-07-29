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

    public interface IJobCategoryDao : IBaseDao<JobCategory, long>
    {
        #region Operational Function

        #endregion

        #region Single Instances Loading Function

        #endregion

        #region List Loading Function
        List<JobCategory> LoadJobCategory(int start, int length, string orderBy, string orderDir, string name, int status);
        #endregion

        #region Others Function
        int JobCategoryRowCount(string name, int status);
        bool CheckDuplicateFields(long id, string name);
        #endregion
    }
    public class JobCategoryDao : BaseDao<JobCategory, long>, IJobCategoryDao
    {
        #region Propertise & Object Initialization

        #endregion

        #region Operational Function

        #endregion

        #region Single Instances Loading Function

        #endregion

        #region List Loading Function
        public List<JobCategory> LoadJobCategory(int start, int length, string orderBy, string orderDir, string name, int status)
        {

            ICriteria criteria = JobCategoryCriteria(name, status);

            if (!String.IsNullOrEmpty(orderBy))
            {
                criteria.AddOrder(orderDir == "ASC" ? Order.Asc(orderBy.Trim()) : Order.Desc(orderBy.Trim()));
            }
            return (List<JobCategory>)criteria.SetFirstResult(start).SetMaxResults(length).List<JobCategory>();
        }



        #endregion

        #region Others Function
        public int JobCategoryRowCount(string name, int status)
        {
            ICriteria criteria = JobCategoryCriteria(name, status);

            criteria.SetProjection(Projections.RowCount());

            return Convert.ToInt32(criteria.UniqueResult());
        }

        public bool CheckDuplicateFields(long id, string name)
        {
            var jobCategoryQueryOver = Session.QueryOver<JobCategory>().Where(x => x.Status != JobCategory.EntityStatus.Delete); ;
            if (id > 0)
            {
                jobCategoryQueryOver = jobCategoryQueryOver.Where(x => x.Id != id);
            }

            if (!string.IsNullOrEmpty(name.Trim()))
            {
                jobCategoryQueryOver = jobCategoryQueryOver.Where(x => x.Name == name.ToString());
            }

            var rowcount = jobCategoryQueryOver.RowCount();
            return rowcount > 0;
        }

        #endregion

        #region Helper Function
        private ICriteria JobCategoryCriteria(string name, int status)
        {
            ICriteria criteria = Session.CreateCriteria<JobCategory>().Add(Restrictions.Not(Restrictions.Eq("Status", JobCategory.EntityStatus.Delete)));

            if (status != 0)
                criteria.Add(Restrictions.Eq("Status", status));

            if (!String.IsNullOrEmpty(name))
                criteria.Add(Restrictions.Like("Name", name, MatchMode.Anywhere));

            return criteria;
        }
        #endregion
    }  
}
