using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.JobLanes.Dto
{
    public class JobSeekerDetailsDto : BaseDto
    {
        public virtual string FatherName { get; set; }
        public virtual string MotherName { get; set; }
        public virtual string Address { get; set; }
        public virtual string ZipCode { get; set; }
        public virtual byte[] Cv { get; set; }
        public virtual DateTime Dob { get; set; }
        public virtual int Gender { get; set; }
        public virtual int MaritalStatus { get; set; }
        public virtual CountryDto Country { get; set; }
        public virtual StateDto State { get; set; }
        public virtual CityDto City { get; set; }
        public virtual JobSeekerDto JobSeeker { get; set; }    
    }
}
