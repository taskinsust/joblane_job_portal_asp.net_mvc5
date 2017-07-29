using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.JobLanes.Entity.Base;

namespace Model.JobLanes.Entity
{
   public class JobSeekerGroup:BaseEntity<long>
   {
       public virtual string Title { get; set; }
       public virtual bool IsStillMember { get; set; }
       //[ValidateDateRange]
       public virtual DateTime DateFrom { get; set; }
       //[ValidateDateRange]
       public virtual DateTime? DateTo { get; set; }
       public virtual string Description { get; set; }
       public virtual JobSeeker JobSeeker { get; set; }
    }
}
