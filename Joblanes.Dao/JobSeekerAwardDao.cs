using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes.Base;
using Model.JobLanes.Entity;

namespace Dao.Joblanes
{
    public interface IJobSeekerAwardDao : IBaseDao<JobSeekerAward, long>
    {
        #region Operational Function

        #endregion

        #region Single Instances Loading Function

        #endregion

        #region List Loading Function

        IList<JobSeekerAward> LoadByJobSeekerId(long jobSeekerId);

        #endregion

        #region Others Function

        #endregion

        bool CheckDuplicateFields(long id, long jobSeekerId, string title, DateTime dateAwarded);
    }
    public class JobSeekerAwardDao : BaseDao<JobSeekerAward, long>, IJobSeekerAwardDao
    {
        #region Propertise & Object Initialization

        #endregion

        #region Operational Function

        #endregion

        #region Single Instances Loading Function

        #endregion

        #region List Loading Function
        public IList<JobSeekerAward> LoadByJobSeekerId(long jobSeekerId)
        {
            return
                Session.QueryOver<JobSeekerAward>()
                    .Where(x => x.JobSeeker.Id == jobSeekerId && x.Status == JobSeekerAward.EntityStatus.Active)
                    .List<JobSeekerAward>();
        }

        public bool CheckDuplicateFields(long id, long jobSeekerId, string title, DateTime dateAwarded)
        {
            var query = Session.QueryOver<JobSeekerAward>().Where(x => x.Status == Region.EntityStatus.Active); ;
            if (id > 0)
            {
                query = query.Where(x => x.Id != id);
            }

            if (!string.IsNullOrEmpty(title.Trim()))
            {
                query = query.Where(x => x.Title == title && x.JobSeeker.Id == jobSeekerId && x.DateAwarded == dateAwarded);
            }

            var rowcount = query.RowCount();
            return rowcount > 0;
        }

        #endregion

        #region Others Function

        #endregion

        #region Helper Function

        #endregion


    }
}
