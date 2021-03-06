﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.JobLanes.Entity.Base;

namespace Model.JobLanes.Entity
{
    public class JobSeekerAward : BaseEntity<long>
    {
        public virtual string Title { get; set; }
        public virtual DateTime DateAwarded { get; set; }
        public virtual string Description { get; set; }
        public virtual JobSeeker JobSeeker { get; set; }
    }
}
