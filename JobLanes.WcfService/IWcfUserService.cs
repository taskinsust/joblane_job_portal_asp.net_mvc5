using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Model.JobLanes.Dto;
using Model.JobLanes.Entity.User;
using Model.JobLanes.ViewModel;

namespace JobLanes.WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IWcfUserService" in both code and config file together.
    [ServiceContract]
    public interface IWcfUserService
    {
        #region user management
        [OperationContract]
        bool Save(UserProfileVm userProfileVm);

        [OperationContract]
        bool Update();

        [OperationContract]
        bool Delete();

        [OperationContract]
        void SendRegisterConfirmationEmail(string receiverEmail, string receptName, string code);
        #endregion

        #region Job Seeker

        #region Delete
        [OperationContract]
        void DeleteEduQualification(long id);

        [OperationContract]
        void DeleteExp(long id);

        //Prev version function 
        //new DeleteCertificate
        [OperationContract]
        void DeleteTraining(long id);

        [OperationContract]
        void DeleteResume(string aspNetUserId);

        [OperationContract]
        void DelSkill(long id);
        [OperationContract]
        void DelAwardDto(long id);
        [OperationContract]
        void DelJobSeekerAdditionalInformationDto(long id);
        [OperationContract]
        void DelJobSeekerGroupDto(long id);
        [OperationContract]
        void DelJobSeekerLinkDto(long id);
        [OperationContract]
        void DelJobSeekerMilitaryDto(long id);
        [OperationContract]
        void DelJobSeekerPatentsDto(long id);
        [OperationContract]
        void DelJobSeekerPublicationsDto(long id);
        [OperationContract]
        void DeleteCertificate(long id);

        [OperationContract]
        void DelJobSeekerDesiredJobsDto(long id);


        #endregion
        
        #region Single Object Load
        [OperationContract]
        JobSeekerDto GetJobSeekerProfileDto(string aspNetUser, int status);


        [OperationContract]
        JobPostsDto GetByJobPostId(long id);

        [OperationContract]
        JobSeekerProfileVm GetJobSeekerProfileData(string userId);

        [OperationContract]
        JobSeekerProfileDto GetJobSeekerProfile(string aspNetUser);

        [OperationContract]
        JobSeekerExpVm GetJobSeekerExpVm(long id);

        [OperationContract]
        JobSeekerEduVm GetJobSeekerEduVm(long id);

        [OperationContract]
        JobSeekerCvBankDto GetCvBankDto(string aspNetUserId);

        [OperationContract]
        JobSeekerSkillDto GetJobSeekerSkillDtoByAspNetUserId(long id);

        [OperationContract]
        JobSeekerPublicationsDto GetJobSeekerPublicationsDtoByAspNetUserId(long id);

        [OperationContract]
        JobSeekerPatentsDto GetJobSeekerPatentsDtoByAspNetUserId(long id);

        [OperationContract]
        JobSeekerMilitaryDto GetJobSeekerMilitaryDtoByAspNetUserId(long id);

        [OperationContract]
        JobSeekerLinkDto GetJobSeekerLinkDtoByAspNetUserId(long id);

        [OperationContract]
        JobSeekerGroupDto GetJobSeekerGroupDtoByAspNetUserId(long id);

        [OperationContract]
        JobSeekerAwardDto GetJobSeekerAwardDtoByAspNetUserId(long id);

        [OperationContract]
        JobSeekerCertificateDto GetJobSeekerCertificateDtoByAspNetUserId(long id);


        [OperationContract]
        JobSeekerAdditionalInformationDto GetAdditionalInformationDtosByAspNetUserId(long id);
        
        [OperationContract]
        JobSeekerDesiredJobDto GetDesiredDtosByAspNetUserId(long id);

        [OperationContract]
        JobPostsDto GetJobPostByJobPostIdAndAspNetUSerId(long jobPostId, string aspNetUserId, bool isApplied = false);
        
        #endregion

        #region Load

        [OperationContract]
        IList<JobPostsDto> LoadAllJob();
        [OperationContract]
        IList<JobPostsDto> LoadAllJobAppliedByUser(string aspNetuserId);

        [OperationContract]
        IList<JobPostsDto> LoadAllJobShortListedByUser(string aspNetuserId);


        [OperationContract]
        IList<JobSeekerAdditionalInformationDto> LoadAditionalAdditionalInformationDtosByAspNetUserId(
            string aspNetUserId);
        [OperationContract]
        IList<JobSeekerAwardDto> LoadJobSeekerAwardDtoAspNetUserId(
            string aspNetUserId);

        [OperationContract]
        IList<JobSeekerGroupDto> LoadJobSeekerGroupDtoByAspNetUserId(
            string aspNetUserId);

        [OperationContract]
        IList<JobSeekerLinkDto> LoadJobSeekerLinkDtoByAspNetUserId(
            string aspNetUserId);

        [OperationContract]
        IList<JobSeekerMilitaryDto> LoadJobSeekerMilitaryDtoByAspNetUserId(
            string aspNetUserId);

        [OperationContract]
        IList<JobSeekerPatentsDto> LoadJobSeekerPatentsDtoByAspNetUserId(
            string aspNetUserId);

        [OperationContract]
        IList<JobSeekerPublicationsDto> LoadJobSeekerPublicationsDtoByAspNetUserId(
            string aspNetUserId);

        [OperationContract]
        IList<JobSeekerSkillDto> LoadJobSeekerSkillDtoByAspNetUserId(
            string aspNetUserId);

        [OperationContract]
        IList<JobSeekerCertificateDto> LoadJobSeekerCertificateDtoAspNetUserId(string aspNetUserId);

        [OperationContract]
        IList<JobSeekerDesiredJobDto> LoadJobSeekerDesiredAspNetUserId(string aspNetUserId);

        [OperationContract]
        IList<JobPostsDto> LoadJobPost(int start, int length, string orderBy, string orderDir, string what = "",
            string where = "", int ageRangeFrom = 0,
            int ageRangeTo = 0, int expRangeFrom = 0, int expRangeTo = 0, int salaryRangeFrom = 0, int salaryRangeTo = 0,
            long[] company = null, long[] jobCategory = null, long[] jobType = null);
        #endregion

        #region Save


        [OperationContract]
        bool SaveJobSeekerProfile(JobSeekerProfileVm profile);

        [OperationContract]
        void SaveEduQualification(JobSeekerEduVm jobSeekerEduVm);

        [OperationContract]
        void SaveExperience(JobSeekerExpVm jobSeekerExpVm);

        [OperationContract]
        void SaveTraining(JobSeekerTrainingVm jobSeekerTrainingVm);


        [OperationContract]
        void SavePhotos(byte[] file, string aspNetUserId);

        [OperationContract]
        void SaveFile(byte[] file, string aspNetUserId);

        [OperationContract]
        void SaveSkill(JobSeekerSkillDto jobSeekerSkillDto);
        [OperationContract]
        void SaveAwardDto(JobSeekerAwardDto jobSeekerAwardDto);
        [OperationContract]
        void SaveJobSeekerAdditionalInformationDto(JobSeekerAdditionalInformationDto jobSeekerAdditionalInformationDto);
        [OperationContract]
        void SaveJobSeekerGroupDto(JobSeekerGroupDto jobSeekerGroupDto);
        [OperationContract]
        void SaveJobSeekerLinkDto(JobSeekerLinkDto jobSeekerLinkDto);
        [OperationContract]
        void SaveJobSeekerMilitaryDto(JobSeekerMilitaryDto jobSeekerMilitaryDto);
        [OperationContract]
        void SaveJobSeekerPatentsDto(JobSeekerPatentsDto jobSeekerPatentsDto);
        [OperationContract]
        void SaveJobSeekerPublicationsDto(JobSeekerPublicationsDto jobSeekerPublicationsDto);

        [OperationContract]
        void SaveJobSeekerCertificate(JobSeekerCertificateDto jobSeekerCertificateDto);

        [OperationContract]
        void SaveJobSeekerDesired(JobSeekerDesiredJobDto jobSeekerDesiredJobDto);

      
        #endregion

        #region Others
        
        [OperationContract]
        void JobApply(string userProfileId, long jobPostId, bool isShortList = false, bool isApplied = false);

        [OperationContract]
        int CountJobPost(string what = "", string @where = "",
            int ageRangeFrom = 0, int ageRangeTo = 0, int expRangeFrom = 0, int expRangeTo = 0, int salaryRangeFrom = 0,
            int salaryRangeTo = 0, long[] company = null, long[] jobCategory = null, long[] jobType = null);

        [OperationContract]
        int CountLoadAllJobAppliedByUser(string aspNetuserId);

        [OperationContract]
        int CountLoadAllJobShortListedByUser(string aspNetuserId);

        #endregion

        #endregion
    }
}
