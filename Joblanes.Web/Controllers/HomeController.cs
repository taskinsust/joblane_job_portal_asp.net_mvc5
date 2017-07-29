using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Joblanes.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Profile()
        {
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if (System.Web.HttpContext.Current.User.IsInRole("Job seekers"))
                {
                    return RedirectPermanent("~/JobSeeker/JobSeekerProfile");
                }
            }
            return View();
        }


        public ActionResult Index()
        {
            return View();
        }



        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }


        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        private void IsValidMobile(string s)
        {
            throw new NotImplementedException();
        }
    }
}