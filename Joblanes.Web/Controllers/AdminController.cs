using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.BuilderProperties;
using Model.JobLanes.Dto;
using Model.JobLanes.Entity;
using Model.JobLanes.ViewModel;
using NHibernate;
using Services.Joblanes;
using Services.Joblanes.Helper;
using Web.Joblanes.Context;
using Web.Joblanes.Models;

using System.Threading.Tasks;
using Rotativa;
using Web.Joblanes.Helper;
using Web.Joblanes.Models.ViewModel;
using Constants = Microsoft.AspNet.Identity.Constants;
using ImageHelper = Web.Joblanes.Helper.ImageHelper;

namespace Web.Joblanes.Controllers
{
    [Authorize]
    [RedirectingAction]
    public class AdminController : Controller
    {
        readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Objects/Properties/Services/Dao & Initialization
        private ApplicationUserManager _userManager;
        private ApplicationDbContext _db;
        //private IWcfUserService _wcfUserService;
        //private IWebAdminService _webAdminService;
        //private ICompanyAdminService _companyAdminService;
        private readonly CommonHelper _commonHelper;

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
        private readonly IRegionService _regionService;
        private readonly ICompanyTypeService _companyTypeService;
        private readonly ICountryService _countryService;
        private readonly IStateService _stateService;
        private readonly ICityService _cityService;
        private readonly IOrganizationTypeService _organizationTypeService;
        private readonly ICompanyService _companyService;
        public AdminController()
        {
            ISession session = NhSessionFactory.OpenSession();
            _userManager = new ApplicationUserManager(new UserStore<ApplicationUser>());
            _db = new ApplicationDbContext();
            _commonHelper = new CommonHelper();
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
            _regionService = new RegionService(session);
            _companyTypeService = new CompanyTypeService(session);
            _countryService = new CountryService(session);
            _stateService = new StateService(session);
            _cityService = new CityService(session);
            _organizationTypeService = new OrganizationTypeService(session);
            _companyService = new CompanyService(session);
        }
        #endregion

        #region dashboard
        [HttpGet]
        public ActionResult DashBoard()
        {
            try
            {
                IList<UserVm> list = new List<UserVm>();
                var userList = _db.Users.ToList();
                foreach (var applicationUser in userList)
                {
                    UserProfileDto profileDto = _userService.GetByAspNetUserId(applicationUser.Id);
                    var userVm = new UserVm();
                    userVm.Email = applicationUser.Email;
                    userVm.Name = applicationUser.UserName;
                    userVm.IsBlock = profileDto.IsBlock;
                    userVm.AspNetUserId = applicationUser.Id;
                    foreach (var appUserRole in _db.Roles.ToList())
                    {
                        if (applicationUser.Roles.ToList()[0].RoleId == appUserRole.Id &&
                            appUserRole.Name.Equals(UserRole.WebAdmin))
                        {
                            userVm.Role = UserRole.WebAdmin;
                            break;
                        }
                        if (applicationUser.Roles.ToList()[0].RoleId == appUserRole.Id &&
                            appUserRole.Name.Equals(UserRole.JobSeeker))
                        {
                            userVm.Role = UserRole.JobSeeker;
                            break;
                        }
                        if (applicationUser.Roles.ToList()[0].RoleId == appUserRole.Id &&
                            appUserRole.Name.Equals(UserRole.Company))
                        {
                            userVm.Role = UserRole.Company; break;
                        }
                    }
                    list.Add(userVm);
                }
                ViewBag.jobSeekList = list.Where(x => x.Role == UserRole.JobSeeker).ToList();
                ViewBag.empList = list.Where(x => x.Role == UserRole.Company).ToList();
                ViewBag.userList = list;
                return View();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw ex;
            }
        }

        [Authorize(Roles = "Web Admin")]
        [HttpGet]
        public ActionResult WebAdmin()
        {
            try
            {
                IList<ApplicationUser> webadminList = new List<ApplicationUser>();
                var userList = _db.Users;

                foreach (var applicationUser in userList.ToList())
                {
                    foreach (var appuserRole in _db.Roles.ToList())
                    {
                        if (applicationUser.Roles.ToList()[0].RoleId == appuserRole.Id &&
                            appuserRole.Name.Equals("Web Admin"))
                        {
                            webadminList.Add(applicationUser);
                            break;

                        }
                    }
                }
                ViewBag.webadminList = webadminList;
                return View();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult BlockUser(string aspNetUserId)
        {
            try
            {
                if (!Request.IsAjaxRequest())
                {
                    return Json(new Response(false, "Invalid Ajax Request"));
                }
                _userService.BlockOrUnblockUser(aspNetUserId, true);
                return Json(new Response(true, "Block Successfully"));
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult UnBlockUser(string aspNetUserId)
        {
            try
            {
                if (!Request.IsAjaxRequest())
                {
                    return Json(new Response(false, "Invalid Ajax Request"));
                }
                _userService.BlockOrUnblockUser(aspNetUserId, false);
                return Json(new Response(true, "UnBlock Successfully"));
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Company List
        [Authorize(Roles = "Web Admin")]
        public ActionResult CompanyList()
        {

            List<RegionDto> regionList = new List<RegionDto>();
            List<CountryDto> countryList = new List<CountryDto>();
            List<StateDto> stateList = new List<StateDto>();
            List<CityDto> cityList = new List<CityDto>();
            List<CompanyTypeDto> companyTypeList = new List<CompanyTypeDto>();
            try
            {
                regionList = _regionService.LoadRegion(EntityStatus.Active).ToList();
                companyTypeList = _companyTypeService.LoadCompanyType(EntityStatus.Active).ToList();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
            ViewBag.PageSize = Contants.PageSize;
            ViewBag.RegionList = regionList;
            ViewBag.CountryList = countryList;
            ViewBag.StateList = stateList;
            ViewBag.CityList = cityList;
            ViewBag.CompanyTypeList = companyTypeList;
            ViewBag.CurrentPage = 1;

            return View();

        }
        [Authorize(Roles = "Web Admin")]
        public JsonResult CompanyListData(int draw, int start, int length, string name, string zip, long companyType, string contactMobile, long region, long country, long state, long city, string status)
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
                            //orderBy = "ct.Name";
                            //break;
                            case "2":
                            case "3":
                                orderBy = "Name";
                                break;
                            case "4":
                            case "5":
                            //orderBy = "cd.Zip";
                            //break;
                            default:
                                orderBy = "Name";
                                break;
                        }
                    }
                    if (string.IsNullOrEmpty(status))
                        status = "0";
                    int recordsTotal = _companyService.CompanyRowCount(Convert.ToInt32(status), companyType, name, contactMobile,
                        "", zip, region, country, state, city);
                    long recordsFiltered = recordsTotal;

                    var dataList = _companyService.LoadCompanyForWebAdmin(start, length, orderBy, orderDir, Convert.ToInt32(status), companyType, name, contactMobile,
                        "", zip, region, country, state, city).ToList();

                    var data = new List<object>();
                    int sl = start + 1;
                    foreach (var d in dataList)
                    {
                        var str = new List<string>();
                        str.Add(sl.ToString());
                        str.Add(d.CompanyType == null ? "" : d.CompanyType.Name);
                        string location = "";
                        string zipCode = "";
                        string address = "";
                        if (d.CompanyDetailList.Any())
                        {

                            var companyDetail = d.CompanyDetailList[0];
                            zipCode = companyDetail.Zip;
                            address = companyDetail.Address;
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

                        str.Add(d.Name);

                        var image = "<img src='/images/noimage.jpg' width='80' height='80' />";
                        if (d.Logo != null)
                        {
                            var imgSrc = String.Format("data:image/gif;base64,{0}", Convert.ToBase64String(d.Logo));
                            image = "<img src='" + imgSrc + "' width='80' height='80' />";
                        }
                        str.Add(image);
                        str.Add(zipCode);
                        str.Add(address);

                        string contactInfo = "";
                        contactInfo += d.ContactPerson + " " + d.ContactPersonDesignation + " " + d.ContactMobile + " " +
                                       d.ContactEmail;
                        str.Add(contactInfo);
                        str.Add(d.StatusText);
                        string link = "<a href='" + Url.Action("CompanyDetails", "Admin") + "?id=" + d.Id.ToString() +
                                      "' class='glyphicon glyphicon-eye-open'> </a>&nbsp;&nbsp;<a href='" + Url.Action("CompanyEdit", "Admin") + "?id=" + d.Id.ToString() +
                                      "' class='glyphicon glyphicon-pencil'> </a>&nbsp;&nbsp;<a href='" +
                                      Url.Action("DownloadCompanyProfile", "Admin") + "?companyId=" + d.Id.ToString() +
                                      "' class='glyphicon glyphicon-download'> </a>&nbsp;&nbsp; ";
                        if (d.UserProfile.IsBlock)
                            link += "<a data-netuser='" + d.UserProfile.AspNetUserId + "' class='glyphicon glyphicon-ok unblock' href='#'>&nbsp;</a>";
                        else
                            link += "<a data-netuser='" + d.UserProfile.AspNetUserId + "' class='glyphicon glyphicon-off block' href='#'>&nbsp;</a>";
                        str.Add(link);
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

        #region Company Create


        #endregion

        #region Company Details

        [Authorize(Roles = "Web Admin")]
        public ActionResult CompanyDetails(long id, string message = "", string messageType = "")
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
            var company = new CompanyDto();
            try
            {

                company = _companyService.GetCompanyById(id, EntityStatus.Active);
                if (company == null || !company.CompanyDetailList.Any())
                {
                    ViewBag.ErrorMessage = " Company Profile Not found.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Company Profile Load Error.";
            }

            return View(company);
        }

        //[Authorize(Roles = "Web Admin")]
        //public ActionResult DownloadCompanyProfilePdf(long companyId)  
        //{
        //    var company = new CompanyDto();
        //    try
        //    {

        //        company = _companyAdminService.GetCompanyById(companyId, EntityStatus.Active);
        //        if (company == null || !company.CompanyDetailList.Any())
        //        {
        //            return RedirectToAction("CompanyDetails", "Admin", new { id = companyId, message = "Company Profile Not found.", messageType = "e" });

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.ErrorMessage = "Company Profile Load Error.";
        //        return RedirectToAction("CompanyDetails", "Admin", new { id = companyId, message = "Company Profile Load Error.", messageType = "e" });
        //    }

        //    return View(company);
        //}

        //[Authorize(Roles = "Web Admin")]
        //public ActionResult DownloadCompanyProfile(long companyId) 
        //{
        //    if (companyId > 0)
        //    {
        //     // return new Rotativa.ViewAsPdf("GeneratePDF", model){FileName = "TestViewAsPdf.pdf"}
        //        return new ActionAsPdf(
        //                       "DownloadCompanyProfilePdf",
        //                       new { companyId = companyId }) { FileName = "CompanyProfile.pdf" };
        //    }
        //    else
        //    {
        //        return RedirectToAction("CompanyDetails", "Admin", new { id = companyId, message = "Company Profile Not found.", messageType = "e" });
        //    }

        //}


        [Authorize(Roles = "Web Admin")]
        public ActionResult DownloadCompanyProfile(long companyId)
        {
            var company = new CompanyDto();
            try
            {

                company = _companyService.GetCompanyById(companyId, EntityStatus.Active);
                if (company == null || !company.CompanyDetailList.Any())
                {
                    return RedirectToAction("CompanyDetails", "Admin", new { id = companyId, message = "Company Profile Not found.", messageType = "e" });
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Company Profile Load Error.";
                return RedirectToAction("CompanyDetails", "Admin", new { id = companyId, message = "Company Profile Load Error.", messageType = "e" });
            }

            // return View(company);
            return new Rotativa.ViewAsPdf("DownloadCompanyProfile", company) { FileName = company.Name + "_Profile.pdf" };

        }

        #endregion

        #region Company Edit
        [Authorize(Roles = "Web Admin")]
        public ActionResult CompanyEdit(long id)
        {
            CompanyViewModel data = new CompanyViewModel();
            var company = new CompanyDto();
            try
            {

                company = _companyService.GetCompanyById(id, EntityStatus.Active);
                if (company == null || !company.CompanyDetailList.Any())
                {
                    ViewBag.ErrorMessage = " Company Profile Not found.";
                }

                if (company != null)
                {
                    data.Id = id;
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

                ViewBag.EmployeeSizeList = new SelectList(_commonHelper.LoadEmumToDictionary<Web.Joblanes.Context.Constants.EmployeeSize>(), "Key", "Value").ToList();

                ViewBag.CompanyTypeList = new SelectList(_companyTypeService.LoadCompanyType(EntityStatus.Active), "Id", "Name", data.CompanyType);
                ViewBag.RegionList = new SelectList(_regionService.LoadRegion(EntityStatus.Active), "Id", "Name", data.Region);
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
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Company Profile Load Error.";
            }

            return View(data);
        }

        [HttpPost]
        [Authorize(Roles = "Web Admin")]
        public ActionResult CompanyEdit(CompanyViewModel data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var logoUpload = data.Logo;
                    var company = _companyService.GetCompanyById(data.Id, EntityStatus.Active);
                    if (company != null)
                    {
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
                            UserProfileId = company.UserProfile.AspNetUserId,
                            //CurrentUserProfileId = _commonHelper.GetCurrentUserProfileId(),
                            LogoBytes = logoUpload == null
                                ? company.Logo
                                : ImageHelper.ConvertFileInByteArray(logoUpload.InputStream, logoUpload.ContentLength)
                        };
                        _companyService.Save(wcfVm);

                        //data = new Models.ViewModel.CityViewModel();
                        ViewBag.SuccessMessage = "Company Profile Succesfully Saved";
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Company Profile Update Fail.";
                    }
                }
            }
            finally
            {
                ViewBag.EmployeeSizeList = new SelectList(_commonHelper.LoadEmumToDictionary<Web.Joblanes.Context.Constants.EmployeeSize>(), "Key", "Value").ToList();

                ViewBag.CompanyTypeList = new SelectList(_companyTypeService.LoadCompanyType(EntityStatus.Active), "Id", "Name", data.CompanyType);
                ViewBag.RegionList = new SelectList(_regionService.LoadRegion(EntityStatus.Active), "Id", "Name", data.Region);
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

        #region JobSeekerList
        [Authorize(Roles = "Web Admin")]
        public ActionResult JobSeekerList()
        {

            List<RegionDto> regionList = new List<RegionDto>();
            List<CountryDto> countryList = new List<CountryDto>();
            List<StateDto> stateList = new List<StateDto>();
            List<CityDto> cityList = new List<CityDto>();
            try
            {
                regionList = _regionService.LoadRegion(EntityStatus.Active).ToList();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
            ViewBag.PageSize = Contants.PageSize;
            ViewBag.RegionList = regionList;
            ViewBag.CountryList = countryList;
            ViewBag.StateList = stateList;
            ViewBag.CityList = cityList;
            ViewBag.CurrentPage = 1;

            return View();

        }

        [Authorize(Roles = "Web Admin")]
        public JsonResult JobSeekerListData(int draw, int start, int length, string name, string zip, string lName, string contactMobile, long region, long country, long state, long city, string status)
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
                            case "2":
                                orderBy = "FirstName";
                                break;
                            case "3":
                                orderBy = "LastName";
                                break;
                            case "4":
                            case "5":
                                orderBy = "cd.Zip";
                                break;
                            default:
                                orderBy = "FirstName";
                                break;
                        }
                    }
                    if (string.IsNullOrEmpty(status))
                        status = "0";
                    int recordsTotal = _jobseekerService.JobSeekerRowCount(Convert.ToInt32(status), name, lName, contactMobile,
                        "", zip, region, country, state, city);
                    long recordsFiltered = recordsTotal;

                    var dataList = _jobseekerService.LoadJobSeekerForWebAdmin(start, length, orderBy, orderDir, Convert.ToInt32(status), name, lName, contactMobile,
                        "", zip, region, country, state, city).ToList();

                    var data = new List<object>();
                    int sl = start + 1;
                    foreach (var d in dataList)
                    {
                        var str = new List<string>();
                        str.Add(sl.ToString());

                        string location = "";
                        string zipCode = "";
                        string address = "";
                        if (d.JobSeekerDetailList.Any())
                        {

                            var companyDetail = d.JobSeekerDetailList[0];
                            zipCode = companyDetail.ZipCode;
                            address = companyDetail.Address;
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

                        str.Add(d.FirstName);
                        str.Add(d.LastName);

                        //var image = "<img src='/images/noimage.jpg' width='80' height='80' />";
                        //if (d.ProfileImage != null)
                        //{
                        //    var imgSrc = String.Format("data:image/gif;base64,{0}", Convert.ToBase64String(d.ProfileImage));
                        //    image = "<img src='" + imgSrc + "' width='80' height='80' />";
                        //}
                        //str.Add(image);
                        str.Add(zipCode);
                        //str.Add(address);

                        string contactInfo = "";
                        contactInfo += d.ContactNumber + " " +
                                       d.ContactEmail;
                        str.Add(contactInfo);
                        str.Add(d.StatusText);
                        string link = "<a href='" + Url.Action("JobSeekerDetails", "Admin") + "?id=" + d.Id.ToString() +
                           "' class='glyphicon glyphicon-eye-open'> </a>&nbsp;&nbsp;<a href='" + Url.Action("DownloadJobSeekerDetails", "Admin") + "?jobSeekerId=" + d.Id.ToString() +
                           "' class='glyphicon glyphicon-download'> </a>&nbsp;&nbsp; ";
                        if (d.UserProfile.IsBlock)
                            link += "<a data-netuser='" + d.UserProfile.AspNetUserId + "' class='glyphicon glyphicon-ok unblock' href='#'>&nbsp;</a>";
                        else
                            link += "<a data-netuser='" + d.UserProfile.AspNetUserId + "' class='glyphicon glyphicon-off block' href='#'>&nbsp;</a>";
                        str.Add(link);
                        str.Add("");
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

        #region JobSeeker Details
        [Authorize(Roles = "Web Admin")]
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

        [Authorize(Roles = "Web Admin")]
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
    }
}