using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes.Base;
using Model.JobLanes.Entity;

namespace Dao.Joblanes
{
    public interface IJobSeekerSkillDao : IBaseDao<JobSeekerSkill, long>
    {
        #region Operational Function

        #endregion

        #region Single Instances Loading Function
        IList<JobSeekerSkill> GetByJobSeekerId(long jobSeekerId);
        #endregion

        #region List Loading Function

        IList<JobSeekerSkill> LoadByJobSeekerId(long jobSeekerId);

        #endregion

        #region Others Function

        #endregion

        bool CheckDuplicateFields(long id, long jobSeekerId, string skill);
    }
    public class JobSeekerSkillDao : BaseDao<JobSeekerSkill, long>, IJobSeekerSkillDao
    {
        #region Propertise & Object Initialization

        #endregion

        #region Operational Function

        #endregion

        #region Single Instances Loading Function
        public IList<JobSeekerSkill> GetByJobSeekerId(long jobSeekerId)
        {
            return Session.QueryOver<JobSeekerSkill>().Where(x => x.JobSeeker.Id == jobSeekerId).List<JobSeekerSkill>();
        }
        #endregion

        #region List Loading Function
        public IList<JobSeekerSkill> LoadByJobSeekerId(long jobSeekerId)
        {
            return
                Session.QueryOver<JobSeekerSkill>()
                    .Where(x => x.JobSeeker.Id == jobSeekerId && x.Status == JobSeekerSkill.EntityStatus.Active)
                    .List<JobSeekerSkill>();
        }



        #endregion

        #region Others Function
        public bool CheckDuplicateFields(long id, long jobSeekerId, string skill)
        {
            var query = Session.QueryOver<JobSeekerSkill>().Where(x => x.Status == Region.EntityStatus.Active); ;
            if (id > 0)
            {
                query = query.Where(x => x.Id != id);
            }

            if (!string.IsNullOrEmpty(skill.Trim()))
            {
                query = query.Where(x => x.Skill == skill.ToString() && x.JobSeeker.Id == jobSeekerId);
            }

            var rowcount = query.RowCount();
            return rowcount > 0;
        }
        #endregion

        #region Helper Function

        #endregion


    }

}
