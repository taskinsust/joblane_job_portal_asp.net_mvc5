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
    public interface IJobSeekerJobPostService : IBaseService
    {
        #region Operational Function

        void SaveOrUpdate(string userProfileId, long jobPostId, bool isShortList = false, bool isApplied = false);

        #endregion

        #region Single Instances Loading Function

        #endregion

        #region List of Loading function

        #endregion

        #region Other Function

        #endregion
    }

    public class JobSeekerJobPostService : BaseService, IJobSeekerJobPostService
    {
        #region Propertise & Object Initialization

        private readonly IJobSeekerDao _jobSeekerDao;
        private readonly ICityDao _cityDao;
        private readonly IRegionDao _regionDao;
        private readonly IStateDao _stateDao;
        private readonly ICountryDao _countryDao;
        private readonly IUserDao _userDao;
        private readonly IJobPostDao _jobPostDao;
        private readonly IJobSeekerJobPostDao _jobSeekerJobPostDao;

        public JobSeekerJobPostService(ISession session)
        {
            Session = session;
            _jobSeekerDao = new JobSeekerDao() { Session = session };
            _cityDao = new CityDao() { Session = session };
            _regionDao = new RegionDao() { Session = session };
            _stateDao = new StateDao() { Session = session };
            _countryDao = new CountryDao() { Session = session };
            _userDao = new UserDao() { Session = session };
            _jobPostDao = new JobPostDao() { Session = session };
            _jobSeekerJobPostDao = new JobSeekerJobPostDao() { Session = session };
        }
        #endregion


        #region Operational Function

        public void SaveOrUpdate(string userProfileId, long jobPostId, bool isShortList = false, bool isApplied = false)
        {
            try
            {
                if (String.IsNullOrEmpty(userProfileId)) { throw new InvalidDataException("Invalid UserProfile "); }
                if (jobPostId <= 0) { throw new InvalidDataException("Invalid JobPost Id "); }
                UserProfile up = _userDao.GetByAspNetUserId(userProfileId);
                if (up == null) throw new InvalidDataException("Invalid User");
                JobPost jp = _jobPostDao.LoadById(jobPostId);
                if (jp == null) throw new InvalidDataException("Invalid JobPost");
                JobSeeker jobSeeker = _jobSeekerDao.GetUsProfileId(up.Id);
                if (jobSeeker == null) throw new InvalidDataException("Please update your profile before applying for job");
                var jobSeekerJobPost = new JobSeekerJobPost();

                using (var trans = Session.BeginTransaction())
                {
                    //IList<JobSeekerJobPost> list = null;
                    IList<JobSeekerJobPost> list = _jobSeekerJobPostDao.LoadByJobPostAndJobSeekerId(jp.Id, jobSeeker.Id);
                    if (list != null && list.Count > 1) { throw new MessageException("Invalid Job Application"); }
                    if (list != null && list.Count == 1)
                    {
                        if (isShortList == true)
                        {
                            if (list[0].IsShortList == true) { throw new MessageException("You have already shortlisted this job"); }
                            list[0].IsShortList = true;
                        }
                        if (isApplied)
                        {
                            if (list[0].IsApplied == true) { throw new MessageException("You have already applied this job"); }
                            list[0].IsApplied = true;
                        }
                        _jobSeekerJobPostDao.Update(list[0]);
                    }
                    if (list == null || list.Count == 0)
                    {
                        if (isApplied == true)
                        {
                            jobSeekerJobPost.IsApplied = true;
                        }
                        jobSeekerJobPost.JobPost = jp;
                        jobSeekerJobPost.JobSeeker = jobSeeker;
                        jobSeekerJobPost.IsShortList = isShortList;
                        _jobSeekerJobPostDao.Save(jobSeekerJobPost);
                    }
                    trans.Commit();
                }

            }
            catch (Exception ex)
            {
                throw ex;
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
