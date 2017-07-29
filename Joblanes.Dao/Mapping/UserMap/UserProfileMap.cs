using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes.Mapping.Base;
using Model.JobLanes.Entity.User;

namespace Dao.Joblanes.Mapping.UserMap
{
    public partial class UserProfileMap : BaseClassMap<UserProfile, long>
    {
        public UserProfileMap()
        {
            Table("UserProfile");
            LazyLoad();
            Polymorphism.Explicit();
            //Map(x => x.Name).Column("Name");
            Map(x => x.NickName).Column("NickName");
            Map(x => x.IsBlock).Column("IsBlock");
            Map(x => x.AspNetUserId).Column("AspNetUserId");
          
        }
    }
}
