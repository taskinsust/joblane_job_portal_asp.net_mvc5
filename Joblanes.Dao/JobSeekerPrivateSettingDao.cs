using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes.Base;
using Model.JobLanes.Entity;

namespace Dao.Joblanes
{
    public interface IJobSeekerPrivateSettingDao : IBaseDao<JobSeekerPrivateSetting, long>
    {
        #region Operational Function

        #endregion

        #region Single Instances Loading Function
        JobSeekerPrivateSetting GetByJobSeekerId(long jobSeekerId);
        #endregion

        #region List Loading Function

        #endregion

        #region Others Function

        #endregion

    }
    public class JobSeekerPrivateSettingDao : BaseDao<JobSeekerPrivateSetting, long>, IJobSeekerPrivateSettingDao
    {
        #region Propertise & Object Initialization

        #endregion

        #region Operational Function

        #endregion

        #region Single Instances Loading Function
        public JobSeekerPrivateSetting GetByJobSeekerId(long jobSeekerId)
        {
            return
                Session.QueryOver<JobSeekerPrivateSetting>()
                    .Where(x => x.JobSeeker.Id == jobSeekerId)
                    .SingleOrDefault<JobSeekerPrivateSetting>();
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
