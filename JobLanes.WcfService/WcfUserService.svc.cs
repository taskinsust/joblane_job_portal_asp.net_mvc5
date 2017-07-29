using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using JobLanes.WcfService.ModelBuilder;
using JobLanes.WcfService.Models;
using Model.JobLanes;
using Model.JobLanes.Dto;
using Model.JobLanes.Entity.User;
using Model.JobLanes.Helper;
using Model.JobLanes.ViewModel;
using NHibernate;
using Services.Joblanes;
using Services.Joblanes.Exceptions;
using Services.Joblanes.Helper;

namespace JobLanes.WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "WcfUserService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select WcfUserService.svc or WcfUserService.svc.cs at the Solution Explorer and start debugging.
    public class WcfUserService : ConnectionBase, IWcfUserService
    {
        private readonly IUserService _userService;
        private readonly IEmailServiceNew _emailSevice;
        private readonly IJobseekerService _jobseekerService;
        private readonly IJobSeekerDetailsService _jobSeekerDetailsService;
        private readonly IJobSeekerEducationalQualificationService _jobSeekerEducationalQualificationService;
        private readonly IJobSeekerExperienceService _jobSeekerExperienceService;
        private readonly IJobSeekerTrainingCoursesService _jobSeekerTrainingCoursesService;
        private readonly IJobPostService _jobPostService;
        private readonly IJobSeekerJobPostService _jobSeekerJobPostService;
        private readonly IJobSeekerCvBankService _jobSeekerCvBankService;
        private readonly IJobSeekerSkillService _jobSeekerSkillService;

        public WcfUserService()
        {
            ISession session = NhSessionFactory.OpenSession();
            _userService = new UserService(session);
            _emailSevice = new EmailServiceNew();
            _jobseekerService = new JobSeekerService(session);
            _jobSeekerDetailsService = new JobSeekerDetailsService(session);
            _jobSeekerEducationalQualificationService = new JobSeekerEducationalQualificationService(session);
            _jobSeekerExperienceService = new JobSeekerExperienceService(session);
            _jobSeekerTrainingCoursesService = new JobSeekerTrainingCoursesService(session);
            _jobPostService = new JobPostService(session);
            _jobSeekerCvBankService = new JobSeekerCvBankService(session);
            _jobSeekerSkillService = new JobSeekerSkillService(session);
            _jobSeekerJobPostService = new JobSeekerJobPostService(session);
        }

        #region user management
        public bool Save(UserProfileVm userProfileVm)
        {
            UserProfile userProfile = CastingBusinessModel.CastToUserProfile(userProfileVm);
            return _userService.Save(userProfile);
        }

        public bool Update()
        {
            throw new NotImplementedException();
        }

        public bool Delete()
        {
            throw new NotImplementedException();
        }

        public void SendRegisterConfirmationEmail(string receiverEmail, string receptName, string code)
        {
            _emailSevice.SendRegisterConfirmationEmail(receiverEmail, receptName, code);
        }
        #endregion

        #region Jobseeker Management

        #region Single Object Load

        public JobSeekerDto GetJobSeekerProfileDto(string aspNetUser, int status)
        {
            try
            {
                return _jobseekerService.GetJobSeekerByAspId(aspNetUser, status);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public JobSeekerProfileDto GetJobSeekerProfile(string aspNetUser)
        {
            try
            {
                return _jobseekerService.GetJobSeeker(aspNetUser);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }
        public JobPostsDto GetByJobPostId(long id)
        {
            try
            {
                return _jobPostService.GetById(id);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }
        public JobSeekerSkillDto GetJobSeekerSkillDtoByAspNetUserId(long id)
        {
            try
            {
                return _jobseekerService.GetJobSeekerSkillDtoByAspNetUserId(id);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public JobSeekerPublicationsDto GetJobSeekerPublicationsDtoByAspNetUserId(long id)
        {
            try
            {
                return _jobseekerService.GetJobSeekerPublicationsDtoByAspNetUserId(id);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public JobSeekerPatentsDto GetJobSeekerPatentsDtoByAspNetUserId(long id)
        {
            try
            {
                return _jobseekerService.GetJobSeekerPatentsDtoByAspNetUserId(id);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public JobSeekerMilitaryDto GetJobSeekerMilitaryDtoByAspNetUserId(long id)
        {
            try
            {
                return _jobseekerService.GetJobSeekerMilitaryDtoByAspNetUserId(id);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public JobSeekerLinkDto GetJobSeekerLinkDtoByAspNetUserId(long id)
        {
            try
            {
                return _jobseekerService.GetJobSeekerLinkDtoByAspNetUserId(id);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public JobSeekerGroupDto GetJobSeekerGroupDtoByAspNetUserId(long id)
        {
            try
            {
                return _jobseekerService.GetJobSeekerGroupDtoByAspNetUserId(id);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public JobSeekerAwardDto GetJobSeekerAwardDtoByAspNetUserId(long id)
        {
            try
            {
                return _jobseekerService.GetJobSeekerAwardDtoByAspNetUserId(id);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public JobSeekerCertificateDto GetJobSeekerCertificateDtoByAspNetUserId(long id)
        {
            try
            {
                return _jobseekerService.GetJobSeekerCertificateDtoByAspNetUserId(id);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public JobSeekerAdditionalInformationDto GetAdditionalInformationDtosByAspNetUserId(long id)
        {
            try
            {
                return _jobseekerService.GetAdditionalInformationDtosByAspNetUserId(id);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public JobSeekerDesiredJobDto GetDesiredDtosByAspNetUserId(long id)
        {
            try
            {
                return _jobseekerService.GetDesiredDtosByAspNetUserId(id);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public JobPostsDto GetJobPostByJobPostIdAndAspNetUSerId(long jobPostId, string aspNetUserId, bool isApplied = false)
        {
            try
            {
                return _jobPostService.GetById(jobPostId, aspNetUserId, isApplied);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }
        #endregion

        #region Delete

        public void DeleteEduQualification(long id)
        {
            try
            {
                _jobSeekerEducationalQualificationService.Delete(id);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public void DeleteExp(long id)
        {
            try
            {
                _jobSeekerExperienceService.Delete(id);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public void DeleteTraining(long id)
        {
            try
            {
                _jobSeekerTrainingCoursesService.Delete(id);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public void DeleteResume(string aspNetUserId)
        {
            try
            {
                _jobseekerService.DeleteResume(aspNetUserId);
            }

            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public void DelSkill(long id)
        {
            try
            {
                _jobseekerService.DeleteSkill(id);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }
        public void DelJobSeekerPublicationsDto(long id)
        {
            try
            {
                _jobseekerService.DeletePublication(id);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public void DelAwardDto(long id)
        {
            try
            {
                _jobseekerService.DeleteAward(id);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public void DelJobSeekerAdditionalInformationDto(long id)
        {
            try
            {
                _jobseekerService.DeleteAdditionalInfo(id);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public void DelJobSeekerGroupDto(long id)
        {
            try
            {
                _jobseekerService.DeleteGroup(id);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message }, ex.Message.ToString());
            }
        }

        public void DelJobSeekerLinkDto(long id)
        {
            try
            {
                _jobseekerService.DeleteLink(id);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public void DelJobSeekerMilitaryDto(long id)
        {
            try
            {
                _jobseekerService.DeleteMalitary(id);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public void DelJobSeekerPatentsDto(long id)
        {
            try
            {
                _jobseekerService.DeletePatent(id);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public void DeleteCertificate(long id)
        {
            try
            {
                _jobseekerService.DeleteCertificate(id);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public void DelJobSeekerDesiredJobsDto(long id)
        {
            try
            {
                _jobseekerService.DeleteDesiredJobs(id);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }
        #endregion

        public void JobApply(string userProfileId, long jobPostId, bool isShortList = false, bool isApplied = false)
        {
            try
            {
                _jobSeekerJobPostService.SaveOrUpdate(userProfileId, jobPostId, isShortList, isApplied);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }


        }

        #region Load
        public IList<JobPostsDto> LoadAllJob()
        {
            return _jobPostService.LoadAll();
        }
        public IList<JobPostsDto> LoadAllJobAppliedByUser(string aspNetuserId)
        {
            try
            {
                return _jobPostService.LoadAllJobAppliedByUser(aspNetuserId);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public IList<JobPostsDto> LoadAllJobShortListedByUser(string aspNetuserId)
        {
            try
            {
                return _jobPostService.LoadAllJobShortListedByUser(aspNetuserId);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public JobSeekerExpVm GetJobSeekerExpVm(long id)
        {
            try
            {
                return _jobSeekerExperienceService.GetById(id);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public JobSeekerEduVm GetJobSeekerEduVm(long id)
        {
            try
            {
                return _jobSeekerEducationalQualificationService.GetById(id);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public JobSeekerCvBankDto GetCvBankDto(string aspNetUserId)
        {
            try
            {
                return _jobSeekerCvBankService.GetByUserProfileId(aspNetUserId);
            }
            catch (NullObjectException noe)
            {
                throw new FaultException<ResponseMessage>(
                  new ResponseMessage() { IsSuccess = false, Type = "NullObjectException", Message = noe.Message },
                  noe.Message.ToString()
              );
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }


        public IList<JobSeekerAdditionalInformationDto> LoadAditionalAdditionalInformationDtosByAspNetUserId(string aspNetUserId)
        {
            try
            {
                return _jobseekerService.LoadAditionalAdditionalInformationDtosByAspNetUserId(aspNetUserId);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public IList<JobSeekerAwardDto> LoadJobSeekerAwardDtoAspNetUserId(string aspNetUserId)
        {
            try
            {
                return _jobseekerService.LoadJobSeekerAwardDtoByAspNetUserId(aspNetUserId);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public IList<JobSeekerGroupDto> LoadJobSeekerGroupDtoByAspNetUserId(string aspNetUserId)
        {
            try
            {
                return _jobseekerService.LoadJobSeekerGroupDtoByAspNetUserId(aspNetUserId);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public IList<JobSeekerLinkDto> LoadJobSeekerLinkDtoByAspNetUserId(string aspNetUserId)
        {
            try
            {
                return _jobseekerService.LoadJobSeekerLinkDtoByAspNetUserId(aspNetUserId);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public IList<JobSeekerMilitaryDto> LoadJobSeekerMilitaryDtoByAspNetUserId(string aspNetUserId)
        {
            try
            {
                return _jobseekerService.LoadJobSeekerMilitaryDtoByAspNetUserId(aspNetUserId);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public IList<JobSeekerPatentsDto> LoadJobSeekerPatentsDtoByAspNetUserId(string aspNetUserId)
        {
            try
            {
                return _jobseekerService.LoadJobSeekerPatentsDtoByAspNetUserId(aspNetUserId);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public IList<JobSeekerPublicationsDto> LoadJobSeekerPublicationsDtoByAspNetUserId(string aspNetUserId)
        {
            try
            {
                return _jobseekerService.LoadJobSeekerPublicationsDtoByAspNetUserId(aspNetUserId);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public IList<JobSeekerSkillDto> LoadJobSeekerSkillDtoByAspNetUserId(string aspNetUserId)
        {
            try
            {
                return _jobseekerService.LoadJobSeekerSkillDtoByAspNetUserId(aspNetUserId);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public IList<JobSeekerCertificateDto> LoadJobSeekerCertificateDtoAspNetUserId(string aspNetUserId)
        {
            try
            {
                return _jobseekerService.LoadJobSeekerCertificateDtoByAspNetUserId(aspNetUserId);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public IList<JobSeekerDesiredJobDto> LoadJobSeekerDesiredAspNetUserId(string aspNetUserId)
        {
            try
            {
                return _jobseekerService.LoadJobSeekerDesiredAspNetUserId(aspNetUserId);
            }

            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public IList<JobPostsDto> LoadJobPost(int start, int length, string orderBy, string orderDir, string what = "",
            string where = "", int ageRangeFrom = 0,
            int ageRangeTo = 0, int expRangeFrom = 0, int expRangeTo = 0, int salaryRangeFrom = 0, int salaryRangeTo = 0,
            long[] company = null, long[] jobCategory = null, long[] jobType = null)
        {
            try
            {
                return _jobPostService.LoadJobPost(start, length, orderBy, orderDir, what, where,
             ageRangeFrom, ageRangeTo, expRangeFrom, expRangeTo, salaryRangeFrom,
             salaryRangeTo, company, jobCategory, jobType);
            }

            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        #endregion

        #region Save
        public bool SaveJobSeekerProfile(JobSeekerProfileVm profile)
        {
            try
            {
                return _jobseekerService.Save(profile);
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageExceptions", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }

        }

        public JobSeekerProfileVm GetJobSeekerProfileData(string userId)
        {
            try
            {
                return _jobseekerService.GetJobSeekerProfileData(userId);
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageExceptions", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }

        }

        public void SaveEduQualification(JobSeekerEduVm jobSeekerEduVm)
        {
            try
            {
                _jobSeekerEducationalQualificationService.Save(jobSeekerEduVm);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageExceptions", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (NullObjectException ex)
            {
                throw new FaultException<ResponseMessage>(
                     new ResponseMessage() { IsSuccess = false, Type = "NullObjectException", Message = ex.Message },
                     ex.Message.ToString()
                 );
            }
            catch (DuplicateEntryException ex)
            {
                throw new FaultException<ResponseMessage>(
                     new ResponseMessage() { IsSuccess = false, Type = "DuplicateEntryException", Message = ex.Message },
                     ex.Message.ToString()
                 );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public void SaveExperience(JobSeekerExpVm jobSeekerExpVm)
        {
            try
            {
                _jobSeekerExperienceService.Save(jobSeekerExpVm);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageExceptions", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (DuplicateEntryException ex)
            {
                throw new FaultException<ResponseMessage>(
                     new ResponseMessage() { IsSuccess = false, Type = "DuplicateEntryException", Message = ex.Message },
                     ex.Message.ToString()
                 );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public void SaveTraining(JobSeekerTrainingVm jobSeekerTrainingVm)
        {
            try
            {
                _jobSeekerTrainingCoursesService.Save(jobSeekerTrainingVm);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageExceptions", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public void SavePhotos(byte[] file, string aspNetUserId)
        {
            try
            {
                _jobseekerService.SavePhoto(file, aspNetUserId);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageExceptions", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public void SaveFile(byte[] file, string aspNetUserId)
        {
            try
            {

                _jobSeekerDetailsService.SaveCv(file, aspNetUserId);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageExceptions", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public void SaveSkill(JobSeekerSkillDto jobSeekerSkillDto)
        {
            try
            {
                _jobseekerService.SaveSkill(jobSeekerSkillDto);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageExceptions", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (DuplicateEntryException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "DuplicateEntryException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public void SaveAwardDto(JobSeekerAwardDto jobSeekerAwardDto)
        {
            try
            {
                _jobseekerService.SaveAward(jobSeekerAwardDto);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageExceptions", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (DuplicateEntryException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "DuplicateEntryException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public void SaveJobSeekerAdditionalInformationDto(JobSeekerAdditionalInformationDto jobSeekerAdditionalInformationDto)
        {
            try
            {
                _jobseekerService.SaveAdditinalInfo(jobSeekerAdditionalInformationDto);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageExceptions", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (DuplicateEntryException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "DuplicateEntryException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public void SaveJobSeekerGroupDto(JobSeekerGroupDto jobSeekerGroupDto)
        {
            try
            {
                _jobseekerService.SaveGroup(jobSeekerGroupDto);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageExceptions", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (DuplicateEntryException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "DuplicateEntryException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public void SaveJobSeekerLinkDto(JobSeekerLinkDto jobSeekerLinkDto)
        {
            try
            {
                _jobseekerService.SaveLink(jobSeekerLinkDto);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageExceptions", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (DuplicateEntryException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "DuplicateEntryException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public void SaveJobSeekerMilitaryDto(JobSeekerMilitaryDto jobSeekerMilitaryDto)
        {
            try
            {
                _jobseekerService.SaveMilitary(jobSeekerMilitaryDto);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageExceptions", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (DuplicateEntryException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "DuplicateEntryException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public void SaveJobSeekerPatentsDto(JobSeekerPatentsDto jobSeekerPatentsDto)
        {
            try
            {
                _jobseekerService.SavePatent(jobSeekerPatentsDto);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageExceptions", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (DuplicateEntryException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "DuplicateEntryException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public void SaveJobSeekerPublicationsDto(JobSeekerPublicationsDto jobSeekerPublicationsDto)
        {
            try
            {
                _jobseekerService.SavePublication(jobSeekerPublicationsDto);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageExceptions", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (DuplicateEntryException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "DuplicateEntryException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public void SaveJobSeekerCertificate(JobSeekerCertificateDto jobSeekerCertificateDto)
        {
            try
            {
                _jobseekerService.SaveCertificate(jobSeekerCertificateDto);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageExceptions", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (DuplicateEntryException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "DuplicateEntryException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public void SaveJobSeekerDesired(JobSeekerDesiredJobDto jobSeekerDesiredJobDto)
        {
            try
            {
                _jobseekerService.SaveDesiredJob(jobSeekerDesiredJobDto);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageExceptions", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (DuplicateEntryException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "DuplicateEntryException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        #endregion

        #region others

        public int CountLoadAllJobAppliedByUser(string aspNetuserId)
        {
            try
            {
                return _jobPostService.CountLoadAllJobAppliedByUser(aspNetuserId);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public int CountLoadAllJobShortListedByUser(string aspNetuserId)
        {
            try
            {
                return _jobPostService.CountLoadAllJobShortListedByUser(aspNetuserId);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }

        public int CountJobPost(string what = "", string where = "",
            int ageRangeFrom = 0, int ageRangeTo = 0, int expRangeFrom = 0, int expRangeTo = 0, int salaryRangeFrom = 0,
            int salaryRangeTo = 0, long[] company = null, long[] jobCategory = null, long[] jobType = null)
        {
            try
            {
                return _jobPostService.CountJobPost(what, where,
             ageRangeFrom, ageRangeTo, expRangeFrom, expRangeTo, salaryRangeFrom,
             salaryRangeTo, company, jobCategory, jobType);
            }
            catch (MessageException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "MessageException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (InvalidDataException ide)
            {
                throw new FaultException<ResponseMessage>(
                   new ResponseMessage() { IsSuccess = false, Type = "InvalidDataException", Message = ide.Message },
                   ide.Message.ToString()
               );
            }
            catch (Exception ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "Exception", Message = ex.Message },
                    ex.Message.ToString()
                    );
            }
        }
        #endregion

        #endregion
    }
}
