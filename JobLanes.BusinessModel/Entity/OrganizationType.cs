
using Model.JobLanes.Entity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.JobLanes.Entity
{
    public class OrganizationType: BaseEntity<long> 
    {
        public OrganizationType()
        {
        }

        public virtual string Description { get; set; }
    
    }
}
