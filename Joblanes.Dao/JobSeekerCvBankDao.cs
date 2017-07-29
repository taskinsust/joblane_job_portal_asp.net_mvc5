using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes.Base;
using Model.JobLanes.Entity;

namespace Dao.Joblanes
{
    public interface IJobSeekerCvBankDao : IBaseDao<JobSeekerCvBank, long>
    {
        #region Operational Function

        #endregion

        #region Single Instances Loading Function
        JobSeekerCvBank LoadByUserProfileId(long userProfileId);
        #endregion

        #region List Loading Function
       
        #endregion

        #region Others Function
      
        #endregion
        
    }
    public class JobSeekerCvBankDao : BaseDao<JobSeekerCvBank, long>, IJobSeekerCvBankDao
    {
        #region Propertise & Object Initialization

        #endregion

        #region Operational Function

        #endregion

        #region Single Instances Loading Function
        public JobSeekerCvBank LoadByUserProfileId(long userProfileId)
        {
            return
                Session.QueryOver<JobSeekerCvBank>()
                    .Where(x => x.UserProfile.Id == userProfileId)
                    .SingleOrDefault<JobSeekerCvBank>();
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
