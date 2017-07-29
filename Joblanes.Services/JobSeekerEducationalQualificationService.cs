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
    public interface IJobSeekerEducationalQualificationService : IBaseService
    {
        #region Operational Function
        void Save(JobSeekerEduVm jobSeekerEduVm);
        void Delete(long id);
        #endregion

        #region List of Loading function

        #endregion

        #region Other Function

        #endregion

        #region Single Object
        JobSeekerEduVm GetById(long id);
        #endregion


    }
    public class JobSeekerEducationalQualificationService : BaseService, IJobSeekerEducationalQualificationService
    {
        #region Propertise & Object Initialization

        private readonly IJobSeekerEducationalQualificationDao _seekerEducationalQualificationDao;
        private readonly IUserDao _userDao;
        private readonly IJobSeekerDao _jobSeekerDao;

        public JobSeekerEducationalQualificationService(ISession session)
        {
            Session = session;
            _seekerEducationalQualificationDao = new JobSeekerEducationalQualificationDao() { Session = session };
            _userDao = new UserDao() { Session = session };
            _jobSeekerDao = new JobSeekerDao() { Session = session };
        }
        #endregion

        #region Operational Function
        public void Save(JobSeekerEduVm jobSeekerEduVm)
        {
            ITransaction transaction = null;
            try
            {
                if (jobSeekerEduVm == null)
                {
                    throw new NullObjectException("Region can not be null");
                }
                var jobSeekerEduModel = new JobSeekerEducationalQualification()
                {
                    Id = jobSeekerEduVm.Id,
                    Institute = jobSeekerEduVm.Institute,
                    Degree = jobSeekerEduVm.Degree,
                    StartingYear = jobSeekerEduVm.StartingYear,
                    PassingYear = jobSeekerEduVm.PassingYear,
                    Result = jobSeekerEduVm.Result,
                    FieldOfStudy = jobSeekerEduVm.FieldOfStudy
                };
                UserProfile userProfile = _userDao.GetByAspNetUserId(jobSeekerEduVm.AspNetuserId);
                JobSeeker jobSeeker = _jobSeekerDao.GetUsProfileId(userProfile.Id);
                if (jobSeeker == null) throw new InvalidDataException("Please Update Profile First");
                jobSeekerEduModel.JobSeeker = jobSeeker;
                ModelValidationCheck(jobSeekerEduModel);
                using (transaction = Session.BeginTransaction())
                {
                    if (jobSeekerEduModel.Id < 1)
                    {
                        CheckDuplicateFields(jobSeekerEduModel, jobSeeker.Id);
                        _seekerEducationalQualificationDao.Save(jobSeekerEduModel);
                    }
                    else
                    {
                        var oldjobSeekerEduModel = _seekerEducationalQualificationDao.LoadById(jobSeekerEduModel.Id);
                        if (oldjobSeekerEduModel == null)
                        {
                            throw new NullObjectException("Model can't be null");
                        }
                        oldjobSeekerEduModel.Institute = jobSeekerEduVm.Institute;
                        oldjobSeekerEduModel.Degree = jobSeekerEduVm.Degree;
                        oldjobSeekerEduModel.StartingYear = jobSeekerEduVm.StartingYear;
                        oldjobSeekerEduModel.PassingYear = jobSeekerEduVm.PassingYear;
                        oldjobSeekerEduModel.Result = jobSeekerEduVm.Result;
                        oldjobSeekerEduModel.FieldOfStudy = jobSeekerEduVm.FieldOfStudy;
                        CheckDuplicateFields(jobSeekerEduModel, jobSeeker.Id, oldjobSeekerEduModel.Id);
                        _seekerEducationalQualificationDao.Update(oldjobSeekerEduModel);
                    }
                    transaction.Commit();

                }
            }
            catch (NullObjectException ex)
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
                if (id <= 0) throw new InvalidDataException("invalid Id found");
                var oldjobSeekerEduModel = _seekerEducationalQualificationDao.LoadById(id);
                if (oldjobSeekerEduModel == null)
                {
                    throw new NullObjectException("Model can't be null");
                }
                oldjobSeekerEduModel.Status = JobSeekerEducationalQualification.EntityStatus.Delete;
                using (transaction = Session.BeginTransaction())
                {
                    _seekerEducationalQualificationDao.Update(oldjobSeekerEduModel);
                    transaction.Commit();
                }
            }
            catch (NullObjectException ex)
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

        private void CheckDuplicateFields(JobSeekerEducationalQualification jobSeekerEduModel, long jobSeekerId, long id = 0)
        {
            try
            {
                var isDuplicateName = _seekerEducationalQualificationDao.CheckDuplicateFields(id, jobSeekerId, jobSeekerEduModel.Degree);
                if (isDuplicateName)
                    throw new DuplicateEntryException("Already added this degree");

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Single Object
        public JobSeekerEduVm GetById(long id)
        {
            try
            {
                JobSeekerEducationalQualification edu = _seekerEducationalQualificationDao.LoadById(id);
                return CastToEduVm(edu);
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
        #region List of Loading function

        #endregion

        #region Other Function

        #endregion

        #region Helper
        private void ModelValidationCheck(JobSeekerEducationalQualification jobSeekerEducationalQualification)
        {
            try
            {
                var validationResult = ValidationHelper.ValidateEntity<JobSeekerEducationalQualification, JobSeekerEducationalQualification>(jobSeekerEducationalQualification);
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
        private JobSeekerEduVm CastToEduVm(JobSeekerEducationalQualification jobSeekerEducationalQualification)
        {
            var jsEduVm = new JobSeekerEduVm();
            jsEduVm.Id = jobSeekerEducationalQualification.Id;
            jsEduVm.Institute = jobSeekerEducationalQualification.Institute;
            jsEduVm.Degree = jobSeekerEducationalQualification.Degree;
            jsEduVm.PassingYear = jobSeekerEducationalQualification.PassingYear;
            jsEduVm.StartingYear = jobSeekerEducationalQualification.StartingYear;
            jsEduVm.Result = jobSeekerEducationalQualification.Result;
            jsEduVm.FieldOfStudy = jobSeekerEducationalQualification.FieldOfStudy;
            return jsEduVm;
        }
        #endregion
    }
}
