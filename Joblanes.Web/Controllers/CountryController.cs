using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Policy;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Model.JobLanes.Dto;
using Model.JobLanes.ViewModel;
using Newtonsoft.Json;
using NHibernate;
using Services.Joblanes;
using Services.Joblanes.Helper;
using Web.Joblanes.Helper;
using Web.Joblanes.Models;
using ImageHelper = Web.Joblanes.Helper.ImageHelper;

namespace Web.Joblanes.Controllers
{
    [Authorize]
    [RedirectingAction]
    public class CountryController : Controller
    {
        #region Logger
        readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Objects/Properties/Services/Dao & Initialization
        // private IWebAdminService _webAdminService;
        private readonly ICommonHelper _commonHelper;
        private readonly IRegionService _regionSevice;
        private readonly ICountryService _countryService;
        private readonly IStateService _stateService;
        private readonly ICityService _cityService;
        private readonly IOrganizationTypeService _organizationTypeService;
        private readonly ICompanyTypeService _companyTypeService;
        private readonly IJobTypeService _jobTypeService;
        private readonly IJobCategoryService _jobCategoryService;
        private readonly IPaymentTypeService _paymentTypeService;
        private readonly IJobseekerService _jobseekerService;
        private readonly ICompanyService _companyService;
        private readonly IUserService _userService;
        public CountryController()
        {
            //_webAdminService = new WebAdminServiceClient();
            _commonHelper = new CommonHelper();
            ISession session = NhSessionFactory.OpenSession();
            _regionSevice = new RegionService(session);
            _countryService = new CountryService(session);
            _stateService = new StateService(session);
            _cityService = new CityService(session);
            _organizationTypeService = new OrganizationTypeService(session);
            _companyTypeService = new CompanyTypeService(session);
            _jobTypeService = new JobTypeService(session);
            _jobCategoryService = new JobCategoryService(session);
            _paymentTypeService = new PaymentTypeService(session);
            _jobseekerService = new JobSeekerService(session);
            _companyService = new CompanyService(session);
            _userService = new UserService(session);
        }
        #endregion

        #region Manage Country
        [Authorize(Roles = "Web Admin")]
        public ActionResult Index()
        {
            List<RegionDto> regionList = new List<RegionDto>();
            try
            {
                regionList = _regionSevice.LoadRegion(EntityStatus.Active).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
            ViewBag.PageSize = Contants.PageSize;
            ViewBag.RegionList = regionList;
            ViewBag.CurrentPage = 1;
            return View();
        }

        [Authorize(Roles = "Web Admin")]
        public JsonResult CountryList(int draw, int start, int length, string name, string shortName, string callingCode, long region, string status)
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
                                orderBy = "Name";
                                break;
                            case "3":
                                orderBy = "ShortName";
                                break;
                            case "4":
                                orderBy = "CallingCode";
                                break;
                            default:
                                orderBy = "";
                                break;
                        }
                    }
                    if (string.IsNullOrEmpty(status))
                        status = "0";
                    int recordsTotal = _countryService.CountryRowCount(name, shortName, callingCode, region, Convert.ToInt32(status));
                    long recordsFiltered = recordsTotal;

                    // List<CountryListDto> dataList = _webAdminService.LoadCountry(start, length, orderBy, orderDir, name, Convert.ToInt32(status)).ToList();
                    var dataList = _countryService.LoadCountry(start, length, orderBy, orderDir, name, shortName, callingCode, region, Convert.ToInt32(status)).ToList();

                    var data = new List<object>();
                    int sl = start + 1;

                    foreach (var d in dataList)
                    {
                        var image = "<img src='/images/noimage.jpg' width='80' height='80' />";
                        if (d.FlagBytes != null)
                        {
                            var imgSrc = String.Format("data:image/gif;base64,{0}", Convert.ToBase64String(d.FlagBytes));
                            image = "<img src='" + imgSrc + "' width='80' height='80' />";
                        }

                        var str = new List<string>();
                        str.Add(sl.ToString());
                        str.Add(d.Region.Name);
                        str.Add(d.Name);
                        str.Add(d.ShortName);
                        str.Add(d.CallingCode);
                        str.Add(image);
                        str.Add(d.StatusText);

                        str.Add("<a href='" + Url.Action("Details", "Country") + "?id=" + d.Id.ToString() +
                            "' class='glyphicon glyphicon-th-list'> </a>&nbsp;&nbsp;<a href='" + Url.Action("Update", "Country") + "?id=" + d.Id.ToString() +
                            "' class='glyphicon glyphicon-pencil'> </a>&nbsp;&nbsp;<a id='" + d.Id.ToString() + "' href='#' data-name='" + d.Name.ToString() + "' class='glyphicon glyphicon-trash'> </a>");
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

        #region Save Operation
        [Authorize(Roles = "Web Admin")]
        public ActionResult Create()
        {
            Models.ViewModel.CountryViewModel data = new Web.Joblanes.Models.ViewModel.CountryViewModel();
            try
            {

                ViewBag.RegionList = new SelectList(_regionSevice.LoadRegion(EntityStatus.Active), "Id", "Name");
            }
            catch (Exception ex)
            {
                throw;
            }
            return View(data);
        }

        [Authorize(Roles = "Web Admin")]
        [HttpPost]

        public ActionResult Create(Models.ViewModel.CountryViewModel data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var flagUpload = data.Flag;
                    var wcfVm = new CountryViewModel()
                    {
                        Id = data.Id,
                        Name = data.Name,
                        ShortName = data.ShortName,
                        Region = data.Region,
                        CallingCode = data.CallingCode,
                        Status = data.Status,
                        CurrentUserProfileId = _commonHelper.GetCurrentUserProfileId(),
                        FlagBytes = flagUpload == null ? null : ImageHelper.ConvertFileInByteArray(flagUpload.InputStream, flagUpload.ContentLength)
                    };

                    _countryService.Save(wcfVm);

                    data = new Models.ViewModel.CountryViewModel();
                    ViewBag.SuccessMessage = "Country Succesfully Saved";
                }
            }

            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Country Save Fail.";
            }
            finally
            {
                ViewBag.RegionList = new SelectList(_regionSevice.LoadRegion(EntityStatus.Active), "Id", "Name", data.Region);
            }

            return View(data);
        }
        #endregion

        #region Update Operation
        [Authorize(Roles = "Web Admin")]
        public ActionResult Update(long id)
        {

            var formDataVm = new Models.ViewModel.CountryViewModel();
            try
            {
                var dataObj = _countryService.GetById(id);
                if (dataObj.Id < 1)
                {
                    return HttpNotFound();
                }

                formDataVm.Id = dataObj.Id;
                formDataVm.Name = dataObj.Name;
                formDataVm.Status = dataObj.Status;
                formDataVm.ShortName = dataObj.ShortName;
                formDataVm.CallingCode = dataObj.CallingCode;
                formDataVm.FlagBytes = dataObj.FlagBytes;
                formDataVm.Region = dataObj.Region.Id;
                ViewBag.RegionList = new SelectList(_regionSevice.LoadRegion(EntityStatus.Active), "Id", "Name", formDataVm.Region);
                ViewBag.StatusList = new SelectList(_commonHelper.GetStatus(), "Value", "Key", formDataVm.Status);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "";

            }

            return View(formDataVm);
        }

        [Authorize(Roles = "Web Admin")]
        [HttpPost]
        public ActionResult Update(Models.ViewModel.CountryViewModel formData)
        {

            try
            {

                if (ModelState.IsValid)
                {
                    var dataObj = _countryService.GetById(formData.Id);
                    if (dataObj.Id < 1)
                    {
                        return HttpNotFound();
                    }
                    var flagUpload = formData.Flag;
                    var wcfVm = new CountryViewModel()
                    {
                        Id = formData.Id,
                        Name = formData.Name,
                        Status = formData.Status,
                        ShortName = formData.ShortName,
                        Region = formData.Region,
                        CallingCode = formData.CallingCode,
                        CurrentUserProfileId = _commonHelper.GetCurrentUserProfileId(),
                        FlagBytes = flagUpload == null ? dataObj.FlagBytes : ImageHelper.ConvertFileInByteArray(
                                    flagUpload.InputStream, flagUpload.ContentLength)

                    };
                    _countryService.Save(wcfVm);
                    ViewBag.SuccessMessage = "Country Succesfully Update";
                }
            }

            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Country Update Fail.";
            }
            finally
            {
                ViewBag.RegionList = new SelectList(_regionSevice.LoadRegion(EntityStatus.Active), "Id",
                    "Name", formData.Region);
            }
            ViewBag.StatusList = new SelectList(_commonHelper.GetStatus(), "Value", "Key", formData.Status);
            return View(formData);
        }
        #endregion

        #region Details View
        [Authorize(Roles = "Web Admin")]
        public ActionResult Details(long id)
        {
            var dataObj = new CountryDto();

            try
            {
                dataObj = _countryService.GetById(id);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong";
            }
            if (dataObj == null)
            {
                return HttpNotFound();
            }

            return View(dataObj);
        }
        #endregion

        #region Delete Operation
        [Authorize(Roles = "Web Admin")]
        public ActionResult Delete(long id)
        {
            string errorMessage = "";
            try
            {
                _countryService.Delete(id);
                return Json(new Response(true, "Country sucessfully deleted."));
            }
            catch (Exception ex)
            {

                errorMessage = "Something went wrong";

            }
            return Json(new Response(false, errorMessage));
        }

        #endregion

        #region Ajax Operation

        public JsonResult LoadCountry(long regions)
        {
            try
            {
                // _webAdminService.LoadCountryByCriteria(EntityStatus.Active, regions);
                var countrySelectList = new SelectList(_countryService.LoadCountry(EntityStatus.Active, regions), "Id", "Name");
                return Json(new { returnList = countrySelectList, IsSuccess = true });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Problem Occurred During Load Country";
                return Json(new Response(false, "Problem Occurred During Load Country"));
            }
        }

        #endregion
    }
}