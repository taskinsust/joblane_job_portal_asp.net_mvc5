using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using Microsoft.AspNet.Identity;
using Model.JobLanes.Entity.Base;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Type;

namespace Dao.Joblanes.Base
{
    public interface IBaseDao<TEntityT, TIdT>
    {
        ISession Session { get; set; }
        void Save(TEntityT data);
        void Delete(TEntityT data);
        void DeleteEntity(TIdT id);
        void Update(TEntityT data);
        void SaveOrUpdate(TEntityT data);
        int ExecuteQuery(string sql);
        void Marge(TEntityT data);

        IList<TEntityT> LoadAllOk();
        IList<TEntityT> LoadByCriteria(TEntityT dataobj, TEntityT searchCriteria, int currentPage, int pageSize);

        TEntityT LoadById(TIdT id);
        IList LoadByName(string name);
        TEntityT LoadSingleByName(string name);
        IList LoadByQuery(string query);

        IList<TEntityT> LoadListByNameLike(string name);
        IList<TEntityT> SearchListByFieldAndValue(string field, object value);
        IList<TEntityT> SearchListByFieldAndLikeValue(string searchBy, object value);

        ArrayList SearchByFieldsAndValues(string[] fields, object[] values, string[] operators, string[] andOrOperator);

        IList Find(string query);
        IList Find(string query, object[] value);
        IList FindByNamedQuery(string namedQuery);
        IList FindByNamedQueryAndParam(string namedQuery, object[] value);
        int DeleteByNamedQuery(string queryName, object value, IType type);
        int DeleteByNamedQuery(string queryName, object[] values, IType[] types);
        IList FindByNamedQuery(string queryName, object value, IType type);
        IList FindByNamedQuery(string queryName, object[] values, IType[] types);
        IList FindPage(string queryString, int pageIndex, int pageSize);
        IList FindPageByNamedQuery(string queryName, int pageIndex, int pageSize);
        IList FindPageByNamedQuery(string queryName, object value, IType type, int pageIndex, int pageSize);
        IList FindPageByNamedQuery(string queryName, object[] values, IType[] types, int pageIndex, int pageSize);
        int CountByNamedQuery(string queryName, object[] values, NHibernate.Type.IType[] types);
        IList FindPageByQuery(string queryText, object[] values, int pageIndex, int pageSize);

        int RowCount(TEntityT entityT);


        IList<TEntityT> LoadAll(int? status=null);


        /// <summary>
        /// refactored function
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        TEntityT LoadByRankDirection(int rank, string action);
        /// <summary>
        /// refactored function
        /// </summary>
        /// <param name="tEntityT"></param>
        /// <returns></returns>
        int GetMinimumRank(TEntityT tEntityT);
        /// <summary>
        /// refactored function
        /// </summary>
        /// <param name="tEntityT"></param>
        /// <returns></returns>
        int GetMaximumRank(TEntityT tEntityT);

    }

    public class BaseDao<TEntityT, TIdT> : IBaseDao<TEntityT, TIdT> where TEntityT : class
    {
        private ISession _session;

        public ISession Session
        {
            get { return _session; }
            set { _session = value; }
        }

        public TEntityT GetById(TIdT id)
        {
            return null;
        }

        public IList LoadByName(string name)
        {
            IList list = new List<TEntityT>();
            Type type = typeof(TEntityT);
            string query = String.Format("@ From {0} as e where e.Name ='{1}'", type.Name, name);
            list = Find(query);
            return list;
        }

        public TEntityT LoadSingleByName(string name)
        {
            TEntityT list = default(TEntityT);
            Type type = typeof(TEntityT);
            string query = String.Format("@ From {0} as e where e.Name = '{1}'", type.Name, name);
            var lists = (ArrayList)Find(query);
            return (TEntityT)lists[0];
        }

        public IList<TEntityT> LoadListByNameLike(string name)
        {
            return Session.CreateCriteria<TEntityT>()
                .Add(Restrictions.InsensitiveLike("Name", name, MatchMode.Anywhere))
                .Add(Restrictions.Eq("Status", BaseEntity<TEntityT>.EntityStatus.Active)).List<TEntityT>();
        }

        public IList LoadByNameLike(string name)
        {
            IList list = new List<TEntityT>();
            Type type = typeof(TEntityT);

            string query = String.Format("@ From {0} as e where e.Name like %'{1}'%", type.Name, name);
            //string query = "From " + type.Name + " as e where e.Name like ?";

            list = Find(query);
            return list;
        }

        public virtual IList LoadByQuery(string query)
        {
            return Find(query);
        }

        public virtual void Save(TEntityT data)
        {

            IBaseEntity<long> entity = (IBaseEntity<long>)data;
            if (entity.CreationDate.Year < 2016)
                entity.CreationDate = DateTime.Now;
            entity.ModificationDate = DateTime.Now;
            if (entity.Status == null || entity.Status == 0)
                entity.Status = BaseEntity<TEntityT>.EntityStatus.Active;
            entity.CreateBy = entity.CreateBy != 0 ? entity.CreateBy : Convert.ToInt64(HttpContext.Current.User.Identity.GetUserId());
            
            entity.ModifyBy = entity.CreateBy;
            _session.Save(data);

        }

        public IList<TEntityT> LoadByCriteria(TEntityT dataobj, TEntityT searchCriteria, int currentPage, int pageSize)
        {
            Type type = typeof(TEntityT);
            var v = type.Name;

            var criteria = Session.CreateCriteria<TEntityT>();
            criteria.Add(Restrictions.Eq("Status", BaseEntity<TEntityT>.EntityStatus.Active));
            if (pageSize != -1)
            {
                criteria.SetFirstResult(((currentPage - 1) * pageSize));
                criteria.SetMaxResults(pageSize);
            }

            // criteria.
            var data = criteria.List<TEntityT>();
            return data;


        }

        public virtual void Delete(TEntityT data)
        {

            IBaseEntity<long> entity = (IBaseEntity<long>)data;
           _session.Delete(data);
        }

        public void DeleteEntity(TIdT id)
        {
            var e = (TEntityT)_session.Get(typeof(TEntityT), id);
            _session.Delete(e);
        }

        public void Update(TEntityT data)
        {

            IBaseEntity<long> entity = (IBaseEntity<long>)data;
            entity.ModificationDate = DateTime.Now;
            if (HttpContext.Current != null && HttpContext.Current.User != null && entity.ModifyBy<=0)
            {
                entity.ModifyBy = Convert.ToInt64(IdentityExtensions.GetUserId(HttpContext.Current.User.Identity));
            }

            _session.Update(data);


        }

        public void Marge(TEntityT data)
        {
            IBaseEntity<long> entity = (IBaseEntity<long>)data;
            if (entity.CreationDate.Year < 2016)
                entity.CreationDate = DateTime.Now;
            entity.ModificationDate = DateTime.Now;

            _session.Merge(data);
        }

        /// <summary>
        /// load active:1 inactive:-1 data
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public virtual IList<TEntityT> LoadAll(int? status = null)
        {
            string queryTxt = "";
            Type type = typeof(TEntityT);
            if (status == null)
            {
                queryTxt = string.Format(@"From {0} as entity where entity.Status!='-404'", type);
            }
            else
            {
                queryTxt = string.Format(@"From {0} as entity where entity.Status='" + status + "'", type);
            }

            IQuery query = _session.CreateQuery(queryTxt);

            return query.List<TEntityT>();
        }
        public virtual IList<TEntityT> LoadAllOk()
        {
            Type type = typeof(TEntityT);
            string queryTxt = string.Format(@"From {0} as entity where entity.Status!='-404'", type);
            IQuery query = _session.CreateQuery(queryTxt);
            return query.List<TEntityT>();
        }

        public virtual TEntityT LoadById(TIdT id)
        {
            IList list = new ArrayList();
            Type type = typeof(TEntityT);
            string query = String.Format("From {0} as e where e.Id = {1} and e.Status !='{2}'", type.Name, id, BaseEntity<TEntityT>.EntityStatus.Delete);
            list = Find(query);
            if (list.Count == 0)
                return null;
            return list[0] as TEntityT;
        }

        public virtual void SaveOrUpdate(TEntityT data)
        {
            IBaseEntity<long> entity = (IBaseEntity<long>)data; 
            entity.ModificationDate = DateTime.Now;
            if (HttpContext.Current != null && HttpContext.Current.User!=null)
            {
                entity.ModifyBy = Convert.ToInt64(IdentityExtensions.GetUserId(HttpContext.Current.User.Identity));
            }
            if (entity.Id < 1)
            {
                entity.CreationDate = entity.ModificationDate;
                entity.CreateBy = entity.ModifyBy;
            }
            _session.SaveOrUpdate(data);

        }

        public IList Find(string queryTxt)
        {
            IQuery query = _session.CreateQuery(queryTxt);
            //IQuery query = _session.CreateQuery("NAi");
            return query.List();

        }
        public int ExecuteQuery(string sql)
        {
            var query = Session.CreateSQLQuery(sql);
            int res = query.ExecuteUpdate();
            return res;
        }

        public IList Find(string queryTxt, object[] value)
        {
            IQuery query = _session.CreateQuery(queryTxt);
            for (int i = 0; i < value.Length; i++)
            {
                query.SetParameter(i, value[i]);
            }
            return query.List();
        }

        public IList FindByNamedQuery(string namedQuery)
        {
            IQuery query = _session.GetNamedQuery(namedQuery);
            return query.List();
        }

        public IList FindByNamedQueryAndParam(string namedQuery, object[] value)
        {
            IQuery query = _session.GetNamedQuery(namedQuery);

            if ((null != value))
            {
                for (int i = 0; i < value.Length; i++)
                {
                    query.SetParameter(i, value[i]);
                }
            }

            return query.List();
        }

        public IList<TEntityT> SearchListByFieldAndValue(string searchBy, object value)
        {
            return Session.CreateCriteria<TEntityT>()
                .Add(Restrictions.Eq(searchBy, value.ToString()))
                .Add(Restrictions.Not(Restrictions.Eq("Status", BaseEntity<TEntityT>.EntityStatus.Delete))).List<TEntityT>();
        }

        public IList<TEntityT> SearchListByFieldAndLikeValue(string searchBy, object value)
        {
            return Session.CreateCriteria<TEntityT>()
                  .Add(Restrictions.InsensitiveLike(searchBy, value.ToString(), MatchMode.Anywhere))
                  .Add(Restrictions.Not(Restrictions.Eq("Status", BaseEntity<TEntityT>.EntityStatus.Delete))).List<TEntityT>();
        }


        private PropertyInfo GetPropertyInfoByName(PropertyInfo[] properties, string searchBy)
        {
            foreach (PropertyInfo p in properties)
            {
                if (p.Name == searchBy)
                    return p;
            }
            return null;
        }

        /// <summary>
        /// Search entity using multiple field and multiple AND or Conditions. 
        /// </summary>
        /// <param name="searchBy">Array of multiple fileds name</param>
        /// <param name="value">Corrosponding value array of fields </param>
        /// <param name="andOrOperator"> Corrosping conditional operator.
        /// Like Name='masum' AND age >= 28 OR address LIKE shyamoli. 
        /// string[] operators is the array of corrosponding =,>=,!=, like,... etc 
        /// string[] conditionalAndOrOperator is the corrosponding AND,OR, !,  conditional operator array.</param>
        /// <returns>List of matched entity object</returns>
        public ArrayList SearchByFieldsAndValues(string[] searchBy, object[] values, string[] operators, string[] conditionalAndOrOperator)
        {

            Type type = typeof(TEntityT);

            string query;
            string conditions = "";

            if (searchBy.Length > 0)
            {
                int i = 0;
                for (; i < searchBy.Length - 1; i++)
                {
                    if (operators[i].ToLower() == "like")
                    {
                        values[i] += "%";
                    }

                    conditions += string.Format("entity.{0} {1} ? {2} ", searchBy[i], operators[i], conditionalAndOrOperator[i]);
                }

                conditions += string.Format("entity.{0} {1} ? ", searchBy[i], operators[i]);

                query = string.Format(@"From {0} as entity where {1} and entity.Status !=?", type.Name, conditions);
                object[] val = new object[values.Length + 1];

                int j = 0;
                for (; j < values.Length; j++)
                {
                    val[j] = values[j];
                }

                val[j] = (object)BaseEntity<TEntityT>.EntityStatus.Delete;

                return (ArrayList)Find(query, val);
            }
            return new ArrayList();

        }

        private FieldInfo GetFieldInfoByName(FieldInfo[] properties, string searchBy)
        {
            foreach (FieldInfo field in properties)
            {
                if (field.Name == searchBy)
                    return field;
            }
            return null;
        }

        public virtual int DeleteByNamedQuery(string queryName, object value, IType type)
        {
            object[] values = new object[] { value };
            IType[] types = new IType[] { type };
            return DeleteByNamedQuery(queryName, values, types);
        }

        public virtual int DeleteByNamedQuery(string queryName, object[] values, IType[] types)
        {
            IQuery query = _session.GetNamedQuery(queryName);

            return _session.Delete(query.QueryString, values, types);
        }


        public virtual IList FindByNamedQuery(string queryName, object value, IType type)
        {
            object[] values = new object[] { value };
            IType[] types = new IType[] { type };
            return FindByNamedQuery(queryName, values, types);
        }
        public virtual IList FindByNamedQuery(string queryName, object[] values, IType[] types)
        {
            IQuery query = _session.GetNamedQuery(queryName);

            if ((null != values) && (null != types))
            {
                for (int i = 0; i < values.Length; i++)
                {
                    query.SetParameter(i, values[i], types[i]);
                }
            }

            return query.List();
        }

        public virtual IList FindPage(string queryString, int pageIndex, int pageSize)
        {
            IQuery query = _session.CreateQuery(queryString);

            query.SetFirstResult(pageSize * pageIndex);
            query.SetMaxResults(pageSize);

            return query.List();
        }

        public virtual IList FindPageByNamedQuery(string queryName, int pageIndex, int pageSize)
        {
            return FindPageByNamedQuery(queryName, null, null, pageIndex, pageSize);
        }

        public IList FindPageByNamedQuery(string queryName, object value, IType type, int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public IList FindPageByNamedQuery(string queryName, object[] values, IType[] types, int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public virtual IList FindPageByQuery(string queryText, object[] values, int pageIndex, int pageSize)
        {

            IQuery query = _session.CreateQuery(queryText);
            if ((null != values))
            {
                for (int i = 0; i < values.Length; i++)
                {
                    query.SetParameter(i, values[i]);
                }
            }
            query.SetFirstResult(pageSize * pageIndex);
            query.SetMaxResults(pageSize);
            return query.List();
        }
        /// <summary>
        /// refactored function
        /// </summary>
        /// <param name="tEntityT"></param>
        /// <returns></returns>
        public int GetMaximumRank(TEntityT tEntityT)
        {
            int rValue = 0;
            ICriteria retObj = Session.CreateCriteria<TEntityT>().Add(Restrictions.Not(Restrictions.Eq("Status", BaseEntity<TEntityT>.EntityStatus.Delete))).SetProjection(Projections.Max("Rank"));
            var rankValue = retObj.UniqueResult();
            rValue = rankValue == null ? 0 : (Int32)rankValue;
            return rValue;
        }
        /// <summary>
        /// refactored function
        /// </summary>
        /// <param name="tEntityT"></param>
        /// <returns></returns>
        public int GetMinimumRank(TEntityT tEntityT)
        {
            int rValue = 0;
            ICriteria retObj = Session.CreateCriteria<TEntityT>().Add(Restrictions.Not(Restrictions.Eq("Status", BaseEntity<TEntityT>.EntityStatus.Delete))).SetProjection(Projections.Min("Rank"));
            var rankValue = retObj.UniqueResult();
            rValue = rankValue == null ? 0 : (Int32)rankValue;
            return rValue;
        }
        /// <summary>
        /// Author : Habib && Taskin 
        /// </summary>
        /// <param name="entityT">Pass Entity </param>
        /// <returns>Get Total Row</returns>
        public int RowCount(TEntityT entityT)
        {
            ICriteria criteria =
                  Session.CreateCriteria<TEntityT>();

            foreach (var data in entityT.GetType().GetProperties())
            {
                object proval = data.GetValue(entityT);
                var strvalue = "";
                int intValue;
                if (data.GetValue(entityT) != null && data.GetValue(entityT) != "" && proval.ToString() != "0")
                {
                    var type = data.PropertyType.Name;
                    var type1 = data.GetType().BaseType;
                    //var type = data.GetMethod.ReturnType.Name;
                    if (type == "String")
                    {
                        strvalue = (string)data.GetValue(entityT);
                        criteria.Add(Restrictions.Like(data.Name, strvalue, MatchMode.Anywhere));
                    }
                    if (type == "Int32")
                    {
                        intValue = Convert.ToInt32(data.GetValue(entityT));
                        criteria.Add(Restrictions.Eq(data.Name, intValue));
                    }
                }
            }

            IList list = criteria.Add(Restrictions.Not(Restrictions.Eq("Status", BaseEntity<TEntityT>.EntityStatus.Delete))).List();
            return list.Count;

        }

        public virtual IList FindPageByQuery(string queryText, object[] values, IType[] types, int pageIndex, int pageSize)
        {
            IQuery query = _session.CreateQuery(queryText);
            if ((null != values))
            {
                for (int i = 0; i < values.Length; i++)
                {
                    query.SetParameter(i, values[i], types[i]);
                }
            }
            query.SetFirstResult(pageSize * pageIndex);
            query.SetMaxResults(pageSize);
            return query.List();
        }

        public virtual int CountByNamedQuery(string queryName, object[] values, NHibernate.Type.IType[] types)
        {
            IQuery query = _session.GetNamedQuery(queryName);

            if ((null != values) && (null != types))
            {
                for (int i = 0; i < values.Length; i++)
                {
                    query.SetParameter(i, values[i], types[i]);
                }
            }
            return (int)query.UniqueResult();
        }


        //public TEntityT LoadByRank(TIdT rank)
        //{
        //    try
        //    {
        //        ICriteria retObj =Session.CreateCriteria<TEntityT>().Add(Restrictions.Not(Restrictions.Eq("Status", BaseEntity<TEntityT>.EntityStatus.Delete)))
        //                .Add(Restrictions.Eq("Rank", rank))
        //                .SetFirstResult(0).SetMaxResults(1);
        //        return (TEntityT)retObj.List();
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        /// <summary>
        /// refactored
        /// </summary>
        /// <param name="rank"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public TEntityT LoadByRankDirection(int rank, string action)
        {

            ICriteria criteria = Session.CreateCriteria<TEntityT>().Add(Restrictions.Not(Restrictions.Eq("Status", BaseEntity<TEntityT>.EntityStatus.Delete)));
            if (action == "down")
                criteria.Add(Restrictions.Ge("Rank", rank)).AddOrder(Order.Asc("Rank"));
            if (action == "up")
                criteria.Add(Restrictions.Le("Rank", rank)).AddOrder(Order.Desc("Rank"));
            return criteria.SetFirstResult(0).SetMaxResults(1).UniqueResult<TEntityT>();

        }
    }
}
