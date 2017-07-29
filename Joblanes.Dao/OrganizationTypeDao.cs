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
    
    public interface IOrganizationTypeDao : IBaseDao<OrganizationType, long>
    {
        #region Operational Function

        #endregion

        #region Single Instances Loading Function

        #endregion

        #region List Loading Function
        List<OrganizationType> LoadOrganizationType(int start, int length, string orderBy, string orderDir, string name, int status);
        #endregion

        #region Others Function
        int OrganizationTypeRowCount(string name, int status);
        bool CheckDuplicateFields(long id, string name);
        #endregion
    }
    public class OrganizationTypeDao : BaseDao<OrganizationType, long>, IOrganizationTypeDao
    {
        #region Propertise & Object Initialization

        #endregion

        #region Operational Function

        #endregion

        #region Single Instances Loading Function

        #endregion

        #region List Loading Function
        public List<OrganizationType> LoadOrganizationType(int start, int length, string orderBy, string orderDir, string name, int status)
        {

            ICriteria criteria = OrganizationTypeCriteria(name, status);

            if (!String.IsNullOrEmpty(orderBy))
            {
                criteria.AddOrder(orderDir == "ASC" ? Order.Asc(orderBy.Trim()) : Order.Desc(orderBy.Trim()));
            }

            return (List<OrganizationType>)criteria.SetFirstResult(start).SetMaxResults(length).List<OrganizationType>();    
        }



        #endregion

        #region Others Function
        public int OrganizationTypeRowCount(string name, int status)
        {
            ICriteria criteria = OrganizationTypeCriteria(name, status);

            criteria.SetProjection(Projections.RowCount());

            return Convert.ToInt32(criteria.UniqueResult());
        }

        public bool CheckDuplicateFields(long id, string name)
        {
            var organizationTypeQueryOver = Session.QueryOver<OrganizationType>().Where(x => x.Status != OrganizationType.EntityStatus.Delete); ;
            if (id > 0)
            {
                organizationTypeQueryOver = organizationTypeQueryOver.Where(x => x.Id != id);
            }

            if (!string.IsNullOrEmpty(name.Trim()))
            {
                organizationTypeQueryOver = organizationTypeQueryOver.Where(x => x.Name == name.ToString());
            }

            var rowcount = organizationTypeQueryOver.RowCount();
            return rowcount > 0;
        }

        #endregion

        #region Helper Function
        private ICriteria OrganizationTypeCriteria(string name, int status)
        {
            ICriteria criteria = Session.CreateCriteria<OrganizationType>().Add(Restrictions.Not(Restrictions.Eq("Status", OrganizationType.EntityStatus.Delete)));

            if (status != 0)
                criteria.Add(Restrictions.Eq("Status", status));

            if (!String.IsNullOrEmpty(name))
                criteria.Add(Restrictions.Like("Name", name, MatchMode.Anywhere));

            return criteria;
        }
        #endregion
    }  
}
