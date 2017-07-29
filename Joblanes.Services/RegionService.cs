using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes;
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
    public interface IRegionService : IBaseService
    {
        #region Operational Function
        void Save(RegionViewModel regionVm);
        void Delete(long id);

        #endregion

        #region Single Instances Loading Function
        RegionDto GetById(long id);  
        #endregion

        #region List of Loading function
        List<RegionDto> LoadRegion(int start, int length, string orderBy, string orderDir, string name, int status);
        List<RegionDto> LoadRegion(int? status = null);  
        #endregion

        #region Other Function
        int RegionRowCount(string name, int status);
        #endregion

    }

    public class RegionService : BaseService, IRegionService
    {
        #region Propertise & Object Initialization
        private readonly IRegionDao _regionDao;
        private readonly IUserDao _userDao;
       
        public RegionService(ISession session)
        {
            Session = session;
            _regionDao = new RegionDao() { Session = session };
            _userDao = new UserDao(){Session = session};
        }
        #endregion

        #region Operational Function
        public void Save(RegionViewModel regionVm)
        {
            ITransaction transaction = null;
            try
            {
                if (regionVm == null)
                {
                    throw new NullObjectException("Region can not be null");
                }
                var region = new Region
                {
                    Id = regionVm.Id,
                    Name = regionVm.Name,
                    Status = regionVm.Status,
                    ModifyBy = regionVm.CurrentUserProfileId
                };

                ModelValidationCheck(region);

                using (transaction = Session.BeginTransaction())
                {
                    if (region.Id < 1)
                    {
                        CheckDuplicateFields(regionVm);
                        region.CreateBy = regionVm.CurrentUserProfileId;
                        _regionDao.Save(region);
                    }
                    else
                    {
                        var oldRegion = _regionDao.LoadById(regionVm.Id);
                        if (oldRegion==null)
                        {
                            throw new NullObjectException("Region can not be null");
                        }
                        CheckDuplicateFields(regionVm, oldRegion.Id);
                        oldRegion.Status = regionVm.Status;
                        oldRegion.Name = regionVm.Name;
                        oldRegion.ModifyBy = regionVm.CurrentUserProfileId <= 0 ? oldRegion.ModifyBy : regionVm.CurrentUserProfileId;
                        _regionDao.Update(oldRegion);

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
                var obj = _regionDao.LoadById(id);
                if (obj == null)
                    throw new NullObjectException("Region is not valid");

                CheckBeforeDelete(obj);
                using (trans = Session.BeginTransaction())
                {
                    obj.Status = Region.EntityStatus.Delete;
                    _regionDao.Update(obj);
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
        public RegionDto GetById(long id)
        {
            try
            {
                var region = _regionDao.LoadById(id);
                if (region != null)
                {
                    string crBy = region.CreateBy>0?_userDao.GetUserNameByUserProfileId(region.CreateBy):"";
                    string moBy = crBy;
                    if (region.CreateBy != region.ModifyBy)
                        moBy = region.ModifyBy > 0 ? _userDao.GetUserNameByUserProfileId(region.ModifyBy) : "";

                    return new RegionDto()
                    {
                        Id = region.Id, 
                        Name = region.Name, 
                        StatusText = StatusTypeText.GetStatusText(region.Status),
                        Status = region.Status,
                        CreateBy = crBy,
                        ModifyBy = moBy,
                        CreateDate = region.CreationDate.ToString("dd-MMMM-yyyy"),
                        ModifyDate = region.ModificationDate.ToString("dd-MMMM-yyyy")
                    };
                }
                else
                {
                    return new RegionDto();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region List of Loading function

        public List<RegionDto> LoadRegion(int start, int length, string orderBy, string orderDir, string name, int status)
        {
            try
            {
                var regions = _regionDao.LoadRegion(start, length, orderBy, orderDir, name, status);
                return regions.Select(r => new RegionDto { Id = r.Id, Name = r.Name, StatusText = StatusTypeText.GetStatusText(r.Status) }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<RegionDto> LoadRegion(int? status = null)
        {
            try
            {
                var regions = _regionDao.LoadAll(status);

                return regions.Select(r => new RegionDto
                {
                    Id = r.Id, Name = r.Name, StatusText = StatusTypeText.GetStatusText(r.Status)
                }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Other Function

        public int RegionRowCount(string name, int status)
        {
            try
            {
                return _regionDao.RegionRowCount(name, status);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Helper Function
        private void CheckDuplicateFields(RegionViewModel regionVm, long id = 0)
        {
            try
            {
                var isDuplicateName = _regionDao.CheckDuplicateFields(id, regionVm.Name);
                if (isDuplicateName)
                    throw new DuplicateEntryException("Region Name can not be duplicate");

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void ModelValidationCheck(Region region)
        {
            try
            {
                var validationResult = ValidationHelper.ValidateEntity<Region, Region>(region);
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
        private void CheckBeforeDelete(Region region)
        {
            if (region.Countries.Count > 0)
                throw new DependencyException("You can't delete this Region, Country is already declared here.");
        }
        #endregion
    }
}
