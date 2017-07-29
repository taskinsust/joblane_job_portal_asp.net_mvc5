using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes;
using Model.JobLanes.Entity;
using Model.JobLanes.Entity.User;
using NHibernate;
using Services.Joblanes.Base;
using Services.Joblanes.Exceptions;

namespace Services.Joblanes
{
    public interface IProfileViewService : IBaseService
    {
        #region Operational Function
        void SaveJobSeekerProfileView(string aspNetUser, long jobSeekerId);
        #endregion

        #region Single Instances Loading Function

        #endregion

        #region List of Loading function
        #endregion

        #region Other Function

        #endregion

        
    }

    public class ProfileViewService : BaseService, IProfileViewService
    {
        #region Propertise & Object Initialization

        private readonly IProfileViewDao _profileViewDao;
        private readonly IUserDao _userDao;
        private readonly ICompanyDao _companyDao;
        private readonly IJobSeekerDao _jobSeekerDao;
        public ProfileViewService(ISession session)
        {
            Session = session;
            _profileViewDao = new ProfileViewDao() { Session = session };
            _userDao = new UserDao(){Session = session};
        }

        #endregion

        #region Operational Function
        public void SaveJobSeekerProfileView(string aspNetUser, long jobSeekerId)
        {
            ITransaction transaction = null;
            try
            {
                if (string.IsNullOrEmpty(aspNetUser) || jobSeekerId <= 0)
                {
                    throw new NullObjectException("Job Seeker Not Found");
                }


                UserProfile userProfile = _userDao.GetByAspNetUserId(aspNetUser);
                Company company = _companyDao.GetCompany(userProfile.Id);
                JobSeeker jobSeeker = _jobSeekerDao.GetJobSeekerById(jobSeekerId, JobSeeker.EntityStatus.Active);

                ProfileView profileView = _profileViewDao.GetProfileView(company.Id, jobSeeker.Id, true);

                using (transaction = Session.BeginTransaction())
                {
                    if (profileView != null && profileView.Id > 0)
                    {
                        _profileViewDao.Update(profileView);
                    }
                    else
                    {
                        ProfileView newProfileView = new ProfileView();
                        newProfileView.JobSeeker = jobSeeker;
                        newProfileView.Company = company;
                        newProfileView.IsCompany = true;
                        _profileViewDao.Save(newProfileView);
                    }
                   

                    transaction.Commit();
                }
            }
            catch (NullObjectException ex)
            {
                throw;
            }
            catch (DuplicateEntryException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (transaction != null && transaction.IsActive)
                    transaction.Rollback();
            }
        }
        #endregion

        #region Single Instances Loading Function

        #endregion

        #region List of Loading function

        #endregion

        #region Other Function

        #endregion

       
    }

}
