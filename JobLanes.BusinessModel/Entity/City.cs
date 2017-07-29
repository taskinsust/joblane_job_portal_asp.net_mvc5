using Model.JobLanes.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.JobLanes.Entity
{
    public class City: BaseEntity<long> 
    {
        public City()
        {
            CompanyDetailses = new List<CompanyDetails>();
            JobSeekerDetailses = new List<JobSeekerDetails>();
        }

        public virtual string ShortName { get; set; }
        public virtual Country Country { get; set; }
        public virtual State State { get; set; }

        public virtual IList<CompanyDetails> CompanyDetailses { get; set; }
        public virtual IList<JobSeekerDetails> JobSeekerDetailses { get; set; }   
    }

     
}
