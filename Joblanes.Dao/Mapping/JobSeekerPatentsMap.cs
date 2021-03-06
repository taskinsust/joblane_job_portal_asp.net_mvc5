﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes.Mapping.Base;
using Model.JobLanes.Entity;

namespace Dao.Joblanes.Mapping
{
    public class JobSeekerPatentsMap : BaseClassMap<JobSeekerPatents, long>
    {
        public JobSeekerPatentsMap()
        {
            Table("JobSeekerPatents");
            LazyLoad();
            References(x => x.JobSeeker).Column("JobSeekerId");
            Map(x => x.PatentNo).Column("PatentNo");
            Map(x => x.Title).Column("Title");
            Map(x => x.Url).Column("Url");
            Map(x => x.PublishDate).Column("PublishDate");
            Map(x => x.Description).Column("Description");
        }
    }
}
