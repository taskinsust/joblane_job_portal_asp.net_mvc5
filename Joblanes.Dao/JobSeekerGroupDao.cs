using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes.Base;
using Model.JobLanes.Entity;

namespace Dao.Joblanes
{
    public interface IJobSeekerGroupDao : IBaseDao<JobSeekerGroup, long>
    {
        #region Operational Function

        #endregion

        #region Single Instances Loading Function

        #endregion

        #region List Loading Function

        IList<JobSeekerGroup> LoadByJobSeekerId(long jobSeekerId);

        #endregion

        #region Others Function

        #endregion



        bool CheckDuplicateFields(long id, long jobSeekerId, string title);
    }
    public class JobSeekerGroupDao : BaseDao<JobSeekerGroup, long>, IJobSeekerGroupDao
    {
        #region Propertise & Object Initialization

        #endregion

        #region Operational Function

        #endregion

        #region Single Instances Loading Function

        #endregion

        #region List Loading Function
        public IList<JobSeekerGroup> LoadByJobSeekerId(long jobSeekerId)
        {
            return
                Session.QueryOver<JobSeekerGroup>()
                    .Where(x => x.JobSeeker.Id == jobSeekerId && x.Status == JobSeekerGroup.EntityStatus.Active)
                    .List<JobSeekerGroup>();
        }

        public bool CheckDuplicateFields(long id, long jobSeekerId, string title)
        {
            var query = Session.QueryOver<JobSeekerGroup>().Where(x => x.Status == Region.EntityStatus.Active); ;
            if (id > 0)
            {
                query = query.Where(x => x.Id != id);
            }

            if (!string.IsNullOrEmpty(title.Trim()))
            {
                query = query.Where(x => x.Title == title.ToString() && x.JobSeeker.Id == jobSeekerId);
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
