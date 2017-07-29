using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.JobLanes.Entity.Base;

namespace Model.JobLanes.Entity.User
{
    public class UserProfile : BaseEntity<long>
    {
        public UserProfile()
        {
        }

        public virtual string NickName { get; set; }
        public virtual byte[] Image { get; set; }
        public virtual string AspNetUserId { get; set; }
        public virtual bool IsBlock { get; set; }
    }
}
