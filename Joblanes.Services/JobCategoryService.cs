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
    public interface IJobCategoryService : IBaseService
    {
        #region Operational Function
        void Save(JobCategoryViewModel jobCategoryViewModel);
        void Delete(long id);
        #endregion

        #region Single Instances Loading Function
        JobCategoryDto GetById(long id);
        #endregion

        #region List of Loading function
        List<JobCategoryDto> LoadJobCategory(int start, int length, string orderBy, string orderDir, string name, int status);
        List<JobCategoryDto> LoadJobCategory(int? status = null);  
        #endregion

        #region Other Function
        int JobCategoryRowCount(string name, int status);
        #endregion 
    }
    public class JobCategoryService : BaseService, IJobCategoryService
    {
        #region Propertise & Object Initialization

        private readonly IJobCategoryDao _jobCategoryDao;
        private readonly IUserDao _userDao;
        public JobCategoryService(ISession session)
        {
            Session = session;
            _jobCategoryDao = new JobCategoryDao() { Session = session };
            _userDao = new UserDao() { Session = session };
        }
        #endregion

        #region Operational Function
        public void Save(JobCategoryViewModel jobCategoryViewModel)
        {
            ITransaction transaction = null;
            try
            {
                if (jobCategoryViewModel == null)
                {
                    throw new NullObjectException("Job Category can not be null");
                }
                var jobCategory = new JobCategory()
                {
                    Id = jobCategoryViewModel.Id,
                    Name = jobCategoryViewModel.Name,
                    Status = jobCategoryViewModel.Status,
                    Description = jobCategoryViewModel.Description
                };

                ModelValidationCheck(jobCategory);

                using (transaction = Session.BeginTransaction())
                {
                    if (jobCategory.Id < 1)
                    {
                        CheckDuplicateFields(jobCategoryViewModel);
                        jobCategory.CreateBy = jobCategoryViewModel.CurrentUserProfileId;
                        jobCategory.ModifyBy = jobCategoryViewModel.CurrentUserProfileId;
                        _jobCategoryDao.Save(jobCategory);
                    }
                    else
                    {
                        var oldJobCategory = _jobCategoryDao.LoadById(jobCategoryViewModel.Id);
                        if (oldJobCategory == null)
                        {
                            throw new NullObjectException("Job Category can not be null");
                        }
                        CheckDuplicateFields(jobCategoryViewModel, oldJobCategory.Id);
                        oldJobCategory.Status = jobCategoryViewModel.Status;
                        oldJobCategory.Name = jobCategoryViewModel.Name;
                        oldJobCategory.Description = jobCategoryViewModel.Description;
                        oldJobCategory.ModifyBy = jobCategoryViewModel.CurrentUserProfileId <= 0 ? oldJobCategory.ModifyBy : jobCategoryViewModel.CurrentUserProfileId;
                        _jobCategoryDao.Update(oldJobCategory);

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
                var obj = _jobCategoryDao.LoadById(id);
                if (obj == null)
                    throw new NullObjectException("Job Category is not valid");

              //  CheckBeforeDelete(obj);
                using (trans = Session.BeginTransaction())
                {
                    obj.Status = JobCategory.EntityStatus.Delete;
                    _jobCategoryDao.Update(obj);
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
        public JobCategoryDto GetById(long id)
        {
            try
            {
                var jobCategory = _jobCategoryDao.LoadById(id);
                if (jobCategory != null)
                {
                    string crBy = jobCategory.CreateBy > 0 ? _userDao.GetUserNameByUserProfileId(jobCategory.CreateBy) : "";
                    string moBy = crBy;
                    if (jobCategory.CreateBy != jobCategory.ModifyBy)
                        moBy = jobCategory.ModifyBy > 0 ? _userDao.GetUserNameByUserProfileId(jobCategory.ModifyBy) : "";

                    return new JobCategoryDto()
                    {
                        Id = jobCategory.Id,
                        Name = jobCategory.Name,
                        StatusText = StatusTypeText.GetStatusText(jobCategory.Status),
                        Status = jobCategory.Status,
                        Description = jobCategory.Description,
                        CreateBy = crBy,
                        ModifyBy = moBy,
                        CreateDate = jobCategory.CreationDate.ToString("dd-MMMM-yyyy"),
                        ModifyDate = jobCategory.ModificationDate.ToString("dd-MMMM-yyyy")
                    };
                }
                else
                {
                    return new JobCategoryDto();
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region List of Loading function

        public List<JobCategoryDto> LoadJobCategory(int start, int length, string orderBy, string orderDir, string name, int status)
        {
            try
            {
                var jobCategiries = _jobCategoryDao.LoadJobCategory(start, length, orderBy, orderDir, name, status);

                return jobCategiries.Select(r => new JobCategoryDto() { Id = r.Id, Name = r.Name, StatusText = StatusTypeText.GetStatusText(r.Status) }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<JobCategoryDto> LoadJobCategory(int? status = null)
        {
            try
            {
                var jobCategiries = _jobCategoryDao.LoadAll(status);

                return jobCategiries.Select(r => new JobCategoryDto() { Id = r.Id, Name = r.Name, StatusText = StatusTypeText.GetStatusText(r.Status) }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Other Function

        public int JobCategoryRowCount(string name, int status)
        {
            try
            {
                return _jobCategoryDao.JobCategoryRowCount(name, status);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Helper Function
        private void CheckDuplicateFields(JobCategoryViewModel jobCategoryVm, long id = 0)
        {
            try
            {
                var isDuplicateName = _jobCategoryDao.CheckDuplicateFields(id, jobCategoryVm.Name);
                if (isDuplicateName)
                    throw new DuplicateEntryException("Job Category Name can not be duplicate");

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void ModelValidationCheck(JobCategory jobCategory)
        {
            try
            {
                var validationResult = ValidationHelper.ValidateEntity<JobCategory, JobCategory>(jobCategory);
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

        private void CheckBeforeDelete(JobCategory jobCategory)
        {
            if (jobCategory.Description.Length > 0)
                throw new DependencyException("You can't delete this Job Category, Job is already declared here.");
        }
        #endregion
    }
}
