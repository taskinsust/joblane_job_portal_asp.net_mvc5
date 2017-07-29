using Model.JobLanes.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.JobLanes.Entity
{
    public class Country : BaseEntity<long> 
    {
        public Country()
        {
            States = new List<State>();
            Cities = new List<City>();
            CompanyDetailses = new List<CompanyDetails>();
            JobSeekerDetailses = new List<JobSeekerDetails>();
        }

        public virtual string ShortName { get; set; }
        public virtual Guid ImageGuid { get; set; }
        public virtual byte[] Flag { get; set; } 
        public virtual string CallingCode { get; set; }
        public virtual Region Region { get; set; }

        public virtual IList<State> States { get; set; }
        public virtual IList<City> Cities { get; set; }
        public virtual IList<CompanyDetails> CompanyDetailses { get; set; }
        public virtual IList<JobSeekerDetails> JobSeekerDetailses { get; set; }   
            
    }
}
