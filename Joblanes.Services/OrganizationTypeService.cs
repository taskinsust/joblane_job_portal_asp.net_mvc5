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
    public interface IOrganizationTypeService : IBaseService
    {
        #region Operational Function
        void Save(OrganizationTypeViewModel organizationTypeViewModel);
        void Delete(long id);
        #endregion

        #region Single Instances Loading Function
        OrganizationTypeDto GetById(long id);
        #endregion

        #region List of Loading function
        List<OrganizationTypeDto> LoadOrganizationType(int start, int length, string orderBy, string orderDir, string name, int status);
        List<OrganizationTypeDto> LoadOrganizationType(int? status = null);   
        #endregion

        #region Other Function
        int OrganizationTypeRowCount(string name, int status); 
        #endregion 
    }
    public class OrganizationTypeService : BaseService, IOrganizationTypeService
    {
        #region Propertise & Object Initialization

        private readonly IOrganizationTypeDao _organizationTypeDao;
        private readonly IUserDao _userDao;
        public OrganizationTypeService(ISession session)
        {
            Session = session;
            _organizationTypeDao = new OrganizationTypeDao() { Session = session };
            _userDao = new UserDao() { Session = session };
        }
        #endregion

        #region Operational Function
        public void Save(OrganizationTypeViewModel organizationTypeViewModel) 
        {
            ITransaction transaction = null;
            try
            {
                if (organizationTypeViewModel == null)
                {
                    throw new NullObjectException("Organization Type can not be null");
                }
                var organizationType = new OrganizationType()
                {
                    Id = organizationTypeViewModel.Id,
                    Name = organizationTypeViewModel.Name,
                    Status = organizationTypeViewModel.Status,
                    Description = organizationTypeViewModel.Description
                };

                ModelValidationCheck(organizationType);

                using (transaction = Session.BeginTransaction())
                {
                    if (organizationType.Id < 1)
                    {
                        CheckDuplicateFields(organizationTypeViewModel);
                        organizationType.CreateBy = organizationTypeViewModel.CurrentUserProfileId;
                        organizationType.ModifyBy = organizationTypeViewModel.CurrentUserProfileId;
                        _organizationTypeDao.Save(organizationType);
                    }
                    else
                    {
                        var oldOrganization = _organizationTypeDao.LoadById(organizationTypeViewModel.Id);
                        if (oldOrganization == null)
                        {
                            throw new NullObjectException("Organization Type can not be null");
                        }
                        CheckDuplicateFields(organizationTypeViewModel, oldOrganization.Id);
                        oldOrganization.Status = organizationTypeViewModel.Status;
                        oldOrganization.Name = organizationTypeViewModel.Name;
                        oldOrganization.Description = organizationTypeViewModel.Description;
                        oldOrganization.ModifyBy = organizationTypeViewModel.CurrentUserProfileId <= 0 ? oldOrganization.ModifyBy : organizationTypeViewModel.CurrentUserProfileId;
                        _organizationTypeDao.Update(oldOrganization);

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
                var obj = _organizationTypeDao.LoadById(id);
                if (obj == null)
                    throw new NullObjectException("Organization Type is not valid");

               // CheckBeforeDelete(obj);
                using (trans = Session.BeginTransaction())
                {
                    obj.Status = OrganizationType.EntityStatus.Delete;
                    _organizationTypeDao.Update(obj);
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
        public OrganizationTypeDto GetById(long id)
        {
            try
            {
                var organizationType = _organizationTypeDao.LoadById(id);
                if (organizationType != null)
                {
                    string crBy = organizationType.CreateBy > 0 ? _userDao.GetUserNameByUserProfileId(organizationType.CreateBy) : "";
                    string moBy = crBy;
                    if (organizationType.CreateBy != organizationType.ModifyBy)
                        moBy = organizationType.ModifyBy > 0 ? _userDao.GetUserNameByUserProfileId(organizationType.ModifyBy) : "";

                    return new OrganizationTypeDto()
                    {
                        Id = organizationType.Id,
                        Name = organizationType.Name,
                        StatusText = StatusTypeText.GetStatusText(organizationType.Status),
                        Status = organizationType.Status,
                        Description = organizationType.Description,
                        CreateBy = crBy,
                        ModifyBy = moBy,
                        CreateDate = organizationType.CreationDate.ToString("dd-MMMM-yyyy"),
                        ModifyDate = organizationType.ModificationDate.ToString("dd-MMMM-yyyy")
                    };
                }
                else
                {
                    return new OrganizationTypeDto();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region List of Loading function

        public List<OrganizationTypeDto> LoadOrganizationType(int start, int length, string orderBy, string orderDir, string name, int status)
        {
            try
            {
                var organizationTypes = _organizationTypeDao.LoadOrganizationType(start, length, orderBy, orderDir, name, status);

                return organizationTypes.Select(r => new OrganizationTypeDto() { Id = r.Id, Name = r.Name, StatusText = StatusTypeText.GetStatusText(r.Status) }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<OrganizationTypeDto> LoadOrganizationType(int? status = null)
        {
            try
            {
                var organizationTypes = _organizationTypeDao.LoadAll(status);

                return organizationTypes.Select(r => new OrganizationTypeDto() { Id = r.Id, Name = r.Name, StatusText = StatusTypeText.GetStatusText(r.Status) }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Other Function

        public int OrganizationTypeRowCount(string name, int status) 
        {
            try
            {
                return _organizationTypeDao.OrganizationTypeRowCount(name, status); 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Helper Function
        private void CheckDuplicateFields(OrganizationTypeViewModel organizationTypeVm, long id = 0) 
        {
            try
            {
                var isDuplicateName = _organizationTypeDao.CheckDuplicateFields(id, organizationTypeVm.Name);
                if (isDuplicateName)
                    throw new DuplicateEntryException("Organization Type Name can not be duplicate");

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void ModelValidationCheck(OrganizationType organizationType)
        {
            try
            {
                var validationResult = ValidationHelper.ValidateEntity<OrganizationType, OrganizationType>(organizationType);
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

        private void CheckBeforeDelete(OrganizationType organizationType)
        {
            if (organizationType.Description.Length > 0)
                throw new DependencyException("You can't delete this Organization Type, Job is already declared here.");
        }
        #endregion

    }
}
