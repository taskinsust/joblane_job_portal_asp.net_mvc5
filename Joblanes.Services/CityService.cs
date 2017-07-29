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
    public interface ICityService:IBaseService
    {
        #region Operational Function
        void Save(CityViewModel cityVm);
        void Delete(long id);

        #endregion

        #region Single Instances Loading Function
        CityDto GetById(long id);
        #endregion

        #region List of Loading function
        List<CityDto> LoadCity(int start, int length, string orderBy, string orderDir, string name, string shortName, long region, long country, long state, int status);
        List<CityDto> LoadCity(long? region = null, long? country = null, long? state = null, int? status = null); 
        #endregion

        #region Other Function
        int CityRowCount(string name,  string shortName, long region, long country, long state,int status);
        #endregion
    }
    public class CityService:BaseService,ICityService
    {
        #region Propertise & Object Initialization

        private readonly ICityDao _cityDao;
        private readonly ICountryDao _countryDao;
        private readonly IStateDao _stateDao;
        private readonly IUserDao _userDao;
        public CityService(ISession session)
        {
            Session = session;
            _cityDao = new CityDao(){Session = session};
            _countryDao = new CountryDao() { Session = session };
            _stateDao = new StateDao() { Session = session };
            _userDao = new UserDao() { Session = session };
        }

        #endregion

        #region Operational Function
        public void Save(CityViewModel cityVm)
        {
            ITransaction transaction = null;
            try
            {
                if (cityVm == null)
                {
                    throw new NullObjectException("City can not be null");
                }
                Country country = _countryDao.LoadById(cityVm.Country);
                State state = _stateDao.LoadById(cityVm.State);
                var city = new City()
                {
                    Id = cityVm.Id,
                    Name = cityVm.Name,
                    Status = cityVm.Status,
                    ShortName = cityVm.ShortName,
                    Country = country,
                    State = state,
                };

                ModelValidationCheck(city);

                using (transaction = Session.BeginTransaction())
                {
                    if (city.Id < 1)
                    {
                        CheckDuplicateFields(cityVm);
                        city.CreateBy = cityVm.CurrentUserProfileId;
                        city.ModifyBy = cityVm.CurrentUserProfileId;
                        _cityDao.Save(city);
                    }
                    else
                    {
                        var oldCity = _cityDao.LoadById(cityVm.Id);
                        if (oldCity == null)
                        {
                            throw new NullObjectException("City can not be null");
                        }
                        CheckDuplicateFields(cityVm, oldCity.Id);
                        oldCity.Status = cityVm.Status;
                        oldCity.Name = cityVm.Name;
                        oldCity.ShortName = cityVm.ShortName;
                        oldCity.Country = country;
                        oldCity.State = state;
                        oldCity.ModifyBy = cityVm.CurrentUserProfileId <= 0 ? oldCity.ModifyBy : cityVm.CurrentUserProfileId;
                        _cityDao.Update(oldCity);

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
                var obj = _cityDao.LoadById(id);
                if (obj == null)
                    throw new NullObjectException("City is not valid");

                CheckBeforeDelete(obj);
                using (trans = Session.BeginTransaction())
                {
                    obj.Status = City.EntityStatus.Delete;
                    _cityDao.Update(obj);
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
        public CityDto GetById(long id)
        {
            try
            {
                var city = _cityDao.LoadById(id);
                if (city != null)
                {
                    string crBy = city.CreateBy > 0 ? _userDao.GetUserNameByUserProfileId(city.CreateBy) : "";
                    string moBy = crBy;
                    if (city.CreateBy != city.ModifyBy)
                        moBy = city.ModifyBy > 0 ? _userDao.GetUserNameByUserProfileId(city.ModifyBy) : "";

                    return new CityDto()
                    {
                        Id = city.Id,
                        Name = city.Name,
                        StatusText = StatusTypeText.GetStatusText(city.Status),
                        Status = city.Status,
                        ShortName = city.ShortName,
                        CreateBy = crBy,
                        ModifyBy = moBy,
                        CreateDate = city.CreationDate.ToString("dd-MMMM-yyyy"),
                        ModifyDate = city.ModificationDate.ToString("dd-MMMM-yyyy"),
                        Country = new CountryDto()
                        {
                            Id = city.Country.Id,
                            Name = city.Country.Name,
                            Region = new RegionDto()
                            {
                                Id = city.Country.Region.Id,
                                Name = city.Country.Region.Name,
                            }
                        },
                        State = city.State == null ? null : new StateDto()
                        {
                            Id = city.State.Id,
                            Name = city.State.Name,
                        }
                    };
                }
                else
                {
                    return new CityDto();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region List of Loading function

        public List<CityDto> LoadCity(int start, int length, string orderBy, string orderDir, string name, string shortName, long region, long country, long state,int status)
        {
            try
            {
                var cities = _cityDao.LoadCity(start, length, orderBy, orderDir, name,  shortName, region, country, state,status);
                return cities.Select(r => new CityDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    StatusText = StatusTypeText.GetStatusText(r.Status),
                    ShortName = r.ShortName,
                    Country = new CountryDto()
                    {
                        Id = r.Country.Id,
                        Name = r.Country.Name,
                        Region = new RegionDto()
                        {
                            Id = r.Country.Region.Id,
                            Name = r.Country.Region.Name,
                        }
                    },
                    State = r.State==null?null:new StateDto()
                    {
                        Id = r.State.Id,
                        Name = r.State.Name,
                    }
                }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<CityDto> LoadCity(long? region = null, long? country = null, long? state = null, int? status = null)
        {
            try
            {
                var cities = _cityDao.LoadCity(region ?? 0, country ?? 0, state ?? 0, status ?? 0); 

                return cities.Select(r => new CityDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    StatusText = StatusTypeText.GetStatusText(r.Status),
                    ShortName = r.ShortName,
                    Country = new CountryDto()
                    {
                        Id = r.Country.Id,
                        Name = r.Country.Name,
                        Region = new RegionDto()
                        {
                            Id = r.Country.Region.Id,
                            Name = r.Country.Region.Name,
                        }
                    },
                    State = r.State == null ? null : new StateDto()
                    {
                        Id = r.State.Id,
                        Name = r.State.Name,
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

        public int CityRowCount(string name, string shortName, long region, long country, long state, int status)
        {
            try
            {
                return _cityDao.CityRowCount(name,  shortName,  region,  country,  state, status);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Helper Function
        private void CheckDuplicateFields(CityViewModel cityVm, long id = 0)
        {
            try
            {
                var isDuplicateName = _cityDao.CheckDuplicateFields(id, cityVm.Name);
                if (isDuplicateName)
                    throw new DuplicateEntryException("City Name can not be duplicate");

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void ModelValidationCheck(City region)
        {
            try
            {
                var validationResult = ValidationHelper.ValidateEntity<City, City>(region);
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
        private void CheckBeforeDelete(City city)
        {

            if (city.CompanyDetailses.Any())
                throw new DependencyException("You can't delete this City, City is already declared here.");
        }
        #endregion

    }
}
