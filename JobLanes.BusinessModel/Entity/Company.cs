using Model.JobLanes.Entity.Base;
using Model.JobLanes.Entity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.JobLanes.Entity
{
    public class Company: BaseEntity<long> 
    {
        public Company()
        {
            CompanyDetailses = new List<CompanyDetails>(); 
        }
        public virtual string ContactPerson { get; set; }
        public virtual Guid ImageGuid { get; set; }
        public virtual byte[] Logo { get; set; }

        public virtual string ContactPersonDesignation { get; set; }
        public virtual string ContactMobile { get; set; }
        public virtual string ContactEmail { get; set; }
        public virtual CompanyType CompanyType { get; set; }

        public virtual UserProfile UserProfile { get; set; }

        public virtual IList<CompanyDetails> CompanyDetailses { get; set; }
        public virtual IList<JobPost> JobPosts { get; set; }
        public virtual IList<ProfileView> CompanyJobPosts { get; set; } 
    }
}
