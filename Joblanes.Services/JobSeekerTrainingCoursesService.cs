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
    public interface IJobSeekerTrainingCoursesService : IBaseService
    {
        #region Operational Function
        void Save(JobSeekerTrainingVm jobSeekerTrainingVm);
        void Delete(long id);

        #endregion

        #region List of Loading function

        #endregion

        #region Other Function

        #endregion



    }
    public class JobSeekerTrainingCoursesService : BaseService, IJobSeekerTrainingCoursesService
    {
        #region Propertise & Object Initialization

        private readonly IJobSeekerTrainingCoursesDao _jobSeekerTrainingCoursesDao;
        private readonly IUserDao _userDao;
        private readonly IJobSeekerDao _jobSeekerDao;
        public JobSeekerTrainingCoursesService(ISession session)
        {
            Session = session;
            _jobSeekerTrainingCoursesDao = new JobSeekerTrainingCoursesDao() { Session = session };
            _userDao = new UserDao() { Session = session };
            _jobSeekerDao = new JobSeekerDao() { Session = session };
        }
        #endregion

        #region Operational Function
        public void Save(JobSeekerTrainingVm jobSeekerTrainVm)
        {
            ITransaction transaction = null;
            try
            {
                if (jobSeekerTrainVm == null)
                {
                    throw new NullObjectException("Region can not be null");
                }
                var jobSeekerTrainModel = new JobSeekerTrainingCourses()
                {
                    Id = jobSeekerTrainVm.Id,
                    Title = jobSeekerTrainVm.Title,
                    Institute = jobSeekerTrainVm.Institute,
                    Description = jobSeekerTrainVm.Description,
                    StartDate = jobSeekerTrainVm.StartDate,
                    CloseDate = jobSeekerTrainVm.CloseDate,
                };
                UserProfile userProfile = _userDao.GetByAspNetUserId(jobSeekerTrainVm.AspNetuserId);
                JobSeeker jobSeeker = _jobSeekerDao.GetUsProfileId(userProfile.Id);
                if (jobSeeker == null) throw new InvalidDataException("Please Update Profile First");
                if (jobSeekerTrainModel.StartDate > jobSeekerTrainVm.CloseDate) throw new InvalidDataException("Invalid datetime time selected");
                jobSeekerTrainModel.JobSeeker = jobSeeker;

                ModelValidationCheck(jobSeekerTrainModel);

                using (transaction = Session.BeginTransaction())
                {
                    if (jobSeekerTrainModel.Id < 1)
                    {

                        CheckDuplicateFields(jobSeekerTrainModel);
                        _jobSeekerTrainingCoursesDao.Save(jobSeekerTrainModel);
                    }
                    else
                    {
                        var oldjobSeekerExpModel = _jobSeekerTrainingCoursesDao.LoadById(jobSeekerTrainModel.Id);
                        if (oldjobSeekerExpModel == null)
                        {
                            throw new NullObjectException("Model can't be null");
                        }
                        CheckDuplicateFields(jobSeekerTrainModel, oldjobSeekerExpModel.Id);
                        oldjobSeekerExpModel.Title = jobSeekerTrainVm.Title;
                        oldjobSeekerExpModel.Institute = jobSeekerTrainVm.Institute;
                        oldjobSeekerExpModel.Description = jobSeekerTrainVm.Description;
                        oldjobSeekerExpModel.StartDate = jobSeekerTrainVm.StartDate;
                        oldjobSeekerExpModel.CloseDate = jobSeekerTrainVm.CloseDate;
                        _jobSeekerTrainingCoursesDao.Update(oldjobSeekerExpModel);
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
                var oldjobSeekerEduModel = _jobSeekerTrainingCoursesDao.LoadById(id);
                if (oldjobSeekerEduModel == null)
                {
                    throw new NullObjectException("Model can't be null");
                }
                oldjobSeekerEduModel.Status = JobSeekerTrainingCourses.EntityStatus.Delete;
                using (transaction = Session.BeginTransaction())
                {
                    _jobSeekerTrainingCoursesDao.Update(oldjobSeekerEduModel);
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

        #endregion

        #region List of Loading function

        #endregion


        #region Other Function

        #endregion

        #region Helper
        private void CheckDuplicateFields(JobSeekerTrainingCourses jobSeekerTrainingCourses, long id = 0)
        {
            try
            {
                bool isDuplicateName = _jobSeekerTrainingCoursesDao.CheckDuplicateFields(id, jobSeekerTrainingCourses.Title, jobSeekerTrainingCourses.StartDate, jobSeekerTrainingCourses.CloseDate);
                if (isDuplicateName)
                    throw new DuplicateEntryException("Already added this Experience");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void ModelValidationCheck(JobSeekerTrainingCourses jobSeekerTrainingCourses)
        {
            try
            {
                var validationResult = ValidationHelper.ValidateEntity<JobSeekerTrainingCourses, JobSeekerTrainingCourses>(jobSeekerTrainingCourses);
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

        #endregion
    }
}
