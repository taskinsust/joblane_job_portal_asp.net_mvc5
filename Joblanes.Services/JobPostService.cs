using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes;
using FluentNHibernate.MappingModel;
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
    public interface IJobPostService : IBaseService
    {
        #region Operational Function
        void Save(JobPostViewModel cityVm);
        void Delete(long id);
        void ShotlistApplication(long id, long companyId, bool isShortListed);
        #endregion

        #region Single Instances Loading Function

        JobPostsDto GetById(long id);
        JobPostsDto GetJobPost(long id, long company);
        JobPostsDto GetById(long jobPostId, string aspNetUserId, bool isApplied);

        #endregion

        #region List of Loading function

        IList<JobPostsDto> LoadAll();
        IList<JobPostsDto> LoadAllJobAppliedByUser(string aspNetuserId);
        IList<JobPostsDto> LoadAllJobShortListedByUser(string aspNetuserId);
        IList<JobPostsDto> LoadJobPost(int start, int length, string orderBy, string orderDir, long company, long jobCategory, long jobType, string jobTitle, int status);

        IList<JobApplicationDto> LoadJobApplication(int start, int length, string orderBy, string orderDir,
            long jobPostId, long companyId, DateTime? deadLineFrom, DateTime? deadlineTo, int deadlineFlag, bool isShortListed, string jobTitle);
        IList<JobPostsDto> LoadJobPost(int start, int length, string orderBy, string orderDir, string what = "", string where = "", int ageRangeFrom = 0,
            int ageRangeTo = 0, int expRangeFrom = 0, int expRangeTo = 0, int salaryRangeFrom = 0, int salaryRangeTo = 0,
            long[] company = null, long[] jobCategory = null, long[] jobType = null);
        #endregion

        #region Other Function
        int JobPostRowCount(long company, long jobCategory, long jobType, string jobTitle, int status);
        int JobApplicationRowCount(long jobPostId, long companyId, DateTime? deadLineFrom, DateTime? deadlineTo, int deadlineFlag, bool isShortListed, string jobTitle);

        int CountJobPost(string what = "", string @where = "",
            int ageRangeFrom = 0, int ageRangeTo = 0, int expRangeFrom = 0, int expRangeTo = 0, int salaryRangeFrom = 0,
            int salaryRangeTo = 0, long[] company = null, long[] jobCategory = null, long[] jobType = null);
        int CountLoadAllJobAppliedByUser(string aspNetuserId);
        int CountLoadAllJobShortListedByUser(string aspNetuserId);

        #endregion




    }
    public class JobPostService : BaseService, IJobPostService
    {
        #region Propertise & Object Initialization

        private readonly IJobPostDao _jobPostDao;
        private readonly IJobSeekerJobPostDao _jobSeekerJobPostDao;
        private readonly IUserDao _userDao;
        private readonly IJobSeekerDao _jobSeekerDao;
        private readonly ICompanyDao _companyDao;
        private readonly IJobCategoryDao _jobCategoryDao;
        private readonly IJobTypeDao _jobTypeDao;
        public JobPostService(ISession session)
        {
            Session = session;
            _jobPostDao = new JobPostDao() { Session = session };
            _jobSeekerJobPostDao = new JobSeekerJobPostDao() { Session = session };
            _userDao = new UserDao() { Session = session };
            _jobSeekerDao = new JobSeekerDao() { Session = session };
            _companyDao = new CompanyDao() { Session = session };
            _jobCategoryDao = new JobCategoryDao() { Session = session };
            _jobTypeDao = new JobTypeDao() { Session = session };
        }

        #endregion

        #region Operational Function
        public void Save(JobPostViewModel jobPostVm)
        {
            ITransaction transaction = null;
            try
            {
                if (jobPostVm == null)
                {
                    throw new NullObjectException("Job can not be null");
                }
                Company company = _companyDao.LoadById(jobPostVm.Company);
                JobCategory jobCategory = _jobCategoryDao.LoadById(jobPostVm.JobCategory);
                JobType jobType = _jobTypeDao.LoadById(jobPostVm.JobType);

                var jobPost = new JobPost()
                {
                    Id = jobPostVm.Id,
                    Status = jobPostVm.Status,
                    JobTitle = jobPostVm.JobTitle,
                    NoOfVacancies = jobPostVm.NoOfVacancies,
                    IsOnlineCv = jobPostVm.IsOnlineCv,
                    IsEmailCv = jobPostVm.IsEmailCv,
                    IsHardCopy = jobPostVm.IsHardCopy,
                    IsPhotographAttach = jobPostVm.IsPhotographAttach,
                    ApplyInstruction = jobPostVm.ApplyInstruction,
                    DeadLine = jobPostVm.DeadLine,
                    IsDisplayCompanyName = jobPostVm.IsDisplayCompanyName,
                    IsDisplayCompanyAddress = jobPostVm.IsDisplayCompanyAddress,
                    IsDisplayCompanyBusiness = jobPostVm.IsDisplayCompanyBusiness,
                    AgeRangeFrom = jobPostVm.AgeRangeFrom,
                    AgeRangeTo = jobPostVm.AgeRangeTo,
                    IsMale = jobPostVm.IsMale,
                    IsFemale = jobPostVm.IsFemale,
                    JobLevel = jobPostVm.JobLevel,
                    EducationalQualification = jobPostVm.EducationalQualification,
                    JobDescription = jobPostVm.JobDescription,
                    AdditionalRequirements = jobPostVm.AdditionalRequirements,
                    IsExperienceRequired = jobPostVm.IsExperienceRequired,
                    ExperienceMax = jobPostVm.ExperienceMax,
                    ExperienceMin = jobPostVm.ExperienceMin,
                    JobLocation = jobPostVm.JobLocation,
                    IsShowSalary = jobPostVm.IsShowSalary,
                    SalaryRange = jobPostVm.SalaryRange,
                    SalaryMin = jobPostVm.SalaryMin,
                    SalaryMax = jobPostVm.SalaryMax,
                    SalaryDetail = jobPostVm.SalaryDetail,
                    OtherBenefit = jobPostVm.OtherBenefit,
                    Company = company,
                    //CompanyLogo = x.Company.Logo,
                    //  PackageId = x.PackageId,
                    JobCategory = jobCategory,
                    JobType = jobType
                };
                ModelValidationCheck(jobPost);
                using (transaction = Session.BeginTransaction())
                {
                    if (jobPost.Id < 1)
                    {
                        //  CheckDuplicateFields(cityVm);
                        _jobPostDao.Save(jobPost);
                    }
                    else
                    {
                        var oldJob = _jobPostDao.LoadById(jobPost.Id);
                        if (oldJob == null)
                        {
                            throw new NullObjectException("Job can not be null");
                        }
                        // CheckDuplicateFields(cityVm, oldCity.Id);

                        oldJob.Status = jobPost.Status;
                        oldJob.JobTitle = jobPost.JobTitle;
                        oldJob.NoOfVacancies = jobPostVm.NoOfVacancies;
                        oldJob.IsOnlineCv = jobPostVm.IsOnlineCv;
                        oldJob.IsEmailCv = jobPostVm.IsEmailCv;
                        oldJob.IsHardCopy = jobPostVm.IsHardCopy;
                        oldJob.IsPhotographAttach = jobPostVm.IsPhotographAttach;
                        oldJob.ApplyInstruction = jobPostVm.ApplyInstruction;
                        oldJob.DeadLine = jobPostVm.DeadLine;
                        oldJob.IsDisplayCompanyName = jobPostVm.IsDisplayCompanyName;
                        oldJob.IsDisplayCompanyAddress = jobPostVm.IsDisplayCompanyAddress;
                        oldJob.IsDisplayCompanyBusiness = jobPostVm.IsDisplayCompanyBusiness;
                        oldJob.AgeRangeFrom = jobPostVm.AgeRangeFrom;
                        oldJob.AgeRangeTo = jobPostVm.AgeRangeTo;
                        oldJob.IsMale = jobPostVm.IsMale;
                        oldJob.IsFemale = jobPostVm.IsFemale;
                        oldJob.JobLevel = jobPostVm.JobLevel;
                        oldJob.EducationalQualification = jobPostVm.EducationalQualification;
                        oldJob.JobDescription = jobPostVm.JobDescription;
                        oldJob.AdditionalRequirements = jobPostVm.AdditionalRequirements;
                        oldJob.IsExperienceRequired = jobPostVm.IsExperienceRequired;
                        oldJob.ExperienceMax = jobPostVm.ExperienceMax;
                        oldJob.ExperienceMin = jobPostVm.ExperienceMin;
                        oldJob.JobLocation = jobPostVm.JobLocation;
                        oldJob.IsShowSalary = jobPostVm.IsShowSalary;
                        oldJob.SalaryRange = jobPostVm.SalaryRange;
                        oldJob.SalaryMin = jobPostVm.SalaryMin;
                        oldJob.SalaryMax = jobPostVm.SalaryMax;
                        oldJob.SalaryDetail = jobPostVm.SalaryDetail;
                        oldJob.OtherBenefit = jobPostVm.OtherBenefit;
                        oldJob.JobCategory = jobPost.JobCategory;
                        oldJob.JobType = jobPost.JobType;

                        _jobPostDao.Update(oldJob);

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

        public void Delete(long id)
        {
            ITransaction trans = null;
            try
            {
                var obj = _jobPostDao.LoadById(id);
                if (obj == null)
                    throw new NullObjectException("Job is not valid");

                CheckBeforeDelete(obj);
                using (trans = Session.BeginTransaction())
                {
                    obj.Status = JobPost.EntityStatus.Delete;
                    _jobPostDao.Update(obj);
                    trans.Commit();
                }
            }
            catch (DependencyException) { throw; }

            catch (NullObjectException) { throw; }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (trans != null && trans.IsActive)
                    trans.Rollback();
            }
        }

        public void ShotlistApplication(long id, long companyId, bool isShortListed)
        {

            ITransaction trans = null;
            try
            {
                var obj = _jobSeekerJobPostDao.GetJobSeekerJobPost(id, companyId);
                if (obj == null)
                    throw new NullObjectException("Job is invalid.");


                using (trans = Session.BeginTransaction())
                {
                    obj.IsShortListedByCompany = isShortListed;
                    _jobSeekerJobPostDao.Update(obj);
                    trans.Commit();
                }
            }

            catch (NullObjectException) { throw; }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (trans != null && trans.IsActive)
                    trans.Rollback();
            }
        }

        #endregion

        #region Single Instances Loading Function
        public JobPostsDto GetById(long id)
        {
            JobPost jobPost = _jobPostDao.LoadById(id);
            return CastModelFromDto(jobPost);
        }

        public JobPostsDto GetJobPost(long id, long company)
        {
            try
            {
                var x = _jobPostDao.GetJobPost(id, company);
                JobPostsDto obj = new JobPostsDto();
                if (x != null)
                {

                    obj.Id = x.Id;
                    obj.StatusText = StatusTypeText.GetStatusText(x.Status);
                    obj.JobTitle = x.JobTitle;
                    obj.NoOfVacancies = x.NoOfVacancies;
                    obj.IsOnlineCv = x.IsOnlineCv;
                    obj.IsEmailCv = x.IsEmailCv;
                    obj.IsHardCopy = x.IsHardCopy;
                    obj.IsPhotographAttach = x.IsPhotographAttach;
                    obj.ApplyInstruction = x.ApplyInstruction;
                    obj.DeadLine = x.DeadLine;
                    obj.IsDisplayCompanyName = x.IsDisplayCompanyName;
                    obj.IsDisplayCompanyAddress = x.IsDisplayCompanyAddress;
                    obj.IsDisplayCompanyBusiness = x.IsDisplayCompanyBusiness;
                    obj.AgeRangeFrom = x.AgeRangeFrom;
                    obj.AgeRangeTo = x.AgeRangeTo;
                    obj.IsMale = x.IsMale;
                    obj.IsFemale = x.IsFemale;
                    obj.JobLevel = x.JobLevel;
                    obj.EducationalQualification = x.EducationalQualification;
                    obj.JobDescription = x.JobDescription;
                    obj.AdditionalRequirements = x.AdditionalRequirements;
                    obj.IsExperienceRequired = x.IsExperienceRequired;
                    obj.ExperienceMax = x.ExperienceMax;
                    obj.ExperienceMin = x.ExperienceMin;
                    obj.JobLocation = x.JobLocation;
                    obj.IsShowSalary = x.IsShowSalary;
                    obj.SalaryRange = x.SalaryRange;
                    obj.SalaryMin = x.SalaryMin;
                    obj.SalaryMax = x.SalaryMax;
                    obj.SalaryDetail = x.SalaryDetail;
                    obj.OtherBenefit = x.OtherBenefit;
                    obj.Company = new CompanyDto()
                    {
                        Id = x.Company.Id,
                        Name = x.Company.Name,
                    };
                    //PackageId = x.PackageId,
                    obj.JobCategory = new JobCategoryDto()
                    {
                        Id = x.JobCategory.Id,
                        Name = x.JobCategory.Name
                    };
                    obj.JobType = new JobTypeDto()
                    {
                        Id = x.JobType.Id,
                        Name = x.JobType.Name
                    };

                }
                return obj;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public JobPostsDto GetById(long jobPostId, string aspNetUserId, bool isApplied)
        {
            try
            {
                if (jobPostId <= 0) throw new InvalidDataException("Invalid jobpost Id ");
                if (String.IsNullOrEmpty(aspNetUserId)) throw new InvalidDataException("Invalid aspNetUser Id");
                JobPostsDto jobPost = this.GetById(jobPostId);
                if (jobPost == null) throw new MessageException("No Job found according this criteria");
                UserProfile userProfile = _userDao.GetByAspNetUserId(aspNetUserId);
                if (userProfile == null) throw new MessageException("No user found in this session");
                JobSeeker jobSeeker = _jobSeekerDao.GetUsProfileId(userProfile.Id);
                if (jobSeeker == null)
                {
                    jobPost.IsAlreadyApplied = false;
                    return jobPost;
                }
                IList<JobSeekerJobPost> jobSeekerJobPosts = _jobSeekerJobPostDao.LoadByJobPostAndJobSeekerId(jobPostId, jobSeeker.Id, isApplied);
                jobPost.IsAlreadyApplied = jobSeekerJobPosts != null && jobSeekerJobPosts.Count > 0;
                return jobPost;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private JobPostsDto CastModelFromDto(JobPost x)
        {
            return new JobPostsDto()
            {
                Id = x.Id,
                JobTitle = x.JobTitle,
                NoOfVacancies = x.NoOfVacancies,
                IsOnlineCv = x.IsOnlineCv,
                IsEmailCv = x.IsEmailCv,
                IsHardCopy = x.IsHardCopy,
                IsPhotographAttach = x.IsPhotographAttach,
                ApplyInstruction = x.ApplyInstruction,
                DeadLine = x.DeadLine,
                IsDisplayCompanyName = x.IsDisplayCompanyName,
                IsDisplayCompanyAddress = x.IsDisplayCompanyAddress,
                IsDisplayCompanyBusiness = x.IsDisplayCompanyBusiness,
                AgeRangeFrom = x.AgeRangeFrom,
                AgeRangeTo = x.AgeRangeTo,
                IsMale = x.IsMale,
                IsFemale = x.IsFemale,
                JobLevel = x.JobLevel,
                EducationalQualification = x.EducationalQualification,
                JobDescription = x.JobDescription,
                AdditionalRequirements = x.AdditionalRequirements,
                IsExperienceRequired = x.IsExperienceRequired,
                ExperienceMax = x.ExperienceMax,
                ExperienceMin = x.ExperienceMin,
                JobLocation = x.JobLocation,
                IsShowSalary = x.IsShowSalary,
                SalaryRange = x.SalaryRange,
                SalaryMin = x.SalaryMin,
                SalaryMax = x.SalaryMax,
                SalaryDetail = x.SalaryDetail,
                OtherBenefit = x.OtherBenefit,
                PublicationDate = x.CreationDate,
                Company = new CompanyDto()
                {
                    Logo = x.Company.Logo,
                    Name = x.Company.Name
                },
                //CompanyLogo = x.Company.Logo,
                PackageId = x.PackageId,
                JobCategory = new JobCategoryDto()
                {
                    Id = x.JobCategory.Id
                },
                JobType = new JobTypeDto()
                {
                    Id = x.JobCategory.Id
                },
                IsAlreadyApplied = true
            };
        }

        #endregion

        #region List of Loading function



        public IList<JobPostsDto> LoadAll()
        {
            IList<JobPost> jobList = _jobPostDao.LoadAll(JobPost.EntityStatus.Active);
            return CastToJobPostDto(jobList);

        }

        public IList<JobPostsDto> LoadAllJobAppliedByUser(string aspNetuserId)
        {
            try
            {
                if (String.IsNullOrEmpty(aspNetuserId)) throw new InvalidDataException("Invalid user");
                var userProfile = _userDao.GetByAspNetUserId(aspNetuserId);
                JobSeeker jobSeeker = _jobSeekerDao.GetUsProfileId(userProfile.Id);
                if (jobSeeker == null) throw new InvalidDataException("Please update your profile before applying for job");
                List<long> jobPostIds = _jobSeekerJobPostDao.LoadAllAppliedJobsByIds(jobSeeker.Id);
                IList<JobPost> jobPosts = _jobPostDao.LoadAll(jobPostIds);
                return CastToJobPostDto(jobPosts);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IList<JobPostsDto> LoadAllJobShortListedByUser(string aspNetuserId)
        {
            try
            {
                if (String.IsNullOrEmpty(aspNetuserId)) throw new InvalidDataException("Invalid user");
                var userProfile = _userDao.GetByAspNetUserId(aspNetuserId);
                JobSeeker jobSeeker = _jobSeekerDao.GetUsProfileId(userProfile.Id);
                if (jobSeeker == null) throw new InvalidDataException("Please update your profile before applying for job");
                List<long> jobPostIds = _jobSeekerJobPostDao.LoadAllShortlistedJobsByIds(jobSeeker.Id);
                IList<JobPost> jobPosts = _jobPostDao.LoadAll(jobPostIds);
                return CastToJobPostDto(jobPosts);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IList<JobPostsDto> LoadJobPost(int start, int length, string orderBy, string orderDir, long company, long jobCategory, long jobType, string jobTitle, int status)
        {
            try
            {
                var jobs = _jobPostDao.LoadJobPost(start, length, orderBy, orderDir, company, jobCategory, jobType, jobTitle, status);
                return jobs.Select(x => new JobPostsDto()
                {
                    Id = x.Id,
                    StatusText = StatusTypeText.GetStatusText(x.Status),
                    JobTitle = x.JobTitle,
                    NoOfVacancies = x.NoOfVacancies,
                    IsOnlineCv = x.IsOnlineCv,
                    IsEmailCv = x.IsEmailCv,
                    IsHardCopy = x.IsHardCopy,
                    IsPhotographAttach = x.IsPhotographAttach,
                    ApplyInstruction = x.ApplyInstruction,
                    DeadLine = x.DeadLine,
                    IsDisplayCompanyName = x.IsDisplayCompanyName,
                    IsDisplayCompanyAddress = x.IsDisplayCompanyAddress,
                    IsDisplayCompanyBusiness = x.IsDisplayCompanyBusiness,
                    AgeRangeFrom = x.AgeRangeFrom,
                    AgeRangeTo = x.AgeRangeTo,
                    IsMale = x.IsMale,
                    IsFemale = x.IsFemale,
                    JobLevel = x.JobLevel,
                    EducationalQualification = x.EducationalQualification,
                    JobDescription = x.JobDescription,
                    AdditionalRequirements = x.AdditionalRequirements,
                    IsExperienceRequired = x.IsExperienceRequired,
                    ExperienceMax = x.ExperienceMax,
                    ExperienceMin = x.ExperienceMin,
                    JobLocation = x.JobLocation,
                    IsShowSalary = x.IsShowSalary,
                    SalaryRange = x.SalaryRange,
                    SalaryMin = x.SalaryMin,
                    SalaryMax = x.SalaryMax,
                    SalaryDetail = x.SalaryDetail,
                    OtherBenefit = x.OtherBenefit,
                    Company = new CompanyDto()
                    {
                        Id = x.Company.Id,
                        Name = x.Company.Name,
                        Logo = x.Company.Logo
                    },
                    //PackageId = x.PackageId,
                    JobCategory = new JobCategoryDto()
                    {
                        Id = x.JobCategory.Id,
                        Name = x.JobCategory.Name
                    },
                    JobType = new JobTypeDto()
                    {
                        Id = x.JobType.Id,
                        Name = x.JobType.Name
                    }
                }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IList<JobApplicationDto> LoadJobApplication(int start, int length, string orderBy, string orderDir, long jobPostId, long companyId,
            DateTime? deadLineFrom, DateTime? deadlineTo, int deadlineFlag, bool isShortListed, string jobTitle)
        {
            try
            {
                var jobApplications = _jobPostDao.LoadJobApplication(start, length, orderBy, orderDir, jobPostId, companyId, deadLineFrom, deadlineTo, deadlineFlag, isShortListed, jobTitle);
                return jobApplications.Select(x => new JobApplicationDto()
                {
                    Id = x.Id,
                    IsShortListedByCompany = x.IsShortListedByCompany,

                    JobPost = new JobPostsDto()
                    {
                        Id = x.JobPost.Id,
                        JobTitle = x.JobPost.JobTitle,
                        DeadLine = x.JobPost.DeadLine,
                        JobCategory = new JobCategoryDto()
                        {
                            Id = x.JobPost.JobCategory.Id,
                            Name = x.JobPost.JobCategory.Name
                        },
                        JobType = new JobTypeDto()
                        {
                            Id = x.JobPost.JobType.Id,
                            Name = x.JobPost.JobType.Name
                        }
                    },
                    JobSeeker = new JobSeekerDto()
                    {
                        Id = x.JobSeeker.Id,
                        StatusText = StatusTypeText.GetStatusText(x.JobSeeker.Status),
                        FirstName = x.JobSeeker.FirstName,
                        LastName = x.JobSeeker.LastName,
                        ContactEmail = x.JobSeeker.ContactEmail,
                        ContactNumber = x.JobSeeker.ContactNumber,
                        JobSeekerDetailList = x.JobSeeker.JobSeekerDetailses.Select(r => new JobSeekerDetailsDto()
                        {
                            Id = r.Id,
                            StatusText = StatusTypeText.GetStatusText(r.Status),
                            Gender = r.Gender,
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
                    }

                }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IList<JobPostsDto> LoadJobPost(int start, int length, string orderBy, string orderDir, string what = "",
            string where = "", int ageRangeFrom = 0,
            int ageRangeTo = 0, int expRangeFrom = 0, int expRangeTo = 0, int salaryRangeFrom = 0, int salaryRangeTo = 0,
            long[] company = null, long[] jobCategory = null, long[] jobType = null)
        {
            try
            {
                var jobs = _jobPostDao.LoadJobPost(start, length, orderBy, orderDir, what, where,
             ageRangeFrom, ageRangeTo, expRangeFrom, expRangeTo, salaryRangeFrom,
             salaryRangeTo, company, jobCategory, jobType);
                return jobs.Select(x => new JobPostsDto()
                {
                    Id = x.Id,
                    StatusText = StatusTypeText.GetStatusText(x.Status),
                    JobTitle = x.JobTitle,
                    NoOfVacancies = x.NoOfVacancies,
                    IsOnlineCv = x.IsOnlineCv,
                    IsEmailCv = x.IsEmailCv,
                    IsHardCopy = x.IsHardCopy,
                    IsPhotographAttach = x.IsPhotographAttach,
                    ApplyInstruction = x.ApplyInstruction,
                    DeadLine = x.DeadLine,
                    IsDisplayCompanyName = x.IsDisplayCompanyName,
                    IsDisplayCompanyAddress = x.IsDisplayCompanyAddress,
                    IsDisplayCompanyBusiness = x.IsDisplayCompanyBusiness,
                    AgeRangeFrom = x.AgeRangeFrom,
                    AgeRangeTo = x.AgeRangeTo,
                    IsMale = x.IsMale,
                    IsFemale = x.IsFemale,
                    JobLevel = x.JobLevel,
                    EducationalQualification = x.EducationalQualification,
                    JobDescription = x.JobDescription,
                    AdditionalRequirements = x.AdditionalRequirements,
                    IsExperienceRequired = x.IsExperienceRequired,
                    ExperienceMax = x.ExperienceMax,
                    ExperienceMin = x.ExperienceMin,
                    JobLocation = x.JobLocation,
                    IsShowSalary = x.IsShowSalary,
                    SalaryRange = x.SalaryRange,
                    SalaryMin = x.SalaryMin,
                    SalaryMax = x.SalaryMax,
                    SalaryDetail = x.SalaryDetail,
                    OtherBenefit = x.OtherBenefit,
                    PublicationDate = x.CreationDate,
                    Company = new CompanyDto()
                    {
                        Id = x.Company.Id,
                        Name = x.Company.Name,
                        Logo = x.Company.Logo
                    },
                    //PackageId = x.PackageId,
                    JobCategory = new JobCategoryDto()
                    {
                        Id = x.JobCategory.Id,
                        Name = x.JobCategory.Name
                    },
                    JobType = new JobTypeDto()
                    {
                        Id = x.JobType.Id,
                        Name = x.JobType.Name
                    }
                }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region Other Function

        public int CountLoadAllJobAppliedByUser(string aspNetuserId)
        {
            try
            {
                if (String.IsNullOrEmpty(aspNetuserId)) throw new InvalidDataException("Invalid user");
                var userProfile = _userDao.GetByAspNetUserId(aspNetuserId);
                JobSeeker jobSeeker = _jobSeekerDao.GetUsProfileId(userProfile.Id);
                if (jobSeeker == null) throw new InvalidDataException("Please update your profile before applying for job");
                List<long> jobPostIds = _jobSeekerJobPostDao.LoadAllAppliedJobsByIds(jobSeeker.Id);
                IList<JobPost> jobPosts = _jobPostDao.LoadAll(jobPostIds);
                if (jobPosts == null || jobPosts.Count == 0) return 0;
                return jobPosts.Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CountLoadAllJobShortListedByUser(string aspNetuserId)
        {
            try
            {
                if (String.IsNullOrEmpty(aspNetuserId)) throw new InvalidDataException("Invalid user");
                var userProfile = _userDao.GetByAspNetUserId(aspNetuserId);
                JobSeeker jobSeeker = _jobSeekerDao.GetUsProfileId(userProfile.Id);
                if (jobSeeker == null) throw new InvalidDataException("Please update your profile before applying for job");
                List<long> jobPostIds = _jobSeekerJobPostDao.LoadAllShortlistedJobsByIds(jobSeeker.Id);
                IList<JobPost> jobPosts = _jobPostDao.LoadAll(jobPostIds);
                if (jobPosts == null || jobPosts.Count == 0) return 0;
                return jobPosts.Count;
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
                return _jobPostDao.JobPostRowCount(company, jobCategory, jobType, jobTitle, status);
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
                return _jobPostDao.JobApplicationRowCount(jobPostId, companyId, deadLineFrom, deadlineTo, deadlineFlag, isShortListed, jobTitle);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CountJobPost(string what = "", string @where = "",
            int ageRangeFrom = 0, int ageRangeTo = 0, int expRangeFrom = 0, int expRangeTo = 0, int salaryRangeFrom = 0,
            int salaryRangeTo = 0, long[] company = null, long[] jobCategory = null, long[] jobType = null)
        {
            try
            {
                return _jobPostDao.CountJobPost(what, where,
             ageRangeFrom, ageRangeTo, expRangeFrom, expRangeTo, salaryRangeFrom,
             salaryRangeTo, company, jobCategory, jobType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Helper Function
        private void ModelValidationCheck(JobPost jobPost)
        {
            try
            {
                var validationResult = ValidationHelper.ValidateEntity<JobPost, JobPost>(jobPost);
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
        IList<JobPostsDto> CastToJobPostDto(IList<JobPost> jobList)
        {
            //IList<JobPostsDto> joblistDto = new List<JobPostsDto>();
            //foreach (var x in jobList)
            //{
            //    var jp = new JobPostsDto();
            //    jp.Id = x.Id;
            //    jp.JobTitle = x.JobTitle;
            //    jp.NoOfVacancies = x.NoOfVacancies;
            //    jp.IsOnlineCv = x.IsOnlineCv;
            //    jp.IsEmailCv = x.IsEmailCv;
            //    jp.IsHardCopy = x.IsHardCopy;
            //    jp.IsPhotographAttach = x.IsPhotographAttach;
            //    jp.ApplyInstruction = x.ApplyInstruction;
            //    jp.DeadLine = x.DeadLine;
            //    jp.IsDisplayCompanyName = x.IsDisplayCompanyName;
            //    jp.IsDisplayCompanyAddress = x.IsDisplayCompanyAddress;
            //    jp.IsDisplayCompanyBusiness = x.IsDisplayCompanyBusiness;
            //    jp.AgeRangeFrom = x.AgeRangeFrom;
            //    jp.AgeRangeTo = x.AgeRangeTo;
            //    jp.IsMale = x.IsMale;
            //    jp.IsFemale = x.IsFemale;
            //    jp.JobLevel = x.JobLevel;
            //    jp.EducationalQualification = x.EducationalQualification;
            //    jp.JobDescription = x.JobDescription;
            //    jp.AdditionalRequirements = x.AdditionalRequirements;
            //    jp.IsExperienceRequired = x.IsExperienceRequired;
            //    jp.ExperienceMax = x.ExperienceMax;
            //    jp.ExperienceMin = x.ExperienceMin;
            //    jp.JobLocation = x.JobLocation;
            //    jp.IsShowSalary = x.IsShowSalary;
            //    jp.SalaryRange = x.SalaryRange;
            //    jp.SalaryMin = x.SalaryMin;
            //    jp.SalaryMax = x.SalaryMax;
            //    jp.SalaryDetail = x.SalaryDetail;
            //    jp.OtherBenefit = x.OtherBenefit;
            //    jp.Company = x.Company.Id;
            //    //CompanyName = x.Company.Name,
            //    //jp.CompanyLogo = x.Company.Logo ?? null,
            //    jp.PackageId = x.PackageId;
            //    jp.JobCategory = x.JobCategory.Id;
            //    jp.JobType = x.JobType.Id;
            //    if (x.Company != null && x.Company.Logo != null)
            //    {
            //        jp.CompanyLogo = x.Company.Logo;
            //    }
            //}
            return jobList.Select(x => new JobPostsDto()
            {
                Id = x.Id,
                JobTitle = x.JobTitle,
                NoOfVacancies = x.NoOfVacancies,
                IsOnlineCv = x.IsOnlineCv,
                IsEmailCv = x.IsEmailCv,
                IsHardCopy = x.IsHardCopy,
                IsPhotographAttach = x.IsPhotographAttach,
                ApplyInstruction = x.ApplyInstruction,
                DeadLine = x.DeadLine,
                IsDisplayCompanyName = x.IsDisplayCompanyName,
                IsDisplayCompanyAddress = x.IsDisplayCompanyAddress,
                IsDisplayCompanyBusiness = x.IsDisplayCompanyBusiness,
                AgeRangeFrom = x.AgeRangeFrom,
                AgeRangeTo = x.AgeRangeTo,
                IsMale = x.IsMale,
                IsFemale = x.IsFemale,
                JobLevel = x.JobLevel,
                EducationalQualification = x.EducationalQualification,
                JobDescription = x.JobDescription,
                AdditionalRequirements = x.AdditionalRequirements,
                IsExperienceRequired = x.IsExperienceRequired,
                ExperienceMax = x.ExperienceMax,
                ExperienceMin = x.ExperienceMin,
                JobLocation = x.JobLocation,
                IsShowSalary = x.IsShowSalary,
                SalaryRange = x.SalaryRange,
                SalaryMin = x.SalaryMin,
                SalaryMax = x.SalaryMax,
                SalaryDetail = x.SalaryDetail,
                OtherBenefit = x.OtherBenefit,

                PackageId = x.PackageId,
                Company = new CompanyDto()
                {
                    Id = x.Company.Id,
                    Name = x.Company.Name,
                    Logo = x.Company.Logo
                },

                JobCategory = new JobCategoryDto()
                {
                    Id = x.JobCategory.Id,
                    Name = x.JobCategory.Name
                },
                JobType = new JobTypeDto()
                {
                    Id = x.JobType.Id,
                    Name = x.JobType.Name
                }
            }).ToList();
        }

        private void CheckBeforeDelete(JobPost jobPost)
        {
            if (jobPost.JobSeekerJobPosts.Any())
                throw new DependencyException("You can't delete this Job, Jobseeker Alreay apply this job");
        }
        #endregion

    }
}
