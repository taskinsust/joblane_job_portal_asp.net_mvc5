using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes.Mapping.Base;
using Model.JobLanes.Entity;

namespace Dao.Joblanes.Mapping
{
    public class JobSeekerDetailsMap : BaseClassMap<JobSeekerDetails, long> 
    {
        public JobSeekerDetailsMap()
        {
            Table("JobSeekerDetails");
            LazyLoad();

            Map(x => x.FatherName).Column("FatherName");
            Map(x => x.MotherName).Column("MotherName");
            Map(x => x.Address).Column("Address");
            Map(x => x.ZipCode).Column("ZipCode");
            Map(x => x.Linkedin).Column("Linkedin");
            Map(x => x.Weblink).Column("Weblink");
            Map(x => x.ImageGuid).Column("ImageGuid");
            Map(x => x.Cv).Column("Cv").CustomSqlType("VARBINARY (MAX) FILESTREAM").Length(2147483647).LazyLoad();
            Map(x => x.Dob).Column("Dob");
            Map(x => x.Gender).Column("Gender");
            Map(x => x.MaritalStatus).Column("MaritalStatus");
            Map(x => x.Expertise).Column("Expertise");

            References(x => x.Country).Column("CountryId");
            References(x => x.State).Column("StateId");
            References(x => x.City).Column("CityId");
            References(x => x.Region).Column("RegionId");
            References(x => x.JobSeeker).Column("JobSeekerId");

        }
    }
}
