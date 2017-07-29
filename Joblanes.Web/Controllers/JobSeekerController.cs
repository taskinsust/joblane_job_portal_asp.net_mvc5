using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Model.JobLanes.Dto;
using Model.JobLanes.Entity;
using NHibernate;
using Services.Joblanes;
using Services.Joblanes.Helper;
using Web.Joblanes.Helper;
using Web.Joblanes.Models;
using Web.Joblanes.Models.Dto;
using Web.Joblanes.Models.ViewModel;
using Constants = Web.Joblanes.Context.Constants;
using ImageHelper = Web.Joblanes.Helper.ImageHelper;
using JobSeekerEduVm = Web.Joblanes.Models.ViewModel.JobSeekerEduVm;
using JobSeekerExpVm = Web.Joblanes.Models.ViewModel.JobSeekerExpVm;
using JobSeekerProfileVm = Web.Joblanes.Models.ViewModel.JobSeekerProfileVm;


namespace Web.Joblanes.Controllers
{
    [Authorize]
    [RedirectingAction]
    public class JobSeekerController : Controller
    {

        #region Logger
        readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Objects/Properties/Services/Dao & Initialization

        //private readonly IWcfUserService _wcfUserService;
        //private readonly IWebAdminService _webAdminService;
        //private ICompanyAdminService _companyAdminService;
        private readonly CommonHelper _commonHelper;
        private readonly string _aspnetUserId;
        private readonly IUserService _userService;
        private readonly IJobseekerService _jobseekerService;
        private readonly IJobSeekerDetailsService _jobSeekerDetailsService;
        private readonly IJobSeekerEducationalQualificationService _jobSeekerEducationalQualificationService;
        private readonly IJobSeekerExperienceService _jobSeekerExperienceService;
        private readonly IJobSeekerTrainingCoursesService _jobSeekerTrainingCoursesService;
        private readonly IJobPostService _jobPostService;
        private readonly IJobSeekerJobPostService _jobSeekerJobPostService;
        private readonly IJobSeekerCvBankService _jobSeekerCvBankService;
        private readonly IJobSeekerSkillService _jobSeekerSkillService;
        private readonly IRegionService _regionSevice;
        private readonly ICountryService _countryService;
        private readonly IStateService _stateService;
        private readonly ICityService _cityService;
        private readonly IOrganizationTypeService _organizationTypeService;
        private readonly IJobCategoryService _jobCategoryService;
        private readonly ICompanyTypeService _companyTypeService;
        private readonly ICompanyService _companyService;
        public JobSeekerController()
        {
            _aspnetUserId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
            //_wcfUserService = new WcfUserServiceClient();
            //_webAdminService = new WebAdminServiceClient();
            //_companyAdminService = new CompanyAdminServiceClient();
            _commonHelper = new CommonHelper();
            ISession session = NhSessionFactory.OpenSession();
            _userService = new UserService(session);
            
            _jobseekerService = new JobSeekerService(session);
            _jobSeekerDetailsService = new JobSeekerDetailsService(session);
            _jobSeekerEducationalQualificationService = new JobSeekerEducationalQualificationService(session);
            _jobSeekerExperienceService = new JobSeekerExperienceService(session);
            _jobSeekerTrainingCoursesService = new JobSeekerTrainingCoursesService(session);
            _jobPostService = new JobPostService(session);
            _jobSeekerCvBankService = new JobSeekerCvBankService(session);
            _jobSeekerSkillService = new JobSeekerSkillService(session);
            _jobSeekerJobPostService = new JobSeekerJobPostService(session);
            _regionSevice = new RegionService(session);
            _countryService = new CountryService(session);
            _stateService = new StateService(session);
            _cityService = new CityService(session);
            _organizationTypeService = new OrganizationTypeService(session);
            _jobCategoryService = new JobCategoryService(session);
            _companyTypeService = new CompanyTypeService(session);
            _companyService = new CompanyService(session);
        }
        #endregion

        #region Save Methods
        //post methods

        [HttpGet]
        [Authorize(Roles = "Job seekers")]
        public ActionResult BuildResume(int type = 0, string message = "")
        {
            try
            {
                Initialize();
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
            return View("");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Job seekers")]
        public ActionResult BuildResume(JobSeekerProfileVm profileVm)
        {
            try
            {
                var profileImageUpload = profileVm.ProfileImage;
                //var cvFileUpload = profileVm.Cv;
                var profile = new Model.JobLanes.ViewModel.JobSeekerProfileVm();
                profile.FirstName = profileVm.FirstName;
                profile.LastName = profileVm.LastName;

                /*if any error occured comment this code */
                profile.Linkedin = profileVm.Linkedin;
                profile.MaritalStatus = profileVm.MaritalStatus;
                profile.FatherName = profileVm.FatherName;
                profile.MotherName = profileVm.MotherName;
                profile.Weblink = profileVm.Weblink;
                profile.ZipCode = profileVm.ZipCode;
                profile.Address = profileVm.Address;
                profile.Dob = profileVm.Dob;
                profile.Expertise = profileVm.Expertise;
                profile.Gender = profileVm.Gender;
                /*end */
                profile.RegionId = profileVm.RegionId;
                profile.StateId = profileVm.StateId;
                profile.CityId = profileVm.CityId;
                profile.CountryId = profileVm.CountryId;

                profile.ContactEmail = profileVm.ContactEmail;
                profile.ContactNumber = profileVm.ContactNumber;
                profile.IsPublicResume = profileVm.IsPublicResume;

                if (profileImageUpload != null)
                    profile.ProfileImageBytes = ImageHelper.ConvertFileInByteArray(profileImageUpload.InputStream,
                        profileImageUpload.ContentLength);
                profile.UserProfileId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                bool issuccess = _jobseekerService.Save(profile);
                if (issuccess)
                    return Json(new Response(true, "Saved Successfully"));
                return Json(new Response(false, "Operation Failed"));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return Json(new Response(false, ex.Message));
            }
        }

        [HttpGet]
        [Authorize(Roles = "Job seekers")]
        public ActionResult AddProfileInfo()
        {
            try
            {
                // var aspnetUserId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                var jobSeekerProfileDto = _jobseekerService.GetJobSeekerProfileData(_aspnetUserId);
                JobSeekerProfileVm destObject = new JobSeekerProfileVm();
                if (jobSeekerProfileDto != null)
                {
                    CopyClass.CopyObject<JobSeekerProfileVm>(jobSeekerProfileDto, ref destObject);
                }
                if (jobSeekerProfileDto != null)
                {
                    ViewBag.GenderList = new SelectList(_commonHelper.LoadEmumToDictionary<Constants.Gender>(), "Key", "Value", jobSeekerProfileDto.Gender).ToList();
                    ViewBag.MaritalList = new SelectList(_commonHelper.LoadEmumToDictionary<Constants.MaritalType>(), "Key", "Value", jobSeekerProfileDto.MaritalStatus).ToList();
                    ViewBag.RegionList = new SelectList(_regionSevice.LoadRegion(0, int.MaxValue, "", "", "", 1), "Id", "Name", jobSeekerProfileDto.RegionId);
                    if (!String.IsNullOrEmpty(destObject.ContactNumber))
                    {
                        ViewBag.CountryList = new SelectList(_countryService.LoadCountry(0, int.MaxValue, "", "", "", "", "", destObject.RegionId, 1), "Id", "Name", jobSeekerProfileDto.CountryId);
                        ViewBag.StateList = new SelectList(_stateService.LoadState(0, int.MaxValue, "", "", "", "", destObject.RegionId, destObject.CountryId, 1), "Id", "Name", jobSeekerProfileDto.StateId);
                        ViewBag.CityList = new SelectList(_cityService.LoadCity(0, int.MaxValue, "", "", "", "", destObject.RegionId, destObject.CountryId, destObject.StateId, 1), "Id", "Name", jobSeekerProfileDto.CityId);
                    }
                    else
                    {
                        ViewBag.CountryList = new SelectList(String.Empty, "Value", "Text");
                        ViewBag.StateList = new SelectList(String.Empty, "Value", "Text");
                        ViewBag.CityList = new SelectList(String.Empty, "Value", "Text");
                    }
                }
                else
                {
                    Initialize();
                }
                destObject.ContactEmail = System.Web.HttpContext.Current.User.Identity.Name;
                return PartialView("Partial/_profileInfo", destObject);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Authorize(Roles = "Job seekers")]
        public ActionResult AddEducationalQualificationNew()
        {
            try
            {
                return PartialView("Partial/_educationalQua");
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpPost]
        [Authorize(Roles = "Job seekers")]
        //[ValidateAntiForgeryToken]GetExpData
        public ActionResult AddEducationalQualification(JobSeekerEduVm profile)
        {
            try
            {
                var jobSeekerEdu = new JobSeekerEduVm();
                jobSeekerEdu.Id = profile.Id;
                jobSeekerEdu.Institute = profile.Institute;
                jobSeekerEdu.Degree = profile.Degree;
                jobSeekerEdu.StartingYear = profile.StartingYear;
                jobSeekerEdu.PassingYear = profile.PassingYear;
                jobSeekerEdu.Result = profile.Result;
                jobSeekerEdu.FieldOfStudy = profile.FieldOfStudy;
                jobSeekerEdu.AspNetuserId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
               
                var destObject = new Model.JobLanes.ViewModel.JobSeekerEduVm();
                CopyClass.CopyObject<Model.JobLanes.ViewModel.JobSeekerEduVm>(jobSeekerEdu, ref destObject);

                _jobSeekerEducationalQualificationService.Save(destObject);
                if (Request.IsAjaxRequest())
                {
                    return Json(new Response(true, "Educational Qualification Updated Successfully"));
                }
                ViewBag.SuccessMessage = "Educational Qualification Added Successfully";
                return View();
            }
          catch (Exception ex)
            {
                if (Request.IsAjaxRequest())
                {
                    return Json(new Response(false, ex.Message));
                }
                ViewBag.ErrorMEssage = ex.Message;
                return View();
            }
        }

        [HttpPost]
        [Authorize(Roles = "Job seekers")]
        public ActionResult AddExperienceNew()
        {
            try
            {
                return PartialView("Partial/_AddExperience");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Authorize(Roles = "Job seekers")]
        //[ValidateAntiForgeryToken]
        public ActionResult AddExperience(JobSeekerExpVm jobSeekerExpVm)
        {
            try
            {
                var jobSeekerExp = new JobSeekerExpVm();
                jobSeekerExp.Id = jobSeekerExpVm.Id;
                jobSeekerExp.Designation = jobSeekerExpVm.Designation;
                jobSeekerExp.CompanyName = jobSeekerExpVm.CompanyName;
                jobSeekerExp.CompanyAddress = jobSeekerExpVm.CompanyAddress;
                jobSeekerExp.DateFrom = jobSeekerExpVm.DateFrom;
                jobSeekerExp.DateTo = jobSeekerExpVm.DateTo;
                jobSeekerExp.IsCurrent = jobSeekerExpVm.IsCurrent;
                jobSeekerExp.Responsibility = jobSeekerExpVm.Responsibility;
                jobSeekerExp.AspNetuserId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                var destObject = new Model.JobLanes.ViewModel.JobSeekerExpVm();
                CopyClass.CopyObject<Model.JobLanes.ViewModel.JobSeekerExpVm>(jobSeekerExp, ref destObject);

                _jobSeekerExperienceService.Save(destObject);
                if (Request.IsAjaxRequest())
                {
                    return Json(new Response(true, "Experience Updated Successfully"));
                }
                ViewBag.SuccessMessage = "Experience Added Successfully";
                return View();
            }
            
            catch (Exception ex)
            {
                if (Request.IsAjaxRequest())
                {
                    return Json(new Response(false, ex.Message));
                }
                ViewBag.ErrorMEssage = ex.Message;
                return View();
            }
        }

        [HttpGet]
        [Authorize(Roles = "Job seekers")]
        public ActionResult AddTraining()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Authorize(Roles = "Job seekers")]
        // [ValidateAntiForgeryToken]
        public ActionResult AddTraining(JobSeekerTrainingVm jobSeekerTrainingVm)
        {
            try
            {
                var jobSeekerTrain = new JobSeekerTrainingVm();
                jobSeekerTrain.Id = jobSeekerTrainingVm.Id;
                jobSeekerTrain.Institute = jobSeekerTrainingVm.Institute;
                jobSeekerTrain.Title = jobSeekerTrainingVm.Title;
                jobSeekerTrain.StartDate = jobSeekerTrainingVm.StartDate;
                jobSeekerTrain.CloseDate = jobSeekerTrainingVm.CloseDate;
                jobSeekerTrain.Description = jobSeekerTrainingVm.Description;
                jobSeekerTrain.AspNetuserId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();

                var destObject = new Model.JobLanes.ViewModel.JobSeekerTrainingVm();
                CopyClass.CopyObject<Model.JobLanes.ViewModel.JobSeekerTrainingVm>(jobSeekerTrain, ref destObject);

                _jobSeekerTrainingCoursesService.Save(destObject);
                if (Request.IsAjaxRequest())
                {
                    return Json(new Response(true, "Training Updated Successfully"));
                }
                ViewBag.SuccessMessage = "Training Added Successfully";
                return View();
            }
          
            catch (Exception ex)
            {
                if (Request.IsAjaxRequest())
                {
                    return Json(new Response(false, ex.Message));
                }
                ViewBag.ErrorMEssage = ex.Message;
                return View();
            }
        }

        [HttpGet]
        [Authorize(Roles = "Job seekers")]
        public ActionResult AddPhotos()
        {
            try
            {
                return PartialView("Partial/_addPhotos");
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        [HttpPost]
        [Authorize(Roles = "Job seekers")]
        public ActionResult AddPhotos(HttpPostedFileBase file)
        {
            try
            {
                if (file != null)
                {
                    var extension = file.FileName.Split('.');
                    if (ValidateExtension(extension[1], 1))//image file check
                    {
                        var fileAry = ImageHelper.ConvertFileInByteArray(file.InputStream, file.ContentLength);
                        //var aspnetUserId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                        _jobseekerService.SavePhoto(fileAry, _aspnetUserId);
                        return Json(new Response(true, "Photo Uploaded Sucessfully"));
                    }
                }
                return Json(new Response(false, "Photo Upload Failed"));
            }
           
            catch (Exception ex)
            {
                return Json(new Response(false, ex.Message));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Job seekers")]
        public ActionResult AddSkill()
        {
            try
            {
                List<SelectListItem> expItems = new List<SelectListItem>();
                expItems.Add(new SelectListItem() { Text = "Less Then 1 Year", Value = "1" });
                expItems.Add(new SelectListItem() { Text = "1 Years", Value = "1" });
                expItems.Add(new SelectListItem() { Text = "2 Years", Value = "2" });
                expItems.Add(new SelectListItem() { Text = "3 Years", Value = "3" });
                expItems.Add(new SelectListItem() { Text = "4 Years", Value = "4" });
                expItems.Add(new SelectListItem() { Text = "5 Years", Value = "5" });
                expItems.Add(new SelectListItem() { Text = "6 Years", Value = "6" });
                expItems.Add(new SelectListItem() { Text = "7 Years", Value = "7" });
                expItems.Add(new SelectListItem() { Text = "8 Years", Value = "8" });
                expItems.Add(new SelectListItem() { Text = "9 Years", Value = "9" });
                expItems.Add(new SelectListItem() { Text = "10+ Years", Value = "10" });

                this.ViewBag.expItems = new SelectList(expItems, "Value", "Text");

                return PartialView("Partial/_addSkill");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult SaveSkill(JobSeekerSkillDto skill)
        {
            try
            {
                //var aspnetUserId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                skill.JobSeeker = _aspnetUserId;
                _jobseekerService.SaveSkill(skill);
                return Json(new Response(true, "Saved Successfully"));
            }
            catch (Exception ex)
            {
                return Json(new Response(false, ex.Message));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Job seekers")]
        public ActionResult AddLink()
        {
            try
            {
                return PartialView("Partial/_addLink");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Authorize(Roles = "Job seekers")]
        public ActionResult SaveLink(JobSeekerLinkVm link)
        {
            try
            {
                //var aspnetUserId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                link.JobSeeker = _aspnetUserId;
                var destObject = new JobSeekerLinkDto();
                if (link != null)
                {
                    CopyClass.CopyObject<JobSeekerLinkDto>(link, ref destObject);
                }
                _jobseekerService.SaveLink(destObject);
                return Json(new Response(true, "Saved Successfully"));
            }
            catch (Exception ex)
            {
                return Json(new Response(false, ex.Message));
            }
        }


        [HttpPost]
        [Authorize(Roles = "Job seekers")]
        public ActionResult AddMilitary()
        {
            try
            {
                ViewBag.countries =
                    new SelectList(_countryService.LoadCountry(EntityStatus.Active, 0), "Id", "Name");
                return PartialView("Partial/_addMilitary");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult SaveMilitary(JobSeekerMilitaryDto link)
        {
            try
            {
                //var aspnetUserId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                link.JobSeeker = _aspnetUserId;
                _jobseekerService.SaveMilitary(link);
                return Json(new Response(true, "Saved Successfully"));
            }
            catch (Exception ex)
            {
                return Json(new Response(false, ex.Message));
            }
        }


        [HttpPost]
        [Authorize(Roles = "Job seekers")]
        public ActionResult AddAward()
        {
            try
            {
                //ViewBag.countries =
                //    new SelectList(_webAdminService.LoadCountry(0, int.MaxValue, "", "", "", "", "", 4, 1), "Id", "Name");
                return PartialView("Partial/_addAward", new JobSeekerAwardVm());
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Authorize(Roles = "Job seekers")]
        public ActionResult SaveAward(JobSeekerAwardVm link)
        {
            try
            {
                //var aspnetUserId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                link.JobSeeker = _aspnetUserId;
                JobSeekerAwardDto destObject = new JobSeekerAwardDto();
                if (link != null)
                {
                    CopyClass.CopyObject<JobSeekerAwardDto>(link, ref destObject);
                }
                _jobseekerService.SaveAward(destObject);
                return Json(new Response(true, "Saved Successfully"));
            }
            catch (Exception ex)
            {
                return Json(new Response(false, ex.Message));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Job seekers")]
        public ActionResult AddCertificate()
        {
            try
            {
                //ViewBag.countries =
                //    new SelectList(_webAdminService.LoadCountry(0, int.MaxValue, "", "", "", "", "", 4, 1), "Id", "Name");
                return PartialView("Partial/_addCertificate", new JobSeekerCertificateVm());
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Authorize(Roles = "Job seekers")]
        public ActionResult SaveCertificate(JobSeekerCertificateVm link)
        {
            try
            {
                //var aspnetUserId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                link.JobSeeker = _aspnetUserId;
                JobSeekerCertificateDto destObject = new JobSeekerCertificateDto();
                if (link != null)
                {
                    CopyClass.CopyObject<JobSeekerCertificateDto>(link, ref destObject);
                }
                _jobseekerService.SaveCertificate(destObject);
                return Json(new Response(true, "Saved Successfully"));
            }
            catch (Exception ex)
            {
                return Json(new Response(false, ex.Message));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Job seekers")]
        public ActionResult AddGroup()
        {
            try
            {
                return PartialView("Partial/_addGroup", new JobSeekerGroupVm());
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Authorize(Roles = "Job seekers")]
        public ActionResult SaveGroup(JobSeekerGroupVm link)
        {
            try
            {
                //var aspnetUserId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                link.JobSeeker = _aspnetUserId;
                JobSeekerGroupDto destObject = new JobSeekerGroupDto();
                if (link != null)
                {
                    CopyClass.CopyObject<JobSeekerGroupDto>(link, ref destObject);
                }
                _jobseekerService.SaveGroup(destObject);
                return Json(new Response(true, "Saved Successfully"));
            }
            catch (Exception ex)
            {
                return Json(new Response(false, ex.Message));
            }
        }


        [HttpPost]
        [Authorize(Roles = "Job seekers")]
        public ActionResult AddPatent()
        {
            try
            {
                return PartialView("Partial/_addPatent", new JobSeekerPatentsVm());
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Authorize(Roles = "Job seekers")]
        public ActionResult SavePatent(JobSeekerPatentsVm link)
        {
            try
            {
                //var aspnetUserId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                link.JobSeeker = _aspnetUserId;
                JobSeekerPatentsDto destObject = new JobSeekerPatentsDto();
                if (link != null)
                {
                    CopyClass.CopyObject<JobSeekerPatentsDto>(link, ref destObject);
                }
                _jobseekerService.SavePatent(destObject);
                return Json(new Response(true, "Saved Successfully"));
            }
            catch (Exception ex)
            {
                return Json(new Response(false, ex.Message));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Job seekers")]
        public ActionResult AddPublication()
        {
            try
            {
                return PartialView("Partial/_addPublication", new JobSeekerPublicationsVm());
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Authorize(Roles = "Job seekers")]
        public ActionResult SavePublication(JobSeekerPublicationsVm link)
        {
            try
            {
                // var aspnetUserId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                link.JobSeeker = _aspnetUserId;
                JobSeekerPublicationsDto destObject = new JobSeekerPublicationsDto();
                if (link != null)
                {
                    CopyClass.CopyObject<JobSeekerPublicationsDto>(link, ref destObject);
                }
                _jobseekerService.SavePublication(destObject);
                return Json(new Response(true, "Saved Successfully"));
            }
            catch (Exception ex)
            {
                return Json(new Response(false, ex.Message));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Job seekers")]
        public ActionResult AddAdditionalInfo()
        {
            try
            {
                return PartialView("Partial/_addAdditionalInfo", new JobSeekerAdditionalInformationVm());
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Authorize(Roles = "Job seekers")]
        public ActionResult SaveAdditionalInfo(JobSeekerAdditionalInformationVm link)
        {
            try
            {
                //var aspnetUserId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                link.JobSeeker = _aspnetUserId;
                JobSeekerAdditionalInformationDto destObject = new JobSeekerAdditionalInformationDto();
                if (link != null)
                {
                    CopyClass.CopyObject<JobSeekerAdditionalInformationDto>(link, ref destObject);
                }
                _jobseekerService.SaveAdditinalInfo(destObject);
                return Json(new Response(true, "Saved Successfully"));
            }
            catch (Exception ex)
            {
                return Json(new Response(false, ex.Message));
            }
        }

        //AddDesire
        [HttpPost]
        [Authorize(Roles = "Job seekers")]
        public ActionResult AddDesire()
        {
            try
            {
                return PartialView("Partial/_addDesire", new JobSeekerDesiredJobVm());
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Authorize(Roles = "Job seekers")]
        public ActionResult SaveDesire(string[] desiretype, string salary, string salaryDuration, string isRelocate, string place, string eligibile, long id = 0)
        {
            try
            {
                if (desiretype.Length <= 0) return Json(new Response(false, "NO JobType Selected"));
                if (String.IsNullOrEmpty(salary)) return Json(new Response(false, "Invalid Salary"));
                if (String.IsNullOrEmpty(salaryDuration)) return Json(new Response(false, "Invalid Salary Duration"));
                JobSeekerDesiredJobDto desiredJobDto = new JobSeekerDesiredJobDto();
                desiredJobDto.Id = id;
                desiredJobDto.DesiredSalary = Convert.ToInt32(salary);
                desiredJobDto.SalaryDurationInDay = Convert.ToInt32(salaryDuration);
                desiredJobDto.IsRelocate = !String.IsNullOrEmpty(isRelocate);
                //will change this later will have t implement auto complete city search option 
                desiredJobDto.RelocatingPlaceOne = Convert.ToInt32(place) == InLocation.InsideUsa ? InLocation.InsideUsa.ToString() : InLocation.OutSideUsa.ToString();
                if (Convert.ToInt32(eligibile) == Eligibility.AuthorizedToWorkInUsa)
                {
                    desiredJobDto.EmploymentEligibility = Eligibility.AuthorizedToWorkInUsa;
                }
                else if (Convert.ToInt32(eligibile) == Eligibility.UnAuthorizedToWorkInUsa) { desiredJobDto.EmploymentEligibility = Eligibility.UnAuthorizedToWorkInUsa; }
                foreach (var desType in desiretype)
                {
                    switch (Convert.ToInt32(desType))
                    {
                        case JobNature.Commision:
                            desiredJobDto.IsCommission = true;
                            break;
                        case JobNature.Contract:
                            desiredJobDto.IsContract = true;
                            break;
                        case JobNature.FullTime:
                            desiredJobDto.IsFullTime = true;
                            break;
                        case JobNature.Internship:
                            desiredJobDto.IsInternship = true;
                            break;
                        case JobNature.PartTime:
                            desiredJobDto.IsPartTime = true;
                            break;
                        case JobNature.Temporary:
                            desiredJobDto.IsTemporary = true;
                            break;
                    }
                }
                //var aspnetUserId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                desiredJobDto.JobSeekerId = _aspnetUserId;
                _jobseekerService.SaveDesiredJob(desiredJobDto);
                return Json(new Response(true, "Saved Successfully"));
            }
            catch (Exception ex)
            {
                return Json(new Response(false, ex.Message));
            }
        }


        private bool ValidateExtension(string extension, int fileType = 0)
        {
            extension = extension.ToLower();
            if (fileType == 1)
            {
                switch (extension)
                {
                    case "jpg":
                        return true;
                    case "png":
                        return true;
                    case "gif":
                        return true;
                    case "jpeg":
                        return true;
                    default:
                        return false;
                }
            }
            if (fileType == 2)
            {
                switch (extension)
                {
                    case "pdf":
                        return true;
                    //case "docs":
                    //    return true;
                    default:
                        return false;
                }
            }
            return false;
        }

        private void Initialize()
        {
            ViewBag.GenderList = new SelectList(_commonHelper.LoadEmumToDictionary<Constants.Gender>(), "Key", "Value").ToList();
            ViewBag.MaritalList = new SelectList(_commonHelper.LoadEmumToDictionary<Constants.MaritalType>(), "Key", "Value").ToList();

            ViewBag.RegionList = new SelectList(_regionSevice.LoadRegion(0, int.MaxValue, "", "", "", 1), "Id", "Name");
            ViewBag.CountryList = new SelectList(String.Empty, "Value", "Text");
            ViewBag.StateList = new SelectList(String.Empty, "Value", "Text");
            ViewBag.CityList = new SelectList(String.Empty, "Value", "Text");
            ViewBag.JobCategory = new SelectList(_jobCategoryService.LoadJobCategory(1), "Id", "Name");
        }

        #endregion

        #region Delete
        [Authorize(Roles = "Job seekers")]
        public ActionResult DelEdu(long id)
        {
            try
            {
                _jobSeekerEducationalQualificationService.Delete(id);
                return Json(new Response(true, "Educational Data Deleted Successfully"));
            }
          
            catch (Exception ex)
            {
                return Json(new Response(false, ex.Message));
            }
        }

        [Authorize(Roles = "Job seekers")]
        public ActionResult DelExp(long id)
        {
            try
            {
                _jobSeekerExperienceService.Delete(id);
                return Json(new Response(true, "Experience Deleted Successfully"));
            }
           
            catch (Exception ex)
            {
                return Json(new Response(false, ex.Message));
            }
        }
        [Authorize(Roles = "Job seekers")]
        public ActionResult DelTraining(long id)
        {
            try
            {
                _jobSeekerTrainingCoursesService.Delete(id);
                return Json(new Response(true, "Training Data Deleted Successfully"));
            }
            
            catch (Exception ex)
            {
                return Json(new Response(false, ex.Message));
            }
        }

        [Authorize(Roles = "Job seekers")]
        public ActionResult DeleteResume()
        {
            try
            {
                //var aspnetUserId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                _jobseekerService.DeleteResume(_aspnetUserId);
                return Json(new Response(true, "Resume Deleted Successfully"));
            }
            
            catch (Exception ex)
            {
                return Json(new Response(false, ex.Message));
            }
        }

        [Authorize(Roles = "Job seekers")]
        public ActionResult DelSkill(long id)
        {
            _jobseekerService.DeleteSkill(id);
            return Json(new Response(true, "Deleted Successfully"));
        }

        [Authorize(Roles = "Job seekers")]
        public ActionResult DelPublication(long id)
        {
            _jobseekerService.DeletePublication(id);
            return Json(new Response(true, "Deleted Successfully"));
        }

        [Authorize(Roles = "Job seekers")]
        public ActionResult DelPatent(long id)
        {
            _jobseekerService.DeletePatent(id);
            return Json(new Response(true, "Deleted Successfully"));
        }

        [Authorize(Roles = "Job seekers")]
        public ActionResult DelMilitary(long id)
        {
            _jobseekerService.DeleteMalitary(id);
            return Json(new Response(true, "Deleted Successfully"));
        }

        [Authorize(Roles = "Job seekers")]
        public ActionResult DelLink(long id)
        {
            _jobseekerService.DeleteLink(id);
            return Json(new Response(true, "Deleted Successfully"));
        }

        [Authorize(Roles = "Job seekers")]
        public ActionResult DelGroup(long id)
        {
            _jobseekerService.DeleteGroup(id);
            return Json(new Response(true, "Deleted Successfully"));
        }

        [Authorize(Roles = "Job seekers")]
        public ActionResult DelAward(long id)
        {
            _jobseekerService.DeleteAward(id);
            return Json(new Response(true, "Deleted Successfully"));
        }

        [Authorize(Roles = "Job seekers")]
        public ActionResult DelCertificate(long id)
        {
            _jobseekerService.DeleteCertificate(id);
            return Json(new Response(true, "Deleted Successfully"));
        }

        [Authorize(Roles = "Job seekers")]
        public ActionResult DelAdditionalInfo(long id)
        {
            _jobseekerService.DeleteAdditionalInfo(id);
            return Json(new Response(true, "Deleted Successfully"));
        }

        [Authorize(Roles = "Job seekers")]
        public ActionResult DelDesiredJobs(long id)
        {
            _jobseekerService.DeleteDesiredJobs(id);
            return Json(new Response(true, "Deleted Successfully"));
        }



        #endregion

        #region Cv Upload

        [HttpGet]
        [Authorize(Roles = "Job seekers")]
        public ActionResult CvUpload()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        [HttpPost]
        [Authorize(Roles = "Job seekers")]
        public ActionResult CvUpload(HttpPostedFileBase file)
        {
            try
            {
                if (file != null)
                {
                    var extension = file.FileName.Split('.');
                    if (extension.Length > 2)
                    {
                        ViewBag.ErrorMessage = "Invalid File Name do not use extra character like .,";
                        return View();
                    }
                    if (ValidateExtension(extension[1], 2))//file
                    {
                        if (file.ContentLength > 5 * 1000 * 1000)
                        {
                            ViewBag.ErrorMessage = "File Size must be Less than 5Mb";
                            return View();
                        }
                        var fileAry = ImageHelper.ConvertFileInByteArray(file.InputStream, file.ContentLength);
                        //var aspnetUserId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                        _jobSeekerDetailsService.SaveCv(fileAry, _aspnetUserId);
                        ViewBag.SuccessMessage = "Cv uploaded Successfully";
                    }
                    else { ViewBag.ErrorMessage = "Invalid Cv Format"; }
                }
                else
                {
                    ViewBag.ErrorMessage = "Invalid File";
                    return View();
                }

            }
          
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Job seekers")]
        public ActionResult ViewCv()
        {
            try
            {
                string imagePath = "";
                //var aspnetUserId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                JobSeekerCvBankDto jobSeekerCvBankDto = _jobSeekerCvBankService.GetByUserProfileId(_aspnetUserId);
                return File(jobSeekerCvBankDto.Cv, "application/pdf");
            }
            catch (Exception ex)
            {
                //throw ex;
                return RedirectToAction("JobSeekerProfile", "JobSeeker", new { message = ex.Message });
                ViewBag.ErrorMessage = ex.Message;
            }
        }

        [HttpGet]
        [Authorize(Roles = "Job seekers")]
        public ActionResult Resume()
        {
            try
            {
                //var aspnetUserId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                var jobSeekerProfileDto = _jobseekerService.GetJobSeekerProfileData(_aspnetUserId);
                //var profileVm = jobSeekerProfileDto.JobSeekerProfileVm;
                return View(jobSeekerProfileDto);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        #endregion

        #region View Resume

        [Authorize(Roles = "Job seekers")]
        public ActionResult ViewResume(string message = "", string messageType = "")
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
                //var aspnetUserId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                jobseeker = _jobseekerService.GetJobSeekerByAspId(_aspnetUserId, EntityStatus.Active);
                if (jobseeker == null || !jobseeker.JobSeekerDetailList.Any())
                {
                    ViewBag.ErrorMessage = " Resume Not found.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Resume Load Error.";
            }

            return View(jobseeker);
        }
        [Authorize(Roles = "Job seekers")]
        public ActionResult DownloadResume()
        {
            var jobseeker = new JobSeekerDto();
            try
            {

                //var aspnetUserId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                jobseeker = _jobseekerService.GetJobSeekerByAspId(_aspnetUserId, EntityStatus.Active);
                if (jobseeker == null || !jobseeker.JobSeekerDetailList.Any())
                {
                    return RedirectToAction("ViewResume", "JobSeeker", new { message = "Resume Not found.", messageType = "e" });
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "ResumeLoad Error.";
                return RedirectToAction("ViewResume", "JobSeeker", new { message = "Resume Load Error.", messageType = "e" });
            }

            // return View(company);
            return new Rotativa.ViewAsPdf("DownloadResume", jobseeker) { FileName = jobseeker.FirstName + "_Profile.pdf" };
        }
        #endregion


        #region Online Application

        [Authorize(Roles = "Job seekers")]
        public ActionResult OnlineApplication()
        {
            try
            {
                Initialize();
                return View();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        [HttpPost]
        [Authorize(Roles = "Job seekers")]
        public ActionResult BrowseJobSeeekerJob(int draw, int start, int length, long jobCategoryId = 0)
        {
            try
            {
                var userId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                if (String.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("Login", "Account");
                }
                IList<JobPostsDto> jobList = _jobPostService.LoadAllJobAppliedByUser(userId);
                int recordsTotal = _jobPostService.CountLoadAllJobAppliedByUser(userId);
                var data = new List<object>();
                foreach (var dto in jobList)
                {
                    dto.JobDescription = GetJobDescription(dto.JobDescription);
                    var str = new List<string>();
                    if (dto.DeadLine != null)
                    {
                        var job = ConvertPartialViewToString(PartialView("Partial/_jobItem", dto));
                        str.Add(job);
                        data.Add(str);
                    }

                    //data.Add(str);
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
                return Json(new
                {
                    draw = draw,
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    start = start,
                    length = length,
                    data = new List<object>()
                });
            }
        }

        #endregion

        #region Shortlisted Job

        [Authorize(Roles = "Job seekers")]
        public ActionResult ShortListedJob()
        {
            try
            {
                Initialize();
                return View();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        [HttpPost]
        [Authorize(Roles = "Job seekers")]
        public ActionResult ShortListedJob(int draw, int start, int length, long jobCategoryId = 0)
        {
            try
            {
                var userId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                if (String.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("Login", "Account");
                }
                IList<JobPostsDto> jobList = _jobPostService.LoadAllJobShortListedByUser(userId);
                int recordsTotal = _jobPostService.CountLoadAllJobShortListedByUser(userId);
                var data = new List<object>();
                foreach (var dto in jobList)
                {
                    dto.JobDescription = GetJobDescription(dto.JobDescription);
                    var str = new List<string>();
                    if (dto.DeadLine != null)
                    {
                        var job = ConvertPartialViewToString(PartialView("Partial/_jobItem", dto));
                        str.Add(job);
                        data.Add(str);
                    }

                    //data.Add(str);
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
                return Json(new
                {
                    draw = draw,
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    start = start,
                    length = length,
                    data = new List<object>()
                });
            }
        }


        #endregion

        #region Edit
        [Authorize(Roles = "Job seekers")]
        public ActionResult EditExp(long id)
        {
            var expVm = _jobSeekerExperienceService.GetById(id);
            JobSeekerExpVm destObject = new JobSeekerExpVm();
            if (expVm != null)
            {
                CopyClass.CopyObject<JobSeekerExpVm>(expVm, ref destObject);
            }
            return PartialView("Partial/_editExp", destObject);
        }

        [Authorize(Roles = "Job seekers")]
        public ActionResult EditProfile(long id)
        {
            //var aspnetUserId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
            var jobSeekerProfileDto = _jobseekerService.GetJobSeekerProfileData(_aspnetUserId);
            var destObject = new Model.JobLanes.ViewModel.JobSeekerProfileVm();
            if (jobSeekerProfileDto != null )
            {
                CopyClass.CopyObject<Model.JobLanes.ViewModel.JobSeekerProfileVm>(jobSeekerProfileDto, ref destObject);
            }
            if (jobSeekerProfileDto != null )
            {
                ViewBag.GenderList = new SelectList(_commonHelper.LoadEmumToDictionary<Constants.Gender>(), "Key", "Value", jobSeekerProfileDto.Gender).ToList();
                ViewBag.MaritalList = new SelectList(_commonHelper.LoadEmumToDictionary<Constants.MaritalType>(), "Key", "Value", jobSeekerProfileDto.MaritalStatus).ToList();
                ViewBag.RegionList = new SelectList(_regionSevice.LoadRegion(0, int.MaxValue, "", "", "", 1), "Id", "Name", jobSeekerProfileDto.RegionId);
                if (!String.IsNullOrEmpty(destObject.ContactNumber))
                {
                    ViewBag.CountryList = new SelectList(_countryService.LoadCountry(0, int.MaxValue, "", "", "", "", "", destObject.RegionId, 1), "Id", "Name", jobSeekerProfileDto.CountryId);
                    ViewBag.StateList = new SelectList(_stateService.LoadState(0, int.MaxValue, "", "", "", "", destObject.RegionId, destObject.CountryId, 1), "Id", "Name", jobSeekerProfileDto.StateId);
                    ViewBag.CityList = new SelectList(_cityService.LoadCity(0, int.MaxValue, "", "", "", "", destObject.RegionId, destObject.CountryId, destObject.StateId, 1), "Id", "Name", jobSeekerProfileDto.CityId);
                }
                else
                {
                    ViewBag.CountryList = new SelectList(String.Empty, "Value", "Text");
                    ViewBag.StateList = new SelectList(String.Empty, "Value", "Text");
                    ViewBag.CityList = new SelectList(String.Empty, "Value", "Text");
                }
            }
            else
            {
                Initialize();
            }
            destObject.ContactEmail = System.Web.HttpContext.Current.User.Identity.Name;
            return PartialView("Partial/_editProfileInfo", destObject);
        }
        [Authorize(Roles = "Job seekers")]
        public ActionResult EditEdu(long id)
        {
            var expVm = _jobSeekerEducationalQualificationService.GetById(id);
            JobSeekerEduVm destObject = new JobSeekerEduVm();
            if (expVm != null)
            {
                CopyClass.CopyObject<JobSeekerEduVm>(expVm, ref destObject);
            }
            return PartialView("Partial/_editEdu", destObject);
        }

        [Authorize(Roles = "Job seekers")]
        public ActionResult EditSkill(long id)
        {
            var expVm = _jobseekerService.GetJobSeekerSkillDtoByAspNetUserId(id);
            JobSeekerSkillDto destObject = new JobSeekerSkillDto();
            if (expVm != null)
            {
                CopyClass.CopyObject<JobSeekerSkillDto>(expVm, ref destObject);
            }
            List<SelectListItem> expItems = new List<SelectListItem>();
            expItems.Add(new SelectListItem() { Text = "Less Then 1 Year", Value = "1" });
            expItems.Add(new SelectListItem() { Text = "1 Years", Value = "1" });
            expItems.Add(new SelectListItem() { Text = "2 Years", Value = "2" });
            expItems.Add(new SelectListItem() { Text = "3 Years", Value = "3" });
            expItems.Add(new SelectListItem() { Text = "4 Years", Value = "4" });
            expItems.Add(new SelectListItem() { Text = "5 Years", Value = "5" });
            expItems.Add(new SelectListItem() { Text = "6 Years", Value = "6" });
            expItems.Add(new SelectListItem() { Text = "7 Years", Value = "7" });
            expItems.Add(new SelectListItem() { Text = "8 Years", Value = "8" });
            expItems.Add(new SelectListItem() { Text = "9 Years", Value = "9" });
            expItems.Add(new SelectListItem() { Text = "10+ Years", Value = "10" });

            this.ViewBag.expItems = new SelectList(expItems, "Value", "Text", destObject.Experence);

            return PartialView("Partial/_editSkill", destObject);
        }

        [Authorize(Roles = "Job seekers")]
        public ActionResult EditPublication(long id)
        {
            var expVm = _jobseekerService.GetJobSeekerPublicationsDtoByAspNetUserId(id);
            JobSeekerPublicationsVm destObject = new JobSeekerPublicationsVm();
            if (expVm != null)
            {
                CopyClass.CopyObject<JobSeekerPublicationsVm>(expVm, ref destObject);
            }
            return PartialView("Partial/_editPublication", destObject);
        }

        [Authorize(Roles = "Job seekers")]
        public ActionResult EditPatent(long id)
        {
            var expVm = _jobseekerService.GetJobSeekerPatentsDtoByAspNetUserId(id);
            JobSeekerPatentsVm destObject = new JobSeekerPatentsVm();
            if (expVm != null)
            {
                CopyClass.CopyObject<JobSeekerPatentsVm>(expVm, ref destObject);
            }
            return PartialView("Partial/_editPatent", destObject);
        }

        [Authorize(Roles = "Job seekers")]
        public ActionResult EditMilitary(long id)
        {
            var expVm = _jobseekerService.GetJobSeekerMilitaryDtoByAspNetUserId(id);
            ViewBag.countries =
                   new SelectList(_countryService.LoadCountry(EntityStatus.Active, 0), "Id", "Name", expVm.Country);
            JobSeekerMilitaryDto destObject = new JobSeekerMilitaryDto();
            if (expVm != null)
            {
                CopyClass.CopyObject<JobSeekerMilitaryDto>(expVm, ref destObject);
            }
            return PartialView("Partial/_editMilitary", destObject);
        }

        [Authorize(Roles = "Job seekers")]
        public ActionResult EditLink(long id)
        {
            var expVm = _jobseekerService.GetJobSeekerLinkDtoByAspNetUserId(id);
            JobSeekerLinkVm destObject = new JobSeekerLinkVm();
            if (expVm != null)
            {
                CopyClass.CopyObject<JobSeekerLinkVm>(expVm, ref destObject);
            }
            return PartialView("Partial/_editLink", destObject);
        }

        [Authorize(Roles = "Job seekers")]
        public ActionResult EditGroup(long id)
        {
            var expVm = _jobseekerService.GetJobSeekerGroupDtoByAspNetUserId(id);
            JobSeekerGroupVm destObject = new JobSeekerGroupVm();
            if (expVm != null)
            {
                CopyClass.CopyObject<JobSeekerGroupVm>(expVm, ref destObject);
            }
            return PartialView("Partial/_editGroup", destObject);
        }

        [Authorize(Roles = "Job seekers")]
        public ActionResult EditAward(long id)
        {
            var expVm = _jobseekerService.GetJobSeekerAwardDtoByAspNetUserId(id);
            JobSeekerAwardVm destObject = new JobSeekerAwardVm();
            if (expVm != null)
            {
                CopyClass.CopyObject<JobSeekerAwardVm>(expVm, ref destObject);
            }
            return PartialView("Partial/_editAward", destObject);
        }

        [Authorize(Roles = "Job seekers")]
        public ActionResult EditCertificate(long id)
        {
            var expVm = _jobseekerService.GetJobSeekerCertificateDtoByAspNetUserId(id);
            JobSeekerCertificateVm destObject = new JobSeekerCertificateVm();
            if (expVm != null)
            {
                CopyClass.CopyObject<JobSeekerCertificateVm>(expVm, ref destObject);
            }
            return PartialView("Partial/_editCertificate", destObject);
        }

        [Authorize(Roles = "Job seekers")]
        public ActionResult EditAdditionalInfo(long id)
        {
            var expVm = _jobseekerService.GetAdditionalInformationDtosByAspNetUserId(id);
            JobSeekerAdditionalInformationVm destObject = new JobSeekerAdditionalInformationVm();
            if (expVm != null)
            {
                CopyClass.CopyObject<JobSeekerAdditionalInformationVm>(expVm, ref destObject);
            }
            return PartialView("Partial/_editAdditionalInfo", destObject);
        }

        [Authorize(Roles = "Job seekers")]
        public ActionResult EditDesire(long id)
        {
            var desireVm = _jobseekerService.GetDesiredDtosByAspNetUserId(id);
            JobSeekerDesiredJobVm destObject = new JobSeekerDesiredJobVm();
            if (desireVm != null)
            {
                CopyClass.CopyObject<JobSeekerDesiredJobVm>(desireVm, ref destObject);
            }
            ViewBag.duration = new SelectList(SalaryDuration.GetSalaryDuration(), "Value", "Key", destObject.SalaryDurationInDay);
            return PartialView("Partial/_editDesire", destObject);
        }

        #endregion

        #region Ajax Request

        [HttpPost]
        [Authorize(Roles = "Job seekers")]
        public ActionResult GetEducationalData()
        {
            try
            {
                if (!Request.IsAjaxRequest()) return Json(new Response(false, "Invalid Ajax Request"));
                //var aspnetUserId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                JobSeekerProfileDto jobSeekerProfileDto = _jobseekerService.GetJobSeeker(_aspnetUserId);
                List<JobSeekerEduVm> destObjectList = new List<JobSeekerEduVm>();
                JobSeekerEduVm seekerEduVm = new JobSeekerEduVm();
                if (jobSeekerProfileDto != null && jobSeekerProfileDto.JobSeekerEduVm != null)
                {
                    CopyClass.CopyObectList(jobSeekerProfileDto.JobSeekerEduVm.ToArray(), seekerEduVm, ref destObjectList);
                }
                else
                {

                    destObjectList.Add(new JobSeekerEduVm());
                }
                ViewBag.eduList = destObjectList;
                return PartialView("Partial/_educationalData");
                //return PartialView("Partial/_educationalData", jobSeekerProfileDto.JobSeekerEduVm);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Authorize(Roles = "Job seekers")]
        public ActionResult GetJobSeekerDesiredData()
        {
            try
            {
                if (!Request.IsAjaxRequest())
                {
                    return Json(new Response(false, "Invalid Ajax Request"));
                }
                //var aspnetUserId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                IList<JobSeekerDesiredJobDto> jobSeekerDesireDto = _jobseekerService.LoadJobSeekerDesiredAspNetUserId(_aspnetUserId);
                var data = new List<object>();
                foreach (var dto in jobSeekerDesireDto)
                {
                    var str = new List<string>();
                    var job = "<span style='font-weight: bold; font-size:medium'>Desired Job Types </span> <br /><span style='font-weight: normal; font-size:small'> " + GetDesiredJob(dto.IsCommission, dto.IsContract, dto.IsInternship, dto.IsTemporary, dto.IsPartTime, dto.IsFullTime) + "</span><br />";
                    if (!String.IsNullOrEmpty(dto.RelocatingPlaceOne))
                    {
                        job += "<span style='font-weight: normal; font-size:small'> Inside USA </span> <br />";
                    }
                    if (!String.IsNullOrEmpty(dto.RelocatingPlaceTwo))
                    {
                        job += "<span style='font-weight: normal; font-size:small'> ,Outside USA </span> <br />";
                    }
                    if (!String.IsNullOrEmpty(dto.RelocatingPlaceThree))
                    {
                        job += "<span style='font-weight: normal; font-size:small'> &nbsp;&nbsp;" + dto.RelocatingPlaceThree + " </span> ";
                    }
                    job += "<span style='font-weight: bold; font-size:medium'>Employment Elegibility </span> <br /> <span style='font-weight: normal; font-size:small'> " + GetEligibolity(dto.EmploymentEligibility) + "</span><br />";
                    var editLink = "<button data-val-id='" + dto.Id +
                            "' class='btn btn-default btn-sm glyphicon glyphicon-edit desireEditBtn'></button>";
                    str.Add(job);
                    str.Add(editLink);

                    data.Add(str);
                }
                return Json(new
                {
                    data = data
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return Json(new
                {
                    data = new List<object>()
                });
            }
        }


        [HttpPost]
        [Authorize(Roles = "Job seekers")]
        public ActionResult GetExpData()
        {
            try
            {
                if (!Request.IsAjaxRequest())
                {
                    return Json(new Response(false, "Invalid Ajax Request"));
                }
                //var aspnetUserId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                JobSeekerProfileDto jobSeekerProfileDto = _jobseekerService.GetJobSeeker(_aspnetUserId);
                List<JobSeekerExpVm> destObjectList = new List<JobSeekerExpVm>();
                JobSeekerExpVm seekerEduVm = new JobSeekerExpVm();
                if (jobSeekerProfileDto != null && jobSeekerProfileDto.JobSeekerExpVm != null)
                {
                    CopyClass.CopyObectList(jobSeekerProfileDto.JobSeekerExpVm.ToArray(), seekerEduVm, ref destObjectList);
                }
                else
                {
                    destObjectList.Add(new JobSeekerExpVm());
                }
                ViewBag.eduList = destObjectList;
                return PartialView("Partial/_experience");
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult GetTrainData()
        {
            try
            {
                if (!Request.IsAjaxRequest())
                {
                    return Json(new Response(false, "Invalid Ajax Request"));
                }
                // var aspnetUserId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                JobSeekerProfileDto jobSeekerProfileDto = _jobseekerService.GetJobSeeker(_aspnetUserId);
                List<Web.Joblanes.Models.ViewModel.JobSeekerTrainingVm> destObjectList = new List<Web.Joblanes.Models.ViewModel.JobSeekerTrainingVm>();
                Web.Joblanes.Models.ViewModel.JobSeekerTrainingVm seekerEduVm = new Web.Joblanes.Models.ViewModel.JobSeekerTrainingVm();
                if (jobSeekerProfileDto != null && jobSeekerProfileDto.JobSeekerExpVm != null)
                {
                    CopyClass.CopyObectList(jobSeekerProfileDto.JobSeekerTrainingVm.ToArray(), seekerEduVm, ref destObjectList);
                }
                else
                {
                    destObjectList.Add(new Web.Joblanes.Models.ViewModel.JobSeekerTrainingVm());
                }
                ViewBag.eduList = destObjectList;
                return PartialView("Partial/_training");
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [Authorize(Roles = "Job seekers")]
        public ActionResult JobApplied(long jobId)
        {
            try
            {
                if (!Request.IsAjaxRequest())
                {
                    return Json(new Response(false, "Invalid Ajax Request"));
                }
                var userId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                var jobPostId = jobId;
                _jobSeekerJobPostService.SaveOrUpdate(userId, jobPostId, false, true);
                return Json(new Response(true, "You have successfully applied to this job"));
            }
            catch (Exception ex)
            {
                return Json(new Response(false, ex.Message));

            }
        }

        [HttpPost]
        public ActionResult JobShortlisted(long jobId)
        {
            try
            {
                if (!Request.IsAjaxRequest())
                {
                    return Json(new Response(false, "Invalid Ajax Request"));
                }
                var userId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                var jobPostId = jobId;
                _jobSeekerJobPostService.SaveOrUpdate(userId, jobPostId, true, false);
                return Json(new Response(true, "You have successfully shortlisted this job"));
            }
            catch (Exception ex)
            {
                return Json(new Response(false, ex.Message));

            }
        }

        [HttpPost]
        [Authorize(Roles = "Job seekers")]
        public ActionResult GetProfileDataTable()
        {
            try
            {
                if (!Request.IsAjaxRequest())
                {
                    return Json(new Response(false, "Invalid Ajax Request"));
                }
                //var aspnetUserId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                JobSeekerProfileDto jobSeekerProfileDto = _jobseekerService.GetJobSeeker(_aspnetUserId);
                var data = new List<object>();

                var str = new List<string>();
                var job = "";
                var obj = jobSeekerProfileDto.JobSeekerProfileVm;
                if (!String.IsNullOrEmpty(obj.FirstName) || !String.IsNullOrEmpty(obj.LastName))
                {
                    job += "<span style='font-weight: normal; font-size:large'> &nbsp;" + jobSeekerProfileDto.JobSeekerProfileVm.FirstName + " " + jobSeekerProfileDto.JobSeekerProfileVm.LastName + "</span><br />";
                }
                if (!String.IsNullOrEmpty(obj.ContactEmail))
                {
                    job += "<span style='font-weight: normal; font-size:small'> &nbsp;&nbsp;" +
                           jobSeekerProfileDto.JobSeekerProfileVm.ContactEmail + " </span><br />";
                }
                if (!String.IsNullOrEmpty(obj.ContactNumber))
                {
                    job += "<span style='font-weight: normal; font-size:small'> &nbsp;&nbsp;" + jobSeekerProfileDto.JobSeekerProfileVm.ContactNumber + "</span><br />";
                }
                if (!String.IsNullOrEmpty(obj.Address))
                {
                    job += "<span style='font-weight: normal; font-size:small'> &nbsp;&nbsp;" + jobSeekerProfileDto.JobSeekerProfileVm.Address + " </span>" + " " + "<span style='font-weight: normal; font-size:small'> &nbsp;&nbsp;" + jobSeekerProfileDto.JobSeekerProfileVm.ZipCode + " </span><br />";
                }
                if (!String.IsNullOrEmpty(obj.FatherName))
                {
                    job += "<span style='font-weight: normal; font-size:small'> &nbsp;&nbsp;" + jobSeekerProfileDto.JobSeekerProfileVm.FatherName + " </span><br />";
                }
                if (!String.IsNullOrEmpty(obj.MotherName))
                {
                    job += "<span style='font-weight: normal; font-size:small'> &nbsp;&nbsp;" + jobSeekerProfileDto.JobSeekerProfileVm.MotherName + " </span><br />";
                }
                if (obj.CityId > 0)
                {
                    var city = _cityService.GetById(obj.CityId);
                    if (city != null)
                        job += "<span style='font-weight: normal; font-size:small'> &nbsp;&nbsp;" + city.Name + " </span>";
                }
                if (obj.CountryId > 0)
                {
                    var country = _countryService.GetById(obj.CountryId);
                    if (country != null)
                        job += "<span style='font-weight: normal; font-size:small'>,&nbsp;" + country.Name + " </span>";
                }
                if (obj.RegionId > 0)
                {
                    var region = _regionSevice.GetById(obj.RegionId);
                    if (region != null)
                        job += "<span style='font-weight: normal; font-size:small'>, &nbsp;" + region.Name + " </span><br />";

                }
                if (!String.IsNullOrEmpty(obj.Expertise))
                {
                    job += "<span style='font-weight: normal; font-size:small'> &nbsp;&nbsp;" + jobSeekerProfileDto.JobSeekerProfileVm.Expertise + " </span><br />";
                }

                if (!String.IsNullOrEmpty(obj.Dob.ToString()))
                {
                    job += "<span style='font-weight: normal; font-size:small'> &nbsp;&nbsp;" + jobSeekerProfileDto.JobSeekerProfileVm.Dob.ToString("M") + " </span><br />";
                }
                var link = "<button id='' data-val-id='" + jobSeekerProfileDto.JobSeekerProfileVm.Id +
                           "' class='btn btn-default btn-sm glyphicon glyphicon-edit profileEditBtn'></button>";
                str.Add(job);
                str.Add(link);

                data.Add(str);
                // }
                return Json(new
                {
                    data = data
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return Json(new
                {
                    data = new List<object>()
                });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Job seekers")]
        public ActionResult GetExpDataTable()
        {
            try
            {
                if (!Request.IsAjaxRequest())
                {
                    return Json(new Response(false, "Invalid Ajax Request"));
                }
                //var aspnetUserId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                JobSeekerProfileDto jobSeekerProfileDto = _jobseekerService.GetJobSeeker(_aspnetUserId);
                var data = new List<object>();
                foreach (var dto in jobSeekerProfileDto.JobSeekerExpVm)
                {
                    var str = new List<string>();
                    var job = "<span style='font-weight: normal; font-size:small'> &nbsp;&nbsp;" + dto.Designation + " </span><br />" +
                        "<span style='font-weight: normal; font-size:small'>&nbsp;&nbsp;" + dto.CompanyName + " - " + dto.CompanyAddress + "</span><br />" +
                        "<span style='font-weight: normal; font-size:small'> &nbsp;&nbsp;" + dto.DateFrom.ToString("Y") + " - ";
                    if (dto.DateTo != null)
                    {
                        job += dto.DateTo.Value.ToString("Y") + "</span><br />";
                    }
                    else
                    {
                        job += " Current </span><br />";
                    }

                    var editLink = "<button data-val-id='" + dto.Id +
                               "' class='btn btn-default btn-sm glyphicon glyphicon-edit expEditBtn'></button> &nbsp;&nbsp;" +
                                   "<button data-val-id='" + dto.Id +
                               "' class='btn btn-default btn-sm glyphicon glyphicon-remove expDeleteBtn'></button>";
                    str.Add(job);
                    str.Add(editLink);

                    data.Add(str);
                }
                return Json(new
                {
                    data = data
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return Json(new
                {
                    data = new List<object>()
                });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Job seekers")]
        public ActionResult GetEduDataTable()
        {
            try
            {
                if (!Request.IsAjaxRequest())
                {
                    return Json(new Response(false, "Invalid Ajax Request"));
                }
                //var aspnetUserId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                JobSeekerProfileDto jobSeekerProfileDto = _jobseekerService.GetJobSeeker(_aspnetUserId);
                var data = new List<object>();
                foreach (var dto in jobSeekerProfileDto.JobSeekerEduVm)
                {
                    var str = new List<string>();
                    var job = "<span style='font-weight: normal; font-size:small'> &nbsp;&nbsp;" + dto.Degree + " in " + dto.FieldOfStudy + " </span><br />" +
                        "<span style='font-weight: normal; font-size:small'>&nbsp;&nbsp;" + dto.Institute + "</span><br />" +
                        "<span style='font-weight: normal; font-size:small'> &nbsp;&nbsp;" + dto.StartingYear.ToString("Y") + " - " + dto.PassingYear.ToString("Y") + "</span><br />";
                    var editLink = "<button data-val-id='" + dto.Id +
                               "' class='btn btn-default btn-sm glyphicon glyphicon-edit eduEditBtn'></button> &nbsp;&nbsp;" +
                                   "<button data-val-id='" + dto.Id +
                               "' class='btn btn-default btn-sm glyphicon glyphicon-remove eduDeleteBtn'></button>";
                    str.Add(job);
                    str.Add(editLink);

                    data.Add(str);
                }
                return Json(new
                {
                    data = data
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return Json(new
                {
                    data = new List<object>()
                });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Job seekers")]
        public ActionResult GetLinkDataTable()
        {
            try
            {
                if (!Request.IsAjaxRequest())
                {
                    return Json(new Response(false, "Invalid Ajax Request"));
                }
                // var aspnetUserId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                IList<JobSeekerLinkDto> jobSeekerSkillDto =
                   _jobseekerService.LoadJobSeekerLinkDtoByAspNetUserId(_aspnetUserId);
                var data = new List<object>();
                foreach (var dto in jobSeekerSkillDto)
                {
                    var str = new List<string>();
                    var job = "<span style='font-weight: normal; font-size:small'> &nbsp;&nbsp;" + dto.Link + "</span><br />";
                    var editLink = "<button data-val-id='" + dto.Id +
                            "' class='btn btn-default btn-sm glyphicon glyphicon-edit linkEditBtn'></button> &nbsp;&nbsp;" +
                                "<button data-val-id='" + dto.Id +
                            "' class='btn btn-default btn-sm glyphicon glyphicon-remove linkDeleteBtn'></button>";
                    str.Add(job);
                    str.Add(editLink);

                    data.Add(str);
                }
                return Json(new
                {
                    data = data
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return Json(new
                {
                    data = new List<object>()
                });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Job seekers")]
        public ActionResult GetMilitaryDataTable()
        {
            try
            {
                if (!Request.IsAjaxRequest())
                {
                    return Json(new Response(false, "Invalid Ajax Request"));
                }
                // var aspnetUserId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                IList<JobSeekerMilitaryDto> jobSeekerSkillDto =
                    _jobseekerService.LoadJobSeekerMilitaryDtoByAspNetUserId(_aspnetUserId);
                var data = new List<object>();
                foreach (var dto in jobSeekerSkillDto)
                {
                    CountryDto countryDto = _countryService.GetById(dto.Country);
                    var str = new List<string>();
                    var job = "<span style='font-weight: normal; font-size:small'> &nbsp;&nbsp;" + countryDto.Name + "</span><br />" +
                        "<span style='font-weight: normal; font-size:small'> &nbsp;&nbsp;" + dto.Branch + "</span><br />" +
                        "<span style='font-weight: normal; font-size:small'> &nbsp;&nbsp;" + dto.Rank + "</span><br />" +
                        "<span style='font-weight: normal; font-size:small'> &nbsp;&nbsp;" + dto.DateFrom.ToString("Y") + " to ";
                    if (dto.DateTo != null)
                    {
                        job += dto.DateTo.Value.ToString("Y") + "</span><br />";
                    }
                    else
                    {
                        job += " Current </span><br />";
                    }

                    job += "<span style='font-weight: normal; font-size:small'> &nbsp;&nbsp;" + dto.Description + "</span><br />" +
                    "<span style='font-weight: normal; font-size:small'> &nbsp;&nbsp;" + dto.Commendations + "</span>";
                    var editLink = "<button data-val-id='" + dto.Id +
                            "' class='btn btn-default btn-sm glyphicon glyphicon-edit militaryEditBtn'></button> &nbsp;&nbsp;" +
                                "<button data-val-id='" + dto.Id +
                            "' class='btn btn-default btn-sm glyphicon glyphicon-remove militaryDeleteBtn'></button>";
                    str.Add(job);
                    str.Add(editLink);

                    data.Add(str);
                }
                return Json(new
                {
                    data = data
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return Json(new
                {
                    data = new List<object>()
                });
            }
        }
        [HttpPost]
        [Authorize(Roles = "Job seekers")]
        public ActionResult GetAwardDataTable()
        {
            try
            {
                if (!Request.IsAjaxRequest())
                {
                    return Json(new Response(false, "Invalid Ajax Request"));
                }
                //var aspnetUserId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                IList<JobSeekerAwardDto> jobSeekerSkillDto =
                   _jobseekerService.LoadJobSeekerAwardDtoByAspNetUserId(_aspnetUserId);
                var data = new List<object>();
                foreach (var dto in jobSeekerSkillDto)
                {
                    var str = new List<string>();
                    var job = "<span style='font-weight: normal; font-size:small'> &nbsp;&nbsp;" + dto.Title +
                              "</span><br />" +
                              "<span style='font-weight: normal; font-size:small'> &nbsp;&nbsp;" + dto.DateAwarded.ToString("M") +
                              "</span><br />" +
                              "<span style='font-weight: normal; font-size:small'> &nbsp;&nbsp;" + dto.Description +
                              "</span>";
                    var editLink = "<button data-val-id='" + dto.Id +
                            "' class='btn btn-default btn-sm glyphicon glyphicon-edit awardEditBtn'></button> &nbsp;&nbsp;" +
                                "<button data-val-id='" + dto.Id +
                            "' class='btn btn-default btn-sm glyphicon glyphicon-remove awardDeleteBtn'></button>";
                    str.Add(job);
                    str.Add(editLink);

                    data.Add(str);
                }
                return Json(new
                {
                    data = data
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return Json(new
                {
                    data = new List<object>()
                });
            }
        }
        [HttpPost]
        [Authorize(Roles = "Job seekers")]
        public ActionResult GetCertificateDataTable()
        {
            try
            {
                if (!Request.IsAjaxRequest())
                {
                    return Json(new Response(false, "Invalid Ajax Request"));
                }
                //var aspnetUserId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                IList<JobSeekerCertificateDto> jobSeekerSkillDto =
                    _jobseekerService.LoadJobSeekerCertificateDtoByAspNetUserId(_aspnetUserId);
                var data = new List<object>();
                foreach (var dto in jobSeekerSkillDto)
                {
                    var str = new List<string>();
                    var job = "<span style='font-weight: normal; font-size:small'> &nbsp;&nbsp;" + dto.Title +
                              "</span><br />" +
                              "<span style='font-weight: normal; font-size:small'> &nbsp;&nbsp;" + dto.StartDate.ToString("M") + " to " + dto.CloseDate.ToString("M") +
                              "</span><br />" +
                              "<span style='font-weight: normal; font-size:small'> &nbsp;&nbsp;" + dto.Description +
                              "</span>";
                    var editLink = "<button data-val-id='" + dto.Id +
                            "' class='btn btn-default btn-sm glyphicon glyphicon-edit certificateEditBtn'></button> &nbsp;&nbsp;" +
                                "<button data-val-id='" + dto.Id +
                            "' class='btn btn-default btn-sm glyphicon glyphicon-remove certificateDeleteBtn'></button>";
                    str.Add(job);
                    str.Add(editLink);

                    data.Add(str);
                }
                return Json(new
                {
                    data = data
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return Json(new
                {
                    data = new List<object>()
                });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Job seekers")]
        public ActionResult GetGroupDataTable()
        {
            try
            {
                if (!Request.IsAjaxRequest())
                {
                    return Json(new Response(false, "Invalid Ajax Request"));
                }
                // var aspnetUserId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                IList<JobSeekerGroupDto> jobSeekerSkillDto =
                    _jobseekerService.LoadJobSeekerGroupDtoByAspNetUserId(_aspnetUserId);
                var data = new List<object>();
                foreach (var dto in jobSeekerSkillDto)
                {
                    var str = new List<string>();
                    var job = "<span style='font-weight: normal; font-size:small'> &nbsp;&nbsp;" + dto.Title +
                              "</span><br />" +
                              "<span style='font-weight: normal; font-size:small'> &nbsp;&nbsp;" +
                              dto.DateFrom.ToString("Y") + " to " + "</span>" +
                              "<span style='font-weight: normal; font-size:small'> &nbsp;";
                    if (dto.DateTo != null && dto.DateTo != DateTime.MinValue)
                    {
                        job += dto.DateTo.Value.ToString("Y");
                    }
                    else
                    {
                        job += " Current ";
                    }
                    job += "</span> <br />" + "<span style='font-weight: normal; font-size:small'> &nbsp;&nbsp;" + dto.Description + "</span>";
                    var editLink = "<button data-val-id='" + dto.Id +
                            "' class='btn btn-default btn-sm glyphicon glyphicon-edit groupEditBtn'></button> &nbsp;&nbsp;" +
                                "<button data-val-id='" + dto.Id +
                            "' class='btn btn-default btn-sm glyphicon glyphicon-remove groupDeleteBtn'></button>";
                    str.Add(job);
                    str.Add(editLink);

                    data.Add(str);
                }
                return Json(new
                {
                    data = data
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return Json(new
                {
                    data = new List<object>()
                });
            }
        }
        [HttpPost]
        [Authorize(Roles = "Job seekers")]
        public ActionResult GetPatentDataTable()
        {
            try
            {
                if (!Request.IsAjaxRequest())
                {
                    return Json(new Response(false, "Invalid Ajax Request"));
                }
                //var aspnetUserId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                IList<JobSeekerPatentsDto> jobSeekerSkillDto = _jobseekerService.LoadJobSeekerPatentsDtoByAspNetUserId(_aspnetUserId);
                var data = new List<object>();
                foreach (var dto in jobSeekerSkillDto)
                {
                    var str = new List<string>();
                    var job = "<span style='font-weight: normal; font-size:small'> &nbsp;&nbsp;" + dto.PatentNo + "</span><br />" +
                              "<span style='font-weight: normal; font-size:small'> &nbsp;&nbsp;" + dto.Title + "</span><br />" +
                              "<span style='font-weight: normal; font-size:small'> &nbsp;&nbsp;" + dto.Url + "</span><br />" +
                              "<span style='font-weight: normal; font-size:small'> &nbsp;&nbsp;" + dto.PublishDate.ToString("M") + "</span><br />" +
                              "<span style='font-weight: normal; font-size:small'> &nbsp;&nbsp;" + dto.Description + "</span>";
                    var editLink = "<button data-val-id='" + dto.Id +
                            "' class='btn btn-default btn-sm glyphicon glyphicon-edit patentEditBtn'></button> &nbsp;&nbsp;" +
                                "<button data-val-id='" + dto.Id +
                            "' class='btn btn-default btn-sm glyphicon glyphicon-remove patentDeleteBtn'></button>";
                    str.Add(job);
                    str.Add(editLink);

                    data.Add(str);
                }
                return Json(new
                {
                    data = data
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return Json(new
                {
                    data = new List<object>()
                });
            }
        }
        [HttpPost]
        [Authorize(Roles = "Job seekers")]
        public ActionResult GetPublicationsDataTable()
        {
            try
            {
                if (!Request.IsAjaxRequest())
                {
                    return Json(new Response(false, "Invalid Ajax Request"));
                }
                // var aspnetUserId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                IList<JobSeekerPublicationsDto> jobSeekerSkillDto =
                    _jobseekerService.LoadJobSeekerPublicationsDtoByAspNetUserId(_aspnetUserId);
                var data = new List<object>();
                foreach (var dto in jobSeekerSkillDto)
                {
                    var str = new List<string>();
                    var job = "<span style='font-weight: normal; font-size:small'> &nbsp;&nbsp;" + dto.Title + "</span><br />" +
                              "<span style='font-weight: normal; font-size:small'> &nbsp;&nbsp;" + dto.Url + "</span><br />" +
                              "<span style='font-weight: normal; font-size:small'> &nbsp;&nbsp;" + dto.PublishDate.ToString("yyyy MMMM dd") + "</span><br />" +
                              "<span style='font-weight: normal; font-size:small'> &nbsp;&nbsp;" + dto.Description + "</span>";
                    var editLink = "<button data-val-id='" + dto.Id +
                            "' class='btn btn-default btn-sm glyphicon glyphicon-edit publicationEditBtn'></button> &nbsp;&nbsp;" +
                                "<button data-val-id='" + dto.Id +
                            "' class='btn btn-default btn-sm glyphicon glyphicon-remove publicationDeleteBtn'></button>";
                    str.Add(job);
                    str.Add(editLink);

                    data.Add(str);
                }
                return Json(new
                {
                    data = data
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return Json(new
                {
                    data = new List<object>()
                });
            }
        }
        [HttpPost]
        [Authorize(Roles = "Job seekers")]
        public ActionResult GetAdditionalInformationDataTable()
        {
            try
            {
                if (!Request.IsAjaxRequest())
                {
                    return Json(new Response(false, "Invalid Ajax Request"));
                }
                // var aspnetUserId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                IList<JobSeekerAdditionalInformationDto> jobSeekerSkillDto =
                    _jobseekerService.LoadAditionalAdditionalInformationDtosByAspNetUserId(_aspnetUserId);
                var data = new List<object>();
                foreach (var dto in jobSeekerSkillDto)
                {
                    var str = new List<string>();
                    var job = "<span style='font-weight: normal; font-size:small'> &nbsp;&nbsp;" + dto.Description + "</span><br />";
                    var editLink = "<button data-val-id='" + dto.Id +
                            "' class='btn btn-default btn-sm glyphicon glyphicon-edit aiEditBtn'></button> &nbsp;&nbsp;" +
                                "<button data-val-id='" + dto.Id +
                            "' class='btn btn-default btn-sm glyphicon glyphicon-remove aiDeleteBtn'></button>";
                    str.Add(job);
                    str.Add(editLink);

                    data.Add(str);
                }
                return Json(new
                {
                    data = data
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return Json(new
                {
                    data = new List<object>()
                });
            }
        }
        [HttpPost]
        [Authorize(Roles = "Job seekers")]
        public ActionResult GetSkillDataTable()
        {
            try
            {
                if (!Request.IsAjaxRequest())
                {
                    return Json(new Response(false, "Invalid Ajax Request"));
                }
                //var aspnetUserId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
                IList<JobSeekerSkillDto> jobSeekerSkillDto =
                    _jobseekerService.LoadJobSeekerSkillDtoByAspNetUserId(_aspnetUserId);
                var data = new List<object>();
                foreach (var dto in jobSeekerSkillDto)
                {
                    var str = new List<string>();
                    var job = "<span style='font-weight: normal; font-size:small'> &nbsp;&nbsp;" + dto.Skill + " ( " +
                              dto.Experence + " )" + " </span><br />";
                    var editLink = "<button data-val-id='" + dto.Id +
                            "' class='btn btn-default btn-sm glyphicon glyphicon-edit skillEditBtn'></button> &nbsp;&nbsp;" +
                                "<button data-val-id='" + dto.Id +
                            "' class='btn btn-default btn-sm glyphicon glyphicon-remove skillDeleteBtn'></button>";
                    str.Add(job);
                    str.Add(editLink);

                    data.Add(str);
                }
                return Json(new
                {
                    data = data
                });
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return Json(new
                {
                    data = new List<object>()
                });
            }
        }

        #endregion

        #region Others

        [Authorize(Roles = "Job seekers")]
        public ActionResult JobSeekerProfile(string message = "")
        {
            //var aspnetUserId = System.Web.HttpContext.Current.User.Identity.GetUserId().ToString();
            JobSeekerDto jobSeekerProfileDto = _jobseekerService.GetJobSeekerByAspId(_aspnetUserId, 1);
            if (jobSeekerProfileDto == null || jobSeekerProfileDto.JobSeekerEducationList == null || !jobSeekerProfileDto.JobSeekerEducationList.Any())
            {
                return View("ProfileDefault");
            }
            else
            {
                if (jobSeekerProfileDto.JobSeekerDesiredJobList != null && jobSeekerProfileDto.JobSeekerDesiredJobList.Any())
                    ViewBag.IsShow = "Hide";
                if (jobSeekerProfileDto.JobSeekerAdditionalInformationList != null && jobSeekerProfileDto.JobSeekerAdditionalInformationList.Any())
                    ViewBag.IsAddShow = "Hide";
                if (!String.IsNullOrEmpty(message))
                {
                    ViewBag.ErrorMessage = message;
                }
                ViewBag.JobSeeker = jobSeekerProfileDto;
                return View("UpdateResume");
            }
        }

        private string GetDesiredJob(bool isCommission, bool isContract, bool isInternship, bool isTemporary,
            bool isPartTime, bool isFulllTime)
        {
            var str = new StringBuilder();
            if (isCommission) str.Append("Commission,");
            if (isContract) str.Append(" Contract,");
            if (isInternship) str.Append(" Internship,");
            if (isTemporary) str.Append(" Temporary,");
            if (isPartTime) str.Append(" PartTime,");
            if (isFulllTime) str.Append(" FulllTime,");

            return str.Remove(str.Length - 1, 1).ToString();
        }

        private string GetEligibolity(int eligibilty)
        {
            var str = new StringBuilder();
            if (eligibilty == 1) return "Authorized to work in the US for any employer";
            return "Sponsorship required to work in the US";

        }
        #endregion

        #region Company Search

        #region Company List
        [Authorize(Roles = "Job seekers")]
        public ActionResult CompanyList()
        {
            List<Model.JobLanes.Dto.RegionDto> regionList = new List<Model.JobLanes.Dto.RegionDto>();
            List<CountryDto> countryList = new List<CountryDto>();
            List<StateDto> stateList = new List<StateDto>();
            List<CityDto> cityList = new List<CityDto>();
            List<Model.JobLanes.Dto.CompanyTypeDto> companyTypeList = new List<Model.JobLanes.Dto.CompanyTypeDto>();
            try
            {
                regionList = _regionSevice.LoadRegion(EntityStatus.Active).ToList();
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
        [Authorize(Roles = "Job seekers")]
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
                                orderBy = "ct.Name";
                                break;
                            case "2":
                            case "3":
                                orderBy = "Name";
                                break;
                            case "4":
                            case "5":
                                orderBy = "cd.Zip";
                                break;
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

                        str.Add("<a href='" + Url.Action("CompanyDetails", "JobSeeker") + "?id=" + d.Id.ToString() +
                            "' class='glyphicon glyphicon-eye-open'> </a>&nbsp;&nbsp;<a href='" + Url.Action("DownloadCompanyProfile", "JobSeeker") + "?companyId=" + d.Id.ToString() +
                            "' class='glyphicon glyphicon-download'> </a>");
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

        #region Company Details

        [Authorize(Roles = "Job seekers")]
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

        [Authorize(Roles = "Job seekers")]
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
        private string GetJobDescription(string description)
        {
            if (String.IsNullOrEmpty(description)) return "";
            var len = description.Length;
            if (len < 200) return description;
            return description.Substring(1, 199);
        }
        #endregion
    }
}