using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes.Base;
using Model.JobLanes.Entity;

namespace Dao.Joblanes
{

    public interface IJobSeekerTrainingCoursesDao : IBaseDao<JobSeekerTrainingCourses, long>
    {
        #region Operational Function

        #endregion

        #region Single Instances Loading Function

        #endregion

        #region List Loading Function

        #endregion

        #region Others Function
        bool CheckDuplicateFields(long id, string title, DateTime fromTime, DateTime? totTime);
        #endregion


        bool CheckDuplicateFields(long id, long jobSeekerId, string institute, string title);
    }
    public class JobSeekerTrainingCoursesDao : BaseDao<JobSeekerTrainingCourses, long>, IJobSeekerTrainingCoursesDao
    {
        #region Propertise & Object Initialization

        #endregion

        #region Operational Function

        #endregion

        #region Single Instances Loading Function

        #endregion

        #region List Loading Function

        #endregion

        #region Others Function
        public bool CheckDuplicateFields(long id, string title, DateTime fromTime, DateTime? totTime)
        {
            var query = Session.QueryOver<JobSeekerTrainingCourses>().Where(x => x.Status == Region.EntityStatus.Active); ;
            if (id > 0)
            {
                query = query.Where(x => x.Id != id);
            }

            if (!string.IsNullOrEmpty(title.Trim()))
            {
                query = query.Where(x => x.Title == title.ToString());
            }
            if (fromTime != DateTime.MinValue || fromTime != DateTime.MaxValue)
            {
                query = query.Where(x => x.StartDate == fromTime);
            }
            if (totTime != DateTime.MinValue || totTime != DateTime.MaxValue)
            {
                query = query.Where(x => x.CloseDate == totTime);
            }
            var rowcount = query.RowCount();
            return rowcount > 0;
        }

        public bool CheckDuplicateFields(long id, long jobSeekerId, string institute, string title)
        {
            var query = Session.QueryOver<JobSeekerTrainingCourses>().Where(x => x.Status == Region.EntityStatus.Active); ;
            if (id > 0)
            {
                query = query.Where(x => x.Id != id);
            }

            if (!string.IsNullOrEmpty(title.Trim()))
            {
                query = query.Where(x => x.JobSeeker.Id == jobSeekerId && x.Title == title);
            }

            var rowcount = query.RowCount();
            return rowcount > 0;
        }

        #endregion

        #region Helper Function

        #endregion


    }
}
