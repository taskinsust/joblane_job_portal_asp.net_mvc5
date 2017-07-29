using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.JobLanes.Entity;

namespace Model.JobLanes.Dto
{
    public class CompanyDetailsDto : BaseDto
    {
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
        public virtual CountryDto Country { get; set; }
        public virtual StateDto State { get; set; }
        public virtual CityDto City { get; set; }
        public virtual CompanyDto Company { get; set; }    
    }
}
