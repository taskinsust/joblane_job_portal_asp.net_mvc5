using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes.Base;
using Model.JobLanes.Entity;

namespace Dao.Joblanes
{

    public interface IJobSeekerEducationalQualificationDao : IBaseDao<JobSeekerEducationalQualification, long>
    {
        #region Operational Function

        #endregion

        #region Single Instances Loading Function

        #endregion

        #region List Loading Function

        #endregion

        #region Others Function
        bool CheckDuplicateFields(long id, long jobSeekerId, string degreeName);
        #endregion
    }
    public class JobSeekerEducationalQualificationDao : BaseDao<JobSeekerEducationalQualification, long>, IJobSeekerEducationalQualificationDao
    {
        #region Propertise & Object Initialization

        #endregion

        #region Operational Function

        #endregion

        #region Single Instances Loading Function

        #endregion

        #region List Loading Function

        #endregion

        #region Others Function
        public bool CheckDuplicateFields(long id, long jobSeekerId, string degreeName)
        {
            var query = Session.QueryOver<JobSeekerEducationalQualification>().Where(x => x.Status == Region.EntityStatus.Active); ;
            if (id > 0)
            {
                query = query.Where(x => x.Id != id);
            }

            if (!string.IsNullOrEmpty(degreeName.Trim()))
            {
                query = query.Where(x => x.Degree == degreeName.ToString() && x.JobSeeker.Id == jobSeekerId);
            }

            var rowcount = query.RowCount();
            return rowcount > 0;
        }
        #endregion

        #region Helper Function

        #endregion
    }
}
