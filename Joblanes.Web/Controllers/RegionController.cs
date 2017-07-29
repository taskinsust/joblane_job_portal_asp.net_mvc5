using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.Mvc;
using Model.JobLanes.Dto;
using Model.JobLanes.ViewModel;
using Services.Joblanes;
using Services.Joblanes.Helper;
using Web.Joblanes.Helper;
using Web.Joblanes.Models;
using ISession = NHibernate.ISession;

namespace Web.Joblanes.Controllers
{
    [Authorize]
    [RedirectingAction]
    public class RegionController : Controller
    {

        #region Logger
        readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Objects/Properties/Services/Dao & Initialization
        //private IWebAdminService _webAdminService;
        private readonly ICommonHelper _commonHelper;
        private readonly IRegionService _regionService;
        public RegionController()
        {
            ISession session = NhSessionFactory.OpenSession();
            _regionService = new RegionService(session);
            //_webAdminService = new WebAdminServiceClient();
            _commonHelper = new CommonHelper();
        }
        #endregion

        #region Manage Region
        [Authorize(Roles = "Web Admin")]
        public ActionResult Index()
        {
            ViewBag.PageSize = Contants.PageSize;
            ViewBag.CurrentPage = 1;
            return View();
        }

        [Authorize(Roles = "Web Admin")]
        public JsonResult RegionList(int draw, int start, int length, string name, string status)
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
                    int recordsTotal = _regionService.RegionRowCount(name, Convert.ToInt32(status));
                    long recordsFiltered = recordsTotal;

                    var dataList = _regionService.LoadRegion(start, length, orderBy, orderDir, name, Convert.ToInt32(status)).ToList();

                    var data = new List<object>();
                    int sl = start + 1;
                    foreach (var d in dataList)
                    {
                        var str = new List<string>();
                        str.Add(sl.ToString());
                        str.Add(d.Name);
                        str.Add(d.StatusText);


                        str.Add("<a href='" + Url.Action("Details", "Region") + "?id=" + d.Id.ToString() +
                            "' class='glyphicon glyphicon-th-list'> </a>&nbsp;&nbsp;<a href='" + Url.Action("Update", "Region") + "?id=" + d.Id.ToString() +
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
        public ActionResult Create(Models.ViewModel.RegionViewModel data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var wcfVm = new RegionViewModel()
                    {
                        Id = data.Id,
                        Name = data.Name,
                        Status = data.Status,
                        CurrentUserProfileId = _commonHelper.GetCurrentUserProfileId()
                    };
                    _regionService.Save(wcfVm);
                    data = new Models.ViewModel.RegionViewModel() { Name = "" };
                    ViewBag.SuccessMessage = "Region Succesfully Saved";
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Region Save Fail.";

            }
            return View(data);
        }
        #endregion

        #region Update Operation
        [Authorize(Roles = "Web Admin")]
        public ActionResult Update(long id)
        {

            var formDataVm = new Models.ViewModel.RegionViewModel();
            try
            {
                var dataObj = _regionService.GetById(id);
                if (dataObj.Id < 1)
                {
                    return HttpNotFound();
                }

                formDataVm.Id = dataObj.Id;
                formDataVm.Name = dataObj.Name;
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
        public ActionResult Update(Models.ViewModel.RegionViewModel formData)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    var dataObj = _regionService.GetById(formData.Id);
                    if (dataObj.Id < 1)
                    {
                        return HttpNotFound();
                    }
                    var wcfVm = new RegionViewModel()
                    {
                        Id = formData.Id,
                        Name = formData.Name,
                        Status = formData.Status,
                        CurrentUserProfileId = _commonHelper.GetCurrentUserProfileId()
                    };
                    _regionService.Save(wcfVm);
                    ViewBag.SuccessMessage = "Region Succesfully Update";
                }
            }

            catch (Exception ex)
            {
                //_logger.Error(ex);
                ViewBag.ErrorMessage = "Region Update failed.";
            }
            ViewBag.StatusList = new SelectList(_commonHelper.GetStatus(), "Value", "Key", formData.Status);
            return View(formData);
        }
        #endregion

        #region Details View
        [Authorize(Roles = "Web Admin")]
        public ActionResult Details(long id)
        {
            var dataObj = new RegionDto();

            try
            {
                dataObj = _regionService.GetById(id);
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
                _regionService.Delete(id);
                return Json(new Response(true, "Region sucessfully deleted."));
            }
            catch (Exception ex)
            {
                errorMessage = "Something went wrong";

            }

            return Json(new Response(false, errorMessage));
        }

        #endregion
    }
}