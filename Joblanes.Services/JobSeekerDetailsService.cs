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
    public interface IJobSeekerDetailsService : IBaseService
    {
        #region Operational Function
        bool Save();
        #endregion

        #region List of Loading function

        #endregion

        #region Other Function
        void SaveCv(byte[] file, string aspNetUserId);
        #endregion


    }
    public class JobSeekerDetailsService : BaseService, IJobSeekerDetailsService
    {
        #region Propertise & Object Initialization

        private readonly IJobSeekerDetailsDao _seekerDetailsDao;
        private readonly IUserDao _userDao;
        private readonly IJobSeekerDao _jobSeekerDao;
        private readonly IJobSeekerCvBankDao _jobSeekerCvBankDao;
        public JobSeekerDetailsService(ISession session)
        {
            Session = session;
            _seekerDetailsDao = new JobSeekerDetailsDao() { Session = session };
            _userDao = new UserDao() { Session = session };
            _jobSeekerDao = new JobSeekerDao() { Session = session };
            _jobSeekerCvBankDao = new JobSeekerCvBankDao() { Session = session };
        }
        #endregion

        #region Operational Function
        public bool Save()
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region List of Loading function

        #endregion


        #region Other Function

        public void SaveCv(byte[] file, string aspNetUserId)
        {
            ITransaction trans = null;
            try
            {
                if (String.IsNullOrEmpty(aspNetUserId)) throw new InvalidDataException("Invalid user");
                UserProfile userProfile = _userDao.GetByAspNetUserId(aspNetUserId);
                if (userProfile == null) throw new MessageException("Invalid user as Logged in Found");
                var jobSeekerCv = new JobSeekerCvBank();
                jobSeekerCv.UserProfile = userProfile;
                jobSeekerCv.CvGuid = Guid.NewGuid();
                jobSeekerCv.Cv = file;
                JobSeekerCvBank oldCv = _jobSeekerCvBankDao.LoadByUserProfileId(userProfile.Id);
                //JobSeeker jobSeeker = _jobSeekerDao.GetUsProfileId(userProfile.Id);
                //if (jobSeeker == null) throw new InvalidDataException("Please Update Your Profile First");
                //if (file != null)
                //{
                //    jobSeeker.JobSeekerDetailses[0].ImageGuid = Guid.NewGuid();
                //    jobSeeker.JobSeekerDetailses[0].Cv = file;
                //}
                using (trans = Session.BeginTransaction())
                {
                    if (oldCv == null)
                    {
                        _jobSeekerCvBankDao.Save(jobSeekerCv);
                    }
                    else
                    {
                        oldCv.UserProfile = userProfile;
                        oldCv.Cv = file;
                        oldCv.CvGuid = Guid.NewGuid();
                        _jobSeekerCvBankDao.Update(oldCv);
                    }
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                if (trans != null && !trans.WasCommitted) { trans.Rollback(); }
                throw ex;
            }
        }

        #endregion
    }
}
