using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using JobLanes.WcfService.Models;
using Model.JobLanes.Dto;
using Model.JobLanes.ViewModel;
using NHibernate;
using Services.Joblanes;
using Services.Joblanes.Exceptions;
using Services.Joblanes.Helper;

namespace JobLanes.WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CompanyAdminService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select CompanyAdminService.svc or CompanyAdminService.svc.cs at the Solution Explorer and start debugging.
    public class CompanyAdminService : ConnectionBase, ICompanyAdminService
    {
        #region Propertise & Object Initialization
        private readonly IUserService _userService;
        private readonly ICompanyService _companyService;
        private readonly ICompanyDetailsService _companyDetailsService;
        private readonly IJobPostService _jobPostService;
        private readonly IProfileViewService _profileViewService;
        private readonly IJobseekerService _jobseekerService; 
        public CompanyAdminService()
        {
            ISession session = NhSessionFactory.OpenSession();
            _userService = new UserService(session);
            _companyService = new CompanyService(session);
            _companyDetailsService = new CompanyDetailsService(session);
            _jobPostService = new JobPostService(session);
            _profileViewService = new ProfileViewService(session);
            _jobseekerService = new JobSeekerService(session);
        }
        #endregion

        #region Company Profile
        public CompanyDto GetCompany(string aspNetUser)
        {
            try
            {
                return _companyService.GetCompany(aspNetUser);
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

        public CompanyDto GetCompanyById(long company, int status)
        {
            try
            {
                return _companyService.GetCompanyById(company, status); 
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

        public void UpdateCompanyProfile(CompanyViewModel companyViewModel)
        {
            try
            {
                _companyService.Save(companyViewModel);
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

        public List<CompanyDto> LoadCompany(int? status = null, long? companyType = null, string zip = null, long? region = null,
            long? country = null, long? state = null, long? city = null)
        {
            try
            {
                List<CompanyDto> dataList = _companyService.LoadCompany(status, companyType, zip, region, country, state,city);
                return dataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CompanyDto> LoadOnlyCompanyByStatusAndType(int? status = null, long? companyType = null)
        {
            try
            {
                List<CompanyDto> dataList = _companyService.LoadOnlyCompanyByStatusAndType(status, companyType);
                return dataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CompanyDto> LoadCompanyForWebAdmin(int start, int length, string orderBy, string orderDir, int? status = null,
          long? companyType = null, string companyName = "", string contactMobile = "", string contactEmail = "",
          string zip = "", long? region = null, long? country = null, long? state = null, long? city = null)
        {
            try
            {
                List<CompanyDto> dataList = _companyService.LoadCompanyForWebAdmin(start, length, orderBy, orderDir, status, companyType, companyName, contactMobile, contactEmail, zip,
                region, country, state, city);
                return dataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CompanyRowCount(int? status = null, long? companyType = null, string companyName = "", string contactMobile = "",
            string contactEmail = "", string zip = "", long? region = null, long? country = null, long? state = null,
            long? city = null)
        {
            try
            {
                return _companyService.CompanyRowCount(status, companyType, companyName, contactMobile, contactEmail, zip,
                region, country, state, city);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveJobSeekerProfileView(string aspNetUser, long jobSeekerId)
        {
            try
            {
                _profileViewService.SaveJobSeekerProfileView(aspNetUser, jobSeekerId); 
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

        #endregion

        #region Job Posting
        public void SaveJobPost(JobPostViewModel jobPostViewModel)
        {
            try
            {
                _jobPostService.Save(jobPostViewModel);
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

        public List<JobPostsDto> LoadJobPost(int start, int length, string orderBy, string orderDir, long company, long jobCategory, long jobType,string jobTitle,
            int status)
        {
            try
            {
                List<JobPostsDto> dataList = _jobPostService.LoadJobPost(start, length, orderBy, orderDir, company, jobCategory, jobType, jobTitle, status).ToList();

                return dataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int JobPostRowCount(long company, long jobCategory, long jobType, string jobTitle, int status)
        {
            try
            {
                return _jobPostService.JobPostRowCount(company, jobCategory, jobType, jobTitle, status);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JobPostsDto GetJobPost(long id, long company) 
        {
            try
            {
                return _jobPostService.GetJobPost(id, company);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteJob(long id)
        {
            try
            {
                _jobPostService.Delete(id); 
            }
            catch (NullObjectException ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "NullObjectException", Message = ex.Message },
                    ex.Message.ToString()
                );
            }
            catch (DependencyException ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "DependencyException", Message = ex.Message },
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

        public List<JobApplicationDto> LoadJobApplication(int start, int length, string orderBy, string orderDir, long jobPostId, long companyId,
            DateTime? deadLineFrom, DateTime? deadlineTo, int deadlineFlag, bool isShortListed, string jobTitle)
        {
            try
            {
                List<JobApplicationDto> dataList = _jobPostService.LoadJobApplication(start, length, orderBy, orderDir, jobPostId, companyId, deadLineFrom, deadlineTo, deadlineFlag,isShortListed,jobTitle).ToList();

                return dataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int JobApplicationRowCount(long jobPostId, long companyId, DateTime? deadLineFrom, DateTime? deadlineTo,
            int deadlineFlag, bool isShortListed, string jobTitle)
        {
            try
            {
                return _jobPostService.JobApplicationRowCount(jobPostId, companyId, deadLineFrom, deadlineTo, deadlineFlag,isShortListed,jobTitle);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ShotlistApplication(long id, long companyId, bool isShortListed)
        {
            try
            {
                _jobPostService.ShotlistApplication(id, companyId, isShortListed); 
            }
            catch (NullObjectException ex)
            {
                throw new FaultException<ResponseMessage>(
                    new ResponseMessage() { IsSuccess = false, Type = "NullObjectException", Message = ex.Message },
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

        #endregion

        #region JobSeeker Search
        
        public List<JobSeekerSearchDto> LoadJobSeekerSearch(int start, int length, string orderBy, string orderDir,
            string whatKey, string whereKey, string[] jobTitle, string[] yearExp, string[] education, string[] company)
        {
            try
            {
                List<JobSeekerSearchDto> dataList = _jobseekerService.LoadJobSeekerSearch(start, length, orderBy, orderDir, whatKey, whereKey,jobTitle,yearExp,education,company);
                return dataList;
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
                return _jobseekerService.JobSeekerSearchRowCount(whatKey, whereKey, jobTitle, yearExp, education, company);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<FindResumeFilterDto> LoadFindResumeFiteDtoList()
        {
            try
            {
                List<FindResumeFilterDto> dataList = _jobseekerService.LoadFindResumeFiteDtoList();
                return dataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
