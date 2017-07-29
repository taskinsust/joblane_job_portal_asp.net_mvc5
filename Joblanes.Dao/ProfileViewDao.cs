using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes.Base;
using Model.JobLanes.Entity;

namespace Dao.Joblanes
{
    public interface IProfileViewDao : IBaseDao<ProfileView, long>
    {
        #region Operational Function

        #endregion

        #region Single Instances Loading Function

        #endregion

        #region List Loading Function

        #endregion

        #region Others Function

        #endregion

        ProfileView GetProfileView(long companyId, long jobseekerId, bool isCompany); 
    }


    public class ProfileViewDao : BaseDao<ProfileView, long>, IProfileViewDao
    {
        #region Operational Function

        #endregion

        #region Single Instances Loading Function
        public ProfileView GetProfileView(long companyId, long jobseekerId, bool isCompany)
        {
            return
                  Session.QueryOver<ProfileView>()
                      .Where(x => x.JobSeeker.Id == jobseekerId && x.Company.Id == companyId && x.IsCompany == isCompany)
                      .SingleOrDefault<ProfileView>();
        }
        #endregion

        #region List Loading Function

        #endregion

        #region Others Function

        #endregion

        
    }
}
