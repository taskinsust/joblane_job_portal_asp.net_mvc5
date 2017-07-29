using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using Dao.Joblanes.Base;
using Model.JobLanes.Entity.User;
using NHibernate;
using Services.Joblanes.Helper;

namespace Services.Joblanes.Base
{
    public interface IBaseService
    {
      string GetEntityTableName(Type entityType);
        ISession Session { get; set; }
        void Flush();
        IList<TEntityT> SearchEntityByKey<TEntityT, TIdT>(IBaseDao<TEntityT, TIdT> dao, string key, object value);
        IList<TEntityT> SearchEntityByKeyAndLikeValue<TEntityT, TIdT>(IBaseDao<TEntityT, TIdT> dao, string field, object value);
        ArrayList SearchEntityByKey<TEntityT, TIdT>(IBaseDao<TEntityT, TIdT> dao, string[] fields, object[] values, string[] operators, string[] conditionalOperator);
        
        DateTime FloorDate(DateTime dt);
        DateTime CeilDate(DateTime dt);

    }

    public class BaseService : IBaseService
    {
        public string GetEntityTableName(Type entityType)
        {
            return NhSessionFactory.GetTableName(entityType);
        }

       

        public IList<TEntityT> SearchEntityByKey<TEntityT, TIdT>(IBaseDao<TEntityT, TIdT> dao, string field, object value)
        {
            return dao.SearchListByFieldAndValue(field, value);
        }

        public IList<TEntityT> SearchEntityByKeyAndLikeValue<TEntityT, TIdT>(IBaseDao<TEntityT, TIdT> dao, string field, object value)
        {
            return dao.SearchListByFieldAndLikeValue(field, value);
        }

        public ArrayList SearchEntityByKey<TEntityT, TIdT>(IBaseDao<TEntityT, TIdT> dao, string[] fields, object[] values, string[] operators, string[] conditionalOperator)
        {
            return dao.SearchByFieldsAndValues(fields, values, operators, conditionalOperator);
        }

        public void Flush()
        {
            Session.Flush();
        }

        public DateTime FloorDate(DateTime dt)
        {
            DateTime nDt = new DateTime(dt.Year, dt.Month, dt.Day);
            return nDt;
        }

        public DateTime CeilDate(DateTime dt)
        {
            DateTime nDt = new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
            return nDt;
        }

        private ISession _session;
        
        public ISession Session
        {
            get { return _session; }
            set { _session = value; }
        }
    }
}
