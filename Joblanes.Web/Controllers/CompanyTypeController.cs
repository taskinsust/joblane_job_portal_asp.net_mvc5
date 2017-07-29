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
using Web.Joblanes.Models.Dto;

namespace Web.Joblanes.Controllers
{
     [Authorize]
     [RedirectingAction]
    public class CompanyTypeController : Controller
    {
        #region Logger
        readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Objects/Properties/Services/Dao & Initialization
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
        private readonly ICommonHelper _commonHelper;

        public CompanyTypeController()
        {
            //_webAdminService = new WebAdminServiceClient();
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
            _commonHelper = new CommonHelper();
        }
        #endregion

        #region Manage Company Type
        [Authorize(Roles = "Web Admin")]
        public ActionResult Index()
        {
            ViewBag.PageSize = Contants.PageSize;
            ViewBag.CurrentPage = 1;
            return View();
        }

        [Authorize(Roles = "Web Admin")]
        public JsonResult CompanyTypeList(int draw, int start, int length, string name, string status) 
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
                                orderBy = "Name";
                                break;
                            default:
                                orderBy = "";
                                break;
                        }
                    }
                    if (string.IsNullOrEmpty(status))
                        status = "0";
                    int recordsTotal = _companyTypeService.CompanyTypeRowCount(name, Convert.ToInt32(status));
                    long recordsFiltered = recordsTotal;

                   // List<CompanyTypeListDto> dataList = _webAdminService.LoadCompanyType(start, length, orderBy, orderDir, name, Convert.ToInt32(status)).ToList();
                    var dataList = _companyTypeService.LoadCompanyType(start, length, orderBy, orderDir, name, Convert.ToInt32(status)).ToList();

                    var data = new List<object>();
                    int sl = start + 1;
                    foreach (var d in dataList)
                    {
                        var str = new List<string>();
                        str.Add(sl.ToString());
                        str.Add(d.Name);
                        str.Add(d.StatusText);

                        str.Add("<a href='" + Url.Action("Details", "CompanyType") + "?id=" + d.Id.ToString() +
                            "' class='glyphicon glyphicon-th-list'> </a>&nbsp;&nbsp;<a href='" + Url.Action("Update", "CompanyType") + "?id=" + d.Id.ToString() +
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
            return View();
        }

           [Authorize(Roles = "Web Admin")]
        [HttpPost]
        public ActionResult Create(Models.ViewModel.CompanyTypeViewModel data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var wcfVm = new CompanyTypeViewModel()
                    {
                        Id = data.Id,
                        Name = data.Name,
                        Description = data.Description,
                        Status = data.Status,
                        CurrentUserProfileId = _commonHelper.GetCurrentUserProfileId()
                    };

                    _companyTypeService.Save(wcfVm);

                    data = new Models.ViewModel.CompanyTypeViewModel();
                    ViewBag.SuccessMessage = "Comnpany Type Succesfully Saved";
                }
            }

            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Company Type Save Fail.";
                
            }           

            return View(data);
        }
        #endregion

        #region Update Operation
           [Authorize(Roles = "Web Admin")]
        public ActionResult Update(long id)
        {

            var formDataVm = new Models.ViewModel.CompanyTypeViewModel();
            try
            {
                var dataObj = _companyTypeService.GetById(id);
                if (dataObj.Id<1)
                {
                    return HttpNotFound();
                }

                formDataVm.Id = dataObj.Id;
                formDataVm.Name = dataObj.Name;
                formDataVm.Description = dataObj.Description;
                formDataVm.Status = dataObj.Status;

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
        public ActionResult Update(Models.ViewModel.CompanyTypeViewModel formData)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    var dataObj = _companyTypeService.GetById(formData.Id);
                    if (dataObj.Id<1)
                    {
                        return HttpNotFound();
                    }
                    var wcfVm = new CompanyTypeViewModel()
                    {
                        Id = formData.Id,
                        Name = formData.Name,
                        Description = formData.Description,
                        Status = formData.Status,
                        CurrentUserProfileId = _commonHelper.GetCurrentUserProfileId()
                    };

                    _companyTypeService.Save(wcfVm);
                    ViewBag.SuccessMessage = "Company Type Succesfully Update";
                }
            }

            catch (Exception ex)
            {
               // _logger.Error(ex);
                ViewBag.ErrorMessage = "Company Type Update failed.";
            }
            ViewBag.StatusList = new SelectList(_commonHelper.GetStatus(), "Value", "Key", formData.Status);
            return View(formData);
        }
        #endregion

        #region Details View
           [Authorize(Roles = "Web Admin")]
        public ActionResult Details(long id)
        {
            var dataObj = new Model.JobLanes.Dto.CompanyTypeDto();

            try
            {
                dataObj = _companyTypeService.GetById(id);
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
                _companyTypeService.Delete(id);
                return Json(new Response(true, "Company Type sucessfully deleted."));
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                
            } 
            return Json(new Response(false, errorMessage));
        }

        #endregion
	}
}