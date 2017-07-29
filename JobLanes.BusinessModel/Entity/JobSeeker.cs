using Model.JobLanes.Entity.Base;
using Model.JobLanes.Entity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.JobLanes.Entity
{
    public class JobSeeker : BaseEntity<long>
    {
        public JobSeeker()
        {
            JobSeekerDetailses = new List<JobSeekerDetails>();
            JobSeekerEducationalQualifications = new List<JobSeekerEducationalQualification>();
            JobSeekerExperiences = new List<JobSeekerExperience>();
            JobSeekerTrainingCoursesList = new List<JobSeekerTrainingCourses>();
            JobSeekerJobPosts = new List<JobSeekerJobPost>();
            JobSeekerDesiredJobs= new List<JobSeekerDesiredJob>();
            JobSeekerSkills = new List<JobSeekerSkill>();
            JobSeekerLinks = new List<JobSeekerLink>();
            JobSeekerMilitarys = new List<JobSeekerMilitary>();
            JobSeekerGroups = new List<JobSeekerGroup>();
            JobSeekerPatentses = new List<JobSeekerPatents>();
            JobSeekerPublicationses = new List<JobSeekerPublications>();
            JobSeekerAdditionalInformations = new List<JobSeekerAdditionalInformation>();
        }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual Guid ImageGuid { get; set; }
        public virtual byte[] ProfileImage { get; set; }
        public virtual string ContactNumber { get; set; }
        public virtual string ContactEmail { get; set; }
        public virtual UserProfile UserProfile { get; set; }

        public virtual IList<JobSeekerDetails> JobSeekerDetailses { get; set; }
        public virtual IList<JobSeekerEducationalQualification> JobSeekerEducationalQualifications { get; set; }
        public virtual IList<JobSeekerExperience> JobSeekerExperiences { get; set; }
        public virtual IList<JobSeekerTrainingCourses> JobSeekerTrainingCoursesList { get; set; }
        public virtual IList<JobSeekerJobPost> JobSeekerJobPosts { get; set; }
        public virtual IList<JobSeekerDesiredJob> JobSeekerDesiredJobs { get; set; }
        public virtual IList<JobSeekerSkill> JobSeekerSkills { get; set; }
        public virtual IList<JobSeekerLink> JobSeekerLinks { get; set; }
        public virtual IList<JobSeekerMilitary> JobSeekerMilitarys { get; set; }
        public virtual IList<JobSeekerAward> JobSeekerAwards { get; set; }
        public virtual IList<JobSeekerGroup> JobSeekerGroups { get; set; }
        public virtual IList<JobSeekerPatents> JobSeekerPatentses { get; set; }
        public virtual IList<JobSeekerPublications> JobSeekerPublicationses { get; set; }
        public virtual IList<JobSeekerAdditionalInformation> JobSeekerAdditionalInformations { get; set; }   
                     
    }
}
