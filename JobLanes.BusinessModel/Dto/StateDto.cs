using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.JobLanes.Dto
{
    public class StateDto : BaseDto  
    {
        public virtual string ShortName { get; set; }
        public virtual CountryDto Country { get; set; }  
    }
}
