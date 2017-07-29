using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.JobLanes.Entity.User;
using Model.JobLanes.ViewModel;
using Model.JobLanes.Entity;

namespace JobLanes.WcfService.ModelBuilder
{
    public static class CastingBusinessModel
    {
        internal static UserProfile CastToUserProfile(UserProfileVm userProfileVm)
        {
            if (userProfileVm != null)
            {
                var userProfile = new UserProfile()
                {
                    Name = userProfileVm.NickName,
                    AspNetUserId = userProfileVm.AspNetUserId
                };
                return userProfile;
            }
            return new UserProfile();
        }

        internal static Region CastToRegion(RegionViewModel regionvm)
        {
            if (regionvm != null)
            {
                var region = new Region()
                {
                    Name = regionvm.Name,
                    Status = regionvm.Status == 0 ? Region.EntityStatus.Active : regionvm.Status
                };
                return region;
            }
            return new Region();
        }
    }
}