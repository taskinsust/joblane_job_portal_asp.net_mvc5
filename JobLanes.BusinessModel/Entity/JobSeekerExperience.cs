using Model.JobLanes.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.JobLanes.Entity
{
    public class JobSeekerExperience : BaseEntity<long>
    {
        public JobSeekerExperience()
        {

        }
        public virtual string CompanyName { get; set; }
        public virtual string CompanyAddress { get; set; }
        public virtual string Designation { get; set; }
        //[ValidateDateRange]
        public virtual DateTime DateFrom { get; set; }
        //[ValidateDateRange]
        public virtual DateTime? DateTo { get; set; }
        public virtual bool IsCurrent { get; set; }
        public virtual string Responsibility { get; set; }
        public virtual JobSeeker JobSeeker { get; set; }

    }
}
