using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobLanes.WcfService.Models
{
    public class RegionListDto
    {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
        public virtual int Status { get; set; }
    }
}