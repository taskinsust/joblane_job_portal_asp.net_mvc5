using Joblanes.Services.Test.ObjectFactory.JobseekerFactory;
using Joblanes.Services.Test.ObjectFactory.WebAdminFactory;
using Joblanes.Services.Test.ObjectFactory.EmployeeFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joblanes.Services.Test.ObjectFactory
{
    public static class FactoryLevels
    {
        private static Dictionary<Type, int> list;
        static FactoryLevels()
        {
            list = new Dictionary<Type, int>();
            int level = 1;

            //Level 1
            list.Add(typeof(RegionFactory), level);
            list.Add(typeof(CompanyTypeFactory), level);
            list.Add(typeof(UserProfileFactory), level);
            list.Add(typeof(JobCategoryFactory), level);
            list.Add(typeof(JobTypeFactory), level);
            list.Add(typeof(OrganizationTypeFactory), level);
            list.Add(typeof(PaymentTypeFactory), level);

            level++;
            list.Add(typeof(CountryFactory), level);
            list.Add(typeof(CompanyFactory), level);
            list.Add(typeof(JobSeekerFactory), level);

            level++;
            list.Add(typeof(CityFactory), level);
            list.Add(typeof(StateFactory), level);
            list.Add(typeof(JobSeekerEducationalQualificationFactory), level);
            list.Add(typeof(JobSeekerExperienceFactory), level);
            list.Add(typeof(JobSeekerTrainingCoursesFactory), level);

            level++;
            list.Add(typeof(CompanyDetailsFactory), level);
            list.Add(typeof(JobSeekerDetailsFactory), level);
        }
        public static int GetLevel(Type modelType)
        {
            if (list.Keys.Contains(modelType))
                return list[modelType];
            return -1;
        }
    }
}
