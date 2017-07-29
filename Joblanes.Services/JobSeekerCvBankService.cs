using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes;
using Model.JobLanes.Dto;
using Model.JobLanes.Entity;
using Model.JobLanes.Entity.User;
using NHibernate;
using Services.Joblanes.Base;
using Services.Joblanes.Exceptions;

namespace Services.Joblanes
{

    public interface IJobSeekerCvBankService : IBaseService
    {
        #region Operational Function

        #endregion

        #region Single Instances Loading Function

        JobSeekerCvBankDto GetByUserProfileId(string aspNetUserId);

        #endregion

        #region List of Loading function

        #endregion

        #region Other Function

        #endregion
    }
    public class JobSeekerCvBankService : BaseService, IJobSeekerCvBankService
    {
        #region Propertise & Object Initialization

        private readonly IJobSeekerCvBankDao _jobSeekerCvBankDao;
        private readonly IUserDao _userDao;

        public JobSeekerCvBankService(ISession session)
        {
            Session = session;
            _jobSeekerCvBankDao = new JobSeekerCvBankDao() { Session = session };
            _userDao= new UserDao(){Session = session};

        }

        #endregion

        #region Operational Function


        #endregion

        #region Single Instances Loading Function
        public JobSeekerCvBankDto GetByUserProfileId(string aspNetUserId)
        {
            try
            {
            UserProfile userProfile = _userDao.GetByAspNetUserId(aspNetUserId);
            JobSeekerCvBank cvBank = _jobSeekerCvBankDao.LoadByUserProfileId(userProfile.Id);
                    if (cvBank == null)
                    {
                        throw new NullObjectException("CV not found. Please upload first.");
                    }
            return new JobSeekerCvBankDto()
            {
                UserProfile = cvBank.UserProfile.Id,
                Cv = cvBank.Cv,
                CvGuid = cvBank.CvGuid
            };
            }
            catch (NullObjectException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region List of Loading function


        #endregion

        #region Other Function
        #endregion

        #region Helper Function

        #endregion


    }


}
