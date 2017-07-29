using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Model.JobLanes.Dto
{
    public class JobSeekerDto : BaseDto
    {
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual byte[] ProfileImage { get; set; }
        public virtual string ContactNumber { get; set; }
        public virtual string ContactEmail { get; set; }
        public virtual UserProfileDto UserProfile { get; set; }
        public virtual IList<JobSeekerDetailsDto> JobSeekerDetailList { get; set; }
        public virtual IList<JobSeekerDesiredJobDto> JobSeekerDesiredJobList { get; set; }
        public virtual IList<JobSeekerExperienceDto> JobSeekerExperienceList { get; set; }
        public virtual IList<JobSeekerEducationDto> JobSeekerEducationList { get; set; }
        public virtual IList<JobSeekerSkillDto> JobSeekerSkillList { get; set; }
        public virtual IList<JobSeekerLinkDto> JobSeekerLinkList { get; set; }
        public virtual IList<JobSeekerMilitaryServiceDto> JobSeekerMilitaryServiceList { get; set; }
        public virtual IList<JobSeekerAwardDto> JobSeekerAwardList { get; set; }
        public virtual IList<JobSeekerCertificateDto> JobSeekerCertificateList { get; set; }
        public virtual IList<JobSeekerGroupDto> JobSeekerGroupList { get; set; }
        public virtual IList<JobSeekerPatentsDto> JobSeekerPatentsList { get; set; }
        public virtual IList<JobSeekerPublicationsDto> JobSeekerPublicationsList { get; set; }
        public virtual IList<JobSeekerAdditionalInformationDto> JobSeekerAdditionalInformationList { get; set; }       

    }
}
