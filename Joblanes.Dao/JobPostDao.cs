using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes.Base;
using FluentNHibernate.Utils;
using Model.JobLanes.Dto;
using Model.JobLanes.Entity;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;

namespace Dao.Joblanes
{
    public interface IJobPostDao : IBaseDao<JobPost, long>
    {
        #region Operational Function

        #endregion

        #region Single Instances Loading Function
        JobPost GetJobPost(long id, long company);
        #endregion

        #region List Loading Function

        IList<JobPost> LoadAll();
        IList<JobPost> LoadAll(List<long> jobPostIds);
        IList<JobPost> LoadJobPost(int start, int length, string orderBy, string orderDir, long company, long jobCategory, long jobType, string jobTitle, int status);
        IList<JobSeekerJobPost> LoadJobApplication(int start, int length, string orderBy, string orderDir, long jobPostId, long companyId, DateTime? deadLineFrom, DateTime? deadlineTo, int deadlineFlag, bool isShortListed, string jobTitle);
        IList<JobPost> LoadJobPost(int start, int length, string orderBy, string orderDir, string what = "", string where = "", int ageRangeFrom = 0,
            int ageRangeTo = 0, int expRangeFrom = 0, int expRangeTo = 0, int salaryRangeFrom = 0, int salaryRangeTo = 0,
            long[] company = null, long[] jobCategory = null, long[] jobType = null);

        #endregion

        #region Others Function
        int JobPostRowCount(long company, long jobCategory, long jobType, string jobTitle, int status);
        int JobApplicationRowCount(long jobPostId, long companyId, DateTime? deadLineFrom, DateTime? deadlineTo, int deadlineFlag, bool isShortListed, string jobTitle);
        int CountJobPost(string what = "", string where = "", int ageRangeFrom = 0,
             int ageRangeTo = 0, int expRangeFrom = 0, int expRangeTo = 0, int salaryRangeFrom = 0, int salaryRangeTo = 0,
             long[] company = null, long[] jobCategory = null, long[] jobType = null);
        #endregion

    }
    public class JobPostDao : BaseDao<JobPost, long>, IJobPostDao
    {
        #region Propertise & Object Initialization

        #endregion

        #region Operational Function

        #endregion

        #region Single Instances Loading Function
        public JobPost GetJobPost(long id, long company)
        {
            ICriteria criteria = Session.CreateCriteria<JobPost>().Add(Restrictions.Not(Restrictions.Eq("Status", Country.EntityStatus.Delete)));
            criteria.Add(Restrictions.Eq("Id", id));

            if (company > 0)
            {
                criteria.CreateAlias("Company", "c", JoinType.InnerJoin, Restrictions.Eq("c.Status", Company.EntityStatus.Active));
                criteria.Add(Restrictions.Eq("c.Id", company));
            }
            return criteria.SetMaxResults(1).UniqueResult<JobPost>();
        }
        #endregion

        #region List Loading Function



        public IList<JobPost> LoadAll()
        {
            return Session.QueryOver<JobPost>().List<JobPost>();
        }

        public IList<JobPost> LoadAll(List<long> jobPostIds)
        {
            return Session.QueryOver<JobPost>().Where(x => x.Id.IsIn(jobPostIds)).List<JobPost>();
        }

        public IList<JobPost> LoadJobPost(int start, int length, string orderBy, string orderDir, long company, long jobCategory, long jobType, string jobTitle, int status)
        {
            ICriteria criteria = JobCriteria(company, jobCategory, jobType, jobTitle, status);
            if (!String.IsNullOrEmpty(orderBy))
            {
                criteria.AddOrder(orderDir == "ASC" ? Order.Asc(orderBy.Trim()) : Order.Desc(orderBy.Trim()));
            }
            //return criteria.List<JobPost>() as List<JobPost>;
            return (List<JobPost>)criteria.SetFirstResult(start).SetMaxResults(length).List<JobPost>();
        }

        public IList<JobSeekerJobPost> LoadJobApplication(int start, int length, string orderBy, string orderDir, long jobPostId, long companyId,
            DateTime? deadLineFrom, DateTime? deadlineTo, int deadlineFlag, bool isShortListed, string jobTitle)
        {
            ICriteria criteria = JobApplicationCriteria(jobPostId, companyId, deadLineFrom, deadlineTo, deadlineFlag, isShortListed, jobTitle);
            if (!String.IsNullOrEmpty(orderBy))
            {
                criteria.AddOrder(orderDir == "ASC" ? Order.Asc(orderBy.Trim()) : Order.Desc(orderBy.Trim()));
            }
            return criteria.List<JobSeekerJobPost>() as List<JobSeekerJobPost>;
            //return (List<JobSeekerJobPost>)criteria.SetFirstResult(start).SetMaxResults(length).List<JobSeekerJobPost>();
        }

        public IList<JobPost> LoadJobPost(int start, int length, string orderBy, string orderDir, string what = "", string where = "",
            int ageRangeFrom = 0, int ageRangeTo = 0, int expRangeFrom = 0, int expRangeTo = 0, int salaryRangeFrom = 0,
            int salaryRangeTo = 0, long[] company = null, long[] jobCategory = null, long[] jobType = null)
        {
            ICriteria criteria = JobSearchCriteria(what, where,
             ageRangeFrom, ageRangeTo, expRangeFrom, expRangeTo, salaryRangeFrom,
             salaryRangeTo, company, jobCategory, jobType);
            criteria.AddOrder(Order.Desc("CreationDate"));
            //criteria.SetResultTransformer(NHibernate.Transform.Transformers.AliasToBean<JobPostsDto>());
            return (List<JobPost>)criteria.SetFirstResult(start).SetMaxResults(length).List<JobPost>();
        }



        #endregion

        #region Others Function
        public int JobPostRowCount(long company, long jobCategory, long jobType, string jobTitle, int status)
        {
            ICriteria criteria = JobCriteria(company, jobCategory, jobType, jobTitle, status);

            criteria.SetProjection(Projections.RowCount());

            return Convert.ToInt32(criteria.UniqueResult());
        }

        public int JobApplicationRowCount(long jobPostId, long companyId, DateTime? deadLineFrom, DateTime? deadlineTo,
            int deadlineFlag, bool isShortListed, string jobTitle)
        {
            ICriteria criteria = JobApplicationCriteria(jobPostId, companyId, deadLineFrom, deadlineTo, deadlineFlag, isShortListed, jobTitle);

            criteria.SetProjection(Projections.RowCount());

            return Convert.ToInt32(criteria.UniqueResult());
        }

        public int CountJobPost(string what = "", string @where = "",
            int ageRangeFrom = 0, int ageRangeTo = 0, int expRangeFrom = 0, int expRangeTo = 0, int salaryRangeFrom = 0,
            int salaryRangeTo = 0, long[] company = null, long[] jobCategory = null, long[] jobType = null)
        {
            ICriteria criteria = JobSearchCriteria(what, where,
             ageRangeFrom, ageRangeTo, expRangeFrom, expRangeTo, salaryRangeFrom,
             salaryRangeTo, company, jobCategory, jobType);

            criteria.SetProjection(Projections.RowCount());

            return Convert.ToInt32(criteria.UniqueResult());
        }

        #endregion

        #region Helper Function

        private ICriteria JobSearchCriteria(string what = "", string where = "",
            int ageRangeFrom = 0, int ageRangeTo = 0, int expRangeFrom = 0, int expRangeTo = 0, int salaryRangeFrom = 0,
            int salaryRangeTo = 0, long[] company = null, long[] jobCategory = null, long[] jobType = null)
        {
            ICriteria criteria = Session.CreateCriteria<JobPost>().Add(Restrictions.Not(Restrictions.Eq("Status", Country.EntityStatus.Delete)));
            criteria.Add(Restrictions.Ge("DeadLine", DateTime.Now));
            if (!String.IsNullOrEmpty(what))
            {
                criteria.Add(Restrictions.Or(Restrictions.Like("JobTitle", what, MatchMode.Anywhere),
                    Restrictions.Like("JobDescription", what, MatchMode.Anywhere)));
            }
            if (!String.IsNullOrEmpty(where))
            {
                criteria.Add(Restrictions.Like("JobLocation", where, MatchMode.Anywhere));
            }
            if (ageRangeFrom > 0 && ageRangeTo >= ageRangeFrom)
            {
                criteria.Add(Restrictions.Ge("AgeRangeFrom", ageRangeFrom));
                criteria.Add(Restrictions.Or(Restrictions.Ge("AgeRangeFrom", ageRangeFrom), Restrictions.Le("AgeRangeTo", ageRangeTo)));
            }
            else if (ageRangeFrom > 0)
            {
                criteria.Add(Restrictions.Ge("AgeRangeFrom", ageRangeFrom));
            }
            else if (ageRangeTo > 0) { criteria.Add(Restrictions.Le("AgeRangeTo", ageRangeTo)); }
            if (expRangeFrom > 0 && expRangeTo >= expRangeFrom)
            {
                criteria.Add(Restrictions.Ge("ExperienceMin", expRangeFrom));
                criteria.Add(Restrictions.Or(Restrictions.Ge("ExperienceMin", expRangeFrom), Restrictions.Le("ExperienceMax", expRangeTo)));
            }
            else if (expRangeFrom > 0) { criteria.Add(Restrictions.Ge("ExperienceMin", expRangeFrom)); }
            else if (expRangeTo > 0) { criteria.Add(Restrictions.Le("ExperienceMax", expRangeTo)); }

            if (salaryRangeFrom > 0 && salaryRangeTo >= salaryRangeFrom)
            {
                criteria.Add(Restrictions.Ge("SalaryMin", salaryRangeFrom));
                criteria.Add(Restrictions.Or(Restrictions.Ge("SalaryMin", salaryRangeFrom), Restrictions.Le("SalaryMax", salaryRangeTo)));
            }
            else if (salaryRangeFrom > 0) { criteria.Add(Restrictions.Ge("SalaryMin", salaryRangeFrom)); }
            else if (salaryRangeTo > 0) { criteria.Add(Restrictions.Le("SalaryMax", salaryRangeTo)); }
            if (company != null && company.Length == 1 && company[0] != 0)
            {
                criteria.CreateAlias("Company", "c", JoinType.InnerJoin, Restrictions.Eq("c.Status", Company.EntityStatus.Active));
                criteria.Add(Restrictions.In("c.Id", company));
            }
            if (jobCategory != null && jobCategory.Length == 1 && jobCategory[0] != 0)
            {
                criteria.CreateAlias("JobCategory", "jc", JoinType.InnerJoin, Restrictions.Eq("jc.Status", JobCategory.EntityStatus.Active));
                criteria.Add(Restrictions.In("jc.Id", jobCategory));
            }
            if (jobType != null && jobType.Length == 1 && jobType[0] != 0)
            {
                criteria.CreateAlias("JobType", "jt", JoinType.InnerJoin, Restrictions.Eq("jt.Status", JobType.EntityStatus.Active));
                criteria.Add(Restrictions.In("jt.Id", jobType));
            }
            return criteria;
        }

        private ICriteria JobCriteria(long company, long jobCategory, long jobType, string jobTitle, int status)
        {
            ICriteria criteria = Session.CreateCriteria<JobPost>().Add(Restrictions.Not(Restrictions.Eq("Status", Country.EntityStatus.Delete)));
            if (status != 0)
                criteria.Add(Restrictions.Eq("Status", status));
            criteria.CreateAlias("Company", "c", JoinType.InnerJoin, Restrictions.Eq("c.Status", Company.EntityStatus.Active));
            criteria.CreateAlias("JobCategory", "jc", JoinType.InnerJoin, Restrictions.Eq("jc.Status", JobCategory.EntityStatus.Active));
            criteria.CreateAlias("JobType", "jt", JoinType.InnerJoin, Restrictions.Eq("jt.Status", JobType.EntityStatus.Active));
            if (!String.IsNullOrEmpty(jobTitle))
                criteria.Add(Restrictions.Like("JobTitle", jobTitle, MatchMode.Anywhere));
            if (company > 0)
                criteria.Add(Restrictions.Eq("c.Id", company));
            if (jobCategory > 0)
                criteria.Add(Restrictions.Eq("jc.Id", jobCategory));
            if (jobType > 0)
                criteria.Add(Restrictions.Eq("jt.Id", jobType));

            return criteria;
        }

        private ICriteria JobApplicationCriteria(long jobPostId, long companyId, DateTime? deadLineFrom, DateTime? deadlineTo, int deadlineFlag, bool isShortListed, string jobTitle)
        {
            ICriteria criteria = Session.CreateCriteria<JobSeekerJobPost>().Add(Restrictions.Eq("Status", JobSeekerJobPost.EntityStatus.Active));
            criteria.CreateAlias("JobSeeker", "js", JoinType.InnerJoin, Restrictions.Eq("js.Status", JobSeeker.EntityStatus.Active));
            criteria.CreateAlias("JobPost", "jp", JoinType.InnerJoin, Restrictions.Eq("jp.Status", JobPost.EntityStatus.Active));

            criteria.Add(Restrictions.Eq("IsApplied", true));
            if (jobPostId > 0)
                criteria.Add(Restrictions.Eq("jp.Id", jobPostId));
            if (companyId > 0)
                criteria.Add(Restrictions.Eq("jp.Company.Id", companyId));
            if (deadLineFrom != null)
            {
                criteria.Add(Restrictions.Ge("jp.DeadLine", deadLineFrom));
            }
            if (deadlineTo != null)
            {
                criteria.Add(Restrictions.Le("jp.DeadLine", deadlineTo));
            }
            if (deadlineFlag > 0)
            {
                var today = DateTime.Now;
                criteria.Add(deadlineFlag == 1
                    ? Restrictions.Gt("jp.DeadLine", today)
                    : Restrictions.Lt("jp.DeadLine", today));
            }
            if (isShortListed == true)
            {
                criteria.Add(Restrictions.Eq("IsShortListedByCompany", true));
            }
            if (string.IsNullOrEmpty(jobTitle))
            {
                criteria.Add(Restrictions.Like("jp.JobTitle", jobTitle, MatchMode.Anywhere));
            }
            return criteria;
        }
        #endregion

    }
}
