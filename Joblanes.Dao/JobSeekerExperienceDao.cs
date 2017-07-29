using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes.Base;
using Model.JobLanes.Entity;

namespace Dao.Joblanes
{

    public interface IJobSeekerExperienceDao : IBaseDao<JobSeekerExperience, long>
    {
        #region Operational Function

        #endregion

        #region Single Instances Loading Function

        #endregion

        #region List Loading Function

        #endregion

        #region Others Function
        bool CheckDuplicateFields(long id, string p, DateTime dateTime1, DateTime? dateTime2);
        #endregion

    }
    public class JobSeekerExperienceDao : BaseDao<JobSeekerExperience, long>, IJobSeekerExperienceDao
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
        public bool CheckDuplicateFields(long id, string companyName, DateTime fromTime, DateTime? totTime)
        {
            var query = Session.QueryOver<JobSeekerExperience>().Where(x => x.Status == Region.EntityStatus.Active); ;
            if (id > 0)
            {
                query = query.Where(x => x.Id != id);
            }

            if (!string.IsNullOrEmpty(companyName.Trim()))
            {
                query = query.Where(x => x.CompanyName == companyName.ToString());
            }
            if (fromTime != DateTime.MinValue || fromTime != DateTime.MaxValue)
            {
                query = query.Where(x => x.DateFrom == fromTime);
            }
            if (totTime != null && totTime != DateTime.MinValue || totTime != DateTime.MaxValue)
            {
                query = query.Where(x => x.DateTo == totTime);
            }
            var rowcount = query.RowCount();
            return rowcount > 0;
        }
        #endregion

        #region Helper Function

        #endregion
    }
}
