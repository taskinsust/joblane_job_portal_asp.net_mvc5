using System;
using Dao.Joblanes;
using Model.JobLanes.Dto;
using Model.JobLanes.Entity.User;
using NHibernate;
using Services.Joblanes.Base;
using Services.Joblanes.Exceptions;

namespace Services.Joblanes
{
    public interface IUserService : IBaseService
    {
        bool Save(UserProfile userProfile);
        UserProfileDto GetByAspNetUserId(string aspNetUserId);
        void BlockOrUnblockUser(string aspNetUser, bool isBlock);
    }

    public class UserService : BaseService, IUserService
    {
        private readonly IUserDao _userDao;
        public UserService(ISession session)
        {
            Session = session;
            _userDao = new UserDao() { Session = session };
        }
        public bool Save(UserProfile userProfile)
        {
            try
            {
                _userDao.Save(userProfile);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UserProfileDto GetByAspNetUserId(string aspNetUserId)
        {
            if (String.IsNullOrEmpty(aspNetUserId)) throw new NullObjectException("Invalid aspNetUser");
            var up = _userDao.GetByAspNetUserId(aspNetUserId);
            return new UserProfileDto()
            {
                IsBlock = up.IsBlock,
                Id = up.Id,
            };
        }

        public void BlockOrUnblockUser(string aspNetUser, bool isBlock)
        {
            ITransaction trans = null;
            try
            {
                if (String.IsNullOrEmpty(aspNetUser)) throw new NullObjectException("Invalid aspnetUSer found ");
                UserProfile userProfile = _userDao.GetByAspNetUserId(aspNetUser);
                userProfile.IsBlock = isBlock;
                using (trans = Session.BeginTransaction())
                {
                    _userDao.Update(userProfile);
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                if (trans != null) trans.Rollback();
                throw ex;
            }
        }
    }
}
