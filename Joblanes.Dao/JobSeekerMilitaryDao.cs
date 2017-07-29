using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes.Base;
using Model.JobLanes.Entity;

namespace Dao.Joblanes
{
    public interface IJobSeekerMilitaryDao : IBaseDao<JobSeekerMilitary, long>
    {
        #region Operational Function

        #endregion

        #region Single Instances Loading Function

        #endregion

        #region List Loading Function

        IList<JobSeekerMilitary> LoadByJobSeekerId(long jobSeekerId);

        #endregion

        #region Others Function

        #endregion

        bool CheckDuplicateFields(long id, long jobSeekerId, DateTime dateFrom);
    }
    public class JobSeekerMilitaryDao : BaseDao<JobSeekerMilitary, long>, IJobSeekerMilitaryDao
    {
        #region Propertise & Object Initialization

        #endregion

        #region Operational Function

        #endregion

        #region Single Instances Loading Function

        #endregion

        #region List Loading Function

        public IList<JobSeekerMilitary> LoadByJobSeekerId(long jobSeekerId)
        {
            return
                Session.QueryOver<JobSeekerMilitary>()
                    .Where(x => x.JobSeeker.Id == jobSeekerId && x.Status == JobSeekerMilitary.EntityStatus.Active)
                    .List<JobSeekerMilitary>();
        }

        public bool CheckDuplicateFields(long id, long jobSeekerId, DateTime dateFrom)
        {
            var query = Session.QueryOver<JobSeekerMilitary>().Where(x => x.Status == Region.EntityStatus.Active); ;
            if (id > 0)
            {
                query = query.Where(x => x.Id != id);
            }

            if (!string.IsNullOrEmpty(dateFrom.ToString().Trim()))
            {
                query = query.Where(x => x.DateFrom == dateFrom && x.JobSeeker.Id == jobSeekerId);
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
