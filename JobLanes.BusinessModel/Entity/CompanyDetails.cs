using Model.JobLanes.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.JobLanes.Entity
{
    public class CompanyDetails: BaseEntity<long> 
    {
        public CompanyDetails()
        {
        }

        public virtual string TradeLicence { get; set; }
        public virtual string WebLink { get; set; }
        public virtual string LinkdinLink { get; set; }
        public virtual string Vision { get; set; }
        public virtual string Mission { get; set; }
        public virtual string Description { get; set; }
        public virtual string Address { get; set; }
        public virtual string Zip { get; set; }
        public virtual string TagLine { get; set; }
        public virtual int EmployeeSize { get; set; }
        public virtual DateTime? EstablishedDate { get; set; }
        public virtual Country Country { get; set; }
        public virtual State State { get; set; }
        public virtual City City { get; set; }
        public virtual Company Company { get; set; }     
    }
}
