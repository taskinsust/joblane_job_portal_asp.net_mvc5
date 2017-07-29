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
    public interface ICompanyTypeService : IBaseService
    {
        #region Operational Function
        void Save(CompanyTypeViewModel companyTypeViewModel);
        void Delete(long id);
        #endregion

        #region Single Instances Loading Function
        CompanyTypeDto GetById(long id);
        #endregion

        #region List of Loading function
        List<CompanyTypeDto> LoadCompanyType(int start, int length, string orderBy, string orderDir, string name, int status);
        List<CompanyTypeDto> LoadCompanyType(int? status = null);  
        #endregion

        #region Other Function
        int CompanyTypeRowCount(string name, int status);
        #endregion 
    }
    public class CompanyTypeService : BaseService, ICompanyTypeService
    {
        #region Propertise & Object Initialization

        private readonly ICompanyTypeDao _companyTypeDao;
        private readonly IUserDao _userDao;
        public CompanyTypeService(ISession session)
        {
            Session = session;
            _companyTypeDao = new CompanyTypeDao() { Session = session };
            _userDao = new UserDao() { Session = session };
        }
        #endregion

        #region Operational Function
        public void Save(CompanyTypeViewModel companyTypeViewModel)
        {
            ITransaction transaction = null;
            try
            {
                if (companyTypeViewModel == null)
                {
                    throw new NullObjectException("Company Type can not be null");
                }
                var companyType = new CompanyType()
                {
                    Id = companyTypeViewModel.Id,
                    Name = companyTypeViewModel.Name,
                    Status = companyTypeViewModel.Status,
                    Description = companyTypeViewModel.Description
                };

                ModelValidationCheck(companyType);

                using (transaction = Session.BeginTransaction())
                {
                    if (companyType.Id < 1)
                    {
                        CheckDuplicateFields(companyTypeViewModel);
                        companyType.CreateBy = companyTypeViewModel.CurrentUserProfileId;
                        companyType.ModifyBy = companyTypeViewModel.CurrentUserProfileId;
                        _companyTypeDao.Save(companyType);
                    }
                    else
                    {
                        var oldCompanyType = _companyTypeDao.LoadById(companyTypeViewModel.Id);
                        if (oldCompanyType == null)
                        {
                            throw new NullObjectException("Company Type can not be null");
                        }
                        CheckDuplicateFields(companyTypeViewModel, oldCompanyType.Id);
                        oldCompanyType.Status = companyTypeViewModel.Status;
                        oldCompanyType.Name = companyTypeViewModel.Name;
                        oldCompanyType.Description = companyTypeViewModel.Description;
                        oldCompanyType.ModifyBy = companyTypeViewModel.CurrentUserProfileId <= 0 ? oldCompanyType.ModifyBy : companyTypeViewModel.CurrentUserProfileId;
                        _companyTypeDao.Update(oldCompanyType);

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
                var obj = _companyTypeDao.LoadById(id);
                if (obj == null)
                    throw new NullObjectException("Company Type is not valid");

                CheckBeforeDelete(obj);
                using (trans = Session.BeginTransaction())
                {
                    obj.Status = CompanyType.EntityStatus.Delete;
                    _companyTypeDao.Update(obj);
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
        public CompanyTypeDto GetById(long id)
        {
            try
            {
                var companyType = _companyTypeDao.LoadById(id);
                if (companyType != null)
                {

                    string crBy = companyType.CreateBy > 0 ? _userDao.GetUserNameByUserProfileId(companyType.CreateBy) : "";
                    string moBy = crBy;
                    if (companyType.CreateBy != companyType.ModifyBy)
                        moBy = companyType.ModifyBy > 0 ? _userDao.GetUserNameByUserProfileId(companyType.ModifyBy) : "";

                    return new CompanyTypeDto()
                    {
                        Id = companyType.Id, 
                        Name = companyType.Name, 
                        StatusText = StatusTypeText.GetStatusText(companyType.Status),
                        Status = companyType.Status, 
                        Description = companyType.Description,
                        CreateBy = crBy,
                        ModifyBy = moBy,
                        CreateDate = companyType.CreationDate.ToString("dd-MMMM-yyyy"),
                        ModifyDate = companyType.ModificationDate.ToString("dd-MMMM-yyyy")
                    };
                }
                else
                {
                    return new CompanyTypeDto();
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region List of Loading function

        public List<CompanyTypeDto> LoadCompanyType(int start, int length, string orderBy, string orderDir, string name, int status)
        {
            try
            {
                var companyTypes = _companyTypeDao.LoadCompanyType(start, length, orderBy, orderDir, name, status);

                return companyTypes.Select(r => new CompanyTypeDto() { Id = r.Id, Name = r.Name, StatusText = StatusTypeText.GetStatusText(r.Status) }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        public List<CompanyTypeDto> LoadCompanyType(int? status = null)
        {
            try
            {
                var companyTypes = _companyTypeDao.LoadAll(status);

                return companyTypes.Select(r => new CompanyTypeDto() { Id = r.Id, Name = r.Name, StatusText = StatusTypeText.GetStatusText(r.Status) }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Other Function

        public int CompanyTypeRowCount(string name, int status)
        {
            try
            {
                return _companyTypeDao.CompanyTypeRowCount(name, status);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Helper Function
        private void CheckDuplicateFields(CompanyTypeViewModel companyTypeVm, long id = 0)
        {
            try
            {
                var isDuplicateName = _companyTypeDao.CheckDuplicateFields(id, companyTypeVm.Name);
                if (isDuplicateName)
                    throw new DuplicateEntryException("Company Type Name can not be duplicate");

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void ModelValidationCheck(CompanyType companyType)
        {
            try
            {
                var validationResult = ValidationHelper.ValidateEntity<CompanyType, CompanyType>(companyType);
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

        private void CheckBeforeDelete(CompanyType company)
        {
            if (company.Companies.Count > 0)
                throw new DependencyException("You can't delete this Company Type, Company is already declared here.");
        }
        #endregion

    }
}
