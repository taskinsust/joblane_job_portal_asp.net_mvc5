using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes.Mapping.Base;
using Model.JobLanes.Entity;

namespace Dao.Joblanes.Mapping
{
    public class JobSeekerTrainingCoursesMap : BaseClassMap<JobSeekerTrainingCourses, long> 
    {
        public JobSeekerTrainingCoursesMap()
        {
            Table("JobSeekerTrainingCourses");
            LazyLoad();

            Map(x => x.Title).Column("Title");
            Map(x => x.Description).Column("Description");
            Map(x => x.Institute).Column("Institute");
            Map(x => x.StartDate).Column("StartDate");
            Map(x => x.CloseDate).Column("CloseDate");
            Map(x => x.ImageGuid).Column("ImageGuid");
            Map(x => x.Cirtificate).Column("Cirtificate").Column("Cirtificate").CustomSqlType("VARBINARY (MAX) FILESTREAM").Length(2147483647).LazyLoad();

            References(x => x.JobSeeker).Column("JobSeekerId");
        }
    }
}
