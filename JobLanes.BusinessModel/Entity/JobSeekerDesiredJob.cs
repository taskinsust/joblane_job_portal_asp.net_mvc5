using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.JobLanes.Entity.Base;

namespace Model.JobLanes.Entity
{
    public class JobSeekerDesiredJob : BaseEntity<long>
    {
        public virtual int DesiredSalary { get; set; }
        public virtual int SalaryDurationInDay { get; set; }
        public virtual bool IsRelocate { get; set; }
        public virtual int EmploymentEligibility { get; set; }
        public virtual bool IsPartTime { get; set; }
        public virtual bool IsInternship { get; set; }
        public virtual bool IsTemporary { get; set; }
        public virtual bool IsFullTime { get; set; }
        public virtual bool IsCommission { get; set; }
        public virtual bool IsContract { get; set; }
        public virtual string RelocatingPlaceOne { get; set; }
        public virtual string RelocatingPlaceTwo { get; set; }
        public virtual string RelocatingPlaceThree { get; set; }
        public virtual JobSeeker JobSeeker { get; set; }

    }
}
