using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;
using Model.JobLanes.Dto;
using Model.JobLanes.ViewModel;
using NHibernate;
using Services.Joblanes;
using Services.Joblanes.Helper;
using Web.Joblanes.Helper;
using Web.Joblanes.Models;

namespace Web.Joblanes.Controllers
{
    [Authorize]
    [RedirectingAction]
    public class CityController : Controller
    {
        #region Logger
        readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Objects/Properties/Services/Dao & Initialization
        //private IWebAdminService _webAdminService;
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
        public CityController()
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

        #region Manage City
        [Authorize(Roles = "Web Admin")]
        public ActionResult Index()
        {
            List<RegionDto> regionList = new List<RegionDto>();
            List<CountryDto> countryList = new List<CountryDto>();
            List<StateDto> stateList = new List<StateDto>();
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
            ViewBag.CountryList = countryList;
            ViewBag.StateList = stateList;
            ViewBag.CurrentPage = 1;
            return View();
        }

        [Authorize(Roles = "Web Admin")]
        public JsonResult CityList(int draw, int start, int length, string name, string shortName, long region, long country, long state, string status)
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
                            case "3":
                            case "4":
                                orderBy = "Name";
                                break;
                            case "5":
                                orderBy = "ShortName";
                                break;
                            default:
                                orderBy = "";
                                break;
                        }
                    }
                    if (string.IsNullOrEmpty(status))
                        status = "0";
                    int recordsTotal = _cityService.CityRowCount(name, shortName, region, country, state, Convert.ToInt32(status));
                    long recordsFiltered = recordsTotal;

                    // List<CityListDto> dataList = _webAdminService.LoadCity(start, length, orderBy, orderDir, name, Convert.ToInt32(status)).ToList();
                    var dataList = _cityService.LoadCity(start, length, orderBy, orderDir, name, shortName, region, country, state, Convert.ToInt32(status)).ToList();

                    var data = new List<object>();
                    int sl = start + 1;
                    foreach (var d in dataList)
                    {
                        var str = new List<string>();
                        str.Add(sl.ToString());
                        str.Add(d.Country.Region.Name);
                        str.Add(d.Country.Name);
                        if (d.State != null)
                        {
                            str.Add(d.State.Name);
                        }
                        else
                        {
                            str.Add("");
                        }
                        str.Add(d.Name);
                        str.Add(d.ShortName);
                        str.Add(d.StatusText);

                        str.Add("<a href='" + Url.Action("Details", "City") + "?id=" + d.Id.ToString() +
                            "' class='glyphicon glyphicon-th-list'> </a>&nbsp;&nbsp;<a href='" + Url.Action("Update", "City") + "?id=" + d.Id.ToString() +
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
            Models.ViewModel.CityViewModel data = new Web.Joblanes.Models.ViewModel.CityViewModel();
            try
            {
                ViewBag.RegionList = new SelectList(_regionSevice.LoadRegion(EntityStatus.Active), "Id", "Name");
                ViewBag.CountryList = new SelectList(string.Empty, "Value", "Text");
                ViewBag.StateList = new SelectList(string.Empty, "Value", "Text");
            }
            catch (Exception ex)
            {
                throw;
            }
            return View(data);
        }

        [HttpPost]
        [Authorize(Roles = "Web Admin")]
        public ActionResult Create(Models.ViewModel.CityViewModel data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var wcfVm = new CityViewModel()
                    {
                        Id = data.Id,
                        Name = data.Name,
                        Status = data.Status,
                        ShortName = data.ShortName,
                        Country = data.Country,
                        CurrentUserProfileId = _commonHelper.GetCurrentUserProfileId(),
                        State = data.State ?? 0,
                    };
                    _cityService.Save(wcfVm);

                    data = new Models.ViewModel.CityViewModel();
                    ViewBag.SuccessMessage = "City Succesfully Saved";
                }
            }
            finally
            {
                ViewBag.RegionList = new SelectList(_regionSevice.LoadRegion(EntityStatus.Active), "Id", "Name", data.Region);
                ViewBag.CountryList = new SelectList(string.Empty, "Value", "Text", data.Country);
                ViewBag.StateList = new SelectList(string.Empty, "Value", "Text");
                if (data.Region > 0)
                {
                    ViewBag.CountryList = new SelectList(_countryService.LoadCountry(EntityStatus.Active, data.Region), "Id", "Name", data.Country);
                }
                if (data.Country > 0)
                {
                    ViewBag.StateList = new SelectList(_stateService.LoadState(null, data.Country, EntityStatus.Active), "Id", "Name", data.State);
                }

            }

            return View(data);
        }
        #endregion

        #region Update Operation
        [Authorize(Roles = "Web Admin")]
        public ActionResult Update(long id)
        {

            var formDataVm = new Models.ViewModel.CityViewModel();
            try
            {
                var dataObj = _cityService.GetById(id);
                if (dataObj.Id < 1)
                {
                    return HttpNotFound();
                }

                formDataVm.Id = dataObj.Id;
                formDataVm.Name = dataObj.Name;
                formDataVm.Status = dataObj.Status;
                formDataVm.ShortName = dataObj.ShortName;
                formDataVm.Country = dataObj.Country.Id;
                formDataVm.Region = dataObj.Country.Region.Id;

                if (dataObj.State != null)
                {
                    formDataVm.State = dataObj.State.Id;
                }
                else
                {
                    formDataVm.State = 0;
                }


                ViewBag.RegionList = new SelectList(_regionSevice.LoadRegion(EntityStatus.Active), "Id", "Name", formDataVm.Region);
                ViewBag.CountryList = new SelectList(_countryService.LoadCountry(EntityStatus.Active, formDataVm.Region), "Id", "Name", formDataVm.Country);
                if (formDataVm.State > 0)
                {
                    ViewBag.StateList =
                        new SelectList(
                             _stateService.LoadState(null, formDataVm.Country, EntityStatus.Active), "Id",
                            "Name", formDataVm.State);
                }
                else
                {
                    ViewBag.StateList = new SelectList(string.Empty, "Value", "Text");
                }

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
        public ActionResult Update(Models.ViewModel.CityViewModel formData)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    var dataObj = _cityService.GetById(formData.Id);
                    if (dataObj.Id < 1)
                    {
                        return HttpNotFound();
                    }
                    var wcfVm = new CityViewModel()
                    {
                        Id = formData.Id,
                        Name = formData.Name,
                        Status = formData.Status,
                        ShortName = formData.ShortName,
                        Country = formData.Country,
                        CurrentUserProfileId = _commonHelper.GetCurrentUserProfileId(),
                        State = formData.State ?? 0,
                    };
                    _cityService.Save(wcfVm);
                    ViewBag.SuccessMessage = "City Succesfully Update";
                }
            }

            finally
            {
                ViewBag.RegionList = new SelectList(_regionSevice.LoadRegion(EntityStatus.Active), "Id",
                    "Name", formData.Region);
                ViewBag.CountryList = new SelectList(string.Empty, "Value", "Text", formData.Country);
                ViewBag.StateList = new SelectList(string.Empty, "Value", "Text");
                if (formData.Region > 0)
                {
                    ViewBag.CountryList = new SelectList(_countryService.LoadCountry(EntityStatus.Active, formData.Region), "Id", "Name", formData.Country);
                }
                if (formData.Country > 0)
                {
                    ViewBag.StateList = new SelectList(_stateService.LoadState(null, formData.Country, EntityStatus.Active), "Id", "Name", formData.State);
                }
            }
            ViewBag.StatusList = new SelectList(_commonHelper.GetStatus(), "Value", "Key", formData.Status);
            return View(formData);
        }
        #endregion

        #region Details View
        [Authorize(Roles = "Web Admin")]
        public ActionResult Details(long id)
        {
            var dataObj = new CityDto();

            try
            {
                dataObj = _cityService.GetById(id);
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
                _cityService.Delete(id);
                return Json(new Response(true, "City sucessfully deleted."));
            }
            catch (Exception e)
            {
                return Json(new Response(false, e.Message));
            }

        }

        #endregion

        #region AjaxOperation
        public JsonResult LoadCity(long region, long country, long state)
        {
            try
            {
                var citySelectList = new SelectList(_cityService.LoadCity(region, country, state, EntityStatus.Active), "Id", "Name");
                return Json(new { returnList = citySelectList, IsSuccess = true });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Problem Occurred During Load City";
                return Json(new Response(false, "Problem Occurred During Load City"));
            }
        }
        #endregion
    }
}