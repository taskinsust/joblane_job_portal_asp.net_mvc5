using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.JobLanes.Dto
{
    public class CountryDto : BaseDto 
    {
        public virtual string ShortName { get; set; }
        public virtual string CallingCode { get; set; }
        public virtual byte[] FlagBytes { get; set; } 
        public virtual RegionDto Region { get; set; }  
    }
}
