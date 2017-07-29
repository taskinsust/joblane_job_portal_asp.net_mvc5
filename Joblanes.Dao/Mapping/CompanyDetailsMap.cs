using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dao.Joblanes.Mapping.Base;
using Model.JobLanes.Entity;

namespace Dao.Joblanes.Mapping
{
    public class CompanyDetailsMap : BaseClassMap<CompanyDetails, long> 
    {
        public CompanyDetailsMap()
        {
            Table("CompanyDetails");
            LazyLoad();

         
            Map(x => x.TradeLicence).Column("TradeLicence");
            Map(x => x.WebLink).Column("WebLink");
            Map(x => x.LinkdinLink).Column("LinkdinLink");
            Map(x => x.Vision).Column("Vision");
            Map(x => x.Mission).Column("Mission");
            Map(x => x.Description).Column("Description");
            Map(x => x.Address).Column("Address");
            Map(x => x.Zip).Column("Zip");
            Map(x => x.TagLine).Column("TagLine");
            Map(x => x.EmployeeSize).Column("EmployeeSize");
            Map(x => x.EstablishedDate).Column("EstablishedDate");

            References(x => x.Country).Column("CountryId");
            References(x => x.State).Column("StateId");
            References(x => x.City).Column("CityId");
            References(x => x.Company).Column("CompanyId");


        }
    }
}
