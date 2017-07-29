using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Office.Interop.Word;
using Model.JobLanes.Dto;
using Model.JobLanes.ViewModel;
using NHibernate;
using Services.Joblanes;
using Services.Joblanes.Helper;
using Web.Joblanes.Helper;
using Web.Joblanes.Models.ViewModel;
using Constants = Web.Joblanes.Context.Constants;
using ImageHelper = Web.Joblanes.Helper.ImageHelper;


namespace Web.Joblanes.Controllers
{
    [Authorize]
    [RedirectingAction]
    public class CompanyController : Controller
    {
        #region Logger
        readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Objects/Properties/Services/Dao & Initialization

        //private readonly IWcfUserService _wcfUserService;
        //private readonly ICompanyAdminService _companyAdminService; 
        //private readonly IWebAdminService _webAdminService;
        private readonly CommonHelper _commonHelper;
        private readonly IRegionService _regionSevice;
        private readonly ICountryService _countryService;
        private readonly IStateService _stateService;
        private readonly ICityService _cityService;
        private readonly IOrganizationTypeService _organizationTypeService;
        private readonly IUserService _userService;
        private readonly ICompanyService _companyService;
        private readonly ICompanyDetailsService _companyDetailsService;
        private readonly IJobPostService _jobPostService;
        private readonly IProfileViewService _profileViewService;
        private readonly IJobseekerService _jobseekerService;
        private readonly ICompanyTypeService _companyTypeService;
        private readonly IJobCategoryService _jobCategoryService;
        private readonly IJobTypeService _jobTypeService;
        public CompanyController()
        {
            _commonHelper = new CommonHelper();

            ISession session = NhSessionFactory.OpenSession();
            _regionSevice = new RegionService(session);
            _countryService = new CountryService(session);
            _stateService = new StateService(session);
            _cityService = new CityService(session);
            _organizationTypeService = new OrganizationTypeService(session);
            _userService = new UserService(session);
            _companyService = new CompanyService(session);
            _companyDetailsService = new CompanyDetailsService(session);
            _jobPostService = new JobPostService(session);
            _profileViewService = new ProfileViewService(session);
            _jobseekerService = new JobSeekerService(session);
            _companyTypeService = new CompanyTypeService(session);
            _jobCategoryService= new JobCategoryService(session);
            _jobTypeService= new JobTypeService(session);
            //_companyTypeService = new CompanyService(session);
        }
        #endregion

        #region Company Profile
        [Authorize(Roles = "Employers")]
        public ActionResult Index()
        {
            var company = new CompanyDto();
            try
            {
                var userId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                company = _companyService.GetCompany(userId);
                if (company == null || !company.CompanyDetailList.Any())
                {
                    ViewBag.ErrorMessage = "Please Add profile information";
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = " Profile Load Error.";
            }

            return View(company);
        }
        #endregion

        #region Edit Profile
        [HttpGet]
        [Authorize(Roles = "Employers")]
        public ActionResult EditProfile(int type = 0, string message = "")
        {
            CompanyViewModel data = new CompanyViewModel();
            try
            {

                var userId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                var company = _companyService.GetCompany(userId);

                if (company != null)
                {
                    data.CompanyName = company.Name;
                    if (company.CompanyType != null)
                    {
                        data.CompanyType = company.CompanyType.Id;
                    }

                    data.ContactEmail = company.ContactEmail;
                    data.ContactMobile = company.ContactMobile;
                    data.ContactPerson = company.ContactPerson;
                    data.ContactPersonDesignation = company.ContactPersonDesignation;
                    data.LogoBytes = company.Logo;
                    data.UserProfileId = company.UserProfile.AspNetUserId;
                    if (company.CompanyDetailList.Any())
                    {
                        var companyDetails = company.CompanyDetailList.FirstOrDefault();
                        if (companyDetails != null)
                        {
                            data.TradeLicence = companyDetails.TradeLicence;
                            data.WebLink = companyDetails.WebLink;
                            data.LinkdinLink = companyDetails.LinkdinLink;
                            data.Vision = companyDetails.Vision;
                            data.Mission = companyDetails.Mission;
                            data.Description = companyDetails.Description;
                            data.Address = companyDetails.Address;
                            data.Zip = companyDetails.Zip;
                            data.EstablishedDate = companyDetails.EstablishedDate;
                            data.EmployeeSize = companyDetails.EmployeeSize;
                            data.Region = companyDetails.Country == null ? 0 : companyDetails.Country.Region.Id;
                            data.Country = companyDetails.Country == null ? 0 : companyDetails.Country.Id;
                            data.State = companyDetails.State != null ? companyDetails.State.Id : 0;
                            data.City = companyDetails.City == null ? 0 : companyDetails.City.Id;
                            data.TagLine = companyDetails.TagLine;
                        }
                    }

                }

                ViewBag.EmployeeSizeList = new SelectList(_commonHelper.LoadEmumToDictionary<Constants.EmployeeSize>(), "Key", "Value").ToList();

                ViewBag.CompanyTypeList = new SelectList(_companyTypeService.LoadCompanyType(EntityStatus.Active), "Id", "Name", data.CompanyType);
                ViewBag.RegionList = new SelectList(_regionSevice.LoadRegion(EntityStatus.Active), "Id", "Name", data.Region);
                ViewBag.CountryList = new SelectList(string.Empty, "Value", "Text");
                ViewBag.StateList = new SelectList(string.Empty, "Value", "Text");
                ViewBag.CityList = new SelectList(string.Empty, "Value", "Text");
                if (data.Region > 0)
                {
                    ViewBag.CountryList = new SelectList(_countryService.LoadCountry(EntityStatus.Active, data.Region), "Id", "Name", data.Country);
                }
                if (data.Country > 0)
                {
                    ViewBag.StateList = new SelectList(_stateService.LoadState(data.Region, data.Country, EntityStatus.Active), "Id", "Name", data.State);
                    ViewBag.CityList = new SelectList(_cityService.LoadCity(data.Region, data.Country, null, EntityStatus.Active), "Id", "Name", data.City);
                }
                if (data.State > 0)
                {
                    ViewBag.CityList = new SelectList(_cityService.LoadCity(data.Region, data.Country, data.State, EntityStatus.Active), "Id", "Name", data.City);
                }
                if (type == 1)
                {
                    ViewBag.SuccessMessage = message;
                }
                else if (type == 2)
                {
                    ViewBag.ErrorMessage = message;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(data);
        }

        [HttpPost]
        [Authorize(Roles = "Employers")]
        public ActionResult EditProfile(CompanyViewModel data)
        {
            try
            {
                if (ModelState.IsValid)
                {


                    var logoUpload = data.Logo;
                    var userId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                    var company = _companyService.GetCompany(userId);



                    var wcfVm = new CompanyViewModel()
                    {
                        Id = data.Id,
                        CompanyName = data.CompanyName,
                        ContactPerson = data.ContactPerson,
                        ContactPersonDesignation = data.ContactPersonDesignation,
                        ContactEmail = data.ContactEmail,
                        ContactMobile = data.ContactMobile,
                        CompanyType = data.CompanyType,
                        TradeLicence = data.TradeLicence,
                        WebLink = data.WebLink,
                        LinkdinLink = data.LinkdinLink,
                        Vision = data.Vision,
                        Mission = data.Mission,
                        Description = data.Description,
                        Address = data.Address,
                        Zip = data.Zip,
                        Region = data.Region,
                        Country = data.Country,
                        State = data.State ?? 0,
                        City = data.City,
                        TagLine = data.TagLine,
                        EmployeeSize = data.EmployeeSize,
                        EstablishedDate = data.EstablishedDate,
                        UserProfileId = userId,
                        CurrentUserProfileId = _commonHelper.GetCurrentUserProfileId(),
                        LogoBytes = logoUpload == null ? company != null ? company.Logo : null :
                            ImageHelper.ConvertFileInByteArray(logoUpload.InputStream, logoUpload.ContentLength)
                    };


                    _companyService.Save(wcfVm);

                    //data = new Models.ViewModel.CityViewModel();
                    ViewBag.SuccessMessage = "Profile Succesfully Saved";
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                ViewBag.ErrorMessage = "Company Profile Update Fail.";
            }
            finally
            {
                ViewBag.EmployeeSizeList = new SelectList(_commonHelper.LoadEmumToDictionary<Constants.EmployeeSize>(), "Key", "Value").ToList();

                ViewBag.CompanyTypeList = new SelectList(_companyTypeService.LoadCompanyType(EntityStatus.Active), "Id", "Name", data.CompanyType);
                ViewBag.RegionList = new SelectList(_regionSevice.LoadRegion(EntityStatus.Active), "Id", "Name", data.Region);
                ViewBag.CountryList = new SelectList(string.Empty, "Value", "Text");
                ViewBag.StateList = new SelectList(string.Empty, "Value", "Text");
                ViewBag.CityList = new SelectList(string.Empty, "Value", "Text");

                if (data.Region > 0)
                {
                    ViewBag.CountryList = new SelectList(_countryService.LoadCountry(EntityStatus.Active, data.Region), "Id", "Name", data.Country);
                }
                if (data.Country > 0)
                {
                    ViewBag.StateList = new SelectList(_stateService.LoadState(data.Region, data.Country, EntityStatus.Active), "Id", "Name", data.State);
                    ViewBag.CityList = new SelectList(_cityService.LoadCity(data.Region, data.Country, null, EntityStatus.Active), "Id", "Name", data.City);
                }
                if (data.State > 0)
                {
                    ViewBag.CityList = new SelectList(_cityService.LoadCity(data.Region, data.Country, data.State, EntityStatus.Active), "Id", "Name", data.City);
                }
            }
            return View(data);
        }
        #endregion

        #region Job Posting
        [Authorize(Roles = "Employers")]
        public ActionResult JobPosting()
        {
            Models.ViewModel.JobPostViewModel data = new Web.Joblanes.Models.ViewModel.JobPostViewModel();
            ViewBag.JobCategoryList = new SelectList(_jobCategoryService.LoadJobCategory(EntityStatus.Active), "Id", "Name");
            ViewBag.JobTypeList = new SelectList(_jobTypeService.LoadJobType(EntityStatus.Active), "Id", "Name");
            ViewBag.GenderList = new SelectList(_commonHelper.LoadEmumToDictionary<Constants.GenderJob>(), "Key", "Value");
            ViewBag.JobLevelList = new SelectList(_commonHelper.LoadEmumToDictionary<Constants.JobLevel>(), "Key", "Value");
            data.IsOnlineCv = true;
            data.IsExperienceRequired = true;
            data.IsShowSalary = true;
            data.IsDisplayCompanyAddress = true;
            data.IsDisplayCompanyBusiness = true;
            data.IsDisplayCompanyName = true;
            data.Gender = (int)Constants.GenderJob.All;
            return View(data);
        }


        [HttpPost]
        [Authorize(Roles = "Employers")]
        public ActionResult JobPosting(Models.ViewModel.JobPostViewModel jobPostVm)
        {
            try
            {
                if (ModelState.IsValid)
                {



                    var userId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                    var company = _companyService.GetCompany(userId);



                    var wcfVm = new Model.JobLanes.ViewModel.JobPostViewModel()
                    {
                        Id = jobPostVm.Id,
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
                        IsMale = jobPostVm.Gender == (int)Constants.GenderJob.Male ? true : false,

                        IsFemale = jobPostVm.Gender == (int)Constants.GenderJob.Female ? true : false,
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
                        Company = company.Id,
                        JobCategory = jobPostVm.JobCategory,
                        JobType = jobPostVm.JobType
                    };


                    _jobPostService.Save(wcfVm);

                    //data = new Models.ViewModel.CityViewModel();
                    ViewBag.SuccessMessage = "Job Posting Succesfully Saved";
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                
            }
            finally
            {
                ViewBag.JobCategoryList = new SelectList(_jobCategoryService.LoadJobCategory(EntityStatus.Active), "Id", "Name");
                ViewBag.JobTypeList = new SelectList(_jobTypeService.LoadJobType(EntityStatus.Active), "Id", "Name");
                ViewBag.GenderList = new SelectList(_commonHelper.LoadEmumToDictionary<Constants.GenderJob>(), "Key", "Value");
                ViewBag.JobLevelList = new SelectList(_commonHelper.LoadEmumToDictionary<Constants.JobLevel>(), "Key", "Value");

            }
            return View(jobPostVm);
        }


        [HttpPost]
        [Authorize(Roles = "Employers")]
        public ActionResult UploadFiles()
        {
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;

                    //var file = files[0];
                    //if (file != null)
                    //{
                    //    string result = new StreamReader(file.InputStream).ReadToEnd();
                    //    //BinaryReader b = new BinaryReader(file.InputStream);
                    //    //byte[] binData = b.ReadBytes(Convert.ToInt32(file.InputStream.Length));

                    //    //string result = System.Text.Encoding.UTF8.GetString(binData);

                    //    return Json(new Response(true, result));
                    //}
                    //else
                    //{
                    //    return Json(new Response(false, "Invalid File."));
                    //}


                    // //createting the object of application class  
                    // Application Objword = new Application();

                    // //creating the object of document class  
                    // Document objdoc = new Document();

                    // //get the uploaded file full path  
                    // dynamic FilePath = Path.GetFullPath(files[0].FileName);

                    // // the optional (missing) parameter to API  
                    // dynamic NA = System.Type.Missing;

                    // //open Word file document   
                    // objdoc = Objword.Documents.Open
                    //               (ref FilePath, ref NA, ref NA, ref NA, ref NA,
                    //                ref NA, ref NA, ref NA, ref NA,
                    //                ref NA, ref NA, ref NA, ref NA,
                    //                ref NA, ref NA, ref NA

                    //                );


                    // //creating the object of string builder class  
                    // StringBuilder sb = new StringBuilder();

                    // for (int Line = 0; Line < objdoc.Paragraphs.Count; Line++)
                    // {
                    //     string Filedata = objdoc.Paragraphs[Line + 1].Range.Text.Trim();

                    //     if (Filedata != string.Empty)
                    //     {
                    //         //Append word files data to stringbuilder  
                    //         sb.AppendLine(Filedata);
                    //     }

                    // }

                    // //closing document object   
                    // ((_Document)objdoc).Close();

                    // //Quit application object to end process  
                    // ((_Application)Objword).Quit();



                    //  return Json(new Response(true, Convert.ToString(sb)));


                    var file = files[0];
                    string fname;

                    // Checking for Internet Explorer  
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = file.FileName;
                    }
                    // Get the complete folder path and store the file inside it.  
                    fname = Path.Combine(Server.MapPath("~/upload/"), fname);

                    file.SaveAs(fname);
                    if (!System.IO.File.Exists(fname))
                    {
                        return Json(new Response(false, "Invalid File."));
                    }

                    if (!(Path.GetExtension(fname) == "doc" || Path.GetExtension(fname) == ".docx"))
                    {
                        return Json(new Response(false, "Invalid File."));
                    }

                    string wordText = GetTextFromWord(fname);
                    if (System.IO.File.Exists(fname))
                    {
                        System.IO.File.Delete(fname);
                    }
                    return Json(new Response(true, wordText));


                    // Returns message that successfully uploaded  
                    //return Json(new Response(false, "Invalid File."));
                }
                catch (Exception ex)
                {

                    return Json(new Response(false, "Error occurred. Error details: " + ex.Message));
                }
            }
            else
            {
                return Json(new Response(false, "No files selected."));
            }
        }

        private string GetTextFromWord(string pathname)
        {
            StringBuilder text = new StringBuilder();



            Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
            object miss = System.Reflection.Missing.Value;
            object path = pathname;
            //  object path = @"D:\Articles2.docx";
            object readOnly = true;
            Microsoft.Office.Interop.Word.Document docs = word.Documents.Open(ref path, ref miss, ref readOnly, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss);

            for (int i = 0; i < docs.Paragraphs.Count; i++)
            {
                text.Append(" \r\n " + docs.Paragraphs[i + 1].Range.Text.ToString());
            }
            //closing document object   
            ((_Document)docs).Close();

            //Quit application object to end process  
            ((_Application)word).Quit();
            return text.ToString();
        }
        #endregion

        #region Job Manage
        [Authorize(Roles = "Employers")]
        public ActionResult JobManage()
        {
            //var userId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
            //var company = _companyAdminService.GetCompany(userId);
            //var jobCount = _companyAdminService.JobPostRowCount(company.Id, 0, 0, 0);
            //var jobList = _companyAdminService.LoadJobPost(0, 10, "Id", "ASC", company.Id, 0, 0, 0);
            //var companyFullList = _companyAdminService.LoadCompany(null,null,null,null,null,null,null);
            //var companyList = _companyAdminService.LoadOnlyCompanyByStatusAndType(null,null);
            ViewBag.JobCategoryList = _jobCategoryService.LoadJobCategory(EntityStatus.Active).ToList();
            ViewBag.JobTypeList = _jobTypeService.LoadJobType(EntityStatus.Active).ToList();

            return View();
        }
        [Authorize(Roles = "Employers")]
        public JsonResult JobList(int draw, int start, int length, long jobCategory, long jobType, string jobTitle, string status)
        {
            if (Request.IsAjaxRequest())
            {
                try
                {
                    NameValueCollection nvc = Request.Form;
                    string orderBy = "";
                    string orderDir = "";

                    if (!string.IsNullOrEmpty(nvc["order[0][column]"]))
                    {
                        orderBy = nvc["order[0][column]"];
                        orderDir = nvc["order[0][dir]"];
                        switch (orderBy)
                        {
                            case "0":
                            case "1":
                                orderBy = "JobCategory";
                                break;
                            case "2":
                                orderBy = "JobType";
                                break;
                            case "3":
                                orderBy = "JobTitle";
                                break;
                            default:
                                orderBy = "";
                                break;
                        }
                    }
                    if (string.IsNullOrEmpty(status))
                        status = "0";

                    var userId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                    var company = _companyService.GetCompany(userId);
                    var recordsTotal = _jobPostService.JobPostRowCount(company.Id, jobCategory, jobType, jobTitle, Convert.ToInt32(status));
                    var dataList = _jobPostService.LoadJobPost(start, length, orderBy, orderDir, company.Id, jobCategory, jobType, jobTitle, Convert.ToInt32(status));


                    long recordsFiltered = recordsTotal;

                    var data = new List<object>();
                    int sl = start + 1;
                    foreach (var d in dataList)
                    {
                        var str = new List<string>();
                        str.Add(sl.ToString());
                        str.Add(d.JobCategory.Name);
                        str.Add(d.JobType.Name);

                        str.Add(d.JobTitle);

                        str.Add(d.StatusText);

                        str.Add("<a href='" + Url.Action("JobDetails", "Company") + "?id=" + d.Id.ToString() +
                            "' class='glyphicon glyphicon-th-list'> </a>&nbsp;&nbsp;<a href='" + Url.Action("JobUpdate", "Company") + "?id=" + d.Id.ToString() +
                            "' class='glyphicon glyphicon-pencil'> </a>&nbsp;&nbsp;<a id='" + d.Id.ToString() + "' href='#' data-name='" + d.JobTitle.ToString() +
                            "' class='glyphicon glyphicon-trash'> </a>&nbsp;&nbsp;<a href='" + Url.Action("JobApplicant", "Company") + "?jobPostId=" + d.Id.ToString() + "' class='glyphicon glyphicon-eye-open'> </a>");

                        data.Add(str);
                        sl++;
                    }
                    return Json(new
                    {
                        draw = draw,
                        recordsTotal = recordsTotal,
                        recordsFiltered = recordsFiltered,
                        start = start,
                        length = length,
                        data = data
                    });
                }
                catch (Exception ex)
                {
                    var data = new List<object>();
                    return Json(new
                    {
                        draw = draw,
                        recordsTotal = 0,
                        recordsFiltered = 0,
                        start = start,
                        length = length,
                        data = data
                    });
                }
            }
            return Json(HttpNotFound());
        }

        #endregion

        #region Job Details
        [Authorize(Roles = "Employers")]
        public ActionResult JobDetails(long id)
        {
            var job = new JobPostsDto();
            try
            {
                var userId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                var company = _companyService.GetCompany(userId);
                job = _jobPostService.GetJobPost(id, company.Id);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Load Job Posting Fail.";
            }


            return View(job);
        }
        #endregion

        #region Job Update
        [Authorize(Roles = "Employers")]
        public ActionResult JobUpdate(long id)
        {
            var obj = new Web.Joblanes.Models.ViewModel.JobPostViewModel();
            try
            {
                var userId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                var company = _companyService.GetCompany(userId);
                var x = _jobPostService.GetJobPost(id, company.Id);

                if (x == null || x.Id < 1)
                {
                    return HttpNotFound();
                }

                obj.Id = x.Id;
                obj.Status = x.Status;
                obj.JobTitle = x.JobTitle;
                obj.NoOfVacancies = x.NoOfVacancies;
                obj.IsOnlineCv = (bool)x.IsOnlineCv;
                obj.IsEmailCv = (bool)x.IsEmailCv;
                obj.IsHardCopy = (bool)x.IsHardCopy;
                obj.IsPhotographAttach = (bool)x.IsPhotographAttach;
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

                obj.Gender = (x.IsMale != null && (bool)x.IsMale)
                    ? (int)Constants.GenderJob.Male
                    : (x.IsFemale != null && (bool)x.IsFemale) ? (int)Constants.GenderJob.Female : (int)Constants.GenderJob.All;
                obj.JobCategory = x.JobCategory.Id;
                obj.JobType = x.JobType.Id;


            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "";
            }
            finally
            {
                ViewBag.JobCategoryList = new SelectList(_jobCategoryService.LoadJobCategory(EntityStatus.Active),
                   "Id", "Name", obj.JobCategory);
                ViewBag.JobTypeList = new SelectList(_jobTypeService.LoadJobType(EntityStatus.Active), "Id",
                    "Name", obj.JobType);
                ViewBag.GenderList = new SelectList(_commonHelper.LoadEmumToDictionary<Constants.GenderJob>(), "Key",
                    "Value", obj.Gender);
                ViewBag.JobLevelList = new SelectList(_commonHelper.LoadEmumToDictionary<Constants.JobLevel>(), "Key",
                    "Value", obj.JobLevel);
                ViewBag.StatusList = new SelectList(_commonHelper.GetStatus(), "Value", "Key", obj.Status);
            }
            return View(obj);
        }

        [HttpPost]
        [Authorize(Roles = "Employers")]
        public ActionResult JobUpdate(Models.ViewModel.JobPostViewModel jobPostVm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                    var company = _companyService.GetCompany(userId);
                    var x = _jobPostService.GetJobPost(jobPostVm.Id, company.Id);

                    if (x == null || x.Id < 1)
                    {
                        return HttpNotFound();
                    }

                    var wcfVm = new Model.JobLanes.ViewModel.JobPostViewModel()
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
                        IsMale = jobPostVm.Gender == (int)Constants.GenderJob.Male ? true : false,

                        IsFemale = jobPostVm.Gender == (int)Constants.GenderJob.Female ? true : false,
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
                        Company = company.Id,
                        JobCategory = jobPostVm.JobCategory,
                        JobType = jobPostVm.JobType
                    };


                    _jobPostService.Save(wcfVm);

                    //data = new Models.ViewModel.CityViewModel();
                    ViewBag.SuccessMessage = "Job Posting Succesfully Updated";
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
               
            }
            finally
            {
                ViewBag.JobCategoryList = new SelectList(_jobCategoryService.LoadJobCategory(EntityStatus.Active), "Id", "Name", jobPostVm.JobCategory);
                ViewBag.JobTypeList = new SelectList(_jobTypeService.LoadJobType(EntityStatus.Active), "Id", "Name", jobPostVm.JobType);
                ViewBag.GenderList = new SelectList(_commonHelper.LoadEmumToDictionary<Constants.GenderJob>(), "Key", "Value", jobPostVm.Gender);
                ViewBag.JobLevelList = new SelectList(_commonHelper.LoadEmumToDictionary<Constants.JobLevel>(), "Key", "Value", jobPostVm.JobLevel);
                ViewBag.StatusList = new SelectList(_commonHelper.GetStatus(), "Value", "Key", jobPostVm.Status);

            }
            return View(jobPostVm);
        }

        #endregion

        #region Delete Job
        [Authorize(Roles = "Employers")]
        public ActionResult DeleteJob(long id)
        {
            string errorMessage = "";
            try
            {
                var userId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                var company = _companyService.GetCompany(userId);
                var x = _jobPostService.GetJobPost(id, company.Id);
                if (x == null || x.Id < 1)
                {
                    return Json(new Response(false, "Invalid Job."));
                }
                _jobPostService.Delete(id);
                return Json(new Response(true, "Job sucessfully deleted."));
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
               
            }
            return Json(new Response(false, errorMessage));
        }
        #endregion

        #region Job Applicant
        [Authorize(Roles = "Employers")]
        public ActionResult JobApplicant(long jobPostId = 0)
        {
            ViewBag.JobPostId = jobPostId;
            return View();
        }

        [Authorize(Roles = "Employers")]
        public JsonResult JobApplicantList(int draw, int start, int length, long jobPostId, string jobTitle, int deadlineFlag,
            DateTime? dateFrom, DateTime? dateTo, bool isShorlisted)
        {
            if (Request.IsAjaxRequest())
            {
                try
                {
                    NameValueCollection nvc = Request.Form;
                    string orderBy = "";
                    string orderDir = "";

                    if (!string.IsNullOrEmpty(nvc["order[0][column]"]))
                    {
                        orderBy = nvc["order[0][column]"];
                        orderDir = nvc["order[0][dir]"];
                        switch (orderBy)
                        {
                            case "0":
                            case "1":
                                orderBy = "jp.JobTitle";
                                break;
                            default:
                                orderBy = "jp.DeadLine";
                                break;
                        }
                    }


                    var userId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                    var company = _companyService.GetCompany(userId);
                    //bool isShort = false;
                    var recordsTotal = _jobPostService.JobApplicationRowCount(jobPostId, company.Id, dateFrom, dateTo, deadlineFlag, isShorlisted, jobTitle);
                    var dataList = _jobPostService.LoadJobApplication(start, length, orderBy, orderDir, jobPostId, company.Id, dateFrom, dateTo, deadlineFlag, isShorlisted, jobTitle);


                    long recordsFiltered = recordsTotal;

                    var data = new List<object>();
                    int sl = start + 1;
                    foreach (var d in dataList)
                    {
                        var str = new List<string>();
                        str.Add(sl.ToString());
                        str.Add(d.JobPost.JobTitle);
                        str.Add(d.JobPost.DeadLine.ToString());
                        str.Add(d.JobSeeker.FirstName + " " + d.JobSeeker.LastName);
                        str.Add(d.JobSeeker.ContactNumber);
                        str.Add(d.JobSeeker.ContactEmail);
                        string location = "";
                        if (d.JobSeeker.JobSeekerDetailList.Any())
                        {

                            var companyDetail = d.JobSeeker.JobSeekerDetailList[0];
                            if (companyDetail.City != null)
                            {
                                location += companyDetail.City.Name + ", ";
                            }
                            if (companyDetail.State != null)
                            {
                                location += companyDetail.State.Name + ", ";
                            }
                            if (companyDetail.Country != null)
                            {
                                location += companyDetail.Country.Name + ", ";
                            }
                            if (companyDetail.Country != null)
                            {
                                location += companyDetail.Country.Region.Name + ", ";
                            }
                        }
                        str.Add(location);

                        //str.Add(d.JobCategory.Name);
                        //str.Add(d.JobType.Name);
                        //str.Add(d.JobTitle);

                        //str.Add(d.StatusText);
                        if (d.IsShortListedByCompany)
                        {
                            str.Add("<a href='" + Url.Action("JobSeekerDetails", "Company") + "?id=" +
                                    d.JobSeeker.Id.ToString() +
                                    "' class='glyphicon glyphicon-eye-open'> </a>&nbsp;&nbsp;<a id='" + d.Id.ToString() +
                                    "' href='#' data-isShort='" + d.IsShortListedByCompany + "' data-name='" +
                                    d.JobSeeker.FirstName.ToString() +
                                    "' class='glyphicon glyphicon-star-empty'> </a>");
                        }
                        else
                        {
                            str.Add("<a href='" + Url.Action("JobSeekerDetails", "Company") + "?id=" +
                                     d.JobSeeker.Id.ToString() +
                                     "' class='glyphicon glyphicon-eye-open'> </a>&nbsp;&nbsp;<a id='" + d.Id.ToString() +
                                     "' href='#' data-isShort='" + d.IsShortListedByCompany + "' data-name='" +
                                     d.JobSeeker.FirstName.ToString() +
                                     "' class='glyphicon glyphicon-star'> </a>");
                        }



                        data.Add(str);
                        sl++;
                    }
                    return Json(new
                    {
                        draw = draw,
                        recordsTotal = recordsTotal,
                        recordsFiltered = recordsFiltered,
                        start = start,
                        length = length,
                        data = data
                    });
                }
                catch (Exception ex)
                {
                    var data = new List<object>();
                    return Json(new
                    {
                        draw = draw,
                        recordsTotal = 0,
                        recordsFiltered = 0,
                        start = start,
                        length = length,
                        data = data
                    });
                }
            }
            return Json(HttpNotFound());
        }

        #endregion

        #region ShortList Application

        [Authorize(Roles = "Employers")]
        public ActionResult ShotlistApplication(long id, bool isShortListed)
        {
            string errorMessage = "";
            try
            {
                var userId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                var company = _companyService.GetCompany(userId);
                isShortListed = !isShortListed;
                if (id <= 0 || company == null)
                {
                    return Json(new Response(false, "Invalid Request"));
                }
                _jobPostService.ShotlistApplication(id, company.Id, isShortListed);
                return Json(new Response(true, "Application sucessfully shortlisted."));
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
               
            }
            return Json(new Response(false, errorMessage));
        }
        #endregion

        #region JobSeeker Details
        [Authorize(Roles = "Employers")]
        public ActionResult JobSeekerDetails(long id, string message = "", string messageType = "")
        {
            if (!String.IsNullOrEmpty(message) && !String.IsNullOrEmpty(messageType))
            {
                if (messageType == "success" || messageType == "s")
                {
                    ViewBag.SuccessMessage = message;
                }
                if (messageType == "error" || messageType == "e")
                {
                    ViewBag.ErrorMessage = message;
                }
            }
            var jobseeker = new JobSeekerDto();
            try
            {

                jobseeker = _jobseekerService.GetJobSeekerById(id, EntityStatus.Active);
                if (jobseeker == null || !jobseeker.JobSeekerDetailList.Any())
                {
                    ViewBag.ErrorMessage = " Job Seeker Profile Not found.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Job Seeker Profile Load Error.";
            }

            return View(jobseeker);
        }

        [Authorize(Roles = "Employers")]
        public ActionResult DownloadJobSeekerDetails(long jobSeekerId)
        {
            var jobseeker = new JobSeekerDto();
            try
            {

                jobseeker = _jobseekerService.GetJobSeekerById(jobSeekerId, EntityStatus.Active);
                if (jobseeker == null || !jobseeker.JobSeekerDetailList.Any())
                {
                    return RedirectToAction("JobSeekerDetails", "Admin", new { id = jobSeekerId, message = "Job Seeker Profile Not found.", messageType = "e" });
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Job Seeker Profile Load Error.";
                return RedirectToAction("JobSeekerDetails", "Admin", new { id = jobSeekerId, message = "Job Seeker Profile Load Error.", messageType = "e" });
            }

            // return View(company);
            return new Rotativa.ViewAsPdf("DownloadJobSeekerDetails", jobseeker) { FileName = jobseeker.FirstName + "_Profile.pdf" };
        }
        #endregion

        #region JobSeeker Search
        [Authorize(Roles = "Employers")]
        public ActionResult FindResume()
        {
            try
            {
                IList<FindResumeFilterDto> list = _jobseekerService.LoadFindResumeFiteDtoList();
                ViewBag.JobTitle = list.Where(x => x.Type == 1).ToList();
                ViewBag.JobExp = list.Where(x => x.Type == 2).ToList();
                ViewBag.Degree = list.Where(x => x.Type == 3).ToList();
                ViewBag.Company = list.Where(x => x.Type == 4).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Employers")]
        public ActionResult FindResumeList(int draw, int start, int length, string what, string where,
            string[] jobTitle, string[] yearExp, string[] education, string[] company)
        {
            try
            {
                int recordsTotal = _jobseekerService.JobSeekerSearchRowCount(what, where, jobTitle, yearExp, education, company);
                IList<JobSeekerSearchDto> resumeList = _jobseekerService.LoadJobSeekerSearch(start, length, "Id", "DESC", what, where, jobTitle, yearExp, education, company);

                var data = new List<object>();
                var resumeGroup = resumeList.GroupBy(m => m.JobSeekerId, (key, group) => new
                {
                    Key = key,
                    Group = group.ToList()
                });
                resumeGroup = resumeGroup.OrderBy(x => x.Key);

                foreach (var dto in resumeGroup)
                {
                    var str = new List<string>();
                    var job = ConvertPartialViewToString(PartialView("_resumeItem", (List<JobSeekerSearchDto>)dto.Group.OrderByDescending(x => x.ExpFromDate).ToList()));
                    str.Add(job);
                    data.Add(str);
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
                var data = new List<object>();
                //throw;
                return Json(new
               {
                   draw = draw,
                   recordsTotal = 0,
                   recordsFiltered = 0,
                   start = start,
                   length = length,
                   data = data
               });
            }

        }

        [HttpPost]
        [Authorize(Roles = "Employers")]
        public ActionResult FindResume(string what, string where)
        {
            ViewBag.TotalData = 197;
            ViewBag.PerPage = 10;
            ViewBag.CurrentPage = 3;
            return View();
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
        #endregion
    }
}