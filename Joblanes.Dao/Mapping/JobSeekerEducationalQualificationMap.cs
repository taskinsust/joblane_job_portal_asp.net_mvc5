using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes.Mapping.Base;
using Model.JobLanes.Entity;

namespace Dao.Joblanes.Mapping
{
    class JobSeekerEducationalQualificationMap : BaseClassMap<JobSeekerEducationalQualification, long>
    {
        public JobSeekerEducationalQualificationMap()
        {
            Table("JobSeekerEducationalQualification");
            LazyLoad();

            Map(x => x.Degree).Column("Degree");
            Map(x => x.Institute).Column("Institute");
            Map(x => x.FieldOfStudy).Column("FieldOfStudy");
            Map(x => x.StartingYear).Column("StartingYear");
            Map(x => x.PassingYear).Column("PassingYear");
            Map(x => x.ImageGuid).Column("ImageGuid");
            Map(x => x.Cirtificate).Column("Cirtificate").CustomSqlType("VARBINARY (MAX) FILESTREAM").Length(2147483647).LazyLoad();
            Map(x => x.Result).Column("Result");

            References(x => x.JobSeeker).Column("JobSeekerId");
        }
    }
}
