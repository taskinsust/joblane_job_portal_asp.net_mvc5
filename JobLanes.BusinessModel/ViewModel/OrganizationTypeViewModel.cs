﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.JobLanes.ViewModel
{
    public class OrganizationTypeViewModel : BaseViewModel
    {
        public virtual long Id { get; set; }
        [Required]
        public virtual string Name { get; set; }
        public virtual int Status { get; set; }
        public virtual string Description { get; set; } 
    }
}
