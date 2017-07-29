using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes.Base;
using Model.JobLanes.Dto;
using Model.JobLanes.Entity;

namespace Dao.Joblanes
{
    public interface IJobSeekerPublicationsDao : IBaseDao<JobSeekerPublications, long>
    {
        #region Operational Function

        #endregion

        #region Single Instances Loading Function

        IList<JobSeekerPublications> LoadByJobSeekerId(long jobSeekerId);

        #endregion

        #region List Loading Function

        #endregion

        #region Others Function

        #endregion

        bool CheckDuplicateFields(long id, long jobSeekerId, string title);
    }
    public class JobSeekerPublicationsDao : BaseDao<JobSeekerPublications, long>, IJobSeekerPublicationsDao
    {
        #region Propertise & Object Initialization

        #endregion

        #region Operational Function

        #endregion

        #region Single Instances Loading Function

        public IList<JobSeekerPublications> LoadByJobSeekerId(long jobSeekerId)
        {
            return
                Session.QueryOver<JobSeekerPublications>()
                    .Where(x => x.JobSeeker.Id == jobSeekerId && x.Status == JobSeekerPublications.EntityStatus.Active)
                    .List<JobSeekerPublications>();
        }

        public bool CheckDuplicateFields(long id, long jobSeekerId, string title)
        {
            var query = Session.QueryOver<JobSeekerPublications>().Where(x => x.Status == Region.EntityStatus.Active); ;
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

        #region List Loading Function


        #endregion

        #region Others Function

        #endregion

        #region Helper Function

        #endregion


    }

}
