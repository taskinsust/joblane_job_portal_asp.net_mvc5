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
    public interface IPaymentTypeService : IBaseService
    {
        #region Operational Function
        void Save(PaymentTypeViewModel paymentTypeViewModel);
        void Delete(long id);
        #endregion

        #region Single Instances Loading Function
        PaymentTypeDto GetById(long id);
        #endregion

        #region List of Loading function
        List<PaymentTypeDto> LoadPaymentType(int start, int length, string orderBy, string orderDir, string name, int status);
        List<PaymentTypeDto> LoadPaymentType(int? status = null);   
        #endregion
       
        #region Other Function
        int PaymentTypeRowCount(string name, int status);
        #endregion 

    }
    public class PaymentTypeService : BaseService, IPaymentTypeService
    {
        #region Propertise & Object Initialization

        private readonly IPaymentTypeDao _paymentTypeDao;
        private readonly IUserDao _userDao;
        public PaymentTypeService(ISession session)
        {
            Session = session;
            _paymentTypeDao = new PaymentTypeDao() { Session = session };
            _userDao = new UserDao() { Session = session };
        }
        #endregion

        #region Operational Function
        public void Save(PaymentTypeViewModel paymentTypeViewModel)
        {
            ITransaction transaction = null;
            try
            {
                if (paymentTypeViewModel == null)
                {
                    throw new NullObjectException("Payment Type can not be null");
                }
                var paymentType = new PaymentType()
                {
                    Id = paymentTypeViewModel.Id,
                    Name = paymentTypeViewModel.Name,
                    Status = paymentTypeViewModel.Status,
                    Description = paymentTypeViewModel.Description
                };

                ModelValidationCheck(paymentType);

                using (transaction = Session.BeginTransaction())
                {
                    if (paymentType.Id < 1)
                    {
                        CheckDuplicateFields(paymentTypeViewModel);
                        paymentType.CreateBy = paymentTypeViewModel.CurrentUserProfileId;
                        paymentType.ModifyBy = paymentTypeViewModel.CurrentUserProfileId;
                        _paymentTypeDao.Save(paymentType);
                    }
                    else
                    {
                        var oldPaymentType = _paymentTypeDao.LoadById(paymentTypeViewModel.Id);
                        if (oldPaymentType == null)
                        {
                            throw new NullObjectException("Payment Type can not be null");
                        }
                        CheckDuplicateFields(paymentTypeViewModel, oldPaymentType.Id);
                        oldPaymentType.Status = paymentTypeViewModel.Status;
                        oldPaymentType.Name = paymentTypeViewModel.Name;
                        oldPaymentType.Description = paymentTypeViewModel.Description;
                        oldPaymentType.ModifyBy = paymentTypeViewModel.CurrentUserProfileId <= 0 ? oldPaymentType.ModifyBy : paymentTypeViewModel.CurrentUserProfileId;
                        _paymentTypeDao.Update(oldPaymentType);

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
                var obj = _paymentTypeDao.LoadById(id);
                if (obj == null)
                    throw new NullObjectException("Payment Type is not valid");

               // CheckBeforeDelete(obj);
                using (trans = Session.BeginTransaction())
                {
                    obj.Status = PaymentType.EntityStatus.Delete;
                    _paymentTypeDao.Update(obj);
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
        public PaymentTypeDto GetById(long id)
        {
            try
            {
                var paymentType = _paymentTypeDao.LoadById(id);
                if (paymentType!=null)
                {
                    string crBy = paymentType.CreateBy > 0 ? _userDao.GetUserNameByUserProfileId(paymentType.CreateBy) : "";
                    string moBy = crBy;
                    if (paymentType.CreateBy != paymentType.ModifyBy)
                        moBy = paymentType.ModifyBy > 0 ? _userDao.GetUserNameByUserProfileId(paymentType.ModifyBy) : "";

                    return new PaymentTypeDto()
                    {
                        Id = paymentType.Id,
                        Name = paymentType.Name, 
                        StatusText = StatusTypeText.GetStatusText(paymentType.Status),
                        Status = paymentType.Status, 
                        Description = paymentType.Description,
                        CreateBy = crBy,
                        ModifyBy = moBy,
                        CreateDate = paymentType.CreationDate.ToString("dd-MMMM-yyyy"),
                        ModifyDate = paymentType.ModificationDate.ToString("dd-MMMM-yyyy")
                    };
                }
                else
                {
                    return new PaymentTypeDto();
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region List of Loading function

        public List<PaymentTypeDto> LoadPaymentType(int start, int length, string orderBy, string orderDir, string name, int status)
        {
            try
            {
                var paymentTypes = _paymentTypeDao.LoadPaymentType(start, length, orderBy, orderDir, name, status);

                return paymentTypes.Select(r => new PaymentTypeDto() { Id = r.Id, Name = r.Name, StatusText = StatusTypeText.GetStatusText(r.Status) }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PaymentTypeDto> LoadPaymentType(int? status = null)
        {
            try
            {
                var paymentTypes = _paymentTypeDao.LoadAll(status);

                return paymentTypes.Select(r => new PaymentTypeDto() { Id = r.Id, Name = r.Name, StatusText = StatusTypeText.GetStatusText(r.Status) }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Other Function

        public int PaymentTypeRowCount(string name, int status)
        {
            try
            {
                return _paymentTypeDao.PaymentTypeRowCount(name, status);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Helper Function
        private void CheckDuplicateFields(PaymentTypeViewModel paymentTypeVm, long id = 0) 
        {
            try
            {
                var isDuplicateName = _paymentTypeDao.CheckDuplicateFields(id, paymentTypeVm.Name); 
                if (isDuplicateName)
                    throw new DuplicateEntryException("Payment Type Name can not be duplicate");

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void ModelValidationCheck(PaymentType paymentType)
        {
            try
            {
                var validationResult = ValidationHelper.ValidateEntity<PaymentType, PaymentType>(paymentType);
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
        private void CheckBeforeDelete(PaymentType paymentType)
        {
            if (paymentType.Description.Length > 0)
                throw new DependencyException("You can't delete this Payment Type, Company is already use this.");
        }
        #endregion

    }
}
