using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Joblanes.Models.ViewModel
{
    public class UserVm
    {
        public virtual string Email { get; set; }
        public virtual string Name { get; set; }
        public virtual string Role { get; set; }
        public virtual bool IsBlock { get; set; }
        public virtual string AspNetUserId { get; set; }
    }
}