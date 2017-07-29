using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes;
using Model.JobLanes.Entity;
using Model.JobLanes.Entity.User;
using Model.JobLanes.ViewModel;
using NHibernate;
using NHibernate.Util;
using Services.Joblanes.Base;
using Services.Joblanes.Exceptions;
using Services.Joblanes.Helper;

namespace Services.Joblanes
{
    public interface IJobSeekerExperienceService : IBaseService
    {
        #region Operational Function
        void Save(JobSeekerExpVm jobSeekerExpVm);
        void Delete(long id);

        #endregion

        #region List of Loading function

        #endregion

        #region Other Function

        #endregion

        #region Single

        JobSeekerExpVm GetById(long id);

        #endregion

    }
    public class JobSeekerExperienceService : BaseService, IJobSeekerExperienceService
    {
        #region Propertise & Object Initialization

        private readonly IJobSeekerExperienceDao _jobSeekerExperienceDao;
        private readonly IUserDao _userDao;
        private readonly IJobSeekerDao _jobSeekerDao;
        public JobSeekerExperienceService(ISession session)
        {
            Session = session;
            _jobSeekerExperienceDao = new JobSeekerExperienceDao() { Session = session };
            _userDao = new UserDao() { Session = session };
            _jobSeekerDao = new JobSeekerDao() { Session = session };
        }
        #endregion

        #region Operational Function
        public void Save(JobSeekerExpVm jobSeekerExpVm)
        {
            ITransaction transaction = null;
            try
            {
                if (jobSeekerExpVm == null)
                {
                    throw new NullObjectException("Region can not be null");
                }
                var jobSeekerExpModel = new JobSeekerExperience()
                {
                    Id = jobSeekerExpVm.Id,
                    CompanyName = jobSeekerExpVm.CompanyName,
                    CompanyAddress = jobSeekerExpVm.CompanyAddress,
                    DateFrom = jobSeekerExpVm.DateFrom,
                    DateTo = jobSeekerExpVm.DateTo,
                    IsCurrent = jobSeekerExpVm.IsCurrent,
                    Designation = jobSeekerExpVm.Designation,
                    Responsibility = jobSeekerExpVm.Responsibility
                };
                UserProfile userProfile = _userDao.GetByAspNetUserId(jobSeekerExpVm.AspNetuserId);
                JobSeeker jobSeeker = _jobSeekerDao.GetUsProfileId(userProfile.Id);
                if (jobSeeker == null) throw new InvalidDataException("Please Update Profile First");
                if (jobSeekerExpModel.DateFrom > jobSeekerExpVm.DateTo) throw new InvalidDataException("Invalid datetime time selected");
                jobSeekerExpModel.JobSeeker = jobSeeker;

                ModelValidationCheck(jobSeekerExpModel);

                using (transaction = Session.BeginTransaction())
                {
                    if (jobSeekerExpModel.Id < 1)
                    {

                        CheckDuplicateFields(jobSeekerExpModel);
                        _jobSeekerExperienceDao.Save(jobSeekerExpModel);
                    }
                    else
                    {
                        var oldjobSeekerExpModel = _jobSeekerExperienceDao.LoadById(jobSeekerExpModel.Id);
                        if (oldjobSeekerExpModel == null)
                        {
                            throw new NullObjectException("Model can't be null");
                        }
                        CheckDuplicateFields(jobSeekerExpModel, oldjobSeekerExpModel.Id);
                        oldjobSeekerExpModel.CompanyName = jobSeekerExpModel.CompanyName;
                        oldjobSeekerExpModel.CompanyAddress = jobSeekerExpModel.CompanyAddress;
                        oldjobSeekerExpModel.DateFrom = jobSeekerExpModel.DateFrom;
                        oldjobSeekerExpModel.DateTo = jobSeekerExpModel.DateTo;
                        oldjobSeekerExpModel.Designation = jobSeekerExpModel.Designation;
                        oldjobSeekerExpModel.Responsibility = jobSeekerExpModel.Responsibility;
                        oldjobSeekerExpModel.IsCurrent = jobSeekerExpModel.IsCurrent;
                        _jobSeekerExperienceDao.Update(oldjobSeekerExpModel);
                    }
                    transaction.Commit();

                }
            }
            catch (NullObjectException ex)
            {
                throw ex;

            }
            catch (InvalidDataException ex)
            {
                throw ex;

            }
            catch (DuplicateEntryException ex)
            {
                throw ex;
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

        public void Delete(long id)
        {
            ITransaction transaction = null;
            try
            {
                if (id <= 0) throw new InvalidDataException("Invalid Jobexperience Data.");
                var oldjobSeekerEduModel = _jobSeekerExperienceDao.LoadById(id);
                if (oldjobSeekerEduModel == null)
                {
                    throw new NullObjectException("Model can't be null");
                }
                oldjobSeekerEduModel.Status = JobSeekerExperience.EntityStatus.Delete;
                using (transaction = Session.BeginTransaction())
                {
                    _jobSeekerExperienceDao.Update(oldjobSeekerEduModel);
                    transaction.Commit();
                }
            }
            catch (NullObjectException ex)
            {
                throw ex;
            }
            catch (InvalidDataException ex)
            {
                throw ex;

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

        #region List of Loading function

        #endregion

        #region Single

        public JobSeekerExpVm GetById(long id)
        {
            try
            {
                if(id<=0) throw new InvalidDataException("Invalid Data ");
                JobSeekerExperience experience = _jobSeekerExperienceDao.LoadById(id);
                return CastToExpVm(experience);
            }
            catch (InvalidDataException ide)
            {
                throw ide;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        #region Other Function

        #endregion

        #region Helper
        private void CheckDuplicateFields(JobSeekerExperience jobSeekerExperience, long id = 0)
        {
            try
            {
                var isDuplicateName = _jobSeekerExperienceDao.CheckDuplicateFields(id, jobSeekerExperience.CompanyName, jobSeekerExperience.DateFrom, jobSeekerExperience.DateTo);
                if (isDuplicateName)
                    throw new DuplicateEntryException("Already added this Experience");

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void ModelValidationCheck(JobSeekerExperience jobSeekerExperience)
        {
            try
            {
                var validationResult = ValidationHelper.ValidateEntity<JobSeekerExperience, JobSeekerExperience>(jobSeekerExperience);
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

        private JobSeekerExpVm CastToExpVm(JobSeekerExperience jobSeekerExperience)
        {
            if (jobSeekerExperience == null) throw new InvalidDataException("Invalid Experience");
            var jsEduVm = new JobSeekerExpVm();
            jsEduVm.Id = jobSeekerExperience.Id;
            jsEduVm.CompanyName = jobSeekerExperience.CompanyName;
            jsEduVm.CompanyAddress = jobSeekerExperience.CompanyAddress;
            jsEduVm.DateFrom = jobSeekerExperience.DateFrom;
            jsEduVm.DateTo = jobSeekerExperience.DateTo;
            jsEduVm.Designation = jobSeekerExperience.Designation;
            jsEduVm.IsCurrent = jobSeekerExperience.IsCurrent;
            jsEduVm.Responsibility = jobSeekerExperience.Responsibility;
            return jsEduVm;
        }

        #endregion
    }
}
