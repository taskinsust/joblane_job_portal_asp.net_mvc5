using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Joblanes.Helper
{
    [Serializable]
    public class UserProfileSessionData
    {
        public long UserId { get; set; } 

        public string EmailAddress { get; set; }

        public string FullName { get; set; }
    }
}