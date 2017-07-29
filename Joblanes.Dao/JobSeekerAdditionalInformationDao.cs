using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes.Base;
using Model.JobLanes.Entity;

namespace Dao.Joblanes
{
    public interface IJobSeekerAdditionalInformationDao : IBaseDao<JobSeekerAdditionalInformation, long>
    {
        #region Operational Function

        #endregion

        #region Single Instances Loading Function

        #endregion

        #region List Loading Function

        IList<JobSeekerAdditionalInformation> LoadByJobSeekerId(long jobSeekerId);

        #endregion

        #region Others Function

        #endregion



        bool CheckDuplicateFields(long id, long jobSeekerId, string p);
    }
    public class JobSeekerAdditionalInformationDao : BaseDao<JobSeekerAdditionalInformation, long>, IJobSeekerAdditionalInformationDao
    {
        #region Propertise & Object Initialization

        #endregion

        #region Operational Function

        #endregion

        #region Single Instances Loading Function

        #endregion

        #region List Loading Function

        public IList<JobSeekerAdditionalInformation> LoadByJobSeekerId(long jobSeekerId)
        {
            return
                Session.QueryOver<JobSeekerAdditionalInformation>()
                    .Where(x => x.JobSeeker.Id == jobSeekerId && x.Status == JobSeekerAdditionalInformation.EntityStatus.Active)
                    .List<JobSeekerAdditionalInformation>();
        }

        public bool CheckDuplicateFields(long id, long jobSeekerId, string additionalInfo)
        {
            var query = Session.QueryOver<JobSeekerAdditionalInformation>().Where(x => x.Status == Region.EntityStatus.Active); ;
            if (id > 0)
            {
                query = query.Where(x => x.Id != id);
            }

            if (!string.IsNullOrEmpty(additionalInfo.Trim()))
            {
                query = query.Where(x => x.Description == additionalInfo.ToString() && x.JobSeeker.Id == jobSeekerId);
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
