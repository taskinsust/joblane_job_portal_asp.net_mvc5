using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using Model.JobLanes.Dto;
using NHibernate;
using Services.Joblanes;
using Services.Joblanes.Helper;

namespace Web.Joblanes.Helper
{
    public class RedirectingAction : ActionFilterAttribute
    {
        //private IWebAdminService _webAdminService;
        private IUserService _userService;
        public RedirectingAction()
        {
            ISession session = NhSessionFactory.OpenSession();
            _userService = new UserService(session);
          //  _webAdminService = new WebAdminServiceClient();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            if (String.IsNullOrEmpty(userId)) return;
            UserProfileDto profileDto = _userService.GetByAspNetUserId(userId);
            if (profileDto.IsBlock)
            {
                //filterContext.Result = new EmptyResult();

                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Account",
                    action = "LogOff"
                }));

                //filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);

            }
            base.OnActionExecuting(filterContext);
        }
    }

   }