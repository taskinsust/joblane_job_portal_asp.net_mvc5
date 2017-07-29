using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes.Base;
using Model.JobLanes.Dto;
using Model.JobLanes.Entity.User;
using NHibernate;

namespace Dao.Joblanes
{
    public interface IUserDao : IBaseDao<UserProfile, long>
    {

        UserProfile GetByAspNetUserId(string aspnetUserId);
        string GetUserNameByUserProfileId(long id); 
    }

    public class UserDao : BaseDao<UserProfile, long>, IUserDao
    {
        public UserDao()
        {

        }

        public UserProfile GetByAspNetUserId(string aspnetUserId)
        {
            return
                Session.QueryOver<UserProfile>()
                    .Where(x => x.AspNetUserId == aspnetUserId)
                    .SingleOrDefault<UserProfile>();
        }

        public string GetUserNameByUserProfileId(long id)
        {
            string query = @"Select au.UserName from [dbo].[UserProfile] as up
                                inner join [dbo].[AspNetUsers] as au on up.AspNetUserId = au.Id
                                where up.Id = "+id;
            IQuery iQuery = Session.CreateSQLQuery(query);
            iQuery.SetTimeout(2000);
            return iQuery.UniqueResult().ToString();
        }
    }
}
