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
    public interface IStateService : IBaseService
    {
        #region Operational Function
        void Save(StateViewModel stateVm);
        void Delete(long id);

        #endregion

        #region Single Instances Loading Function
        StateDto GetById(long id);
        #endregion

        #region List of Loading function
        List<StateDto> LoadState(int start, int length, string orderBy, string orderDir, string name, string shortName, long region, long country, int status);
        List<StateDto> LoadState(long? region = null, long? country = null, int? status = null);
        #endregion

        #region Other Function
        int StateRowCount(string name, string shortName, long region, long country, int status);
        #endregion
    }
    public class StateService : BaseService, IStateService
    {
        #region Propertise & Object Initialization

        private readonly IStateDao _stateDao;
        private readonly ICountryDao _countryDao;
        private readonly IUserDao _userDao;
        public StateService(ISession session)
        {
            Session = session;
            _stateDao = new StateDao() { Session = session };
            _countryDao = new CountryDao() { Session = session };
            _userDao = new UserDao() { Session = session };
        }
        #endregion

        #region Operational Function
        public void Save(StateViewModel stateVm)
        {
            ITransaction transaction = null;
            try
            {
                if (stateVm == null)
                {
                    throw new NullObjectException("State can not be null");
                }
                Country country = _countryDao.LoadById(stateVm.Country); 
                var state = new State()
                {
                    Id = stateVm.Id,
                    Name = stateVm.Name,
                    Status = stateVm.Status,
                    ShortName = stateVm.ShortName,
                    Country = country,
                };

                ModelValidationCheck(state);

                using (transaction = Session.BeginTransaction())
                {
                    if (state.Id < 1)
                    {
                        CheckDuplicateFields(stateVm);
                        state.CreateBy = stateVm.CurrentUserProfileId;
                        state.ModifyBy = stateVm.CurrentUserProfileId;
                        _stateDao.Save(state);
                    }
                    else
                    {
                        var oldState = _stateDao.LoadById(stateVm.Id);
                        if (oldState == null)
                        {
                            throw new NullObjectException("State can not be null");
                        }
                        CheckDuplicateFields(stateVm, oldState.Id);
                        oldState.Status = stateVm.Status;
                        oldState.Name = stateVm.Name;
                        oldState.ShortName = stateVm.ShortName;
                        oldState.Country = country;
                        oldState.ModifyBy = stateVm.CurrentUserProfileId <= 0 ? oldState.ModifyBy : stateVm.CurrentUserProfileId;
                        _stateDao.Update(oldState);

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
                var obj = _stateDao.LoadById(id);
                if (obj == null)
                    throw new NullObjectException("State is not valid");

                CheckBeforeDelete(obj);
                using (trans = Session.BeginTransaction())
                {
                    obj.Status = State.EntityStatus.Delete;
                    _stateDao.Update(obj);
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
        public StateDto GetById(long id)
        {
            try
            {
                var state = _stateDao.LoadById(id);
                if (state != null)
                {
                    string crBy = state.CreateBy > 0 ? _userDao.GetUserNameByUserProfileId(state.CreateBy) : "";
                    string moBy = crBy;
                    if (state.CreateBy != state.ModifyBy)
                        moBy = state.ModifyBy > 0 ? _userDao.GetUserNameByUserProfileId(state.ModifyBy) : "";

                    return new StateDto()
                    {
                        Id = state.Id,
                        Name = state.Name,
                        StatusText = StatusTypeText.GetStatusText(state.Status),
                        Status = state.Status,
                        ShortName = state.ShortName,
                        CreateBy = crBy,
                        ModifyBy = moBy,
                        CreateDate = state.CreationDate.ToString("dd-MMMM-yyyy"),
                        ModifyDate = state.ModificationDate.ToString("dd-MMMM-yyyy"),
                        Country = new CountryDto()
                        {
                            Id = state.Country.Id,
                            Name = state.Country.Name,
                            Region = new RegionDto()
                            {
                                Id = state.Country.Region.Id,
                                Name = state.Country.Region.Name,
                            }
                        }
                    };
                }
                else
                {
                    return new StateDto();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region List of Loading function

        public List<StateDto> LoadState(int start, int length, string orderBy, string orderDir, string name, string shortName, long region, long country, int status)
        {
            try
            {
                var states = _stateDao.LoadState(start, length, orderBy, orderDir, name, shortName, region, country, status);

                return states.Select(r => new StateDto
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
                    }
                }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<StateDto> LoadState( long? region = null, long? country = null, int? status = null) 
        {
            try
            {
                var states = _stateDao.LoadState(region ?? 0,country ?? 0, status ?? 0);

                return states.Select(r => new StateDto
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

        public int StateRowCount(string name, string shortName, long region, long country, int status)
        {
            try
            {
                return _stateDao.StateRowCount(name,  shortName,  region,  country, status);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Helper Function
        private void CheckDuplicateFields(StateViewModel stateVm, long id = 0)
        {
            try
            {
                var isDuplicateName = _stateDao.CheckDuplicateFields(id, stateVm.Name);
                if (isDuplicateName)
                    throw new DuplicateEntryException("State Name can not be duplicate");

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void ModelValidationCheck(State region)
        {
            try
            {
                var validationResult = ValidationHelper.ValidateEntity<State, State>(region);
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
        private void CheckBeforeDelete(State state)
        {
             if (state.Cities.Count > 0)
                throw new DependencyException("You can't delete this State, City is already declared here.");
        }
        #endregion

    }
}
