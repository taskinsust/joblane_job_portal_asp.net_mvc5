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
    public interface IJobSeekerJobPostDao : IBaseDao<JobSeekerJobPost, long>
    {
        #region Operational Function

        #endregion

        #region Single Instances Loading Function
        JobSeekerJobPost GetJobSeekerJobPost(long id, long companyId); 
        #endregion

        #region List Loading Function
        IList<JobSeekerJobPost> LoadByJobPostAndJobSeekerId(long jobPostId, long jobSeekerId, bool isApplied = false);
        List<long> LoadAllAppliedJobsByIds(long id);
        List<long> LoadAllShortlistedJobsByIds(long id);
        #endregion

        #region Others Function

        #endregion





    }
    public class JobSeekerJobPostDao : BaseDao<JobSeekerJobPost, long>, IJobSeekerJobPostDao
    {
        #region Propertise & Object Initialization

        #endregion

        #region Operational Function

        #endregion

        #region Single Instances Loading Function
        public JobSeekerJobPost GetJobSeekerJobPost(long id, long companyId)
        {
            ICriteria criteria = Session.CreateCriteria<JobSeekerJobPost>().Add(Restrictions.Eq("Status", JobSeekerJobPost.EntityStatus.Active));         
            criteria.CreateAlias("JobPost", "jp", JoinType.InnerJoin, Restrictions.Eq("jp.Status", JobPost.EntityStatus.Active));
            criteria.Add(Restrictions.Eq("IsApplied", true));
            criteria.Add(Restrictions.Eq("jp.Company.Id", companyId));
            criteria.Add(Restrictions.Eq("Id", id));
            return criteria.List<JobSeekerJobPost>().FirstOrDefault();
            //.List<JobSeekerJobPost>();
        }
        #endregion

        #region List Loading Function
        public IList<JobSeekerJobPost> LoadByJobPostAndJobSeekerId(long jobPostId, long jobSeekerId, bool isApplied = false)
        {
            var queryOver = Session.QueryOver<JobSeekerJobPost>()
                .Where(x => x.JobPost.Id == jobPostId && x.JobSeeker.Id == jobSeekerId);
            if (isApplied)
            {
                queryOver.Where(x => x.IsApplied == true);
            }
            return queryOver.List<JobSeekerJobPost>();
            //        .List<JobSeekerJobPost>();

        }

        public List<long> LoadAllAppliedJobsByIds(long id)
        {
            return
                Session.QueryOver<JobSeekerJobPost>()
                    .Where(x => x.JobSeeker.Id == id && x.IsApplied == true)
                    .List<JobSeekerJobPost>()
                    .Select(x => x.JobPost.Id)
                    .ToList();
        }

        public List<long> LoadAllShortlistedJobsByIds(long id)
        {
            return
                 Session.QueryOver<JobSeekerJobPost>()
                     .Where(x => x.JobSeeker.Id == id && x.IsShortList == true)
                     .List<JobSeekerJobPost>()
                     .Select(x => x.JobPost.Id)
                     .ToList();
        }

        #endregion

        #region Others Function

        #endregion

        #region Helper Function

        #endregion


    }

}
