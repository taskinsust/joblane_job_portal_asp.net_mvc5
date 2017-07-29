using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes.Base;
using Model.JobLanes.Dto;
using Model.JobLanes.Entity;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;

namespace Dao.Joblanes
{

    public interface IJobSeekerDao : IBaseDao<JobSeeker, long>
    {
        #region Operational Function

        #endregion

        #region Single Instances Loading Function
        JobSeeker GetUsProfileId(long userProfileId);
        JobSeeker GetAllByProfileId(long userProfileId);
        JobSeeker GetJobSeekerById(long jobSeekerId, int status); 
        #endregion

        #region List Loading Function
        IList<JobSeeker> LoadOnlyJobSeeker(int status);
        IList<JobSeeker> LoadJobSeekerForWebAdmin(int start, int length, string orderBy, string orderDir, int status, string firstName, string lastName, string contactMobile, string contactEmail, string zip, long region, long country, long state, long city);

        List<JobSeekerSearchDto> LoadJobSeekerSearch(int start, int length, string orderBy, string orderDir, string whatKey, string whereKey
            ,string[] jobTitle, string[] yearExp, string[] education, string[] company
            );

        List<FindResumeFilterDto> LoadFindResumeFiteDtoList(); 
        #endregion

        #region Others Function
        int JobSeekerRowCount(int status, string firstName, string lastName, string contactMobile, string contactEmail, string zip, long region, long country, long state, long city);
        int JobSeekerSearchRowCount(string whatKey, string whereKey, string[] jobTitle, string[] yearExp, string[] education, string[] company); 
        #endregion
        
    }
    public class JobSeekerDao : BaseDao<JobSeeker, long>, IJobSeekerDao
    {
        #region Propertise & Object Initialization

        #endregion

        #region Operational Function

        #endregion

        #region Single Instances Loading Function
        public JobSeeker GetUsProfileId(long userProfileId)
        {
            return
                Session.QueryOver<JobSeeker>()
                    .Where(x => x.UserProfile.Id == userProfileId && x.Status == JobSeeker.EntityStatus.Active)
                    .SingleOrDefault<JobSeeker>();

        }
        public JobSeeker GetAllByProfileId(long userProfileId)
        {
            return
                Session.QueryOver<JobSeeker>()
                    .Where(x => x.UserProfile.Id == userProfileId)
                    .SingleOrDefault<JobSeeker>();

        }

        public JobSeeker GetJobSeekerById(long jobSeekerId, int status)
        {
            return
                  Session.QueryOver<JobSeeker>()
                      .Where(x => x.Id == jobSeekerId && x.Status == status)
                      .SingleOrDefault<JobSeeker>();
        }

        #endregion

        #region List Loading Function
        public IList<JobSeeker> LoadOnlyJobSeeker(int status)
        {
            ICriteria criteria = Session.CreateCriteria<JobSeeker>().Add(Restrictions.Not(Restrictions.Eq("Status", JobSeeker.EntityStatus.Delete)));

            if (status != 0)
                criteria.Add(Restrictions.Eq("Status", status));

            return criteria.List<JobSeeker>().OrderBy(x => x.Name).ToList();
        }

        public IList<JobSeeker> LoadJobSeekerForWebAdmin(int start, int length, string orderBy, string orderDir, int status, string firstName,
            string lastName, string contactMobile, string contactEmail, string zip, long region, long country, long state,
            long city)
        {
            ICriteria criteria = JobSeekerCriteria(status, firstName, lastName, contactMobile, contactEmail, zip,
               region, country, state, city);

            if (!String.IsNullOrEmpty(orderBy))
            {
                criteria.AddOrder(orderDir == "ASC" ? Order.Asc(orderBy.Trim()) : Order.Desc(orderBy.Trim()));
            }
            return criteria.List<JobSeeker>() as List<JobSeeker>;
            // return (List<JobSeeker>)criteria.SetFirstResult(start).SetMaxResults(length).List<JobSeeker>();  
        }

        public List<JobSeekerSearchDto> LoadJobSeekerSearch(int start, int length, string orderBy, string orderDir, string whatKey, string whereKey
            , string[] jobTitle, string[] yearExp, string[] education, string[] company
            )
        {
            string innerQuery = GetInnerQueryJobSeekerSearch(whatKey, whereKey,jobTitle, yearExp, education, company);

            string pagination = " ";
            if (length > 0)
            {
                pagination = " where pagination.RowNum BETWEEN (@start) AND (@start + @rowsperpage-1) ";
            }
//            string query =
//                @"DECLARE @rowsperpage INT 
//                            DECLARE @start INT 
//                            SET @start = " + (start + 1) + @"
//                            SET @rowsperpage = " + length + @"
//
//                            Select * FROM (
//	                            SELECT row_number() OVER (ORDER BY A.JobSeekerId asc) AS RowNum, A.* FROM (" + innerQuery + @" ) as A ) as pagination " + pagination;
            string secondInnerQuery = @"Select a.*
                                        ,Je.CompanyName
                                        ,je.Designation
                                        ,je.DateFrom as ExpFromDate
                                        ,je.DateTo as ExpToDate
                                        from (
	                                        " + innerQuery + @"
                                        ) as a
                                        left join [dbo].[JobSeekerExperience] as je on je.JobSeekerId = a.JobSeekerId AND je.Status = 1";
            string query =
               @"DECLARE @rowsperpage INT 
                            DECLARE @start INT 
                            SET @start = " + (start + 1) + @"
                            SET @rowsperpage = " + length + @"

                            Select * FROM (
	                            SELECT row_number() OVER (ORDER BY A.JobSeekerId asc) AS RowNum, A.* FROM (" + secondInnerQuery + @" ) as A ) as pagination " + pagination;

            IQuery iQuery = Session.CreateSQLQuery(query);
            iQuery.SetResultTransformer(Transformers.AliasToBean<JobSeekerSearchDto>());
            iQuery.SetTimeout(5000);
            return iQuery.List<JobSeekerSearchDto>().ToList();
        }

        public List<FindResumeFilterDto> LoadFindResumeFiteDtoList()
        {
            string query = @"Select a.Title,a.TotalResume,a.Type from ( 
	                            Select a.*,row_number() OVER (ORDER BY a.TotalResume desc) AS RowNum from (
		                            Select a.Designation as Title, count(*) as TotalResume, 1 as Type from (
			                            Select Designation,JobSeekerId from [dbo].[JobSeekerExperience] WHERE [status] = "+JobSeekerExperience.EntityStatus.Active+@" group by JobSeekerId,Designation
		                            ) as a group by a.Designation
	                            ) as a 
                            ) as a where a.RowNum<=5
                            union all
                            Select a.Title,a.TotalResume,a.Type from ( 
	                            Select a.*,row_number() OVER (ORDER BY a.TotalResume desc) AS RowNum from (
		                            Select a.CompanyName as Title, count(*) as TotalResume, 4 as Type from (
			                            Select CompanyName,JobSeekerId from [dbo].[JobSeekerExperience] WHERE [status] = " + JobSeekerExperience.EntityStatus.Active + @" group by JobSeekerId,CompanyName
		                            ) as a group by a.CompanyName
	                            ) as a 
                            ) as a where a.RowNum<=5
                            union all
                            Select a.Title,a.TotalResume,a.Type from ( 
	                            Select a.*,row_number() OVER (ORDER BY a.TotalResume desc) AS RowNum from (
		                            Select a.Degree as Title, count(*) as TotalResume, 3 as Type from (
			                            Select Degree,JobSeekerId from [dbo].[JobSeekerEducationalQualification] WHERE [status] = " + JobSeekerEducationalQualification.EntityStatus.Active + @"  
			                            group by JobSeekerId,Degree
		                            ) as a group by a.Degree
	                            ) as a 
                            ) as a where a.RowNum<=5
                            union all
                            Select a.Title,a.TotalResume,a.Type from ( 
	                            Select a.*,row_number() OVER (ORDER BY a.TotalResume desc) AS RowNum from (
		                            Select a.expType as Title, count(*) as TotalResume, 2 as Type from (
			                            Select expType,JobSeekerId from (
				                            Select JobSeekerId, SUM(experence) as totalExp 
				                            ,(CASE 
					                            WHEN SUM(experence) IS NULL OR SUM(experence) = 0 THEN '0'
					                            WHEN SUM(experence) >=1 AND SUM(experence)<=2 THEN '1'
					                            WHEN SUM(experence) >=3 AND SUM(experence)<=5 THEN '3'
					                            WHEN SUM(experence) >=6 AND SUM(experence)<=10 THEN '6'
					                            WHEN SUM(experence) >10 THEN '10'
				                            END) as expType
				                            from (
					                            Select JobSeekerId, DateFrom, DateTo
					                            ,DATEDIFF(yy, DateFrom,CASE WHEN  DateTo IS NULL THEN GETDATE() ELSE DateTo END) as experence
					                            from [dbo].[JobSeekerExperience] WHERE [status] = " + JobSeekerExperience.EntityStatus.Active + @" 
				                            ) as a group by a.JobSeekerId
			                            ) as a
			                            group by JobSeekerId,expType
		                            ) as a group by a.expType
	                            ) as a 
                            ) as a where a.RowNum<=5";
            IQuery iQuery = Session.CreateSQLQuery(query);
            iQuery.SetResultTransformer(Transformers.AliasToBean<FindResumeFilterDto>());
            iQuery.SetTimeout(5000);
            return iQuery.List<FindResumeFilterDto>().ToList();
        }

        #endregion

        #region Others Function
        public int JobSeekerRowCount(int status, string firstName, string lastName, string contactMobile, string contactEmail,
            string zip, long region, long country, long state, long city)
        {
            ICriteria criteria = JobSeekerCriteria(status, firstName, lastName, contactMobile, contactEmail, zip,
                  region, country, state, city);

            criteria.SetProjection(Projections.RowCount());

            return Convert.ToInt32(criteria.UniqueResult());
        }

        public int JobSeekerSearchRowCount(string whatKey, string whereKey, string[] jobTitle, string[] yearExp, string[] education, string[] company)
        {
            string innerQuery = GetInnerQueryJobSeekerSearch(whatKey, whereKey, jobTitle,yearExp,education,company);
            //string query = "SELECT count(A.JobSeekerId) as total FROM( Select a.JobSeekerId from ( " + innerQuery + " ) as a group by a.JobSeekerId ) as A";
            string query = "SELECT count(A.JobSeekerId) as total FROM( " + innerQuery + " ) as A";
            IQuery iQuery = Session.CreateSQLQuery(query);
            iQuery.SetTimeout(2000);
            return Convert.ToInt32(iQuery.UniqueResult());
        }

        private string GetInnerQueryJobSeekerSearch(string whatKey, string whereKey, string[] jobTitle, string[] yearExp, string[] education, string[] company)
        {
            string condition = "";
            string totalExperienceQuery = "";

            if (!string.IsNullOrEmpty(whereKey))
            {
                condition +=
                    " AND (jd.ZipCode like '%" + whereKey + "%' OR jd.Address like '%" + whereKey + "%' OR c.Name like '%" + whereKey + "%' OR s.Name Like '%" + whereKey + "%' OR co.Name like '%" + whereKey + "%')";
            }

            if (!string.IsNullOrEmpty(whatKey))
            {
                condition +=
                    " AND (je.CompanyName like '%" + whatKey + "%' OR je.Designation like '%" + whatKey + "%' OR sk.Skill like '%" + whatKey + "%' OR eq.Degree LIKE '%" + whatKey + "%' OR eq.FieldOfStudy LIKE '%" + whatKey + "%')";
            }
            if (jobTitle != null && jobTitle.Any())
            {
                condition += " AND je.Designation IN ('" + string.Join("','", jobTitle) + "')";
            }
            if (education != null && education.Any())
            {
                condition += " AND eq.Degree IN ('" + string.Join("', '", education) + "')";
            }
            if (company != null && company.Any())
            {
                condition += " AND je.CompanyName IN ('" + string.Join("','", company) + "')";
            }

            if(yearExp!=null && yearExp.Any())
            {
                totalExperienceQuery = @" left join (
							                Select JobSeekerId, SUM(experence) as totalExp from (
								                Select JobSeekerId, DateFrom, DateTo
								                ,DATEDIFF(yy, DateFrom,CASE WHEN  DateTo IS NULL THEN GETDATE() ELSE DateTo END) as experence
								                from [dbo].[JobSeekerExperience] WHERE [Status] = 1
							                ) as a group by a.JobSeekerId
						                ) as te ON te.JobSeekerId = j.Id ";

                
                string tf = "";
                int i = 0;
                string ors = "";
                foreach (var ye in yearExp) 
                {
                    if (i>0)
                    {
                        ors = "OR";
                    }
                    switch (ye)
                    {
                        case "0":
                            tf +=  ors +" te.totalExp = 0 OR  te.totalExp IS NULL ";
                            break;
                        case "1":
                            tf += ors + " te.totalExp = 1 OR  te.totalExp = 2 ";
                            break;
                        case "3":
                            tf += ors + " (te.totalExp >= 3 AND  te.totalExp <= 5)";
                            break;
                        case "6":
                            tf += ors + " (te.totalExp >= 6 AND  te.totalExp <= 10)";
                            break;
                        case "10":
                            tf += ors + " (te.totalExp > 10)";
                            break;
                    }
                    i++;
                }



              


                condition += " AND ( " + tf + " ) ";
               
                //condition += " AND te.totalExp IN (" + string.Join(",", yearExp) + ")";
            }

            string query = "";
            query = @"Select a.* from (
	                    Select j.Id as JobSeekerId, j.ContactEmail,j.ContactNumber
                            ,CONCAT(j.FirstName,' ',j.LastName) as JobSeekerName
                            ,jd.ZipCode 
                            ,CONCAT(c.Name,' ',s.Name,' ',co.Name) as JobSeekerLocation                          
                            from [dbo].[JobSeeker] as j
                            left JOIN [dbo].[JobSeekerDetails] as jd On jd.JobSeekerId = j.Id
                            left JOIN [dbo].[City] as c on c.Id = jd.CityId
                            left JOIN [dbo].[Country] as co on co.Id = jd.CountryId
                            left JOIN [dbo].[State] as s on s.Id = jd.StateId
                            left join [dbo].[JobSeekerExperience] as je on je.JobSeekerId = j.Id AND je.Status = 1
                            left join [dbo].[JobSeekerSkill] as sk on sk.JobSeekerId = j.Id AND sk.Status = 1
                            left join [dbo].[JobSeekerEducationalQualification] as eq on eq.JobSeekerId = j.Id AND eq.Status = 1
						    " + totalExperienceQuery + @"
                            WHERE j.Status = 1 
						   " + condition + @"
                        ) as a  WHERE a.JobSeekerName!='' group by a.JobSeekerId, a.ContactEmail, a.ContactNumber, a.JobSeekerName, 
                    a.ZipCode, a.JobSeekerLocation";
//            query = @"Select a.* from (
//	                    Select j.Id as JobSeekerId, j.ContactEmail,j.ContactNumber
//                            ,CONCAT(j.FirstName,' ',j.LastName) as JobSeekerName
//                            ,jd.ZipCode 
//                            ,CONCAT(c.Name,' ',s.Name,' ',co.Name) as JobSeekerLocation
//                            --,c.Name as City
//                            --,s.Name as StateName
//                            --,co.Name as Country
//                            ,Je.CompanyName
//                            ,je.Designation
//                            ,je.DateFrom as ExpFromDate
//                            ,je.DateTo as ExpToDate
//                            -- ,sk.Skill as Skill
//                            -- ,sk.Experence as SkillExp
//                            -- ,eq.Degree
//                            -- ,eq.FieldOfStudy
//						    --,te.totalExp
//                            from [dbo].[JobSeeker] as j
//                            left JOIN [dbo].[JobSeekerDetails] as jd On jd.JobSeekerId = j.Id
//                            left JOIN [dbo].[City] as c on c.Id = jd.CityId
//                            left JOIN [dbo].[Country] as co on co.Id = jd.CountryId
//                            left JOIN [dbo].[State] as s on s.Id = jd.StateId
//                            left join [dbo].[JobSeekerExperience] as je on je.JobSeekerId = j.Id AND je.Status = 1
//                            left join [dbo].[JobSeekerSkill] as sk on sk.JobSeekerId = j.Id AND sk.Status = 1
//                            left join [dbo].[JobSeekerEducationalQualification] as eq on eq.JobSeekerId = j.Id AND eq.Status = 1
//						    "+totalExperienceQuery+@"
//                            WHERE j.Status = 1 
//						   " + condition + @"
//                        ) as a group by a.JobSeekerId, a.ContactEmail, a.ContactNumber, a.JobSeekerName, 
//                    a.ZipCode, a.JobSeekerLocation, a.CompanyName,a.Designation, a.ExpFromDate,a.ExpToDate ";

            return query;
        }

        #endregion

        #region Helper Function
        private ICriteria JobSeekerCriteria(int status, string firstName, string lastName, string contactMobile, string contactEmail, string zip, long region, long country, long state, long city)
        {
            ICriteria criteria = Session.CreateCriteria<JobSeeker>().Add(Restrictions.Not(Restrictions.Eq("Status", JobSeeker.EntityStatus.Delete)));

            criteria.CreateAlias("JobSeekerDetailses", "cd", JoinType.InnerJoin);
            criteria.CreateAlias("cd.Country", "c", JoinType.InnerJoin, Restrictions.Eq("c.Status", State.EntityStatus.Active));
            criteria.CreateAlias("cd.State", "s", JoinType.InnerJoin, Restrictions.Eq("s.Status", State.EntityStatus.Active));
            criteria.CreateAlias("cd.City", "city", JoinType.InnerJoin, Restrictions.Eq("city.Status", State.EntityStatus.Active));
            criteria.CreateAlias("c.Region", "r", JoinType.InnerJoin, Restrictions.Eq("r.Status", State.EntityStatus.Active));

            if (status != 0)
                criteria.Add(Restrictions.Eq("Status", status));
           
            if (region > 0)
                criteria.Add(Restrictions.Eq("r.Id", region));
            if (country > 0)
                criteria.Add(Restrictions.Eq("c.Id", country));
            if (state > 0)
                criteria.Add(Restrictions.Eq("ss.Id", state));
            if (city > 0)
                criteria.Add(Restrictions.Eq("city.Id", city));
            if (!string.IsNullOrEmpty(zip))
                criteria.Add(Restrictions.Eq("cd.zip", zip));
            if (!string.IsNullOrEmpty(contactMobile))
                criteria.Add(Restrictions.Eq("ContactMobile", contactMobile));
            if (!string.IsNullOrEmpty(contactEmail))
                criteria.Add(Restrictions.Eq("ContactEmail", contactEmail));
            if (!string.IsNullOrEmpty(firstName))
                criteria.Add(Restrictions.Eq("FirstName", firstName));
            if (!string.IsNullOrEmpty(lastName))
                criteria.Add(Restrictions.Eq("LastName", lastName));
            return criteria;
        }
        #endregion


    }
}
