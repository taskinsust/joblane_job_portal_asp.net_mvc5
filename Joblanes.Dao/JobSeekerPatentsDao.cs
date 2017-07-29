using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes.Base;
using Model.JobLanes.Entity;

namespace Dao.Joblanes
{
    public interface IJobSeekerPatentsDao : IBaseDao<JobSeekerPatents, long>
    {
        #region Operational Function

        #endregion

        #region Single Instances Loading Function

        IList<JobSeekerPatents> LoadByJobSeekerId(long jobSeekerId);

        #endregion

        #region List Loading Function

        #endregion

        #region Others Function

        #endregion



        bool CheckDuplicateFields(long id, long jobSeekerId, string title);
    }
    public class JobSeekerPatentsDao : BaseDao<JobSeekerPatents, long>, IJobSeekerPatentsDao
    {
        #region Propertise & Object Initialization

        #endregion

        #region Operational Function

        #endregion

        #region Single Instances Loading Function

        public IList<JobSeekerPatents> LoadByJobSeekerId(long jobSeekerId)
        {
            return
                Session.QueryOver<JobSeekerPatents>().Where(x => x.JobSeeker.Id == jobSeekerId && x.Status == JobSeekerPatents.EntityStatus.Active).List<JobSeekerPatents>();
        }

        public bool CheckDuplicateFields(long id, long jobSeekerId, string title)
        {
            var query = Session.QueryOver<JobSeekerPatents>().Where(x => x.Status == Region.EntityStatus.Active); ;
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
