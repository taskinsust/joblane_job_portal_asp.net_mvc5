using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.JobLanes.Dto
{
    public class JobSeekerAwardDto
    {
        public virtual long Id { get; set; }
        [Required(ErrorMessage = "Please enter Title")]
        public virtual string Title { get; set; }

        [Required(ErrorMessage = "Please enter Date")]
        [Display(Name = "Date Awarded")]
        public virtual DateTime DateAwarded { get; set; }
        [Display(Name = "Description")]
        public virtual string Description { get; set; }

        [Required(ErrorMessage = "Please enter Description")]
        public virtual string JobSeeker { get; set; }
    }
}
