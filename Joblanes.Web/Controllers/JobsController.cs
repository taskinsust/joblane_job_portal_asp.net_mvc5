using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Model.JobLanes.Dto;
using NHibernate;
using Services.Joblanes;
using Services.Joblanes.Helper;
using Web.Joblanes.Helper;


namespace Web.Joblanes.Controllers
{
    [Authorize]
    public class JobsController : Controller
    {
        #region Logger
        readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Objects/Properties/Services/Dao & Initialization

        //private readonly IWcfUserService _wcfUserService;
        //private readonly IWebAdminService _webAdminService;
        //private ICompanyAdminService _companyAdminService;
        private readonly CommonHelper _commonHelper;
        private readonly IJobseekerService _jobseekerService;
        private readonly IJobSeekerDetailsService _jobSeekerDetailsService;
        private readonly IJobSeekerEducationalQualificationService _jobSeekerEducationalQualificationService;
        private readonly IJobSeekerExperienceService _jobSeekerExperienceService;
        private readonly IJobSeekerTrainingCoursesService _jobSeekerTrainingCoursesService;
        private readonly IJobPostService _jobPostService;
        private readonly IJobSeekerJobPostService _jobSeekerJobPostService;
        private readonly IJobSeekerCvBankService _jobSeekerCvBankService;
        private readonly IJobSeekerSkillService _jobSeekerSkillService;
        private readonly IRegionService _regionSevice;
        private readonly ICountryService _countryService;
        private readonly IStateService _stateService;
        private readonly ICityService _cityService;
        private readonly IOrganizationTypeService _organizationTypeService;
        private readonly IJobCategoryService _jobCategoryService;
        private readonly IJobTypeService _jobTypeService;
        private readonly ICompanyService _companyService;
        public JobsController()
        {
            _commonHelper = new CommonHelper();
          //  _wcfUserService = new WcfUserServiceClient();
          //_webAdminService = new WebAdminServiceClient();
          //  _companyAdminService = new CompanyAdminServiceClient();
            ISession session = NhSessionFactory.OpenSession();
           _jobseekerService = new JobSeekerService(session);
            _jobSeekerDetailsService = new JobSeekerDetailsService(session);
            _jobSeekerEducationalQualificationService = new JobSeekerEducationalQualificationService(session);
            _jobSeekerExperienceService = new JobSeekerExperienceService(session);
            _jobSeekerTrainingCoursesService = new JobSeekerTrainingCoursesService(session);
            _jobPostService = new JobPostService(session);
            _jobSeekerCvBankService = new JobSeekerCvBankService(session);
            _jobSeekerSkillService = new JobSeekerSkillService(session);
            _jobSeekerJobPostService = new JobSeekerJobPostService(session);
            _regionSevice = new RegionService(session);
            _countryService = new CountryService(session);
            _stateService = new StateService(session);
            _cityService = new CityService(session);
            _organizationTypeService = new OrganizationTypeService(session);
            _jobCategoryService = new JobCategoryService(session);
            _jobTypeService = new JobTypeService(session);
            _companyService = new CompanyService(session);
        }
        #endregion

        #region Browse Job
        [AllowAnonymous]
        public ActionResult BrowseJob(string what = "")
        {
            try
            {
                Initialize();
                if (!String.IsNullOrEmpty(what))
                {
                    var ary = what.Split(new[] { "_??!_" }, StringSplitOptions.None);
                    what = ary[0];
                    var where = ary[1];
                    ViewBag.what = what;
                    ViewBag.where = where;
                }
                return View();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult BrowseJobPost(int draw, int start, int length, string orderBy, string orderDir, string what = "",
            string where = "", int ageRangeFrom = 0, int ageRangeTo = 0, int expRangeFrom = 0, int expRangeTo = 0,
            int salaryRangeFrom = 0, int salaryRangeTo = 0, long[] company = null, long[] jobCategory = null, long[] jobType = null)
        {
            try
            {
                IList<JobPostsDto> jobList = _jobPostService.LoadJobPost(start, length, "", "", what, where, ageRangeFrom, ageRangeTo, expRangeFrom, expRangeTo, salaryRangeFrom, salaryRangeTo, company, jobCategory, jobType);
                int recordsTotal = _jobPostService.CountJobPost(what, where, ageRangeFrom, ageRangeTo, expRangeFrom, expRangeTo, salaryRangeFrom, salaryRangeTo, company, jobCategory, jobType);
                var data = new List<object>();
                foreach (var dto in jobList)
                {
                    dto.JobDescription = GetJobDescription(dto.JobDescription);
                    var str = new List<string>();
                    if (dto.DeadLine != null)
                    {
                        var job = ConvertPartialViewToString(PartialView("_jobItem", dto));
                        str.Add(job);
                        data.Add(str);
                    }


                }
                return Json(new
                {
                    draw = draw,
                    recordsTotal = recordsTotal,
                    recordsFiltered = recordsTotal,
                    start = start,
                    length = length,
                    data = data
                });

            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        private string GetJobDescription(string description)
        {
            if (String.IsNullOrEmpty(description)) return "";
            var len = description.Length;
            if (len < 200) return description;
            return description.Substring(0, 199);
        }
        [AllowAnonymous]
        public ActionResult JobPostDetail(long id)
        {
            try
            {
                if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    var userId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                    JobPostsDto jobPostDto = _jobPostService.GetById(id, userId, true);
                    //var jobcategory = _webAdminService.GetJobCategoryById(jobPostDto.JobCategory);
                    return View(jobPostDto);
                }
                else
                {
                    JobPostsDto jobPostDto = _jobPostService.GetById(id);
                    return View(jobPostDto);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }


        #endregion

        #region Helper
        protected string ConvertPartialViewToString(PartialViewResult partialView)
        {
            using (var sw = new StringWriter())
            {
                partialView.View = ViewEngines.Engines
                  .FindPartialView(ControllerContext, partialView.ViewName).View;

                var vc = new ViewContext(
                  ControllerContext, partialView.View, partialView.ViewData, partialView.TempData, sw);
                partialView.View.Render(vc, sw);

                var partialViewString = sw.GetStringBuilder().ToString();

                return partialViewString;
            }
        }
        private void Initialize()
        {
            ViewBag.GenderList = new SelectList(_commonHelper.LoadEmumToDictionary<Web.Joblanes.Context.Constants.Gender>(), "Key", "Value").ToList();
            ViewBag.MaritalList = new SelectList(_commonHelper.LoadEmumToDictionary<Web.Joblanes.Context.Constants.MaritalType>(), "Key", "Value").ToList();

            ViewBag.RegionList = new SelectList(_regionSevice.LoadRegion(0, int.MaxValue, "", "", "", 1), "Id", "Name");
            ViewBag.CountryList = new SelectList(String.Empty, "Value", "Text");
            ViewBag.StateList = new SelectList(String.Empty, "Value", "Text");
            ViewBag.CityList = new SelectList(String.Empty, "Value", "Text");
            ViewBag.JobCategory = new SelectList(_jobCategoryService.LoadJobCategory(1), "Id", "Name");
            ViewBag.jobType = new SelectList(_jobTypeService.LoadJobType(0, int.MaxValue - 2, "", "", "", 1), "Id", "Name");
        }
        #endregion

        #region Home Page

        [AllowAnonymous]
        [HttpGet]
        public ActionResult FeaturedJobs()
        {
            try
            {
                IList<CompanyDto> companyDto = _companyService.LoadCompany(1, 0, "", 0, 0, 0, 0);
                return PartialView("_featuredJob", companyDto);

            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult LatestJob()
        {
            try
            {
                IList<JobPostsDto> jobList = _jobPostService.LoadJobPost(0, 10, "CreationDate", "desc", "", "", 0, 0, 0, 0, 0, 0, null, null, null);
                foreach (var dto in jobList)
                {
                    dto.JobDescription = GetJobDescription(dto.JobDescription);
                }
                return PartialView("_latestJob", jobList);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult JobCategory()
        {
            try
            {
                IList<JobCategoryDto> jobCategoryDtos = _jobCategoryService.LoadJobCategory(0, int.MaxValue - 2, "", "", "", 1);
                return PartialView("_jobCategory", jobCategoryDtos);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult JobCategoryJobs(long id)
        {
            try
            {
                var jobCategoryDto = _jobCategoryService.GetById(id);
                ViewBag.categoryName = jobCategoryDto.Name;
                ViewBag.jobcategoryId = id;
                return View("JobCategoryJobs");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw ex;
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult JobCategoryJobsPost(int draw, int start, int length, long id)
        {
            try
            {
                IList<JobPostsDto> jobList = _jobPostService.LoadJobPost(start, length, "CreationDate", "desc", 0, id, 0, "", 1);
                var recordsTotal = _jobPostService.JobPostRowCount(0, id, 0, "", 1);
                var data = new List<object>();
                foreach (var dto in jobList)
                {
                    dto.JobDescription = GetJobDescription(dto.JobDescription);
                    var str = new List<string>();
                    if (dto.DeadLine != null)
                    {
                        var job = ConvertPartialViewToString(PartialView("_jobItemFromCompanyWebService", dto));
                        str.Add(job);
                        data.Add(str);
                    }
                }
                return Json(new
                {
                    draw = draw,
                    recordsTotal = recordsTotal,
                    recordsFiltered = recordsTotal,
                    start = start,
                    length = length,
                    data = data
                });
            }
            catch (Exception)
            {
                return Json(new
                {
                    draw = draw,
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    start = start,
                    length = length,
                    data = new List<object>()
                });
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult CompanyJobs(long id)
        {
            try
            {
                var company = _companyService.GetCompanyById(id, 1);
                ViewBag.companyName = company.Name;
                ViewBag.companyId = id;
                return View("CompanyJobs");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw ex;
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult CompanyJobsPost(int draw, int start, int length, long id)
        {
            try
            {
                IList<JobPostsDto> jobList = _jobPostService.LoadJobPost(start, length, "CreationDate", "desc", id, 0, 0, "", 1);
                var recordsTotal = _jobPostService.JobPostRowCount(id, 0, 0, "", 1);
                var data = new List<object>();
                foreach (var dto in jobList)
                {
                    dto.JobDescription = GetJobDescription(dto.JobDescription);
                    var str = new List<string>();
                    if (dto.DeadLine != null)
                    {
                        var job = ConvertPartialViewToString(PartialView("_jobItemFromCompanyWebService", dto));
                        str.Add(job);
                        data.Add(str);
                    }
                }
                return Json(new
                {
                    draw = draw,
                    recordsTotal = recordsTotal,
                    recordsFiltered = recordsTotal,
                    start = start,
                    length = length,
                    data = data
                });
            }
            catch (Exception)
            {
                return Json(new
                {
                    draw = draw,
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    start = start,
                    length = length,
                    data = new List<object>()
                });
            }
        }
        #endregion
    }
}