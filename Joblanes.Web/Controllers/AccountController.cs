using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Model.JobLanes.Dto;
using Model.JobLanes.Entity.User;
using Model.JobLanes.ViewModel;
using NHibernate;
using Owin;
using Services.Joblanes;
using Services.Joblanes.Helper;
using Web.Joblanes.Context;
using Web.Joblanes.Helper;
using Web.Joblanes.Models;

namespace Web.Joblanes.Controllers
{
    [Authorize]
    // [RedirectingAction]
    public class AccountController : Controller
    {
        readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ApplicationUserManager _userManager;
        protected ApplicationDbContext _db;
        //private IWcfUserService _wcfUserService;
        //private IWebAdminService _webAdminService;
        //private ICompanyAdminService _companyAdminService;
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
        private readonly ICompanyService _companyService;

        public AccountController()
        {
            //_wcfUserService = new WcfUserServiceClient();
            //_companyAdminService = new CompanyAdminServiceClient();
            //_webAdminService = new WebAdminServiceClient();
            _db = new ApplicationDbContext();
            ISession session = NhSessionFactory.OpenSession();
            _userService = new UserService(session);
            _emailSevice = new EmailServiceNew();
            _jobseekerService = new Services.Joblanes.JobSeekerService(session);
            _jobSeekerDetailsService = new JobSeekerDetailsService(session);
            _jobSeekerEducationalQualificationService = new JobSeekerEducationalQualificationService(session);
            _jobSeekerExperienceService = new JobSeekerExperienceService(session);
            _jobSeekerTrainingCoursesService = new JobSeekerTrainingCoursesService(session);
            _jobPostService = new JobPostService(session);
            _jobSeekerCvBankService = new JobSeekerCvBankService(session);
            _jobSeekerSkillService = new JobSeekerSkillService(session);
            _jobSeekerJobPostService = new JobSeekerJobPostService(session);
            _companyService= new CompanyService(session);
        }

        public AccountController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
            //s_db = new ApplicationDbContext();
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]

        public ActionResult Login(string returnUrl)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Profile", "Home");
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.Email, model.Password);
                if (user != null)
                {
                    var profileDto = _userService.GetByAspNetUserId(user.Id);

                    if (profileDto.IsBlock)
                    {
                        ModelState.AddModelError("",
                            "Sorry your account is temporary blocked by admin for details contact with Joblanes!");
                        return View(model);
                        //return RedirectToAction("LogOff","Account", new { message = "Sorry your account is temporary blocked by admin for details contact with Joblanes!" });
                    }
                    await SignInAsync(user, model.RememberMe);
                    var roleList = user.Roles;
                    Session["UserProfileId"] = profileDto.Id;

                    foreach (var iUserRole in roleList)
                    {
                        foreach (var source in _db.Roles.ToList())
                        {
                            if (source.Name == UserRole.WebAdmin && iUserRole.RoleId == source.Id)
                            {
                                //web admin page 
                                return RedirectToActionPermanent("DashBoard", "Admin", new { area = "" });
                            }
                            if (source.Name == UserRole.JobSeeker && iUserRole.RoleId == source.Id)
                            {
                                //job seeker page
                                return RedirectToActionPermanent("Profile", "Home");
                            }

                            if (source.Name == UserRole.Company && iUserRole.RoleId == source.Id)
                            {
                                //Employeer page
                                return RedirectToActionPermanent("EditProfile", "Company");
                            }
                        }
                    }
                    //var profileData = new UserProfileSessionData
                    //{
                    //    UserId = profileDto.Id,
                    //    EmailAddress = user.Email,
                    //    FullName = user.UserName
                    //};



                    return RedirectToAction("Profile", "Home");
                }
                ModelState.AddModelError("", "Invalid username or password.");
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register(string message = "", int type = 0)
        {
            switch (type)
            {
                case 1:
                    ViewBag.SuccessMessage = message;
                    ViewBag.loginUrl = "viewLoginLink";
                    break;
                case 2:
                    ViewBag.ErrorMessage = message;
                    break;
            }
            IList<ApplicationRole> roles = _db.Roles.ToList<ApplicationRole>().Where(x => x.Name != "Web Admin").ToList();
            ViewBag.roleList = new SelectList(roles, "Name", "Name");
            return View();
        }

        public ActionResult RegistrationView(string message = "", int type = 0)
        {
            switch (type)
            {
                case 1:
                    ViewBag.SuccessMessage = message;
                    break;
                case 2:
                    ViewBag.ErrorMessage = message;
                    break;
            }
            return View();
        }
        private UserProfile CastToUserProfile(UserProfileVm userProfileVm)
        {
            if (userProfileVm != null)
            {
                var userProfile = new UserProfile()
                {
                    Name = userProfileVm.NickName,
                    AspNetUserId = userProfileVm.AspNetUserId,
                    IsBlock = userProfileVm.IsBlock
                };
                return userProfile;
            }
            return new UserProfile();
        }
        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            List<string> errorMess = new List<string>();
            string errM = "";
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };
                    IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        //await SignInAsync(user, isPersistent: false);
                        // assign role to user 
                        var assignedRole = await UserManager.AddToRoleAsync(user.Id, model.UserRole);
                        if (assignedRole.Succeeded)
                        {
                            var profileVm = new UserProfileVm()
                            {
                                NickName = model.Name,
                                AspNetUserId = user.Id,
                                IsBlock = false
                            };
                            if (model.UserRole == "Employers")
                            {
                                var companyProfile = new CompanyViewModel();
                                companyProfile.ContactEmail = model.Email;
                                companyProfile.UserProfileId = user.Id;
                                var profileModel = CastToUserProfile(profileVm);
                                _userService.Save(profileModel);
                                _companyService.Save(companyProfile);
                            }
                            else if (model.UserRole == "Job seekers")
                            {
                                var jobSeeker = new JobSeekerProfileVm();
                                jobSeeker.ContactEmail = model.Email;
                                jobSeeker.UserProfileId = user.Id;
                                jobSeeker.Dob = DateTime.Now;
                                var profileModel = CastToUserProfile(profileVm);
                                _userService.Save(profileModel);
                                _jobseekerService.Save(jobSeeker);
                            }

                        }
                        //string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                        //_wcfUserService.SendRegisterConfirmationEmail(model.Email, model.Name, code);
                        return RedirectToAction("Register", new { message = "Registered Successfully ", type = 1 });

                    }
                    AddErrors(result);
                    errorMess = result.Errors.ToList();
                    errM = errorMess.Aggregate("", (current, err) => current + (err + "\n"));
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);

            }
            // If we got this far, something failed, redisplay form
            return RedirectToAction("Register", new { message = errM, type = 2 });
        }


        [Authorize(Roles = "Web Admin")]
        public ActionResult CreateWebAdmin(string message = "", int type = 0)
        {
            switch (type)
            {
                case 1:
                    ViewBag.SuccessMessage = message;
                    break;
                case 2:
                    ViewBag.ErrorMessage = message;
                    break;
            }
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [Authorize(Roles = "Web Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateWebAdmin(RegisterViewModel model)
        {
            List<string> errorMess = new List<string>();
            string errM = "";
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };
                    IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        // await SignInAsync(user, isPersistent: false);
                        // assign role to user 
                        var assignedRole = await UserManager.AddToRoleAsync(user.Id, model.UserRole);
                        if (assignedRole.Succeeded)
                        {
                            var profileVm = new UserProfileVm()
                            {
                                NickName = model.Name,
                                AspNetUserId = user.Id,
                                IsBlock = false
                            };
                            var profileModel = CastToUserProfile(profileVm);
                            _userService.Save(profileModel);
                        }
                        return RedirectToActionPermanent("CreateWebAdmin", "Account", new { message = "Admin user create successfully", type = 1 });

                    }
                    AddErrors(result);
                    errorMess = result.Errors.ToList();
                    errM = errorMess.Aggregate("", (current, err) => current + (err + "\n"));
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);

            }
            // If we got this far, something failed, redisplay form
            return RedirectToAction("CreateWebAdmin", new { message = errM, type = 2 });
        }



        [Authorize(Roles = "Web Admin")]
        public ActionResult CreateCompany(string message = "", int type = 0)
        {
            switch (type)
            {
                case 1:
                    ViewBag.SuccessMessage = message;
                    break;
                case 2:
                    ViewBag.ErrorMessage = message;
                    break;
            }
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [Authorize(Roles = "Web Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateCompany(RegisterViewModel model)
        {
            List<string> errorMess = new List<string>();
            string errM = "";
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };
                    IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        // await SignInAsync(user, isPersistent: false);
                        // assign role to user 
                        var assignedRole = await UserManager.AddToRoleAsync(user.Id, model.UserRole);
                        var company = new CompanyDto();
                        if (assignedRole.Succeeded)
                        {
                            var profileVm = new UserProfileVm()
                            {
                                NickName = model.Name,
                                AspNetUserId = user.Id,
                                IsBlock = false
                            };
                            var companyProfile = new CompanyViewModel();
                            companyProfile.ContactEmail = model.Email;
                            companyProfile.UserProfileId = user.Id;
                            var profileModel = CastToUserProfile(profileVm);
                            _userService.Save(profileModel);
                            _companyService.Save(companyProfile);
                            company = _companyService.GetCompany(companyProfile.UserProfileId);
                        }
                        return RedirectToActionPermanent("CompanyEdit", "Admin", new { id = company.Id });

                    }
                    AddErrors(result);
                    errorMess = result.Errors.ToList();
                    errM = errorMess.Aggregate("", (current, err) => current + (err + "\n"));
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);

            }
            // If we got this far, something failed, redisplay form
            return RedirectToAction("CreateCompany", new { message = errM, type = 2 });
        }


        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }

            IdentityResult result = await UserManager.ConfirmEmailAsync(userId, code);
            if (result.Succeeded)
            {
                return View("ConfirmEmail");
            }
            else
            {
                AddErrors(result);
                return View();
            }
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    ModelState.AddModelError("", "The user either does not exist or is not confirmed.");
                    return View();
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            if (code == null)
            {
                return View("Error");
            }
            return View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "No user found.");
                    return View();
                }
                IdentityResult result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("ResetPasswordConfirmation", "Account");
                }
                else
                {
                    AddErrors(result);
                    return View();
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                await SignInAsync(user, isPersistent: false);
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                        await SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };
                IdentityResult result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);

                        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                        // Send an email with this link
                        // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                        // SendEmail(user.Email, callbackUrl, "Confirm your account", "Please confirm your account by clicking this link");

                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        [HttpGet]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login", "Account");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private void SendEmail(string email, string callbackUrl, string subject, string message)
        {
            // For information on sending mail, please visit http://go.microsoft.com/fwlink/?LinkID=320771
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}