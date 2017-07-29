using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using JobLanes.WcfService.Models;
using Model.JobLanes.Dto;
using Model.JobLanes.ViewModel;

namespace JobLanes.WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICompanyAdminService" in both code and config file together.
    [ServiceContract]
    public interface ICompanyAdminService
    {
        #region Company Profile
        [OperationContract]
        CompanyDto GetCompany(string aspNetUser);

        [OperationContract]
        CompanyDto GetCompanyById(long company, int status); 

        [OperationContract]
        [FaultContract(typeof(ResponseMessage))]
        void UpdateCompanyProfile(CompanyViewModel companyViewModel);

        [OperationContract]
        List<CompanyDto> LoadCompany(int? status = null, long? companyType = null, string zip = null, long? region = null, long? country = null, long? state = null, long? city = null);
        
        [OperationContract]
        List<CompanyDto> LoadOnlyCompanyByStatusAndType(int? status = null, long? companyType = null);
        [OperationContract]
        List<CompanyDto> LoadCompanyForWebAdmin(int start, int length, string orderBy, string orderDir, int? status = null,
           long? companyType = null, string companyName = "", string contactMobile = "", string contactEmail = "",
           string zip = "", long? region = null, long? country = null, long? state = null, long? city = null);

        [OperationContract]
        int CompanyRowCount(int? status = null, long? companyType = null, string companyName = "", string contactMobile = "",
           string contactEmail = "", string zip = "", long? region = null, long? country = null, long? state = null,
           long? city = null);

        [OperationContract]
        [FaultContract(typeof(ResponseMessage))]
        void SaveJobSeekerProfileView(string aspNetUser, long jobSeekerId); 
        #endregion

        #region Job Post Management
        [OperationContract]
        [FaultContract(typeof(ResponseMessage))]
        void SaveJobPost(JobPostViewModel jobPostViewModel);

        [OperationContract]
        List<JobPostsDto> LoadJobPost(int start, int length, string orderBy, string orderDir, long company, long jobCategory, long jobType, string jobTitle, int status);

        [OperationContract]
        int JobPostRowCount(long company, long jobCategory, long jobType, string jobTitle, int status);

        [OperationContract]
        JobPostsDto GetJobPost(long id, long company);

        [OperationContract]
        [FaultContract(typeof(ResponseMessage))]
        void DeleteJob(long id);

        [OperationContract]
        List<JobApplicationDto> LoadJobApplication(int start, int length, string orderBy, string orderDir, long jobPostId, long companyId, DateTime? deadLineFrom, DateTime? deadlineTo, int deadlineFlag, bool isShortListed, string jobTitle);

        [OperationContract]
        int JobApplicationRowCount(long jobPostId, long companyId, DateTime? deadLineFrom, DateTime? deadlineTo, int deadlineFlag, bool isShortListed, string jobTitle);

        [OperationContract]
        [FaultContract(typeof(ResponseMessage))]
        void ShotlistApplication(long id, long companyId, bool isShortListed); 
        #endregion

        #region JobSeeker Search

        [OperationContract]
        List<JobSeekerSearchDto> LoadJobSeekerSearch(int start, int length, string orderBy, string orderDir, string whatKey, string whereKey, string[] jobTitle, string[] yearExp, string[] education, string[] company);

        [OperationContract]
        int JobSeekerSearchRowCount(string whatKey, string whereKey, string[] jobTitle, string[] yearExp, string[] education, string[] company);

        [OperationContract]
        List<FindResumeFilterDto> LoadFindResumeFiteDtoList();

        #endregion
    }
}
