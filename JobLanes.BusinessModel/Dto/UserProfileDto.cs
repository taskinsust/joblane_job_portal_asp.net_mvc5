using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.JobLanes.Dto
{
    public class UserProfileDto : BaseDto
    {
        public virtual string NickName { get; set; }
        public virtual byte[] Image { get; set; }
        public virtual string AspNetUserId { get; set; }
        public virtual bool IsBlock { get; set; }
    }
}
