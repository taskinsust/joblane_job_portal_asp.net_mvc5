using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes.Base;
using Model.JobLanes.Entity;

namespace Dao.Joblanes
{
    public interface IJobSeekerLinkDao : IBaseDao<JobSeekerLink, long>
    {
        #region Operational Function

        #endregion

        #region Single Instances Loading Function

        #endregion

        #region List Loading Function

        IList<JobSeekerLink> LoadByJobSeekerId(long jobSeekerId);

        #endregion

        #region Others Function

        #endregion



        bool CheckDuplicateFields(long id, long jobSeekerId, string link);
    }
    public class JobSeekerLinkDao : BaseDao<JobSeekerLink, long>, IJobSeekerLinkDao
    {
        #region Propertise & Object Initialization

        #endregion

        #region Operational Function

        #endregion

        #region Single Instances Loading Function

        #endregion

        #region List Loading Function
        public IList<JobSeekerLink> LoadByJobSeekerId(long jobSeekerId)
        {
            return
                Session.QueryOver<JobSeekerLink>()
                    .Where(x => x.JobSeeker.Id == jobSeekerId && x.Status == JobSeekerLink.EntityStatus.Active)
                    .List<JobSeekerLink>();
        }

        public bool CheckDuplicateFields(long id, long jobSeekerId, string link)
        {
            var query = Session.QueryOver<JobSeekerLink>().Where(x => x.Status == Region.EntityStatus.Active); ;
            if (id > 0)
            {
                query = query.Where(x => x.Id != id);
            }

            if (!string.IsNullOrEmpty(link.Trim()))
            {
                query = query.Where(x => x.Link == link.ToString() && x.JobSeeker.Id == jobSeekerId);
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
