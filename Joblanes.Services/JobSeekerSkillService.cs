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
using NHibernate.Linq;
using Services.Joblanes.Base;

namespace Services.Joblanes
{
    public interface IJobSeekerSkillService : IBaseService
    {
        #region Operational Function

        #endregion

        #region Single Instances Loading Function

        IList<JobSeekerSkillDto> LoadByUserProfileId(string aspNetUserId);

        #endregion

        #region List of Loading function

        #endregion

        #region Other Function

        #endregion
    }
    public class JobSeekerSkillService : BaseService, IJobSeekerSkillService
    {
        #region Propertise & Object Initialization

        private readonly IJobSeekerCvBankDao _jobSeekerCvBankDao;
        private readonly IUserDao _userDao;
        private readonly IJobSeekerDao _jobSeekerDao;
        private readonly IJobSeekerSkillDao _jobSeekerSkillDao;
        public JobSeekerSkillService(ISession session)
        {
            Session = session;
            _jobSeekerCvBankDao = new JobSeekerCvBankDao() { Session = session };
            _userDao = new UserDao() { Session = session };
            _jobSeekerDao = new JobSeekerDao() { Session = session };
            _jobSeekerSkillDao = new JobSeekerSkillDao() { Session = session };
        }

        #endregion

        #region Operational Function


        #endregion

        #region Single Instances Loading Function

        public IList<JobSeekerSkillDto> LoadByUserProfileId(string aspNetUserId)
        {
            UserProfile userProfile = _userDao.GetByAspNetUserId(aspNetUserId);
            JobSeeker jobSeeker = _jobSeekerDao.GetAllByProfileId(userProfile.Id);
            IList<JobSeekerSkill> jobSeekerSkills = _jobSeekerSkillDao.GetByJobSeekerId(jobSeeker.Id);
            IList<JobSeekerSkillDto> list = new List<JobSeekerSkillDto>();
            foreach (var jobSeekerSkill in jobSeekerSkills)
            {
                JobSeekerSkillDto dto = new JobSeekerSkillDto();
                dto.Skill = jobSeekerSkill.Skill;
                dto.Experence = jobSeekerSkill.Experence;
                dto.JobSeeker = jobSeekerSkill.JobSeeker.Id.ToString();
                list.Add(dto);
            }
            return list;
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
