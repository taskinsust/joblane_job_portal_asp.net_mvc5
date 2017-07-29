using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Dao.Joblanes;
using Microsoft;
using Microsoft.AspNet.Identity;
using Model.JobLanes.Dto;
using Model.JobLanes.Entity;
using Model.JobLanes.Entity.Base;
using Model.JobLanes.Entity.User;
using Model.JobLanes.ViewModel;
using NHibernate;
using NHibernate.Util;
using Services.Joblanes.Base;
using Services.Joblanes.Exceptions;
using Services.Joblanes.Helper;

namespace Services.Joblanes
{
    public interface ICompanyService : IBaseService
    {
        #region Operational Function
        void Save(CompanyViewModel companyViewModel);  
        #endregion

        #region Single Instances Loading Function
        CompanyDto GetCompany(string aspNetUserId);
        CompanyDto GetCompanyById(long companyId, int status);
        #endregion

        #region List of Loading function
        List<CompanyDto> LoadCompany(int? status = null, long? companyType = null, string zip = null, long? region = null, long? country = null, long? state = null, long? city = null);
        List<CompanyDto> LoadOnlyCompanyByStatusAndType(int? status = null, long? companyType = null);

        List<CompanyDto> LoadCompanyForWebAdmin(int start, int length, string orderBy, string orderDir,
            int? status = null, long? companyType = null, string companyName = "",  string contactMobile = "", string contactEmail = "",
            string zip = "", long? region = null, long? country = null, long? state = null, long? city = null
            );      
        #endregion

        #region Other Function
        int CompanyRowCount(int? status = null, long? companyType = null, string companyName = "", string contactMobile = "", string contactEmail = "",
            string zip = "", long? region = null, long? country = null, long? state = null, long? city = null);  
        #endregion
    }
    public class CompanyService : BaseService, ICompanyService
    {
        #region Propertise & Object Initialization

        private readonly ICompanyDao _companyDao;      
        private readonly ICityDao _cityDao;
        private readonly IRegionDao _regionDao;
        private readonly IStateDao _stateDao;
        private readonly ICountryDao _countryDao;
        private readonly IUserDao _userDao;
        private readonly ICompanyTypeDao _companyTypeDao;
        public CompanyService(ISession session)
        {
            Session = session;
            _companyDao = new CompanyDao() { Session = session };
            _cityDao = new CityDao() { Session = session };
            _regionDao = new RegionDao() { Session = session };
            _stateDao = new StateDao() { Session = session };
            _countryDao = new CountryDao() { Session = session };
            _userDao = new UserDao() { Session = session };
            _companyTypeDao = new CompanyTypeDao() { Session = session };
            _userDao = new UserDao() { Session = session };
        }
        #endregion

        #region Operational Function
        public  void Save(CompanyViewModel companyViewModel)
        {
            ITransaction transaction = null;
            try
            {
                if (companyViewModel == null)
                {
                    throw new NullObjectException("Company can not be null");
                }

                var profile = new Company();
                profile.UserProfile = _userDao.GetByAspNetUserId(companyViewModel.UserProfileId);
                Company company = _companyDao.GetCompany(profile.UserProfile.Id);
                IList<CompanyDetails> companyDetailses = new List<CompanyDetails>();
                CompanyDetails companyDetails = new CompanyDetails();

                if (company != null)
                {
                    profile = company;
                    if (profile.CompanyDetailses.Any())
                    {
                        companyDetails = profile.CompanyDetailses.FirstOrDefault();
                    }
                    profile.ModifyBy = companyViewModel.CurrentUserProfileId <= 0 ? profile.ModifyBy : companyViewModel.CurrentUserProfileId; 
                }
                else
                {
                    profile.CreateBy = companyViewModel.CurrentUserProfileId;
                    profile.ModifyBy = companyViewModel.CurrentUserProfileId;
                }

                //Region region = _regionDao.LoadById(companyViewModel.Region); 

                CompanyType companyType = _companyTypeDao.LoadById(companyViewModel.CompanyType);
                Country country = _countryDao.LoadById(companyViewModel.Country);
                State state = _stateDao.LoadById((long) companyViewModel.State);
                City city = _cityDao.LoadById(companyViewModel.City);

                profile.Name = companyViewModel.CompanyName;
                profile.ContactPerson = companyViewModel.ContactPerson;
                profile.ContactPersonDesignation = companyViewModel.ContactPersonDesignation;
                profile.ContactEmail = companyViewModel.ContactEmail;
                profile.ContactMobile = companyViewModel.ContactMobile;
                profile.Logo = companyViewModel.LogoBytes;

                profile.CompanyType = companyType;

                if (companyDetails != null)
                {
                    companyDetails.TradeLicence = companyViewModel.TradeLicence;
                    companyDetails.WebLink = companyViewModel.WebLink;
                    companyDetails.LinkdinLink = companyViewModel.LinkdinLink;
                    companyDetails.Vision = companyViewModel.Vision;
                    companyDetails.Mission = companyViewModel.Mission;
                    companyDetails.Description = companyViewModel.Description;
                    companyDetails.Address = companyViewModel.Address;
                    companyDetails.Zip = companyViewModel.Zip;
                    companyDetails.TagLine = companyViewModel.TagLine;
                    companyDetails.EmployeeSize = companyViewModel.EmployeeSize;
                    companyDetails.EstablishedDate = companyViewModel.EstablishedDate;
                    companyDetails.Country = country;
                    companyDetails.State = state;
                    companyDetails.City = city;

                    companyDetails.Status = CompanyDetails.EntityStatus.Active;
                    if (companyDetails.Id>0)
                    {
                        companyDetails.ModificationDate = DateTime.Now;
                    }
                    else
                    {
                        companyDetails.CreationDate = DateTime.Now;
                        companyDetails.ModificationDate = DateTime.Now;
                    }
                    companyDetails.CreateBy = companyDetails.CreateBy != 0 ? companyDetails.CreateBy : Convert.ToInt64(HttpContext.Current.User.Identity.GetUserId());

                    companyDetails.ModifyBy = companyDetails.CreateBy;

                    companyDetails.Company = profile;
                }
                companyDetailses.Add(companyDetails);
                profile.CompanyDetailses.Clear();
                profile.CompanyDetailses.Add(companyDetails); 

                CompanyModelValidationCheck(profile);
                CheckDuplicateFields(profile);

                using (transaction = Session.BeginTransaction())
                {
                    if (profile.Id <= 0)
                        _companyDao.Save(profile);
                    else
                    {
                        _companyDao.Update(profile);
                    }
                        
                    transaction.Commit();
                }
            }
            catch (NullObjectException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (transaction != null && transaction.IsActive)
                    transaction.Rollback();
            }
        }
        #endregion

        #region Single Instances Loading Function

        public CompanyDto GetCompany(string aspNetUserId)
        {
            try
            {
                var companyDto = new CompanyDto();
                UserProfile userProfile = _userDao.GetByAspNetUserId(aspNetUserId);
                Company company = _companyDao.GetCompany(userProfile.Id);
                if (company != null)
                {
                    companyDto.Id = company.Id;
                    companyDto.Name = company.Name;
                    companyDto.ContactPerson = company.ContactPerson;
                    companyDto.Logo = company.Logo;
                    companyDto.ContactPersonDesignation = company.ContactPersonDesignation;
                    companyDto.ContactMobile = company.ContactMobile;
                    companyDto.ContactEmail = company.ContactEmail;
                    companyDto.CompanyType = company.CompanyType == null
                            ? null
                            :  new CompanyTypeDto()
                    {
                        Id = company.CompanyType.Id,
                        Name = company.CompanyType.Name
                    };
                    companyDto.UserProfile = new UserProfileDto()
                    {
                        NickName = company.UserProfile.NickName,
                        AspNetUserId = company.UserProfile.AspNetUserId
                    };
                    companyDto.CompanyDetailList = company.CompanyDetailses.Select(r => new CompanyDetailsDto()
                    {
                        Id = r.Id,

                        StatusText = StatusTypeText.GetStatusText(r.Status),
                        TradeLicence = r.TradeLicence,
                        WebLink = r.WebLink,
                        LinkdinLink = r.LinkdinLink,
                        Vision = r.Vision,
                        Mission = r.Mission,
                        Description = r.Description,
                        Address = r.Address,
                        Zip = r.Zip,
                        TagLine = r.TagLine,
                        Country = r.Country == null
                            ? null
                            : new CountryDto()
                        {
                            Id = r.Country.Id,
                            Name = r.Country.Name,
                            Region = new RegionDto()
                            {
                                Id = r.Country.Region.Id,
                                Name = r.Country.Region.Name,
                            }
                        },
                        State = r.State == null
                            ? null
                            : new StateDto()
                            {
                                Id = r.State.Id,
                                Name = r.State.Name,
                            },
                        City = r.City == null
                            ? null
                            :  new CityDto()
                        {
                            Id = r.City.Id,
                            Name = r.City.Name,
                        },
                        EstablishedDate = r.EstablishedDate,
                        EmployeeSize = r.EmployeeSize,

                    }).ToList();

                }
                else
                {
                    companyDto = null;
                }
                return companyDto;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public CompanyDto GetCompanyById(long companyId, int status)
        {
            try
            {
                var companyDto = new CompanyDto();
               
                Company company = _companyDao.GetCompany(companyId,status);
                if (company != null)
                {
                    companyDto.Id = company.Id;
                    companyDto.Name = company.Name;
                    companyDto.ContactPerson = company.ContactPerson;
                    companyDto.Logo = company.Logo;
                    companyDto.ContactPersonDesignation = company.ContactPersonDesignation;
                    companyDto.ContactMobile = company.ContactMobile;
                    companyDto.ContactEmail = company.ContactEmail;
                    companyDto.CompanyType = company.CompanyType == null
                            ? null
                            : new CompanyTypeDto()
                            {
                                Id = company.CompanyType.Id,
                                Name = company.CompanyType.Name
                            };
                    companyDto.UserProfile = new UserProfileDto()
                    {
                        NickName = company.UserProfile.NickName,
                        AspNetUserId = company.UserProfile.AspNetUserId
                    };
                    companyDto.CompanyDetailList = company.CompanyDetailses.Select(r => new CompanyDetailsDto()
                    {
                        Id = r.Id,

                        StatusText = StatusTypeText.GetStatusText(r.Status),
                        TradeLicence = r.TradeLicence,
                        WebLink = r.WebLink,
                        LinkdinLink = r.LinkdinLink,
                        Vision = r.Vision,
                        Mission = r.Mission,
                        Description = r.Description,
                        Address = r.Address,
                        Zip = r.Zip,
                        TagLine = r.TagLine,
                        Country = r.Country == null
                            ? null
                            : new CountryDto()
                            {
                                Id = r.Country.Id,
                                Name = r.Country.Name,
                                Region = new RegionDto()
                                {
                                    Id = r.Country.Region.Id,
                                    Name = r.Country.Region.Name,
                                }
                            },
                        State = r.State == null
                            ? null
                            : new StateDto()
                            {
                                Id = r.State.Id,
                                Name = r.State.Name,
                            },
                        City = r.City == null
                            ? null
                            : new CityDto()
                            {
                                Id = r.City.Id,
                                Name = r.City.Name,
                            },
                        EstablishedDate = r.EstablishedDate,
                        EmployeeSize = r.EmployeeSize,

                    }).ToList();

                }
                else
                {
                    companyDto = null;
                }
                return companyDto;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region List of Loading function
        public List<CompanyDto> LoadCompany(int? status = null, long? companyType = null, string zip = null, long? region = null,
    long? country = null, long? state = null, long? city = null)
        {
            try
            {
                var companyList = _companyDao.LoadCompany(status ?? 0, companyType ?? 0, zip, region ?? 0, country ?? 0, state ?? 0, city ?? 0);

                return companyList.Select(c => new CompanyDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    StatusText = StatusTypeText.GetStatusText(c.Status),
                    ContactPerson = c.ContactPerson,
                    Logo = c.Logo,
                    ContactPersonDesignation = c.ContactPersonDesignation,
                    ContactMobile = c.ContactMobile,
                    ContactEmail = c.ContactEmail,
                    CompanyType = c.CompanyType == null
                            ? null
                            : new CompanyTypeDto()
                            {
                                Id = c.CompanyType.Id,
                                Name = c.CompanyType.Name
                            },
                    CompanyDetailList = c.CompanyDetailses.Select(r => new CompanyDetailsDto()
                    {
                        Id = r.Id,

                        StatusText = StatusTypeText.GetStatusText(r.Status),
                        TradeLicence = r.TradeLicence,
                        WebLink = r.WebLink,
                        LinkdinLink = r.LinkdinLink,
                        Vision = r.Vision,
                        Mission = r.Mission,
                        Description = r.Description,
                        Address = r.Address,
                        Zip = r.Zip,
                        TagLine = r.TagLine,
                        Country = r.Country == null
                            ? null
                            : new CountryDto()
                            {
                                Id = r.Country.Id,
                                Name = r.Country.Name,
                                Region = new RegionDto()
                                {
                                    Id = r.Country.Region.Id,
                                    Name = r.Country.Region.Name,
                                }
                            },
                        State = r.State == null
                            ? null
                            : new StateDto()
                            {
                                Id = r.State.Id,
                                Name = r.State.Name,
                            },
                        City = r.City == null
                            ? null
                            : new CityDto()
                            {
                                Id = r.City.Id,
                                Name = r.City.Name,
                            },
                        EstablishedDate = r.EstablishedDate,
                        EmployeeSize = r.EmployeeSize,

                    }).ToList(),
                }).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<CompanyDto> LoadOnlyCompanyByStatusAndType(int? status = null, long? companyType = null)
        {
            try
            {
                var companyList = _companyDao.LoadOnlyCompanyByStatusAndType(status ?? 0, companyType ?? 0);
                return companyList.Select(c => new CompanyDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    StatusText = StatusTypeText.GetStatusText(c.Status),
                    ContactPerson = c.ContactPerson,
                    ContactPersonDesignation = c.ContactPersonDesignation,
                    ContactMobile = c.ContactMobile,
                    ContactEmail = c.ContactEmail,
                    Logo = c.Logo,
                    CompanyType = c.CompanyType == null
                            ? null
                            : new CompanyTypeDto()
                            {
                                Id = c.CompanyType.Id,
                                Name = c.CompanyType.Name
                            },
                }).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<CompanyDto> LoadCompanyForWebAdmin(int start, int length, string orderBy, string orderDir, int? status = null,
            long? companyType = null, string companyName = "", string contactMobile = "", string contactEmail = "",
            string zip = "", long? region = null, long? country = null, long? state = null, long? city = null)
        {
            try
            {
                var companyList = _companyDao.LoadCompanyForWebAdmin(start, length, orderBy, orderDir,
                    status ?? 0, companyType ?? 0, companyName, contactMobile, contactEmail, zip,
                    region ?? 0, country ?? 0, state ?? 0, city ?? 0
                    );
                return companyList.Select(c => new CompanyDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    StatusText = StatusTypeText.GetStatusText(c.Status),
                    ContactPerson = c.ContactPerson,
                    ContactPersonDesignation = c.ContactPersonDesignation,
                    ContactMobile = c.ContactMobile,
                    ContactEmail = c.ContactEmail,
                    Logo = c.Logo,
                    UserProfile = c.UserProfile == null ? null : new UserProfileDto()
                    {
                        AspNetUserId = c.UserProfile.AspNetUserId,
                        IsBlock = c.UserProfile.IsBlock
                    },
                    CompanyType = c.CompanyType == null
                            ? null
                            : new CompanyTypeDto()
                            {
                                Id = c.CompanyType.Id,
                                Name = c.CompanyType.Name
                            },
                    CompanyDetailList = c.CompanyDetailses.Select(r => new CompanyDetailsDto()
                    {
                        Id = r.Id,
                        WebLink = r.WebLink,
                        Zip = r.Zip,
                        Country = r.Country == null
                            ? null
                            : new CountryDto()
                            {
                                Id = r.Country.Id,
                                Name = r.Country.Name,
                                Region = new RegionDto()
                                {
                                    Id = r.Country.Region.Id,
                                    Name = r.Country.Region.Name,
                                }
                            },
                        State = r.State == null
                            ? null
                            : new StateDto()
                            {
                                Id = r.State.Id,
                                Name = r.State.Name,
                            },
                        City = r.City == null
                            ? null
                            : new CityDto()
                            {
                                Id = r.City.Id,
                                Name = r.City.Name,
                            },
                        EstablishedDate = r.EstablishedDate,
                        EmployeeSize = r.EmployeeSize,
                    }).ToList(),
                }).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }
        #endregion

        #region Other Function
        public int CompanyRowCount(int? status = null, long? companyType = null, string companyName = "", string contactMobile = "",
            string contactEmail = "", string zip = "", long? region = null, long? country = null, long? state = null,
            long? city = null)
        {
            try
            {
                return _companyDao.CompanyRowCount(status ?? 0, companyType ?? 0, companyName, contactMobile, contactEmail, zip,
                    region ?? 0, country ?? 0, state ?? 0, city ?? 0
                    );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Helper
        private void CompanyModelValidationCheck(Company profile) 
        {
            try
            {
                var validationResult = ValidationHelper.ValidateEntity<Company, Company>(profile); 
                if (validationResult.HasError)
                {
                    string errorMessage = "";
                    validationResult.Errors.ForEach(r => errorMessage = errorMessage + r.ErrorMessage + Environment.NewLine);
                    throw new InvalidDataException(errorMessage);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void CompanyDetailsModelValidationCheck(CompanyDetails companyDetails)
        {
            try
            {
                var validationResult = ValidationHelper.ValidateEntity<CompanyDetails, CompanyDetails>(companyDetails);
                if (validationResult.HasError)
                {
                    string errorMessage = "";
                    validationResult.Errors.ForEach(r => errorMessage = errorMessage + r.ErrorMessage + Environment.NewLine);
                    throw new InvalidDataException(errorMessage);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void CheckDuplicateFields(Company company)
        {
            try
            {
                string fieldName;
                var isDuplicateContactEmail = _companyDao.IsDuplicateCompany(out fieldName, company.ContactEmail, null, company.Id);
                if (isDuplicateContactEmail)
                {
                    if (fieldName == "ContactEmail")
                        throw new DuplicateEntryException("Duplicate Contact Eamil found");
                }

                var isDuplicateContactMobile = _companyDao.IsDuplicateCompany(out fieldName, company.ContactMobile, null, company.Id);
                if (isDuplicateContactMobile)
                {
                    if (fieldName == "ContactMobile")
                        throw new DuplicateEntryException("Duplicate Contact Mobile found");
                }

                //var isDuplicateContactEmail = _companyDao.isDuplicateCompany(contactEmail: company.ContactEmail);
                //if (isDuplicateContactEmail)
                //    throw new DuplicateEntryException("Contact Email Name can not be duplicate");

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
    }
}
