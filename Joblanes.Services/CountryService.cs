using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes;
using Model.JobLanes.Dto;
using Model.JobLanes.Entity;
using Model.JobLanes.Entity.Base;
using Model.JobLanes.ViewModel;
using NHibernate;
using NHibernate.Util;
using Services.Joblanes.Base;
using Services.Joblanes.Exceptions;
using Services.Joblanes.Helper;

namespace Services.Joblanes
{
    public interface ICountryService : IBaseService
    {
        #region Operational Function
        void Save(CountryViewModel countryVm); 
        void Delete(long id);

        #endregion

        #region Single Instances Loading Function
        CountryDto GetById(long id);
        #endregion

        #region List of Loading function
        List<CountryDto> LoadCountry(int start, int length, string orderBy, string orderDir, string name, string shortName, string callingCode, long region, int status);
        List<CountryDto> LoadCountry(int? status = null, long? region = null); 
        #endregion

        #region Other Function
        int CountryRowCount(string name, string shortName, string callingCode, long region, int status); 
        #endregion
    }
    public class CountryService : BaseService, ICountryService
    {
        #region Propertise & Object Initialization

        private readonly ICountryDao _countryDao;
        private readonly IRegionDao _regionDao;
        private readonly IUserDao _userDao;
        public CountryService(ISession session)
        {
            Session = session;
            _countryDao = new CountryDao() { Session = session };
            _regionDao = new RegionDao() {Session = session};
            _userDao = new UserDao() { Session = session };
        }
        #endregion

        #region Operational Function
        public void Save(CountryViewModel countryVm)
        {
            ITransaction transaction = null;
            try
            {
                if (countryVm == null)
                {
                    throw new NullObjectException("Country can not be null");
                }
                Region region = _regionDao.LoadById(countryVm.Region);
                //profile.ImageGuid = Guid.NewGuid();
                //profile.ProfileImage = jobSeekerProfileVm.ProfileImageBytes;
                var country = new Country()
                {
                    Id = countryVm.Id,
                    Name = countryVm.Name,
                    Status = countryVm.Status,
                    ShortName = countryVm.ShortName,
                    CallingCode = countryVm.CallingCode,
                    ImageGuid =  Guid.NewGuid(),
                    Flag = countryVm.FlagBytes,
                    Region = region
                };

                ModelValidationCheck(country);

                using (transaction = Session.BeginTransaction())
                {
                    if (country.Id < 1)
                    {
                        CheckDuplicateFields(countryVm);
                        country.CreateBy = countryVm.CurrentUserProfileId;
                        country.ModifyBy = countryVm.CurrentUserProfileId;
                        _countryDao.Save(country);
                    }
                    else
                    {
                        var oldCountry = _countryDao.LoadById(countryVm.Id);
                        if (oldCountry == null)
                        {
                            throw new NullObjectException("Country can not be null");
                        }
                        CheckDuplicateFields(countryVm, oldCountry.Id);
                        oldCountry.Status = countryVm.Status;
                        oldCountry.Name = countryVm.Name;
                        oldCountry.ShortName = countryVm.ShortName;
                        oldCountry.CallingCode = countryVm.CallingCode;
                        oldCountry.ImageGuid = Guid.NewGuid();
                        oldCountry.Flag = countryVm.FlagBytes;
                        oldCountry.Region = region;
                        oldCountry.ModifyBy = countryVm.CurrentUserProfileId <= 0 ? oldCountry.ModifyBy : countryVm.CurrentUserProfileId;
                        _countryDao.Update(oldCountry);

                    }

                    transaction.Commit();
                }
            }
            catch (NullObjectException ex)
            {
                throw;
            }
            catch (DuplicateEntryException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (transaction != null && transaction.IsActive)
                    transaction.Rollback();
            }

        }

        public void Delete(long id)
        {
            ITransaction trans = null;
            try
            {
                var obj = _countryDao.LoadById(id);
                if (obj == null)
                    throw new NullObjectException("Country is not valid");

                CheckBeforeDelete(obj);
                using (trans = Session.BeginTransaction())
                {
                    obj.Status = Country.EntityStatus.Delete;
                    _countryDao.Update(obj);
                    trans.Commit();
                }
            }
            catch (DependencyException) { throw; }

            catch (NullObjectException) { throw; }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (trans != null && trans.IsActive)
                    trans.Rollback();
            }
        }

        #endregion

        #region Single Instances Loading Function
        public CountryDto GetById(long id)
        {
            try
            {
                var country = _countryDao.LoadById(id);
                if (country != null)
                {
                    string crBy = country.CreateBy > 0 ? _userDao.GetUserNameByUserProfileId(country.CreateBy) : "";
                    string moBy = crBy;
                    if (country.CreateBy != country.ModifyBy)
                        moBy = country.ModifyBy > 0 ? _userDao.GetUserNameByUserProfileId(country.ModifyBy) : "";

                    return new CountryDto()
                    {
                        Id = country.Id,
                        Name = country.Name,
                        StatusText = StatusTypeText.GetStatusText(country.Status),
                        Status = country.Status,
                        ShortName = country.ShortName,
                        CallingCode = country.CallingCode,
                        FlagBytes = country.Flag,
                        CreateBy = crBy,
                        ModifyBy = moBy,
                        CreateDate = country.CreationDate.ToString("dd-MMMM-yyyy"),
                        ModifyDate = country.ModificationDate.ToString("dd-MMMM-yyyy"),
                        Region = new RegionDto()
                        {
                            Id = country.Region.Id,
                            Name = country.Region.Name,
                        }
                    };
                }
                else
                {
                    return new CountryDto();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region List of Loading function

        public List<CountryDto> LoadCountry(int start, int length, string orderBy, string orderDir, string name, string shortName, string callingCode, long region, int status)
        {
            try
            {
                var countries = _countryDao.LoadCountry(start, length, orderBy, orderDir, name,  shortName,  callingCode,  region,status);
                return countries.Select(r => new CountryDto
                {
                    Id = r.Id, 
                    Name = r.Name, 
                    StatusText = StatusTypeText.GetStatusText(r.Status),
                    ShortName = r.ShortName,
                    CallingCode = r.CallingCode,
                    FlagBytes = r.Flag,
                    Region = new RegionDto()
                    {
                        Id = r.Region.Id,
                        Name = r.Region.Name,
                    }
                }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CountryDto> LoadCountry(int? status = null, long? region = null) 
        {
            try
            {
                var countries = _countryDao.LoadCountry(status ?? 0, region ?? 0);

                return countries.Select(r => new CountryDto
                {
                    Id = r.Id, 
                    Name = r.Name, 
                    StatusText = StatusTypeText.GetStatusText(r.Status),
                    ShortName = r.ShortName,
                    CallingCode = r.CallingCode,
                    FlagBytes = r.Flag,
                    Region = new RegionDto()
                    {
                        Id = r.Region.Id,
                        Name = r.Region.Name,
                    }
                }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Other Function

        public int CountryRowCount(string name, string shortName, string callingCode, long region, int status) 
        {
            try
            {
                return _countryDao.CountryRowCount(name,  shortName,  callingCode,  region, status);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Helper Function
        private void CheckDuplicateFields(CountryViewModel countryVm, long id = 0)
        {
            try
            {
                var isDuplicateName = _countryDao.CheckDuplicateFields(id, countryVm.Name);
                if (isDuplicateName)
                    throw new DuplicateEntryException("Country Name can not be duplicate");

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void ModelValidationCheck(Country region)
        {
            try
            {
                var validationResult = ValidationHelper.ValidateEntity<Country, Country>(region);
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
        private void CheckBeforeDelete(Country country)
        {
            if (country.States.Count > 0)
                throw new DependencyException("You can't delete this Country, State is already declared here.");
            if (country.Cities.Count > 0)
                throw new DependencyException("You can't delete this Country, City is already declared here.");
        }
        #endregion

    }
}
