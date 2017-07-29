using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes.Mapping.Base;
using Model.JobLanes.Entity;

namespace Dao.Joblanes.Mapping
{
    public class JobCategoryMap : BaseClassMap<JobCategory, long> 
    {
        public JobCategoryMap()
        {
            Table("JobCategory");
            LazyLoad();

            Map(x => x.Name).Column("Name");
            Map(x => x.Description).Column("Description");
        }
    }
}
