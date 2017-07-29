using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes.Base;
using Model.JobLanes.Dto;
using Model.JobLanes.Entity;
using NHibernate;
using NHibernate.Criterion;

namespace Dao.Joblanes
{

    public interface IPaymentTypeDao : IBaseDao<PaymentType, long>
    {
        #region Operational Function

        #endregion

        #region Single Instances Loading Function

        #endregion

        #region List Loading Function
        List<PaymentType> LoadPaymentType(int start, int length, string orderBy, string orderDir, string name, int status);
        #endregion

        #region Others Function
        int PaymentTypeRowCount(string name, int status);
        bool CheckDuplicateFields(long id, string name);
        #endregion
    }
    public class PaymentTypeDao : BaseDao<PaymentType, long>, IPaymentTypeDao
    {
        #region Propertise & Object Initialization

        #endregion

        #region Operational Function

        #endregion

        #region Single Instances Loading Function

        #endregion

        #region List Loading Function
        public List<PaymentType> LoadPaymentType(int start, int length, string orderBy, string orderDir, string name, int status)
        {

            ICriteria criteria = PaymentTypeCriteria(name, status);

            if (!String.IsNullOrEmpty(orderBy))
            {
                criteria.AddOrder(orderDir == "ASC" ? Order.Asc(orderBy.Trim()) : Order.Desc(orderBy.Trim()));
            }

            return (List<PaymentType>)criteria.SetFirstResult(start).SetMaxResults(length).List<PaymentType>();         
        }



        #endregion

        #region Others Function
        public int PaymentTypeRowCount(string name, int status)
        {
            ICriteria criteria = PaymentTypeCriteria(name, status);

            criteria.SetProjection(Projections.RowCount());

            return Convert.ToInt32(criteria.UniqueResult());
        }

        public bool CheckDuplicateFields(long id, string name)
        {
            var paymentTypeQueryOver = Session.QueryOver<PaymentType>().Where(x => x.Status != PaymentType.EntityStatus.Delete); ;
            if (id > 0)
            {
                paymentTypeQueryOver = paymentTypeQueryOver.Where(x => x.Id != id);
            }

            if (!string.IsNullOrEmpty(name.Trim()))
            {
                paymentTypeQueryOver = paymentTypeQueryOver.Where(x => x.Name == name.ToString());
            }

            var rowcount = paymentTypeQueryOver.RowCount();
            return rowcount > 0;
        }

        #endregion

        #region Helper Function
        private ICriteria PaymentTypeCriteria(string name, int status)
        {
            ICriteria criteria = Session.CreateCriteria<PaymentType>().Add(Restrictions.Not(Restrictions.Eq("Status", PaymentType.EntityStatus.Delete)));

            if (status != 0)
                criteria.Add(Restrictions.Eq("Status", status));

            if (!String.IsNullOrEmpty(name))
                criteria.Add(Restrictions.Like("Name", name, MatchMode.Anywhere));

            return criteria;
        }
        #endregion
    }  
}
