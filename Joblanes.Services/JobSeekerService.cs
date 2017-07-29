using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes;
using Model.JobLanes.Dto;
using Model.JobLanes.Entity;
using Model.JobLanes.Entity.Base;
using Model.JobLanes.Entity.User;
using Model.JobLanes.ViewModel;
using NHibernate;
using NHibernate.Util;
using Services.Joblanes.Base;
using Services.Joblanes.Exceptions;
using Services.Joblanes.Helper;

namespace Services.Joblanes
{
    public interface IJobseekerService : IBaseService
    {
        #region Operational Function
        bool Save(JobSeekerProfileVm jobSeekerProfileVm);
        void SaveSkill(JobSeekerSkillDto jobSeekerSkillDto);
        void DeleteResume(string aspnetUserId);

        void SaveLink(JobSeekerLinkDto jobSeekerLinkDto);
        void SaveMilitary(JobSeekerMilitaryDto jobSeekerMilitaryDto);
        void SaveAward(JobSeekerAwardDto jobSeekerAwardDto);
        void SaveCertificate(JobSeekerCertificateDto jobSeekerCertificateDto);
        void SaveGroup(JobSeekerGroupDto jobSeekerGroupDto);
        void SavePatent(JobSeekerPatentsDto jobSeekerPatentsDto);
        void SavePublication(JobSeekerPublicationsDto jobSeekerPublicationsDto);
        void SaveAdditinalInfo(JobSeekerAdditionalInformationDto jobSeekerAdditionalInformationDto);
        void SaveDesiredJob(JobSeekerDesiredJobDto jobSeekerDesiredJobDto);

        #region Delete

        void DeleteSkill(long id);
        void DeletePublication(long id);
        void DeleteAward(long id);
        void DeleteAdditionalInfo(long id);
        void DeleteGroup(long id);
        void DeleteLink(long id);
        void DeleteMalitary(long id);
        void DeletePatent(long id);
        void DeleteCertificate(long id);
        void DeleteDesiredJobs(long id);

        #endregion

        #endregion

        #region List of Loading function
        List<JobSeekerDto> LoadOnlyJobSeeker(int? status = null);
        List<JobSeekerDto> LoadJobSeekerForWebAdmin(int start, int length, string orderBy, string orderDir,
           int? status = null, string firstName = "", string lastName = "", string contactMobile = "", string contactEmail = "",
           string zip = "", long? region = null, long? country = null, long? state = null, long? city = null
           );

        List<JobSeekerSearchDto> LoadJobSeekerSearch(int start, int length, string orderBy, string orderDir, string whatKey, string whereKey
            , string[] jobTitle, string[] yearExp, string[] education, string[] company
            );

        List<FindResumeFilterDto> LoadFindResumeFiteDtoList();
        IList<JobSeekerSkillDto> LoadJobSeekerSkillDtoByAspNetUserId(string aspNetUserId);
        IList<JobSeekerPublicationsDto> LoadJobSeekerPublicationsDtoByAspNetUserId(string aspNetUserId);
        IList<JobSeekerPatentsDto> LoadJobSeekerPatentsDtoByAspNetUserId(string aspNetUserId);
        IList<JobSeekerMilitaryDto> LoadJobSeekerMilitaryDtoByAspNetUserId(string aspNetUserId);
        IList<JobSeekerLinkDto> LoadJobSeekerLinkDtoByAspNetUserId(string aspNetUserId);
        IList<JobSeekerGroupDto> LoadJobSeekerGroupDtoByAspNetUserId(string aspNetUserId);
        IList<JobSeekerAwardDto> LoadJobSeekerAwardDtoByAspNetUserId(string aspNetUserId);
        IList<JobSeekerCertificateDto> LoadJobSeekerCertificateDtoByAspNetUserId(string aspNetUserId);
        IList<JobSeekerAdditionalInformationDto> LoadAditionalAdditionalInformationDtosByAspNetUserId(string aspNetUserId);
        IList<JobSeekerDesiredJobDto> LoadJobSeekerDesiredAspNetUserId(string aspNetUserId);
        #endregion

        #region Other Function
        void SavePhoto(byte[] file, string aspNetUserId);
        int JobSeekerRowCount(int? status = null, string firstName = null, string lastName = "", string contactMobile = "", string contactEmail = "",
            string zip = "", long? region = null, long? country = null, long? state = null, long? city = null);

        int JobSeekerSearchRowCount(string whatKey, string whereKey, string[] jobTitle, string[] yearExp, string[] education, string[] company);
        #endregion

        #region Single Instance Load

        JobSeekerProfileVm GetJobSeekerProfileData(string userId);
        JobSeekerProfileDto GetJobSeeker(string aspNetUserId);

        JobSeekerSkillDto GetJobSeekerSkillDtoByAspNetUserId(long id);
        JobSeekerPublicationsDto GetJobSeekerPublicationsDtoByAspNetUserId(long id);
        JobSeekerPatentsDto GetJobSeekerPatentsDtoByAspNetUserId(long id);
        JobSeekerMilitaryDto GetJobSeekerMilitaryDtoByAspNetUserId(long id);
        JobSeekerLinkDto GetJobSeekerLinkDtoByAspNetUserId(long id);
        JobSeekerGroupDto GetJobSeekerGroupDtoByAspNetUserId(long id);
        JobSeekerAwardDto GetJobSeekerAwardDtoByAspNetUserId(long id);
        JobSeekerCertificateDto GetJobSeekerCertificateDtoByAspNetUserId(long id);
        JobSeekerAdditionalInformationDto GetAdditionalInformationDtosByAspNetUserId(long id);
        JobSeekerDesiredJobDto GetDesiredDtosByAspNetUserId(long id);

        JobSeekerDto GetJobSeekerById(long jobSeekerId, int status);
        JobSeekerDto GetJobSeekerByAspId(string aspNetUser, int status);  
        #endregion



        
    }
    public class JobSeekerService : BaseService, IJobseekerService
    {
        #region Propertise & Object Initialization

        private readonly IJobSeekerDao _jobSeekerDao;
        private readonly ICityDao _cityDao;
        private readonly IRegionDao _regionDao;
        private readonly IStateDao _stateDao;
        private readonly ICountryDao _countryDao;
        private readonly IUserDao _userDao;
        private readonly IJobSeekerPrivateSettingDao _jobSeekerPrivateSettingDao;
        private readonly IJobSeekerAdditionalInformationDao _jobSeekerAdditionalInformationDao;
        private readonly IJobSeekerAwardDao _jobSeekerAwardDao;
        private readonly IJobSeekerGroupDao _jobSeekerGroupDao;
        private readonly IJobSeekerSkillDao _jobSeekerSkillDao;
        private readonly IJobSeekerPatentsDao _jobSeekerPatentsDao;
        private readonly IJobSeekerMilitaryDao _jobSeekerMilitaryDao;
        private readonly IJobSeekerLinkDao _jobSeekerLinkDao;
        private readonly IJobSeekerPublicationsDao _jobSeekerPublicationsDao;
        private readonly IJobSeekerTrainingCoursesDao _jobSeekerTrainingCoursesDao;
        private readonly IJobSeekerDesiredDao _jobSeekerDesiredDao;
        public JobSeekerService(ISession session)
        {
            Session = session;
            _jobSeekerDao = new JobSeekerDao() { Session = session };
            _cityDao = new CityDao() { Session = session };
            _regionDao = new RegionDao() { Session = session };
            _stateDao = new StateDao() { Session = session };
            _countryDao = new CountryDao() { Session = session };
            _userDao = new UserDao() { Session = session };
            _jobSeekerPrivateSettingDao = new JobSeekerPrivateSettingDao() { Session = session };
            _jobSeekerAdditionalInformationDao = new JobSeekerAdditionalInformationDao() { Session = session };
            _jobSeekerAwardDao = new JobSeekerAwardDao() { Session = session };
            _jobSeekerGroupDao = new JobSeekerGroupDao() { Session = session };
            _jobSeekerSkillDao = new JobSeekerSkillDao() { Session = session };
            _jobSeekerPatentsDao = new JobSeekerPatentsDao() { Session = session };
            _jobSeekerMilitaryDao = new JobSeekerMilitaryDao() { Session = session };
            _jobSeekerLinkDao = new JobSeekerLinkDao() { Session = session };
            _jobSeekerPublicationsDao = new JobSeekerPublicationsDao() { Session = session };
            _jobSeekerTrainingCoursesDao = new JobSeekerTrainingCoursesDao() { Session = session };
            _jobSeekerDesiredDao = new JobSeekerDesiredDao() { Session = session };
        }
        #endregion

        #region Operational Function
        public bool Save(JobSeekerProfileVm jobSeekerProfileVm)
        {
            ITransaction trans = null;
            try
            {
                JobSeekerPrivateSetting jobSeekerPrivateSetting = null;
                var profile = new JobSeeker();
                profile.UserProfile = _userDao.GetByAspNetUserId(jobSeekerProfileVm.UserProfileId);
                JobSeeker jobSeeker = _jobSeekerDao.GetAllByProfileId(profile.UserProfile.Id);
                if (jobSeeker != null)
                {
                    jobSeekerPrivateSetting = _jobSeekerPrivateSettingDao.GetByJobSeekerId(jobSeeker.Id);
                    profile = jobSeeker;
                }
                profile.FirstName = jobSeekerProfileVm.FirstName;
                profile.LastName = jobSeekerProfileVm.LastName;
                profile.ContactEmail = jobSeekerProfileVm.ContactEmail;
                profile.ContactNumber = jobSeekerProfileVm.ContactNumber;
                if (jobSeekerProfileVm.ProfileImageBytes != null)
                {
                    profile.ImageGuid = Guid.NewGuid();
                    profile.ProfileImage = jobSeekerProfileVm.ProfileImageBytes;
                }

                var profileDetail = new JobSeekerDetails();
                profileDetail.Address = jobSeekerProfileVm.Address;
                profileDetail.Weblink = jobSeekerProfileVm.Weblink;
                profileDetail.Region = _regionDao.LoadById(jobSeekerProfileVm.RegionId);
                profileDetail.City = _cityDao.LoadById(jobSeekerProfileVm.CityId);
                profileDetail.State = _stateDao.LoadById(jobSeekerProfileVm.StateId);
                profileDetail.Country = _countryDao.LoadById(jobSeekerProfileVm.CountryId);
                //if (jobSeekerProfileVm.CvBytes != null)
                //{
                //    profileDetail.ImageGuid = Guid.NewGuid();
                //    profileDetail.Cv = jobSeekerProfileVm.CvBytes;
                //}
                profileDetail.Linkedin = jobSeekerProfileVm.Linkedin;
                profileDetail.MaritalStatus = jobSeekerProfileVm.MaritalStatus;
                //profileDetail.Dob = jobSeekerProfileVm.Dob;
                //Temporary
                profileDetail.Dob = DateTime.Now;
                //if (profileDetail.Dob <= DateTime.MinValue || profileDetail.Dob >= DateTime.MaxValue) throw new MessageException("invalid date of birth selected");

                profileDetail.Expertise = jobSeekerProfileVm.Expertise;
                profileDetail.FatherName = jobSeekerProfileVm.FatherName;
                profileDetail.MotherName = jobSeekerProfileVm.MotherName;
                profileDetail.Gender = jobSeekerProfileVm.Gender;
                profileDetail.JobSeeker = profile;
                profileDetail.ZipCode = jobSeekerProfileVm.ZipCode;
                profile.JobSeekerDetailses.Clear();
                profile.JobSeekerDetailses.Add(profileDetail);
                using (trans = Session.BeginTransaction())
                {
                    if (profile.Id <= 0)
                        _jobSeekerDao.Save(profile);
                    else
                    {
                        profile.Status = JobSeeker.EntityStatus.Active;
                        _jobSeekerDao.Update(profile);
                    }
                    if (jobSeekerPrivateSetting == null)
                    {
                        jobSeekerPrivateSetting = new JobSeekerPrivateSetting();
                        jobSeekerPrivateSetting.IsPublicResume = jobSeekerProfileVm.IsPublicResume;
                        jobSeekerPrivateSetting.JobSeeker = profile;
                        _jobSeekerPrivateSettingDao.Save(jobSeekerPrivateSetting);
                    }
                    else
                    {
                        jobSeekerPrivateSetting.IsPublicResume = jobSeekerProfileVm.IsPublicResume;
                        jobSeekerPrivateSetting.JobSeeker = profile;
                        _jobSeekerPrivateSettingDao.Update(jobSeekerPrivateSetting);
                    }
                    trans.Commit();
                }
                return true;
            }
            catch (DuplicateEntryException ide)
            {
                throw ide;
            }
            catch (Exception ex)
            {
                if (trans != null && !trans.WasCommitted) { trans.Rollback(); }
                throw ex;
            }
        }

        public void SaveSkill(JobSeekerSkillDto jobSeekerSkillDto)
        {
            ITransaction transaction = null;
            try
            {
                if (jobSeekerSkillDto == null) throw new InvalidDataException("Invalid Object");
                if (jobSeekerSkillDto.Experence <= 0) throw new MessageException("Invalid Experience Selected");
                if (String.IsNullOrEmpty(jobSeekerSkillDto.Skill)) throw new MessageException("Invalid Skill");
                var userProfile = _userDao.GetByAspNetUserId(jobSeekerSkillDto.JobSeeker.ToString());
                JobSeeker jobSeeker = _jobSeekerDao.GetAllByProfileId(userProfile.Id);
                var jSkill = new JobSeekerSkill();
                jSkill.Experence = jobSeekerSkillDto.Experence;
                jSkill.Skill = jobSeekerSkillDto.Skill;
                jSkill.JobSeeker = jobSeeker;

                using (transaction = Session.BeginTransaction())
                {
                    var oldObj = _jobSeekerSkillDao.LoadById(jobSeekerSkillDto.Id);
                    if (oldObj == null)
                    {
                        CheckDuplicateFields(jSkill, jobSeeker.Id);
                        _jobSeekerSkillDao.Save(jSkill);
                    }
                    else
                    {
                        oldObj.Experence = jobSeekerSkillDto.Experence;
                        oldObj.Skill = jobSeekerSkillDto.Skill;
                        oldObj.JobSeeker = jobSeeker;
                        CheckDuplicateFields(oldObj, jobSeeker.Id, oldObj.Id);
                        _jobSeekerSkillDao.Update(oldObj);
                    }
                    transaction.Commit();
                }

            }
            catch (InvalidDataException ex)
            {
                throw ex;

            }
            catch (MessageException ex)
            {
                throw ex;

            }
            catch (DuplicateEntryException ide)
            {
                throw ide;
            }
            catch (Exception ex)
            {
                if (transaction != null && !transaction.WasCommitted) { transaction.Rollback(); }
                throw ex;
            }
        }

        public void SaveLink(JobSeekerLinkDto jobSeekerLinkDto)
        {
            ITransaction transaction = null;
            try
            {
                if (jobSeekerLinkDto == null) throw new InvalidDataException("Invalid Object");
                if (String.IsNullOrEmpty(jobSeekerLinkDto.Link)) throw new MessageException("Invalid Experience Selected");

                var userProfile = _userDao.GetByAspNetUserId(jobSeekerLinkDto.JobSeeker.ToString());
                JobSeeker jobSeeker = _jobSeekerDao.GetAllByProfileId(userProfile.Id);
                var jLink = new JobSeekerLink();
                jLink.Link = jobSeekerLinkDto.Link;
                jLink.JobSeeker = jobSeeker;

                using (transaction = Session.BeginTransaction())
                {
                    var oldObj = _jobSeekerLinkDao.LoadById(jobSeekerLinkDto.Id);
                    if (oldObj == null)
                    {
                        CheckDuplicateFields(jLink, jobSeeker.Id);
                        _jobSeekerLinkDao.Save(jLink);
                    }
                    else
                    {
                        oldObj.Link = jobSeekerLinkDto.Link;
                        oldObj.JobSeeker = jobSeeker;
                        CheckDuplicateFields(oldObj, jobSeeker.Id, oldObj.Id);
                        _jobSeekerLinkDao.Update(oldObj);
                    }
                    transaction.Commit();
                }

            }
            catch (MessageException ex)
            {
                throw ex;

            }
            catch (DuplicateEntryException ide)
            {
                throw ide;
            }
            catch (Exception ex)
            {
                if (transaction != null && !transaction.WasCommitted) { transaction.Rollback(); }
                throw ex;
            }
        }

        public void SaveMilitary(JobSeekerMilitaryDto jobSeekerMilitaryDto)
        {
            ITransaction transaction = null;
            try
            {
                if (jobSeekerMilitaryDto == null) throw new InvalidDataException("Invalid Object");
                if (String.IsNullOrEmpty(jobSeekerMilitaryDto.Branch)) throw new MessageException("Invalid Branch Selected");
                if (String.IsNullOrEmpty(jobSeekerMilitaryDto.Commendations)) throw new MessageException("Invalid Commendations Selected");
                if (String.IsNullOrEmpty(jobSeekerMilitaryDto.Description)) throw new MessageException("Invalid Description Selected");
                if (String.IsNullOrEmpty(jobSeekerMilitaryDto.Rank)) throw new MessageException("Invalid Rank Selected");
                if (String.IsNullOrEmpty(jobSeekerMilitaryDto.DateFrom.ToString())) throw new MessageException("Invalid DateFrom Selected");
                //if (String.IsNullOrEmpty(jobSeekerMilitaryDto.DateTo.ToString())) throw new MessageException("Invalid DateTo Selected");
                if (jobSeekerMilitaryDto.Country <= 0) throw new MessageException("Invalid Country Selected");

                var userProfile = _userDao.GetByAspNetUserId(jobSeekerMilitaryDto.JobSeeker.ToString());
                JobSeeker jobSeeker = _jobSeekerDao.GetAllByProfileId(userProfile.Id);
                var jMilitary = new JobSeekerMilitary();
                jMilitary.Country = _countryDao.LoadById(jobSeekerMilitaryDto.Country);
                jMilitary.Branch = jobSeekerMilitaryDto.Branch;
                jMilitary.Commendations = jobSeekerMilitaryDto.Commendations;
                jMilitary.DateFrom = jobSeekerMilitaryDto.DateFrom;
                jMilitary.DateTo = jobSeekerMilitaryDto.DateTo;
                jMilitary.Description = jobSeekerMilitaryDto.Description;
                jMilitary.IsStillServing = jobSeekerMilitaryDto.IsStillServing;
                jMilitary.JobSeeker = jobSeeker;
                jMilitary.Rank = jobSeekerMilitaryDto.Rank;

                using (transaction = Session.BeginTransaction())
                {
                    var oldObj = _jobSeekerMilitaryDao.LoadById(jobSeekerMilitaryDto.Id);
                    if (oldObj == null)
                    {
                        CheckDuplicateFields(jMilitary, jobSeeker.Id);

                        _jobSeekerMilitaryDao.Save(jMilitary);
                    }
                    else
                    {
                        oldObj.Country = _countryDao.LoadById(jobSeekerMilitaryDto.Country);
                        oldObj.Branch = jobSeekerMilitaryDto.Branch;
                        oldObj.Commendations = jobSeekerMilitaryDto.Commendations;
                        oldObj.DateFrom = jobSeekerMilitaryDto.DateFrom;
                        oldObj.DateTo = jobSeekerMilitaryDto.DateTo;
                        oldObj.Description = jobSeekerMilitaryDto.Description;
                        oldObj.IsStillServing = jobSeekerMilitaryDto.IsStillServing;
                        oldObj.JobSeeker = jobSeeker;
                        oldObj.Rank = jobSeekerMilitaryDto.Rank;

                        CheckDuplicateFields(oldObj, jobSeeker.Id, oldObj.Id);
                        _jobSeekerMilitaryDao.Update(oldObj);
                    }
                    transaction.Commit();
                }

            }
            catch (InvalidDataException ex)
            {
                throw ex;

            }
            catch (MessageException ex)
            {
                throw ex;

            }
            catch (DuplicateEntryException ide)
            {
                throw ide;
            }
            catch (Exception ex)
            {
                if (transaction != null && !transaction.WasCommitted) { transaction.Rollback(); }
                throw ex;
            }
        }

        public void SaveAward(JobSeekerAwardDto jobSeekerAwardDto)
        {
            ITransaction transaction = null;
            try
            {
                var userProfile = _userDao.GetByAspNetUserId(jobSeekerAwardDto.JobSeeker.ToString());
                JobSeeker jobSeeker = _jobSeekerDao.GetAllByProfileId(userProfile.Id);
                var jAward = new JobSeekerAward();
                jAward.Title = jobSeekerAwardDto.Title;
                jAward.Description = jobSeekerAwardDto.Description;
                jAward.DateAwarded = jobSeekerAwardDto.DateAwarded;
                jAward.JobSeeker = jobSeeker;
                ModelValidationCheck(jAward);
                using (transaction = Session.BeginTransaction())
                {
                    var oldObj = _jobSeekerAwardDao.LoadById(jobSeekerAwardDto.Id);
                    if (oldObj == null)
                    {
                        CheckDuplicateFields(jAward, jobSeeker.Id);
                        _jobSeekerAwardDao.Save(jAward);
                    }
                    else
                    {
                        oldObj.Title = jobSeekerAwardDto.Title;
                        oldObj.Description = jobSeekerAwardDto.Description;
                        oldObj.DateAwarded = jobSeekerAwardDto.DateAwarded;
                        oldObj.JobSeeker = jobSeeker;

                        CheckDuplicateFields(oldObj, jobSeeker.Id, oldObj.Id);
                        _jobSeekerAwardDao.Update(oldObj);
                    }
                    transaction.Commit();
                }

            }
            catch (DuplicateEntryException ide)
            {
                throw ide;
            }
            catch (InvalidDataException ex)
            {
                throw ex;

            }
            catch (MessageException ex)
            {
                throw ex;

            }
            catch (Exception ex)
            {
                if (transaction != null && !transaction.WasCommitted) { transaction.Rollback(); }
                throw ex;
            }
        }


        public void SaveCertificate(JobSeekerCertificateDto jobSeekerCertificateDto)
        {
            ITransaction transaction = null;
            try
            {
                var userProfile = _userDao.GetByAspNetUserId(jobSeekerCertificateDto.JobSeeker.ToString());
                JobSeeker jobSeeker = _jobSeekerDao.GetAllByProfileId(userProfile.Id);
                var jCertificate = new JobSeekerTrainingCourses();
                jCertificate.Title = jobSeekerCertificateDto.Title;
                jCertificate.Description = jobSeekerCertificateDto.Description;
                jCertificate.StartDate = jobSeekerCertificateDto.StartDate;
                jCertificate.CloseDate = jobSeekerCertificateDto.CloseDate;
                jCertificate.JobSeeker = jobSeeker;
                ModelValidationCheck(jCertificate);
                using (transaction = Session.BeginTransaction())
                {
                    var oldObj = _jobSeekerTrainingCoursesDao.LoadById(jobSeekerCertificateDto.Id);
                    if (oldObj == null)
                    {
                        CheckDuplicateFields(jCertificate, jobSeeker.Id);
                        _jobSeekerTrainingCoursesDao.Save(jCertificate);
                    }
                    else
                    {
                        oldObj.Title = jobSeekerCertificateDto.Title;
                        oldObj.Description = jobSeekerCertificateDto.Description;
                        oldObj.StartDate = jobSeekerCertificateDto.StartDate;
                        oldObj.CloseDate = jobSeekerCertificateDto.CloseDate;
                        oldObj.JobSeeker = jobSeeker;

                        CheckDuplicateFields(oldObj, jobSeeker.Id, oldObj.Id);
                        _jobSeekerTrainingCoursesDao.Update(oldObj);
                    }
                    transaction.Commit();
                }

            }
            catch (DuplicateEntryException ide)
            {
                throw ide;
            }
            catch (InvalidDataException ex)
            {
                throw ex;

            }
            catch (MessageException ex)
            {
                throw ex;

            }
            catch (Exception ex)
            {
                if (transaction != null && !transaction.WasCommitted) { transaction.Rollback(); }
                throw ex;
            }
        }

        public void SaveGroup(JobSeekerGroupDto jobSeekerGroupDto)
        {
            ITransaction transaction = null;
            try
            {
                var userProfile = _userDao.GetByAspNetUserId(jobSeekerGroupDto.JobSeeker.ToString());
                JobSeeker jobSeeker = _jobSeekerDao.GetAllByProfileId(userProfile.Id);
                var jGroup = new JobSeekerGroup();
                jGroup.Title = jobSeekerGroupDto.Title;
                jGroup.Description = jobSeekerGroupDto.Description;
                jGroup.DateFrom = jobSeekerGroupDto.DateFrom;
                if (!jobSeekerGroupDto.IsStillMember)
                {
                    jGroup.DateTo = jobSeekerGroupDto.DateTo;
                }
                else
                {
                    jGroup.DateTo = DateTime.Now;
                }
                jGroup.IsStillMember = jobSeekerGroupDto.IsStillMember;
                jGroup.JobSeeker = jobSeeker;

                ModelValidationCheck(jGroup);
                using (transaction = Session.BeginTransaction())
                {
                    var oldObj = _jobSeekerGroupDao.LoadById(jobSeekerGroupDto.Id);
                    if (oldObj == null)
                    {
                        CheckDuplicateFields(jGroup, jobSeeker.Id);

                        _jobSeekerGroupDao.Save(jGroup);
                    }
                    else
                    {
                        oldObj.Title = jobSeekerGroupDto.Title;
                        oldObj.Description = jobSeekerGroupDto.Description;
                        oldObj.DateFrom = jobSeekerGroupDto.DateFrom;
                        oldObj.DateTo = jobSeekerGroupDto.DateTo;
                        oldObj.JobSeeker = jobSeeker;

                        CheckDuplicateFields(oldObj, jobSeeker.Id, oldObj.Id);
                        _jobSeekerGroupDao.Update(oldObj);
                    }
                    transaction.Commit();
                }

            }
            catch (DuplicateEntryException ide)
            {
                throw ide;
            }
            catch (InvalidDataException ex)
            {
                throw ex;

            }
            catch (MessageException ex)
            {
                throw ex;

            }
            catch (Exception ex)
            {
                if (transaction != null && !transaction.WasCommitted) { transaction.Rollback(); }
                throw ex;
            }
        }

        public void SavePatent(JobSeekerPatentsDto jobSeekerPatentsDto)
        {
            ITransaction transaction = null;
            try
            {
                var userProfile = _userDao.GetByAspNetUserId(jobSeekerPatentsDto.JobSeeker.ToString());
                JobSeeker jobSeeker = _jobSeekerDao.GetAllByProfileId(userProfile.Id);
                var jGroup = new JobSeekerPatents();
                jGroup.Title = jobSeekerPatentsDto.Title;
                jGroup.Description = jobSeekerPatentsDto.Description;
                jGroup.PatentNo = jobSeekerPatentsDto.PatentNo;
                jGroup.PublishDate = jobSeekerPatentsDto.PublishDate;
                jGroup.Url = jobSeekerPatentsDto.Url;
                jGroup.JobSeeker = jobSeeker;

                ModelValidationCheck(jGroup);
                using (transaction = Session.BeginTransaction())
                {
                    var oldObj = _jobSeekerPatentsDao.LoadById(jobSeekerPatentsDto.Id);
                    if (oldObj == null)
                    {
                        CheckDuplicateFields(jGroup, jobSeeker.Id);

                        _jobSeekerPatentsDao.Save(jGroup);
                    }
                    else
                    {
                        oldObj.Title = jobSeekerPatentsDto.Title;
                        oldObj.Description = jobSeekerPatentsDto.Description;
                        oldObj.PatentNo = jobSeekerPatentsDto.PatentNo;
                        oldObj.PublishDate = jobSeekerPatentsDto.PublishDate;
                        oldObj.JobSeeker = jobSeeker;
                        oldObj.Url = jobSeekerPatentsDto.Url;

                        CheckDuplicateFields(oldObj, jobSeeker.Id, oldObj.Id);
                        _jobSeekerPatentsDao.Update(oldObj);
                    }
                    transaction.Commit();
                }

            }
            catch (DuplicateEntryException ide)
            {
                throw ide;
            }
            catch (InvalidDataException ex)
            {
                throw ex;

            }
            catch (MessageException ex)
            {
                throw ex;

            }
            catch (Exception ex)
            {
                if (transaction != null && !transaction.WasCommitted) { transaction.Rollback(); }
                throw ex;
            }
        }

        public void SavePublication(JobSeekerPublicationsDto jobSeekerPublicationsDto)
        {
            ITransaction transaction = null;
            try
            {
                var userProfile = _userDao.GetByAspNetUserId(jobSeekerPublicationsDto.JobSeeker.ToString());
                JobSeeker jobSeeker = _jobSeekerDao.GetAllByProfileId(userProfile.Id);
                var jGroup = new JobSeekerPublications();
                jGroup.Title = jobSeekerPublicationsDto.Title;
                jGroup.Description = jobSeekerPublicationsDto.Description;
                jGroup.PublishDate = jobSeekerPublicationsDto.PublishDate;
                jGroup.Url = jobSeekerPublicationsDto.Url;
                jGroup.JobSeeker = jobSeeker;

                ModelValidationCheck(jGroup);
                using (transaction = Session.BeginTransaction())
                {
                    var oldObj = _jobSeekerPublicationsDao.LoadById(jobSeekerPublicationsDto.Id);
                    if (oldObj == null)
                    {
                        CheckDuplicateFields(jGroup, jobSeeker.Id);
                        _jobSeekerPublicationsDao.Save(jGroup);
                    }
                    else
                    {
                        oldObj.Title = jobSeekerPublicationsDto.Title;
                        oldObj.Description = jobSeekerPublicationsDto.Description;
                        oldObj.PublishDate = jobSeekerPublicationsDto.PublishDate;
                        oldObj.JobSeeker = jobSeeker;
                        oldObj.Url = jobSeekerPublicationsDto.Url;

                        CheckDuplicateFields(oldObj, jobSeeker.Id, oldObj.Id);
                        _jobSeekerPublicationsDao.Update(oldObj);
                    }
                    transaction.Commit();
                }

            }
            catch (DuplicateEntryException ide)
            {
                throw ide;
            }
            catch (InvalidDataException ex)
            {
                throw ex;

            }
            catch (MessageException ex)
            {
                throw ex;

            }
            catch (Exception ex)
            {
                if (transaction != null && !transaction.WasCommitted) { transaction.Rollback(); }
                throw ex;
            }
        }
        public void SaveDesiredJob(JobSeekerDesiredJobDto jobSeekerDesiredJobDto)
        {
            ITransaction transaction = null;
            try
            {
                if (jobSeekerDesiredJobDto == null) throw new InvalidDataException("Invalid Object");
                if (jobSeekerDesiredJobDto.DesiredSalary <= 0)
                    throw new MessageException("Invalid Salary");

                var userProfile = _userDao.GetByAspNetUserId(jobSeekerDesiredJobDto.JobSeekerId.ToString());
                JobSeeker jobSeeker = _jobSeekerDao.GetAllByProfileId(userProfile.Id);
                var jDesired = new JobSeekerDesiredJob();
                jDesired.DesiredSalary = jobSeekerDesiredJobDto.DesiredSalary;
                jDesired.EmploymentEligibility = jobSeekerDesiredJobDto.EmploymentEligibility;
                jDesired.IsCommission = jobSeekerDesiredJobDto.IsCommission;
                jDesired.IsContract = jobSeekerDesiredJobDto.IsContract;
                jDesired.IsFullTime = jobSeekerDesiredJobDto.IsFullTime;
                jDesired.IsInternship = jobSeekerDesiredJobDto.IsInternship;
                jDesired.IsPartTime = jobSeekerDesiredJobDto.IsPartTime;
                jDesired.IsTemporary = jobSeekerDesiredJobDto.IsTemporary;
                jDesired.RelocatingPlaceOne = jobSeekerDesiredJobDto.RelocatingPlaceOne;
                jDesired.RelocatingPlaceTwo = jobSeekerDesiredJobDto.RelocatingPlaceTwo;
                jDesired.IsRelocate = jobSeekerDesiredJobDto.IsRelocate;
                jDesired.SalaryDurationInDay = jobSeekerDesiredJobDto.SalaryDurationInDay;
                jDesired.JobSeeker = jobSeeker;

                using (transaction = Session.BeginTransaction())
                {
                    var oldObj = _jobSeekerDesiredDao.LoadById(jobSeekerDesiredJobDto.Id);
                    if (oldObj == null)
                    {
                        _jobSeekerDesiredDao.Save(jDesired);
                    }
                    else
                    {
                        oldObj.IsCommission = false;
                        oldObj.IsContract = false;
                        oldObj.IsFullTime = false;
                        oldObj.IsInternship = false;
                        oldObj.IsPartTime = false;
                        oldObj.IsTemporary = false;

                        oldObj.DesiredSalary = jobSeekerDesiredJobDto.DesiredSalary;
                        oldObj.EmploymentEligibility = jobSeekerDesiredJobDto.EmploymentEligibility;
                        oldObj.IsCommission = jobSeekerDesiredJobDto.IsCommission;
                        oldObj.IsContract = jobSeekerDesiredJobDto.IsContract;
                        oldObj.IsFullTime = jobSeekerDesiredJobDto.IsFullTime;
                        oldObj.IsInternship = jobSeekerDesiredJobDto.IsInternship;
                        oldObj.IsPartTime = jobSeekerDesiredJobDto.IsPartTime;
                        oldObj.IsTemporary = jobSeekerDesiredJobDto.IsTemporary;
                        oldObj.RelocatingPlaceOne = jobSeekerDesiredJobDto.RelocatingPlaceOne;
                        oldObj.RelocatingPlaceTwo = jobSeekerDesiredJobDto.RelocatingPlaceTwo;
                        oldObj.IsRelocate = jobSeekerDesiredJobDto.IsRelocate;
                        oldObj.SalaryDurationInDay = jobSeekerDesiredJobDto.SalaryDurationInDay;
                        oldObj.JobSeeker = jobSeeker;

                        _jobSeekerDesiredDao.Update(oldObj);
                    }
                    transaction.Commit();
                }
            }
            catch (DuplicateEntryException ide)
            {
                throw ide;
            }
            catch (InvalidDataException ex)
            {
                throw ex;

            }
            catch (MessageException ex)
            {
                throw ex;

            }
            catch (Exception ex)
            {
                if (transaction != null && !transaction.WasCommitted)
                {
                    transaction.Rollback();
                }
                throw ex;
            }
        }

        public void SaveAdditinalInfo(JobSeekerAdditionalInformationDto jobSeekerAdditionalInformationDto)
        {
            ITransaction transaction = null;
            try
            {
                if (jobSeekerAdditionalInformationDto == null) throw new InvalidDataException("Invalid Object");
                if (String.IsNullOrEmpty(jobSeekerAdditionalInformationDto.Description))
                    throw new MessageException("Invalid Experience Selected");

                var userProfile = _userDao.GetByAspNetUserId(jobSeekerAdditionalInformationDto.JobSeeker.ToString());
                JobSeeker jobSeeker = _jobSeekerDao.GetAllByProfileId(userProfile.Id);
                var jLink = new JobSeekerAdditionalInformation();
                jLink.Description = jobSeekerAdditionalInformationDto.Description;
                jLink.JobSeeker = jobSeeker;

                using (transaction = Session.BeginTransaction())
                {
                    var oldObj = _jobSeekerAdditionalInformationDao.LoadById(jobSeekerAdditionalInformationDto.Id);
                    if (oldObj == null)
                    {
                        CheckDuplicateFields(jLink, jobSeeker.Id);
                        _jobSeekerAdditionalInformationDao.Save(jLink);
                    }
                    else
                    {
                        oldObj.Description = jobSeekerAdditionalInformationDto.Description;
                        oldObj.JobSeeker = jobSeeker;
                        CheckDuplicateFields(oldObj, jobSeeker.Id, oldObj.Id);
                        _jobSeekerAdditionalInformationDao.Update(oldObj);
                    }
                    transaction.Commit();
                }
            }
            catch (DuplicateEntryException ide)
            {
                throw ide;
            }
            catch (InvalidDataException ex)
            {
                throw ex;

            }

            catch (MessageException ex)
            {
                throw ex;

            }
            catch (Exception ex)
            {
                if (transaction != null && !transaction.WasCommitted)
                {
                    transaction.Rollback();
                }
                throw ex;
            }
        }


        #region Delete
        public void DeleteResume(string aspNetUserId)
        {
            ITransaction trans = null;
            try
            {
                if (String.IsNullOrEmpty(aspNetUserId)) throw new InvalidDataException("Invalid user");
                UserProfile userProfile = _userDao.GetByAspNetUserId(aspNetUserId);
                JobSeeker jobSeeker = _jobSeekerDao.GetUsProfileId(userProfile.Id);
                if (jobSeeker == null) throw new InvalidDataException("Please Update Your Profile First");
                if (jobSeeker.JobSeekerJobPosts != null && jobSeeker.JobSeekerJobPosts.Count > 0)
                {
                    if (jobSeeker.JobSeekerJobPosts.Any(jobSeekerJobPost => jobSeekerJobPost.IsApplied))
                    {
                        throw new MessageException("You can't delete your resume because you have already applied for Jobs");
                    }
                }
                jobSeeker.Status = JobSeeker.EntityStatus.Delete;
                jobSeeker.JobSeekerEducationalQualifications.Clear();
                jobSeeker.JobSeekerExperiences.Clear();
                jobSeeker.JobSeekerTrainingCoursesList.Clear();

                using (trans = Session.BeginTransaction())
                {
                    _jobSeekerDao.Update(jobSeeker);
                    trans.Commit();
                }
            }
            catch (InvalidDataException ex)
            {
                throw ex;

            }
            catch (MessageException ex)
            {
                throw ex;

            }
            catch (Exception exception)
            {
                if (trans != null && !trans.WasCommitted) { trans.Rollback(); }
                throw;
            }
        }


        public void DeleteSkill(long id)
        {
            ITransaction trans = null;
            try
            {
                if (id <= 0) throw new InvalidDataException("Invalid Id found");
                JobSeekerSkill dto = _jobSeekerSkillDao.LoadById(id);
                using (trans = Session.BeginTransaction())
                {
                    dto.Status = JobSeekerSkill.EntityStatus.Delete;
                    _jobSeekerSkillDao.Update(dto);
                    trans.Commit();
                }
            }
            catch (InvalidDataException ex)
            {
                throw ex;

            }
            catch (Exception)
            {
                if (trans != null && !trans.WasCommitted) { trans.Rollback(); }
                throw;
            }
        }

        public void DeletePublication(long id)
        {
            ITransaction trans = null;
            try
            {
                if (id <= 0) throw new InvalidDataException("Invalid Id found");
                var dto = _jobSeekerPublicationsDao.LoadById(id);
                using (trans = Session.BeginTransaction())
                {
                    dto.Status = JobSeekerSkill.EntityStatus.Delete;
                    _jobSeekerPublicationsDao.Update(dto);
                    trans.Commit();
                }
            }
            catch (InvalidDataException ex)
            {
                throw ex;

            }
            catch (Exception)
            {
                if (trans != null && !trans.WasCommitted) { trans.Rollback(); }
                throw;
            }
        }

        public void DeleteAward(long id)
        {
            ITransaction trans = null;
            try
            {
                if (id <= 0) throw new InvalidDataException("Invalid Id found");
                JobSeekerAward dto = _jobSeekerAwardDao.LoadById(id);
                using (trans = Session.BeginTransaction())
                {
                    dto.Status = JobSeekerSkill.EntityStatus.Delete;
                    _jobSeekerAwardDao.Update(dto);
                    trans.Commit();
                }
            }
            catch (InvalidDataException ex)
            {
                throw ex;

            }
            catch (Exception)
            {
                if (trans != null && !trans.WasCommitted) { trans.Rollback(); }
                throw;
            }
        }

        public void DeleteAdditionalInfo(long id)
        {
            ITransaction trans = null;
            try
            {
                if (id <= 0) throw new InvalidDataException("Invalid Id found");
                var dto = _jobSeekerAdditionalInformationDao.LoadById(id);
                using (trans = Session.BeginTransaction())
                {
                    dto.Status = JobSeekerSkill.EntityStatus.Delete;
                    _jobSeekerAdditionalInformationDao.Update(dto);
                    trans.Commit();
                }
            }
            catch (InvalidDataException ex)
            {
                throw ex;

            }
            catch (Exception)
            {
                if (trans != null && !trans.WasCommitted) { trans.Rollback(); }
                throw;
            }
        }

        public void DeleteGroup(long id)
        {
            ITransaction trans = null;
            try
            {
                if (id <= 0) throw new InvalidDataException("Invalid Id found");
                var dto = _jobSeekerGroupDao.LoadById(id);
                using (trans = Session.BeginTransaction())
                {
                    dto.Status = JobSeekerSkill.EntityStatus.Delete;
                    _jobSeekerGroupDao.Update(dto);
                    trans.Commit();
                }
            }
            catch (InvalidDataException ex)
            {
                throw ex;

            }
            catch (Exception)
            {
                if (trans != null && !trans.WasCommitted) { trans.Rollback(); }
                throw;
            }
        }

        public void DeleteLink(long id)
        {
            ITransaction trans = null;
            try
            {
                if (id <= 0) throw new InvalidDataException("Invalid Id found");
                var dto = _jobSeekerLinkDao.LoadById(id);
                using (trans = Session.BeginTransaction())
                {
                    dto.Status = JobSeekerSkill.EntityStatus.Delete;
                    _jobSeekerLinkDao.Update(dto);
                    trans.Commit();
                }
            }
            catch (InvalidDataException ex)
            {
                throw ex;

            }
            catch (Exception)
            {
                if (trans != null && !trans.WasCommitted) { trans.Rollback(); }
                throw;
            }
        }

        public void DeleteMalitary(long id)
        {
            ITransaction trans = null;
            try
            {
                if (id <= 0) throw new InvalidDataException("Invalid Id found");
                var dto = _jobSeekerMilitaryDao.LoadById(id);
                using (trans = Session.BeginTransaction())
                {
                    dto.Status = JobSeekerSkill.EntityStatus.Delete;
                    _jobSeekerMilitaryDao.Update(dto);
                    trans.Commit();
                }
            }
            catch (InvalidDataException ex)
            {
                throw ex;

            }
            catch (Exception)
            {
                if (trans != null && !trans.WasCommitted) { trans.Rollback(); }
                throw;
            }
        }

        public void DeletePatent(long id)
        {
            ITransaction trans = null;
            try
            {
                if (id <= 0) throw new InvalidDataException("Invalid Id found");
                var dto = _jobSeekerPatentsDao.LoadById(id);
                using (trans = Session.BeginTransaction())
                {
                    dto.Status = JobSeekerSkill.EntityStatus.Delete;
                    _jobSeekerPatentsDao.Update(dto);
                    trans.Commit();
                }
            }
            catch (InvalidDataException ex)
            {
                throw ex;

            }
            catch (Exception)
            {
                if (trans != null && !trans.WasCommitted) { trans.Rollback(); }
                throw;
            }
        }

        public void DeleteCertificate(long id)
        {
            ITransaction trans = null;
            try
            {
                if (id <= 0) throw new InvalidDataException("Invalid Id found");
                var dto = _jobSeekerTrainingCoursesDao.LoadById(id);
                using (trans = Session.BeginTransaction())
                {
                    dto.Status = JobSeekerSkill.EntityStatus.Delete;
                    _jobSeekerTrainingCoursesDao.Update(dto);
                    trans.Commit();
                }
            }
            catch (InvalidDataException ex)
            {
                throw ex;

            }
            catch (Exception)
            {
                if (trans != null && !trans.WasCommitted) { trans.Rollback(); }
                throw;
            }
        }

        public void DeleteDesiredJobs(long id)
        {
            ITransaction trans = null;
            try
            {
                if (id <= 0) throw new InvalidDataException("Invalid Id found");
                var dto = _jobSeekerDesiredDao.LoadById(id);
                using (trans = Session.BeginTransaction())
                {
                    dto.Status = JobSeekerDesiredJob.EntityStatus.Delete;
                    _jobSeekerDesiredDao.Update(dto);
                    trans.Commit();
                }
            }
            catch (InvalidDataException ex)
            {
                throw ex;
            }
            catch (Exception)
            {
                if (trans != null && !trans.WasCommitted) { trans.Rollback(); }
                throw;
            }
        }


        #endregion

        public void SavePhoto(byte[] file, string aspNetUserId)
        {
            ITransaction trans = null;
            try
            {
                if (String.IsNullOrEmpty(aspNetUserId)) throw new InvalidDataException("Invalid user");
                UserProfile userProfile = _userDao.GetByAspNetUserId(aspNetUserId);
                JobSeeker jobSeeker = _jobSeekerDao.GetUsProfileId(userProfile.Id);
                if (jobSeeker == null) throw new InvalidDataException("Please Update Your Profile First");
                if (file != null)
                {
                    jobSeeker.ImageGuid = Guid.NewGuid();
                    jobSeeker.ProfileImage = file;
                }
                using (trans = Session.BeginTransaction())
                {
                    _jobSeekerDao.Update(jobSeeker);
                    trans.Commit();
                }
            }
            catch (InvalidDataException ex)
            {
                throw ex;

            }
            catch (Exception exception)
            {
                if (trans != null && !trans.WasCommitted) { trans.Rollback(); }
                throw;
            }
        }


       

        public JobSeekerProfileVm GetJobSeekerProfileData(string userId)
        {
            try
            {
                UserProfile userProfile = _userDao.GetByAspNetUserId(userId);
                JobSeeker jobSeeker = _jobSeekerDao.GetUsProfileId(userProfile.Id);
                return CastToProfileVm(jobSeeker);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JobSeekerProfileDto GetJobSeeker(string aspNetUserId)
        {
            try
            {
                UserProfile userProfile = _userDao.GetByAspNetUserId(aspNetUserId);
                JobSeeker jobSeeker = _jobSeekerDao.GetUsProfileId(userProfile.Id);
                var jsDto = new JobSeekerProfileDto();
                if (jobSeeker == null) return jsDto;
                jsDto.JobSeekerProfileVm = CastToProfileVm(jobSeeker);
                jsDto.JobSeekerEduVm = CastToEduVm(jobSeeker.JobSeekerEducationalQualifications.Where(x => x.Status == JobSeekerEducationalQualification.EntityStatus.Active).ToList());
                jsDto.JobSeekerExpVm = CastToExpVm(jobSeeker.JobSeekerExperiences.Where(x => x.Status == JobSeekerEducationalQualification.EntityStatus.Active).ToList());
                jsDto.JobSeekerTrainingVm = CastToTrainVm(jobSeeker.JobSeekerTrainingCoursesList.Where(x => x.Status == JobSeekerEducationalQualification.EntityStatus.Active).ToList());
                return jsDto;
            }
            catch (Exception)
            {
                throw;
            }
        }



        private JobSeekerProfileVm CastToProfileVm(JobSeeker jobSeeker)
        {
            var jsPVm = new JobSeekerProfileVm();
            jsPVm.Id = jobSeeker.Id;
            jsPVm.FirstName = jobSeeker.FirstName;
            jsPVm.LastName = jobSeeker.LastName;
            jsPVm.Address = jobSeeker.JobSeekerDetailses[0].Address;
            jsPVm.ContactEmail = jobSeeker.ContactEmail;
            jsPVm.ContactNumber = jobSeeker.ContactNumber;
            jsPVm.Dob = jobSeeker.JobSeekerDetailses[0].Dob;
            jsPVm.Expertise = jobSeeker.JobSeekerDetailses[0].Expertise;
            jsPVm.FatherName = jobSeeker.JobSeekerDetailses[0].FatherName;
            jsPVm.MotherName = jobSeeker.JobSeekerDetailses[0].MotherName;
            jsPVm.Linkedin = jobSeeker.JobSeekerDetailses[0].Linkedin;
            jsPVm.Weblink = jobSeeker.JobSeekerDetailses[0].Weblink;
            jsPVm.ZipCode = jobSeeker.JobSeekerDetailses[0].ZipCode;
            if (jobSeeker.JobSeekerDetailses[0].Region != null) { jsPVm.RegionId = jobSeeker.JobSeekerDetailses[0].Region.Id; }
            if (jobSeeker.JobSeekerDetailses[0].Country != null) { jsPVm.CountryId = jobSeeker.JobSeekerDetailses[0].Country.Id; }
            if (jobSeeker.JobSeekerDetailses[0].State != null) { jsPVm.StateId = jobSeeker.JobSeekerDetailses[0].State.Id; }
            if (jobSeeker.JobSeekerDetailses[0].City != null) { jsPVm.CityId = jobSeeker.JobSeekerDetailses[0].City.Id; }
            jsPVm.Gender = jobSeeker.JobSeekerDetailses[0].Gender;
            jsPVm.MaritalStatus = jobSeeker.JobSeekerDetailses[0].MaritalStatus;

            return jsPVm;
        }

        private IList<JobSeekerTrainingVm> CastToTrainVm(IList<JobSeekerTrainingCourses> list)
        {
            IList<JobSeekerTrainingVm> jobSeekerEduVms = new List<JobSeekerTrainingVm>();
            if (list != null && list.Count > 0)
            {
                foreach (var jobSeekerEducationalQualification in list)
                {
                    var jsEduVm = new JobSeekerTrainingVm();
                    jsEduVm.Id = jobSeekerEducationalQualification.Id;
                    jsEduVm.Institute = jobSeekerEducationalQualification.Institute;
                    jsEduVm.Description = jobSeekerEducationalQualification.Description;
                    jsEduVm.StartDate = jobSeekerEducationalQualification.StartDate;
                    jsEduVm.Title = jobSeekerEducationalQualification.Title;
                    if (jobSeekerEducationalQualification.CloseDate != null)
                        jsEduVm.CloseDate = (DateTime)jobSeekerEducationalQualification.CloseDate;
                    jobSeekerEduVms.Add(jsEduVm);
                }
            }
            return jobSeekerEduVms;
        }

        private IList<JobSeekerExpVm> CastToExpVm(IList<JobSeekerExperience> list)
        {
            IList<JobSeekerExpVm> jobSeekerEduVms = new List<JobSeekerExpVm>();
            if (list != null && list.Count > 0)
            {
                foreach (var jobSeekerEducationalQualification in list)
                {
                    var jsEduVm = new JobSeekerExpVm();
                    jsEduVm.Id = jobSeekerEducationalQualification.Id;
                    jsEduVm.CompanyName = jobSeekerEducationalQualification.CompanyName;
                    jsEduVm.CompanyAddress = jobSeekerEducationalQualification.CompanyAddress;
                    jsEduVm.DateFrom = jobSeekerEducationalQualification.DateFrom;
                    jsEduVm.DateTo = jobSeekerEducationalQualification.DateTo;
                    jsEduVm.Designation = jobSeekerEducationalQualification.Designation;
                    jsEduVm.IsCurrent = jobSeekerEducationalQualification.IsCurrent;
                    jsEduVm.Responsibility = jobSeekerEducationalQualification.Responsibility;
                    jobSeekerEduVms.Add(jsEduVm);
                }
            }
            return jobSeekerEduVms;
        }

        private IList<JobSeekerEduVm> CastToEduVm(IList<JobSeekerEducationalQualification> list)
        {
            IList<JobSeekerEduVm> jobSeekerEduVms = new List<JobSeekerEduVm>();
            if (list != null && list.Count > 0)
            {
                foreach (var jobSeekerEducationalQualification in list)
                {
                    var jsEduVm = new JobSeekerEduVm();
                    jsEduVm.Id = jobSeekerEducationalQualification.Id;
                    jsEduVm.Institute = jobSeekerEducationalQualification.Institute;
                    jsEduVm.Degree = jobSeekerEducationalQualification.Degree;
                    jsEduVm.PassingYear = jobSeekerEducationalQualification.PassingYear;
                    jsEduVm.StartingYear = jobSeekerEducationalQualification.StartingYear;
                    jsEduVm.Result = jobSeekerEducationalQualification.Result;
                    jsEduVm.FieldOfStudy = jobSeekerEducationalQualification.FieldOfStudy;
                    jobSeekerEduVms.Add(jsEduVm);
                }
            }
            return jobSeekerEduVms;
        }

        #endregion

        #region Single Object Load
        public JobSeekerSkillDto GetJobSeekerSkillDtoByAspNetUserId(long id)
        {
            try
            {
                if (id <= 0) throw new InvalidDataException("Invalid Id Found");
                var obj = _jobSeekerSkillDao.LoadById(id);
                return new JobSeekerSkillDto()
                {
                    Id = obj.Id,
                    Skill = obj.Skill,
                    Experence = obj.Experence,
                    JobSeeker = obj.JobSeeker.Id.ToString()
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JobSeekerPublicationsDto GetJobSeekerPublicationsDtoByAspNetUserId(long id)
        {
            try
            {
                if (id <= 0) throw new InvalidDataException("Invalid Id Found");
                var obj = _jobSeekerPublicationsDao.LoadById(id);
                return new JobSeekerPublicationsDto()
                {
                    Id = obj.Id,
                    JobSeeker = obj.JobSeeker.Id.ToString(),
                    Title = obj.Title,
                    Description = obj.Description,
                    Url = obj.Url,
                    PublishDate = obj.PublishDate
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JobSeekerPatentsDto GetJobSeekerPatentsDtoByAspNetUserId(long id)
        {
            try
            {
                if (id <= 0) throw new InvalidDataException("Invalid Id Found");
                var obj = _jobSeekerPatentsDao.LoadById(id);
                return new JobSeekerPatentsDto()
                {
                    Id = obj.Id,
                    JobSeeker = obj.JobSeeker.Id.ToString(),
                    Title = obj.Title,
                    Description = obj.Description,
                    Url = obj.Url,
                    PatentNo = obj.PatentNo,
                    PublishDate = obj.PublishDate
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JobSeekerMilitaryDto GetJobSeekerMilitaryDtoByAspNetUserId(long id)
        {
            try
            {
                if (id <= 0) throw new InvalidDataException("Invalid Id Found");
                var obj = _jobSeekerMilitaryDao.LoadById(id);
                var returnObj = new JobSeekerMilitaryDto()
                {
                    Id = obj.Id,
                    JobSeeker = obj.JobSeeker.Id.ToString(),
                    Branch = obj.Branch,
                    Commendations = obj.Commendations,
                    Country = obj.Country.Id,
                    DateFrom = obj.DateFrom,
                    Description = obj.Description,
                    IsStillServing = obj.IsStillServing,
                    Rank = obj.Rank
                };
                if (obj.DateTo != null) returnObj.DateTo = (DateTime)obj.DateTo;
                return returnObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JobSeekerLinkDto GetJobSeekerLinkDtoByAspNetUserId(long id)
        {
            try
            {
                if (id <= 0) throw new InvalidDataException("Invalid Id Found");
                var obj = _jobSeekerLinkDao.LoadById(id);
                return new JobSeekerLinkDto()
                {
                    Id = obj.Id,
                    JobSeeker = obj.JobSeeker.Id.ToString(),
                    Link = obj.Link
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JobSeekerGroupDto GetJobSeekerGroupDtoByAspNetUserId(long id)
        {
            try
            {
                if (id <= 0) throw new InvalidDataException("Invalid Id Found");
                var obj = _jobSeekerGroupDao.LoadById(id);
                var returnObj = new JobSeekerGroupDto()
                {
                    Id = obj.Id,
                    JobSeeker = obj.JobSeeker.Id.ToString(),
                    DateFrom = obj.DateFrom,
                    Description = obj.Description,
                    IsStillMember = obj.IsStillMember,
                    Title = obj.Title
                };
                if (obj.DateTo != null) returnObj.DateTo = (DateTime)obj.DateTo;
                return returnObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JobSeekerAwardDto GetJobSeekerAwardDtoByAspNetUserId(long id)
        {
            try
            {
                if (id <= 0) throw new InvalidDataException("Invalid Id Found");
                var obj = _jobSeekerAwardDao.LoadById(id);
                return new JobSeekerAwardDto()
                {
                    Id = obj.Id,
                    JobSeeker = obj.JobSeeker.Id.ToString(),
                    DateAwarded = obj.DateAwarded,
                    Title = obj.Title,
                    Description = obj.Description
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JobSeekerCertificateDto GetJobSeekerCertificateDtoByAspNetUserId(long id)
        {
            try
            {
                if (id <= 0) throw new InvalidDataException("Invalid Id Found");
                var obj = _jobSeekerTrainingCoursesDao.LoadById(id);
                var returnObj = new JobSeekerCertificateDto()
                 {
                     Id = obj.Id,
                     Title = obj.Title,
                     StartDate = obj.StartDate,
                     Description = obj.Description
                 };
                if (obj.CloseDate != null) returnObj.CloseDate = (DateTime)obj.CloseDate;
                return returnObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JobSeekerAdditionalInformationDto GetAdditionalInformationDtosByAspNetUserId(long id)
        {
            try
            {
                if (id <= 0) throw new InvalidDataException("Invalid Id Found");
                var obj = _jobSeekerAdditionalInformationDao.LoadById(id);
                var returnObj = new JobSeekerAdditionalInformationDto()
                {
                    Id = obj.Id,
                    Description = obj.Description
                };
                return returnObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JobSeekerDesiredJobDto GetDesiredDtosByAspNetUserId(long id)
        {
            try
            {
                if (id <= 0) throw new InvalidDataException("Invalid Id Found");
                var obj = _jobSeekerDesiredDao.LoadById(id);
                var returnObj = new JobSeekerDesiredJobDto()
                {
                    Id = obj.Id,
                    DesiredSalary = obj.DesiredSalary,
                    EmploymentEligibility = obj.EmploymentEligibility,
                    IsCommission = obj.IsCommission,
                    IsContract = obj.IsContract,
                    IsFullTime = obj.IsFullTime,
                    IsInternship = obj.IsInternship,
                    IsPartTime = obj.IsPartTime,
                    IsRelocate = obj.IsRelocate,
                    IsTemporary = obj.IsTemporary,
                    JobSeekerId = obj.JobSeeker.Id.ToString(),
                    RelocatingPlaceOne = obj.RelocatingPlaceOne,
                    RelocatingPlaceTwo = obj.RelocatingPlaceTwo,
                    RelocatingPlaceThree = obj.RelocatingPlaceThree,
                    SalaryDurationInDay = obj.SalaryDurationInDay
                };
                return returnObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JobSeekerDto GetJobSeekerById(long jobSeekerId, int status)
        {
            try
            {
                JobSeekerDto jobSeekerDto;

                var jobSeeker = _jobSeekerDao.GetJobSeekerById(jobSeekerId, status); 
                if (jobSeeker!=null)
                {
                    jobSeekerDto = CasTJodSeekerDto(jobSeeker);
                }
                else 
                {
                    jobSeekerDto = null;
                }
                return jobSeekerDto;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public JobSeekerDto GetJobSeekerByAspId(string aspNetUser, int status) 
        { 
            try
            {
                JobSeekerDto jobSeekerDto;
                UserProfile userProfile = _userDao.GetByAspNetUserId(aspNetUser);
                JobSeeker jobSeeker = _jobSeekerDao.GetUsProfileId(userProfile.Id);
                if (jobSeeker != null)
                {
                    jobSeekerDto = CasTJodSeekerDto(jobSeeker);
                }
                else
                {
                    jobSeekerDto = null;
                }
                return jobSeekerDto;
            }
            
            catch (Exception ex)
            {
                throw;
            }
           
        }

        private JobSeekerDto CasTJodSeekerDto(JobSeeker jobSeeker)
        {
            var jobSeekerDto = new JobSeekerDto();
            #region Personal Info
            jobSeekerDto.Id = jobSeeker.Id;
            jobSeekerDto.Name = jobSeeker.Name;
            jobSeekerDto.StatusText = StatusTypeText.GetStatusText(jobSeeker.Status);
            jobSeekerDto.FirstName = jobSeeker.FirstName;
            jobSeekerDto.LastName = jobSeeker.LastName;
            jobSeekerDto.ProfileImage = jobSeeker.ProfileImage;
            jobSeekerDto.ContactEmail = jobSeeker.ContactEmail;
            jobSeekerDto.ContactNumber = jobSeeker.ContactNumber;
            #endregion

            #region Details
            var jobSeekerDetailsDtos = new List<JobSeekerDetailsDto>();
            if (jobSeeker.JobSeekerDetailses.Any())
            {
                var r = jobSeeker.JobSeekerDetailses[0];
                var jobSeekerDetailsDto = new JobSeekerDetailsDto
                {
                    Id = r.Id,
                    StatusText = StatusTypeText.GetStatusText(r.Status),
                    FatherName = r.FatherName,
                    MotherName = r.MotherName,
                    Dob = r.Dob,
                    Gender = r.Gender,
                    MaritalStatus = r.MaritalStatus,
                    Address = r.Address,
                    ZipCode = r.ZipCode,
                    Country = r.Country == null
                        ? null
                        : new CountryDto()
                        {
                            Id = r.Country.Id,
                            Name = r.Country.Name,
                            Region = new RegionDto()
                            {
                                Id = r.Country.Region.Id,
                                Name = r.Country.Region.Name,
                            }
                        },
                    State = r.State == null
                        ? null
                        : new StateDto()
                        {
                            Id = r.State.Id,
                            Name = r.State.Name,
                        },
                    City = r.City == null
                        ? null
                        : new CityDto()
                        {
                            Id = r.City.Id,
                            Name = r.City.Name,
                        }
                };
                jobSeekerDetailsDtos.Add(jobSeekerDetailsDto);
            }
            jobSeekerDto.JobSeekerDetailList = jobSeekerDetailsDtos;

            #endregion

            #region Desire Job
            var jobSeekerDesiredJobDtos = new List<JobSeekerDesiredJobDto>();
            if (jobSeeker.JobSeekerDesiredJobs.Any(x => x.Status == JobSeekerDesiredJob.EntityStatus.Active))
            {
                var dj = jobSeeker.JobSeekerDesiredJobs[0];
                var jobSeekerDesiredJobDto = new JobSeekerDesiredJobDto
                {
                    Id = dj.Id,
                    DesiredSalary = dj.DesiredSalary,
                    SalaryDurationInDay = dj.SalaryDurationInDay,
                    IsRelocate = dj.IsRelocate,
                    EmploymentEligibility = dj.EmploymentEligibility,
                    IsPartTime = dj.IsPartTime,
                    IsInternship = dj.IsInternship,
                    IsTemporary = dj.IsTemporary,
                    IsFullTime = dj.IsFullTime,
                    IsCommission = dj.IsCommission,
                    IsContract = dj.IsContract,
                    RelocatingPlaceOne = dj.RelocatingPlaceOne,
                    RelocatingPlaceTwo = dj.RelocatingPlaceTwo,
                    RelocatingPlaceThree = dj.RelocatingPlaceThree
                };
                jobSeekerDesiredJobDtos.Add(jobSeekerDesiredJobDto);
            }
            jobSeekerDto.JobSeekerDesiredJobList = jobSeekerDesiredJobDtos;
            #endregion

            #region JobSeeker Work Experience
            var jobSeekerExperienceDtos = new List<JobSeekerExperienceDto>();
            if (jobSeeker.JobSeekerExperiences.Any())
            {
                jobSeekerExperienceDtos.AddRange(jobSeeker.JobSeekerExperiences.Select(jobExperience => new JobSeekerExperienceDto
                {
                    Id = jobExperience.Id,
                    CompanyName = jobExperience.CompanyName,
                    CompanyAddress = jobExperience.CompanyAddress,
                    Designation = jobExperience.Designation,
                    DateFrom = jobExperience.DateFrom,
                    DateTo = jobExperience.DateTo,
                    Responsibility = jobExperience.Responsibility,
                    IsCurrent = jobExperience.IsCurrent
                }));
            }
            jobSeekerDto.JobSeekerExperienceList = jobSeekerExperienceDtos;
            #endregion

            #region JobSeeker Education
            var jobSeekerEducationDtos = new List<JobSeekerEducationDto>();
            if (jobSeeker.JobSeekerEducationalQualifications.Any())
            {
                jobSeekerEducationDtos.AddRange(jobSeeker.JobSeekerEducationalQualifications.Select(
                    jobEducational => new JobSeekerEducationDto
                    {
                        Id = jobEducational.Id,
                        Degree = jobEducational.Degree,
                        Institute = jobEducational.Institute,
                        FieldOfStudy = jobEducational.FieldOfStudy,
                        StartingYear = jobEducational.StartingYear,
                        PassingYear = jobEducational.PassingYear
                    }));
            }
            jobSeekerDto.JobSeekerEducationList = jobSeekerEducationDtos;
            #endregion

            #region JobSeeker Skill
            var jobSeekerSkillDtos = new List<JobSeekerSkillDto>();
            if (jobSeeker.JobSeekerSkills.Any())
            {
                jobSeekerSkillDtos.AddRange(jobSeeker.JobSeekerSkills.Select(
                    jobSeekerSkill => new JobSeekerSkillDto
                    {
                        Id = jobSeekerSkill.Id,
                        Experence = jobSeekerSkill.Experence,
                        Skill = jobSeekerSkill.Skill
                    }));
            }
            jobSeekerDto.JobSeekerSkillList = jobSeekerSkillDtos;
            #endregion

            #region JobSeeker Link
            var jobSeekerLinkDtos = new List<JobSeekerLinkDto>();
            if (jobSeeker.JobSeekerLinks.Any())
            {
                jobSeekerLinkDtos.AddRange(jobSeeker.JobSeekerLinks.Select(
                    jobSeekerLinks => new JobSeekerLinkDto
                    {
                        Id = jobSeekerLinks.Id,
                        Link = jobSeekerLinks.Link
                    }));
            }
            jobSeekerDto.JobSeekerLinkList = jobSeekerLinkDtos;
            #endregion

            #region JobSeeker Military
            var jobSeekerMilitaryDtos = new List<JobSeekerMilitaryServiceDto>();
            if (jobSeeker.JobSeekerMilitarys.Any())
            {
                jobSeekerMilitaryDtos.AddRange(jobSeeker.JobSeekerMilitarys.Select(
                    jobSeekerMilitarys => new JobSeekerMilitaryServiceDto
                    {
                        Id = jobSeekerMilitarys.Id,
                        Country = jobSeekerMilitarys.Country == null
                        ? null
                        : new CountryDto()
                        {
                            Id = jobSeekerMilitarys.Country.Id,
                            Name = jobSeekerMilitarys.Country.Name,
                        },
                        Branch = jobSeekerMilitarys.Branch,
                        Rank = jobSeekerMilitarys.Rank,
                        IsStillServing = jobSeekerMilitarys.IsStillServing,
                        DateFrom = jobSeekerMilitarys.DateFrom,
                        DateTo = jobSeekerMilitarys.DateTo,
                        Description = jobSeekerMilitarys.Description,
                        Commendations = jobSeekerMilitarys.Commendations

                    }));
            }
            jobSeekerDto.JobSeekerMilitaryServiceList = jobSeekerMilitaryDtos;
            #endregion

            #region JobSeeker Award
            var jobAwardDtos = new List<JobSeekerAwardDto>();
            if (jobSeeker.JobSeekerAwards.Any())
            {
                jobAwardDtos.AddRange(jobSeeker.JobSeekerAwards.Select(
                    jobSeekerAwardDto => new JobSeekerAwardDto
                    {
                        Id = jobSeekerAwardDto.Id,
                        Title = jobSeekerAwardDto.Title,
                        Description = jobSeekerAwardDto.Description,
                        DateAwarded = jobSeekerAwardDto.DateAwarded
                    }));
            }
            jobSeekerDto.JobSeekerAwardList = jobAwardDtos;
            #endregion

            #region JobSeeker Certificate
            var jobSeekerCertificateDtos = new List<JobSeekerCertificateDto>();
            if (jobSeeker.JobSeekerTrainingCoursesList.Any())
            {
                jobSeekerCertificateDtos.AddRange(jobSeeker.JobSeekerTrainingCoursesList.Select(
                    jobSeekerCertificateDto => new JobSeekerCertificateDto
                    {
                        Id = jobSeekerCertificateDto.Id,
                        Title = jobSeekerCertificateDto.Title,
                        Description = jobSeekerCertificateDto.Description,
                        Institute = jobSeekerCertificateDto.Institute,
                        StartDate = jobSeekerCertificateDto.StartDate,
                        CloseDate = jobSeekerCertificateDto.CloseDate
                    }));
            }
            jobSeekerDto.JobSeekerCertificateList = jobSeekerCertificateDtos;
            #endregion

            #region JobSeeker Groups
            var jobSeekerGroupsDtos = new List<JobSeekerGroupDto>();
            if (jobSeeker.JobSeekerGroups.Any())
            {
                jobSeekerGroupsDtos.AddRange(jobSeeker.JobSeekerGroups.Select(
                    jobSeekerGroup => new JobSeekerGroupDto
                    {
                        Id = jobSeekerGroup.Id,
                        Title = jobSeekerGroup.Title,
                        Description = jobSeekerGroup.Description,
                        DateFrom = jobSeekerGroup.DateFrom,
                        DateTo = jobSeekerGroup.DateTo,
                        IsStillMember = jobSeekerGroup.IsStillMember
                    }));
            }
            jobSeekerDto.JobSeekerGroupList = jobSeekerGroupsDtos;
            #endregion

            #region JobSeeker Patents
            var jobSeekerPatentsDtos = new List<JobSeekerPatentsDto>();
            if (jobSeeker.JobSeekerPatentses.Any())
            {
                jobSeekerPatentsDtos.AddRange(jobSeeker.JobSeekerPatentses.Select(
                    jobSeekerPatents => new JobSeekerPatentsDto
                    {
                        Id = jobSeekerPatents.Id,
                        Title = jobSeekerPatents.Title,
                        Description = jobSeekerPatents.Description,
                        PatentNo = jobSeekerPatents.PatentNo,
                        Url = jobSeekerPatents.Url,
                        PublishDate = jobSeekerPatents.PublishDate
                    }));
            }
            jobSeekerDto.JobSeekerPatentsList = jobSeekerPatentsDtos;
            #endregion

            #region JobSeeker Publications
            var jobSeekerPublicationsDtos = new List<JobSeekerPublicationsDto>();
            if (jobSeeker.JobSeekerPublicationses.Any())
            {
                jobSeekerPublicationsDtos.AddRange(jobSeeker.JobSeekerPublicationses.Select(
                    jobSeekerPublications => new JobSeekerPublicationsDto
                    {
                        Id = jobSeekerPublications.Id,
                        Title = jobSeekerPublications.Title,
                        Description = jobSeekerPublications.Description,
                        Url = jobSeekerPublications.Url,
                        PublishDate = jobSeekerPublications.PublishDate
                    }));
            }
            jobSeekerDto.JobSeekerPublicationsList = jobSeekerPublicationsDtos;
            #endregion

            #region JobSeeker Additional Information
            var jobSeekerAdditionalInformationDtos = new List<JobSeekerAdditionalInformationDto>();
            if (jobSeeker.JobSeekerAdditionalInformations.Any(x => x.Status == JobSeekerAdditionalInformation.EntityStatus.Active))
            {
                jobSeekerAdditionalInformationDtos.AddRange(jobSeeker.JobSeekerAdditionalInformations.Select(
                    jobSeekerAdditionalInformation => new JobSeekerAdditionalInformationDto
                    {
                        Id = jobSeekerAdditionalInformation.Id,
                        Description = jobSeekerAdditionalInformation.Description
                    }));
            }
            jobSeekerDto.JobSeekerAdditionalInformationList = jobSeekerAdditionalInformationDtos;
            #endregion

            return jobSeekerDto;
        }

        #endregion

        #region List of Loading function

        public List<JobSeekerDto> LoadOnlyJobSeeker(int? status = null)
        {
            try
            {
                var jobseekerList = _jobSeekerDao.LoadOnlyJobSeeker(status ?? 0);
                return jobseekerList.Select(c => new JobSeekerDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    StatusText = StatusTypeText.GetStatusText(c.Status),
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    ProfileImage = c.ProfileImage,
                    ContactEmail = c.ContactEmail,
                    ContactNumber = c.ContactNumber,
                    JobSeekerDetailList = c.JobSeekerDetailses.Select(r => new JobSeekerDetailsDto()
                    {
                        Id = r.Id,
                        StatusText = StatusTypeText.GetStatusText(r.Status),
                        FatherName = r.FatherName,
                        MotherName = r.MotherName,
                        Dob = r.Dob,
                        Gender = r.Gender,
                        MaritalStatus = r.MaritalStatus,
                        Address = r.Address,
                        ZipCode = r.ZipCode,
                        Country = r.Country == null
                            ? null
                            : new CountryDto()
                        {
                            Id = r.Country.Id,
                            Name = r.Country.Name,
                            Region = new RegionDto()
                            {
                                Id = r.Country.Region.Id,
                                Name = r.Country.Region.Name,
                            }
                        },
                        State = r.State == null
                            ? null
                            : new StateDto()
                            {
                                Id = r.State.Id,
                                Name = r.State.Name,
                            },
                        City = r.City == null
                            ? null
                            : new CityDto()
                        {
                            Id = r.City.Id,
                            Name = r.City.Name,
                        }

                    }).ToList()
                }).ToList();

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<JobSeekerDto> LoadJobSeekerForWebAdmin(int start, int length, string orderBy, string orderDir, int? status = null,
           string firstName = "", string lastName = "", string contactMobile = "", string contactEmail = "", string zip = "",
           long? region = null, long? country = null, long? state = null, long? city = null)
        {
            try
            {
                var jobseekerList = _jobSeekerDao.LoadJobSeekerForWebAdmin(start, length, orderBy, orderDir,
                  status ?? 0, firstName, lastName, contactMobile, contactEmail, zip,
                  region ?? 0, country ?? 0, state ?? 0, city ?? 0);

                return jobseekerList.Select(c => new JobSeekerDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    StatusText = StatusTypeText.GetStatusText(c.Status),
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    ProfileImage = c.ProfileImage,
                    ContactEmail = c.ContactEmail,
                    ContactNumber = c.ContactNumber,
                    UserProfile = c.UserProfile == null ? null : new UserProfileDto()
                    {
                        AspNetUserId = c.UserProfile.AspNetUserId,
                        IsBlock = c.UserProfile.IsBlock
                    },
                    JobSeekerDetailList = c.JobSeekerDetailses.Select(r => new JobSeekerDetailsDto()
                    {
                        Id = r.Id,
                        StatusText = StatusTypeText.GetStatusText(r.Status),
                        FatherName = r.FatherName,
                        MotherName = r.MotherName,
                        Dob = r.Dob,
                        Gender = r.Gender,
                        MaritalStatus = r.MaritalStatus,
                        Address = r.Address,
                        ZipCode = r.ZipCode,
                        Country = r.Country == null
                            ? null
                            : new CountryDto()
                            {
                                Id = r.Country.Id,
                                Name = r.Country.Name,
                                Region = new RegionDto()
                                {
                                    Id = r.Country.Region.Id,
                                    Name = r.Country.Region.Name,
                                }
                            },
                        State = r.State == null
                            ? null
                            : new StateDto()
                            {
                                Id = r.State.Id,
                                Name = r.State.Name,
                            },
                        City = r.City == null
                            ? null
                            : new CityDto()
                            {
                                Id = r.City.Id,
                                Name = r.City.Name,
                            }

                    }).ToList()
                }).ToList();

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<JobSeekerSearchDto> LoadJobSeekerSearch(int start, int length, string orderBy, string orderDir, string whatKey, string whereKey
            , string[] jobTitle, string[] yearExp, string[] education, string[] company
            )
        {
            try
            {
                return _jobSeekerDao.LoadJobSeekerSearch(start, length, orderBy, orderDir, whatKey, whereKey,jobTitle,yearExp,education,company);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<FindResumeFilterDto> LoadFindResumeFiteDtoList()
        {
            try
            {
                return _jobSeekerDao.LoadFindResumeFiteDtoList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IList<JobSeekerAdditionalInformationDto> LoadAditionalAdditionalInformationDtosByAspNetUserId(string aspNetUserId)
        {
            UserProfile userProfile = _userDao.GetByAspNetUserId(aspNetUserId);
            JobSeeker jobSeeker = _jobSeekerDao.GetUsProfileId(userProfile.Id);
            IList<JobSeekerAdditionalInformation> list =
                _jobSeekerAdditionalInformationDao.LoadByJobSeekerId(jobSeeker.Id);
            return list.Select(x => new JobSeekerAdditionalInformationDto()
            {
                Id = x.Id,
                Description = x.Description,
                JobSeeker = x.JobSeeker.Id.ToString()
            }).ToList();
        }
        public IList<JobSeekerAwardDto> LoadJobSeekerAwardDtoByAspNetUserId(string aspNetUserId)
        {
            UserProfile userProfile = _userDao.GetByAspNetUserId(aspNetUserId);
            JobSeeker jobSeeker = _jobSeekerDao.GetUsProfileId(userProfile.Id);
            IList<JobSeekerAward> list = _jobSeekerAwardDao.LoadByJobSeekerId(jobSeeker.Id);
            return list.Select(x => new JobSeekerAwardDto()
            {
                Id = x.Id,
                Title = x.Title,
                DateAwarded = x.DateAwarded,
                Description = x.Description,
                JobSeeker = x.JobSeeker.Id.ToString()
            }).ToList();
        }
        public IList<JobSeekerGroupDto> LoadJobSeekerGroupDtoByAspNetUserId(string aspNetUserId)
        {
            UserProfile userProfile = _userDao.GetByAspNetUserId(aspNetUserId);
            JobSeeker jobSeeker = _jobSeekerDao.GetUsProfileId(userProfile.Id);
            IList<JobSeekerGroup> list =
                _jobSeekerGroupDao.LoadByJobSeekerId(jobSeeker.Id);
            return list.Select(x => new JobSeekerGroupDto()
            {
                Id = x.Id,
                Description = x.Description,
                JobSeeker = x.JobSeeker.Id.ToString(),
                DateFrom = x.DateFrom,
                DateTo = (DateTime)x.DateTo,
                IsStillMember = x.IsStillMember,
                Title = x.Title
            }).ToList();
        }
        public IList<JobSeekerLinkDto> LoadJobSeekerLinkDtoByAspNetUserId(string aspNetUserId)
        {
            UserProfile userProfile = _userDao.GetByAspNetUserId(aspNetUserId);
            JobSeeker jobSeeker = _jobSeekerDao.GetUsProfileId(userProfile.Id);
            IList<JobSeekerLink> list =
                _jobSeekerLinkDao.LoadByJobSeekerId(jobSeeker.Id);
            return list.Select(x => new JobSeekerLinkDto()
            {
                Id = x.Id,
                Link = x.Link,
                JobSeeker = x.JobSeeker.Id.ToString()
            }).ToList();
        }
        public IList<JobSeekerMilitaryDto> LoadJobSeekerMilitaryDtoByAspNetUserId(string aspNetUserId)
        {
            UserProfile userProfile = _userDao.GetByAspNetUserId(aspNetUserId);
            JobSeeker jobSeeker = _jobSeekerDao.GetUsProfileId(userProfile.Id);
            IList<JobSeekerMilitary> list =
                _jobSeekerMilitaryDao.LoadByJobSeekerId(jobSeeker.Id);
            return list.Select(x => new JobSeekerMilitaryDto()
            {
                Id = x.Id,
                Branch = x.Branch,
                Commendations = x.Commendations,
                Country = x.Country.Id,
                DateFrom = x.DateFrom,
                DateTo = x.DateTo,
                IsStillServing = x.IsStillServing,
                Description = x.Description,
                JobSeeker = x.JobSeeker.Id.ToString(),
                Rank = x.Rank
            }).ToList();
        }
        public IList<JobSeekerPatentsDto> LoadJobSeekerPatentsDtoByAspNetUserId(string aspNetUserId)
        {
            UserProfile userProfile = _userDao.GetByAspNetUserId(aspNetUserId);
            JobSeeker jobSeeker = _jobSeekerDao.GetUsProfileId(userProfile.Id);
            IList<JobSeekerPatents> list = _jobSeekerPatentsDao.LoadByJobSeekerId(jobSeeker.Id);
            return list.Select(x => new JobSeekerPatentsDto()
            {
                Id = x.Id,
                Description = x.Description,
                JobSeeker = x.JobSeeker.Id.ToString(),
                PatentNo = x.PatentNo,
                PublishDate = x.PublishDate,
                Title = x.Title,
                Url = x.Url
            }).ToList();
        }
        public IList<JobSeekerPublicationsDto> LoadJobSeekerPublicationsDtoByAspNetUserId(string aspNetUserId)
        {
            UserProfile userProfile = _userDao.GetByAspNetUserId(aspNetUserId);
            JobSeeker jobSeeker = _jobSeekerDao.GetUsProfileId(userProfile.Id);
            IList<JobSeekerPublications> list =
                _jobSeekerPublicationsDao.LoadByJobSeekerId(jobSeeker.Id);
            return list.Select(x => new JobSeekerPublicationsDto()
            {
                Id = x.Id,
                Description = x.Description,
                JobSeeker = x.JobSeeker.Id.ToString(),
                PublishDate = x.PublishDate,
                Title = x.Title,
                Url = x.Url
            }).ToList();
        }
        public IList<JobSeekerSkillDto> LoadJobSeekerSkillDtoByAspNetUserId(string aspNetUserId)
        {
            UserProfile userProfile = _userDao.GetByAspNetUserId(aspNetUserId);
            JobSeeker jobSeeker = _jobSeekerDao.GetUsProfileId(userProfile.Id);
            IList<JobSeekerSkill> list =
                _jobSeekerSkillDao.LoadByJobSeekerId(jobSeeker.Id);
            return list.Select(x => new JobSeekerSkillDto()
            {
                Id = x.Id,
                JobSeeker = x.JobSeeker.Id.ToString(),
                Experence = x.Experence,
                Skill = x.Skill
            }).ToList();
        }

        public IList<JobSeekerCertificateDto> LoadJobSeekerCertificateDtoByAspNetUserId(string aspNetUserId)
        {
            UserProfile userProfile = _userDao.GetByAspNetUserId(aspNetUserId);
            JobSeeker jobSeeker = _jobSeekerDao.GetUsProfileId(userProfile.Id);
            return jobSeeker.JobSeekerTrainingCoursesList.Where(x => x.Status == JobSeekerTrainingCourses.EntityStatus.Active).Select(x => new JobSeekerCertificateDto()
            {
                Id = x.Id,
                JobSeeker = x.JobSeeker.Id.ToString(),
                Title = x.Title,
                StartDate = x.StartDate,
                CloseDate = x.CloseDate,
                Description = x.Description,
            }).ToList();
        }

        public IList<JobSeekerDesiredJobDto> LoadJobSeekerDesiredAspNetUserId(string aspNetUserId)
        {
            UserProfile userProfile = _userDao.GetByAspNetUserId(aspNetUserId);
            JobSeeker jobSeeker = _jobSeekerDao.GetUsProfileId(userProfile.Id);
            return jobSeeker.JobSeekerDesiredJobs.Where(x => x.Status == JobSeekerDesiredJob.EntityStatus.Active).Select(x => new JobSeekerDesiredJobDto()
            {
                Id = x.Id,
                JobSeekerId = x.JobSeeker.Id.ToString(),
                DesiredSalary = x.DesiredSalary,
                EmploymentEligibility = x.EmploymentEligibility,
                IsCommission = x.IsCommission,
                IsContract = x.IsContract,
                IsFullTime = x.IsFullTime,
                IsInternship = x.IsInternship,
                IsPartTime = x.IsPartTime,
                IsRelocate = x.IsRelocate,
                IsTemporary = x.IsTemporary,
                RelocatingPlaceOne = x.RelocatingPlaceOne,
                RelocatingPlaceThree = x.RelocatingPlaceThree,
                RelocatingPlaceTwo = x.RelocatingPlaceTwo,
                SalaryDurationInDay = x.SalaryDurationInDay,
            }).ToList();
        }
        #endregion

        #region Other Function

        private void CheckDuplicateFields(JobSeekerSkill jobSeekerSkillModel, long jobSeekerId, long id = 0)
        {
            try
            {
                var isDuplicateName = _jobSeekerSkillDao.CheckDuplicateFields(id, jobSeekerId, jobSeekerSkillModel.Skill);
                if (isDuplicateName)
                    throw new DuplicateEntryException("Already added this skill");

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void CheckDuplicateFields(JobSeekerLink jobSeekerLinkModel, long jobSeekerId, long id = 0)
        {
            try
            {
                var isDuplicateName = _jobSeekerLinkDao.CheckDuplicateFields(id, jobSeekerId, jobSeekerLinkModel.Link);
                if (isDuplicateName)
                    throw new DuplicateEntryException("Already added this Link");

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void CheckDuplicateFields(JobSeekerMilitary jobSeekerMilitaryModel, long jobSeekerId, long id = 0)
        {
            try
            {
                bool isDuplicateName = _jobSeekerMilitaryDao.CheckDuplicateFields(id, jobSeekerId, jobSeekerMilitaryModel.DateFrom);
                if (isDuplicateName)
                    throw new DuplicateEntryException("Already added this Military Training");

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void CheckDuplicateFields(JobSeekerAward jobSeekerAwardModel, long jobSeekerId, long id = 0)
        {
            try
            {
                var isDuplicateName = _jobSeekerAwardDao.CheckDuplicateFields(id, jobSeekerId, jobSeekerAwardModel.Title, jobSeekerAwardModel.DateAwarded);
                if (isDuplicateName)
                    throw new DuplicateEntryException("Already added this Award");

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void CheckDuplicateFields(JobSeekerTrainingCourses jobSeekerTrainModel, long jobSeekerId, long id = 0)
        {
            try
            {
                var isDuplicateName = _jobSeekerTrainingCoursesDao.CheckDuplicateFields(id, jobSeekerId, jobSeekerTrainModel.Institute, jobSeekerTrainModel.Title);
                if (isDuplicateName)
                    throw new DuplicateEntryException("Already added this Certificate");

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void CheckDuplicateFields(JobSeekerGroup jobSeekerGroupModel, long jobSeekerId, long id = 0)
        {
            try
            {
                var isDuplicateName = _jobSeekerGroupDao.CheckDuplicateFields(id, jobSeekerId, jobSeekerGroupModel.Title);
                if (isDuplicateName)
                    throw new DuplicateEntryException("Already added this Group");

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void CheckDuplicateFields(JobSeekerPatents jobSeekerPatentModel, long jobSeekerId, long id = 0)
        {
            try
            {
                var isDuplicateName = _jobSeekerPatentsDao.CheckDuplicateFields(id, jobSeekerId, jobSeekerPatentModel.Title);
                if (isDuplicateName)
                    throw new DuplicateEntryException("Already added this Patent");

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void CheckDuplicateFields(JobSeekerPublications jobSeekerPublicationModel, long jobSeekerId, long id = 0)
        {
            try
            {
                var isDuplicateName = _jobSeekerPublicationsDao.CheckDuplicateFields(id, jobSeekerId, jobSeekerPublicationModel.Title);
                if (isDuplicateName)
                    throw new DuplicateEntryException("Already added this Publication");

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void CheckDuplicateFields(JobSeekerAdditionalInformation jobSeekerAdditionalModel, long jobSeekerId, long id = 0)
        {
            try
            {
                var isDuplicateName = _jobSeekerAdditionalInformationDao.CheckDuplicateFields(id, jobSeekerId, jobSeekerAdditionalModel.Description);
                if (isDuplicateName)
                    throw new DuplicateEntryException("Already added this Additional Information");

            }
            catch (Exception ex)
            {
                throw;
            }
        }


        #endregion

        #region Helper
        private void ModelValidationCheck(JobSeekerAward jobSeekerAward)
        {
            try
            {
                var validationResult = ValidationHelper.ValidateEntity<JobSeekerAward, JobSeekerAward>(jobSeekerAward);
                if (validationResult.HasError)
                {
                    string errorMessage = "";
                    validationResult.Errors.ForEach(r => errorMessage = errorMessage + r.ErrorMessage + Environment.NewLine);
                    throw new InvalidDataException(errorMessage);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void ModelValidationCheck(JobSeekerTrainingCourses jobSeekerTrainingCourses)
        {
            try
            {
                var validationResult = ValidationHelper.ValidateEntity<JobSeekerTrainingCourses, JobSeekerTrainingCourses>(jobSeekerTrainingCourses);
                if (validationResult.HasError)
                {
                    string errorMessage = "";
                    validationResult.Errors.ForEach(r => errorMessage = errorMessage + r.ErrorMessage + Environment.NewLine);
                    throw new InvalidDataException(errorMessage);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void ModelValidationCheck(JobSeekerGroup jGroup)
        {
            try
            {
                var validationResult = ValidationHelper.ValidateEntity<JobSeekerGroup, JobSeekerGroup>(jGroup);
                if (validationResult.HasError)
                {
                    string errorMessage = "";
                    validationResult.Errors.ForEach(r => errorMessage = errorMessage + r.ErrorMessage + Environment.NewLine);
                    throw new InvalidDataException(errorMessage);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void ModelValidationCheck(JobSeekerPatents jGroup)
        {
            try
            {
                var validationResult = ValidationHelper.ValidateEntity<JobSeekerPatents, JobSeekerPatents>(jGroup);
                if (validationResult.HasError)
                {
                    string errorMessage = "";
                    validationResult.Errors.ForEach(r => errorMessage = errorMessage + r.ErrorMessage + Environment.NewLine);
                    throw new InvalidDataException(errorMessage);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void ModelValidationCheck(JobSeekerPublications jGroup)
        {
            try
            {
                var validationResult = ValidationHelper.ValidateEntity<JobSeekerPublications, JobSeekerPublications>(jGroup);
                if (validationResult.HasError)
                {
                    string errorMessage = "";
                    validationResult.Errors.ForEach(r => errorMessage = errorMessage + r.ErrorMessage + Environment.NewLine);
                    throw new InvalidDataException(errorMessage);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public int JobSeekerRowCount(int? status = null, string firstName = null, string lastName = "", string contactMobile = "",
                  string contactEmail = "", string zip = "", long? region = null, long? country = null, long? state = null,
                  long? city = null)
        {
            try
            {
                return _jobSeekerDao.JobSeekerRowCount(status ?? 0, firstName, lastName, contactMobile, contactEmail, zip,
                    region ?? 0, country ?? 0, state ?? 0, city ?? 0
                    );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int JobSeekerSearchRowCount(string whatKey, string whereKey, string[] jobTitle, string[] yearExp, string[] education, string[] company)
        {
            try
            {
                return _jobSeekerDao.JobSeekerSearchRowCount(whatKey, whereKey,jobTitle,yearExp,education,company);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

    }
}
