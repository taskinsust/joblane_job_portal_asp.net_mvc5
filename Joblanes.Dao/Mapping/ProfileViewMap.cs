using System; 
using System.Collections.Generic; 
using System.Text;
using Dao.Joblanes.Mapping.Base;
using FluentNHibernate.Mapping;
using Model.JobLanes.Entity; 

namespace Dao.Joblanes.Mapping {


    public class ProfileViewMap : BaseClassMap<ProfileView, long>
    {
        
        public ProfileViewMap() {
            Table("ProfileView");
			LazyLoad();
			References(x => x.JobSeeker).Column("JobSeekerId");
			References(x => x.Company).Column("CompanyId");
            Map(x => x.IsCompany).Column("IsCompany");
			}
    }
}
