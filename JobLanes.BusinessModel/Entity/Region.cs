using Model.JobLanes.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.JobLanes.Entity
{
    public class Region : BaseEntity<long>
    {
        public Region()
        {
            Countries = new List<Country>();
        }
        public virtual IList<Country> Countries { get; set; }  
        
    }
}
