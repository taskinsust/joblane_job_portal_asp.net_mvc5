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
    public class StateController : Controller
    {
        #region Logger
        readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Objects/Properties/Services/Dao & Initialization
        
        private readonly ICommonHelper _commonHelper;
        private readonly IStateService _stateService;
        private readonly IRegionService _regionService;
        private readonly ICountryService _countryService;

        public StateController()
        {
            ISession session = NhSessionFactory.OpenSession();
            _stateService = new StateService(session);
            _regionService = new RegionService(session);
            _commonHelper = new CommonHelper();
            _countryService = new CountryService(session);
        }
        #endregion

        #region Manage State
        [Authorize(Roles = "Web Admin")]
        public ActionResult Index()
        {
            List<RegionDto> regionList = new List<RegionDto>();
            List<CountryDto> countryList = new List<CountryDto>(); 
            try
            {
                regionList = _regionService.LoadRegion(EntityStatus.Active).ToList();
               
            }
            catch (Exception ex)
            {
                throw;
            }
            ViewBag.PageSize = Contants.PageSize;
            ViewBag.RegionList = regionList;
            ViewBag.CountryList = countryList;
            ViewBag.CurrentPage = 1;
            return View();
        }

        [Authorize(Roles = "Web Admin")]
        public JsonResult StateList(int draw, int start, int length, string name, string shortName, long region, long country, string status) 
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
                                orderBy = "Name";
                                break;
                            case "4":
                                orderBy = "ShortName";
                                break;
                            default:
                                orderBy = "";
                                break;
                        }
                    }
                    if (string.IsNullOrEmpty(status))
                        status = "0";
                    int recordsTotal = _stateService.StateRowCount(name,   shortName,  region,  country,Convert.ToInt32(status));
                    long recordsFiltered = recordsTotal;

                   // List<StateListDto> dataList = _webAdminService.LoadState(start, length, orderBy, orderDir, name, Convert.ToInt32(status)).ToList();
                    var dataList = _stateService.LoadState(start, length, orderBy, orderDir, name,  shortName,  region,  country, Convert.ToInt32(status)).ToList();

                    var data = new List<object>();
                    int sl = start + 1;
                    foreach (var d in dataList)
                    {
                        var str = new List<string>();
                        str.Add(sl.ToString());
                        str.Add(d.Country.Region.Name);
                        str.Add(d.Country.Name);
                        str.Add(d.Name);
                        str.Add(d.ShortName);
                        str.Add(d.StatusText);

                        str.Add("<a href='" + Url.Action("Details", "State") + "?id=" + d.Id.ToString() +
                            "' class='glyphicon glyphicon-th-list'> </a>&nbsp;&nbsp;<a href='" + Url.Action("Update", "State") + "?id=" + d.Id.ToString() +
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
            Models.ViewModel.StateViewModel data = new Web.Joblanes.Models.ViewModel.StateViewModel();
            try
            {
                ViewBag.RegionList = new SelectList(_regionService.LoadRegion(EntityStatus.Active), "Id", "Name");
                ViewBag.CountryList = new SelectList(string.Empty, "Value", "Text"); 
            }
            catch (Exception ex)
            {
                throw;
            }
            return View(data);
        }

          [Authorize(Roles = "Web Admin")]
        [HttpPost]
        public ActionResult Create(Models.ViewModel.StateViewModel data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var wcfVm = new StateViewModel()
                    {
                        Id = data.Id,
                        Name = data.Name,
                        Status = data.Status,
                        ShortName = data.ShortName,
                        Country = data.Country,
                        CurrentUserProfileId = _commonHelper.GetCurrentUserProfileId()
                    };
                    _stateService.Save(wcfVm);

                    data = new Models.ViewModel.StateViewModel();
                    ViewBag.SuccessMessage = "State Succesfully Saved";
                }
            }
            catch (Exception ex)
            {
                    ViewBag.ErrorMessage = "State Save Fail.";
                
            }
            finally
            {
                ViewBag.RegionList = new SelectList(_regionService.LoadRegion(EntityStatus.Active), "Id", "Name",data.Region);
                ViewBag.CountryList = new SelectList(string.Empty, "Value", "Text", data.Country);
                if (data.Region>0)
                {
                    ViewBag.CountryList = new SelectList(_countryService.LoadCountry(EntityStatus.Active, data.Region), "Id", "Name", data.Country);
                }
               
            }

            return View(data);
        }
        #endregion

        #region Update Operation
          [Authorize(Roles = "Web Admin")]
        public ActionResult Update(long id)
        {
              var formDataVm = new Models.ViewModel.StateViewModel();
            try
            {
                var dataObj = _stateService.GetById(id);
                if (dataObj.Id<1)
                {
                    return HttpNotFound();
                }

                formDataVm.Id = dataObj.Id;
                formDataVm.Name = dataObj.Name;
                formDataVm.ShortName = dataObj.ShortName;
                formDataVm.Country = dataObj.Country.Id;
                formDataVm.Region = dataObj.Country.Region.Id; 
                formDataVm.Status = dataObj.Status;
                ViewBag.RegionList = new SelectList(_regionService.LoadRegion(EntityStatus.Active), "Id", "Name", formDataVm.Region);
                ViewBag.CountryList = new SelectList(_countryService.LoadCountry(EntityStatus.Active,formDataVm.Region), "Id", "Name", formDataVm.Country);
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
        public ActionResult Update(Models.ViewModel.StateViewModel formData)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    var dataObj = _stateService.GetById(formData.Id);
                    if (dataObj.Id < 1)
                    {
                        return HttpNotFound();
                    }
                    var wcfVm = new StateViewModel()
                    {
                        Id = formData.Id,
                        Name = formData.Name,
                        Status = formData.Status,
                        ShortName = formData.ShortName,
                        Country = formData.Country,
                        CurrentUserProfileId = _commonHelper.GetCurrentUserProfileId()
                    };
                    _stateService.Save(wcfVm);
                    ViewBag.SuccessMessage = "State Succesfully Update";
                }
            }

            catch (Exception ex)
            {
                    ViewBag.ErrorMessage = "State Update Fail.";
                
            }
            finally
            {
                ViewBag.RegionList = new SelectList(_regionService.LoadRegion(EntityStatus.Active), "Id",
                    "Name", formData.Region);
                ViewBag.CountryList = new SelectList(string.Empty, "Value", "Text", formData.Country);
                if (formData.Region > 0)
                {
                    ViewBag.CountryList = new SelectList(_countryService.LoadCountry(EntityStatus.Active, formData.Region), "Id", "Name", formData.Country);
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
            var dataObj = new StateDto();

            try
            {
                dataObj = _stateService.GetById(id);
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
                _stateService.Delete(id);
                return Json(new Response(true, "State sucessfully deleted."));
            }
            catch (Exception ex)
            {
                    errorMessage = "Something went wrong";
                
            } 
            return Json(new Response(false, errorMessage));
        }

        #endregion

        #region AjaxOperation
        public JsonResult LoadState(long region,long country) 
        {
            try
            {
                var stateSelectList = new SelectList(_stateService.LoadState( region,country, EntityStatus.Active), "Id", "Name");
                return Json(new { returnList = stateSelectList, IsSuccess = true });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Problem Occurred During Load State";
                return Json(new Response(false, "Problem Occurred During Load State"));
            }
        } 
        #endregion
    }
}