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
    public interface IJobTypeService : IBaseService
    {
        #region Operational Function
        void Save(JobTypeViewModel jobTypeViewModel);
        void Delete(long id);
        #endregion

        #region Single Instances Loading Function
        JobTypeDto GetById(long id); 
        #endregion

        #region List of Loading function
        List<JobTypeDto> LoadJobType(int start, int length, string orderBy, string orderDir, string name, int status);
        List<JobTypeDto> LoadJobType(int? status = null);  
        #endregion

        #region Other Function
        int JobTypeRowCount(string name, int status);
        #endregion 
    }
    public class JobTypeService : BaseService, IJobTypeService
    {
        #region Propertise & Object Initialization

        private readonly IJobTypeDao _jobTypeDao;
        private readonly IUserDao _userDao;
        public JobTypeService(ISession session)
        {
            Session = session;
            _jobTypeDao = new JobTypeDao() { Session = session };
            _userDao = new UserDao() { Session = session };
        }
        #endregion

        #region Operational Function
        public void Save(JobTypeViewModel jobTypeViewModel)
        {
            ITransaction transaction = null;
            try
            {
                if (jobTypeViewModel == null)
                {
                    throw new NullObjectException("Job Type can not be null");
                }
                var jobType = new JobType()
                {
                    Id = jobTypeViewModel.Id,
                    Name = jobTypeViewModel.Name,
                    Description = jobTypeViewModel.Description,
                    Status = jobTypeViewModel.Status
                };

                ModelValidationCheck(jobType);

                using (transaction = Session.BeginTransaction())
                {
                    if (jobType.Id < 1)
                    {
                        CheckDuplicateFields(jobTypeViewModel);
                        jobType.CreateBy = jobTypeViewModel.CurrentUserProfileId;
                        jobType.ModifyBy = jobTypeViewModel.CurrentUserProfileId;
                        _jobTypeDao.Save(jobType);
                    }
                    else
                    {
                        var oldJobType = _jobTypeDao.LoadById(jobTypeViewModel.Id);
                        if (oldJobType == null)
                        {
                            throw new NullObjectException("Job Type can not be null");
                        }
                        CheckDuplicateFields(jobTypeViewModel, oldJobType.Id);
                        oldJobType.Status = jobTypeViewModel.Status;
                        oldJobType.Name = jobTypeViewModel.Name;
                        oldJobType.Description = jobTypeViewModel.Description;
                        oldJobType.ModifyBy = jobTypeViewModel.CurrentUserProfileId <= 0 ? oldJobType.ModifyBy : jobTypeViewModel.CurrentUserProfileId;
                        _jobTypeDao.Update(oldJobType);

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
                var obj = _jobTypeDao.LoadById(id);
                if (obj == null)
                    throw new NullObjectException("Job Type is not valid");

                //CheckBeforeDelete(obj);
                using (trans = Session.BeginTransaction())
                {
                    obj.Status = JobType.EntityStatus.Delete;
                    _jobTypeDao.Update(obj);
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
        public JobTypeDto GetById(long id)
        {
            try
            {
                var jobType = _jobTypeDao.LoadById(id);
                if (jobType != null)
                {
                    string crBy = jobType.CreateBy > 0 ? _userDao.GetUserNameByUserProfileId(jobType.CreateBy) : "";
                    string moBy = crBy;
                    if (jobType.CreateBy != jobType.ModifyBy)
                        moBy = jobType.ModifyBy > 0 ? _userDao.GetUserNameByUserProfileId(jobType.ModifyBy) : "";


                    return new JobTypeDto()
                    {
                        Id = jobType.Id,
                        Name = jobType.Name,
                        StatusText = StatusTypeText.GetStatusText(jobType.Status),
                        Status = jobType.Status,
                        Description = jobType.Description,
                        CreateBy = crBy,
                        ModifyBy = moBy,
                        CreateDate = jobType.CreationDate.ToString("dd-MMMM-yyyy"),
                        ModifyDate = jobType.ModificationDate.ToString("dd-MMMM-yyyy")

                    };
                }
                else
                {
                    return new JobTypeDto();
                }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region List of Loading function

        public List<JobTypeDto> LoadJobType(int start, int length, string orderBy, string orderDir, string name, int status)
        {
            try
            {
                var jobTypes = _jobTypeDao.LoadJobType(start, length, orderBy, orderDir, name, status);

                return jobTypes.Select(r => new JobTypeDto() { Id = r.Id, Name = r.Name, StatusText = StatusTypeText.GetStatusText(r.Status) }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<JobTypeDto> LoadJobType(int? status = null)
        {
            try
            {
                var jobTypes = _jobTypeDao.LoadAll(status);

                return jobTypes.Select(r => new JobTypeDto() { Id = r.Id, Name = r.Name, StatusText = StatusTypeText.GetStatusText(r.Status) }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Other Function

        public int JobTypeRowCount(string name, int status)
        {
            try
            {
                return _jobTypeDao.JobTypeRowCount(name, status);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Helper Function
        private void CheckDuplicateFields(JobTypeViewModel jobTypeVm, long id = 0)
        {
            try
            {
                var isDuplicateName = _jobTypeDao.CheckDuplicateFields(id, jobTypeVm.Name);
                if (isDuplicateName)
                    throw new DuplicateEntryException("Job Type Name can not be duplicate");

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void ModelValidationCheck(JobType jobType)
        {
            try
            {
                var validationResult = ValidationHelper.ValidateEntity<JobType, JobType>(jobType);
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

        private void CheckBeforeDelete(JobType jobType)
        {
            if (jobType.Description.Length > 0)
                throw new DependencyException("You can't delete this Job Type, Job is already declared here.");
        }
        #endregion
    }
}
