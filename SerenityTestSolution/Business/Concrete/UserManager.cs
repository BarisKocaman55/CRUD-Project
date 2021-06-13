using Business.Abstract;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        private IUserDal _userDal;

        public UserManager(IUserDal userDal)
        {
            _userDal = userDal;
        }

        public IDataResult<User> Register(User user)
        {
            IResult result = BusinessRules.Run(CheckIfEmailExists(user.Email));

            if(result != null)
            {
                return new ErrorDataResult<User>("This Email is Already Used!!!");
            }

            user.Password = GetMD5(user.Password);
            _userDal.Add(user);
            return new SuccessDataResult<User>(user, "Successfully Registered!!!");
        }

        public IDataResult<User> Login(User user)
        {
            user.Password = GetMD5(user.Password);
            var data = _userDal.GetList(u => u.Email.Equals(user.Email) && u.Password.Equals(user.Password));

            if(data.Count() > 0)
            {
                return new SuccessDataResult<User>(user, "Successfully Logined!!!");
            }

            else
            {
                return new ErrorDataResult<User>("Login Failed!!!");
            }
        }

        private IResult CheckIfEmailExists(string email)
        {
            var result = _userDal.GetList(u => u.Email == email).Any();
            if (result)
            {
                return new ErrorResult("This email already used!!!");
            }

            return new SuccessResult();
        }

        private static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");
            }

            return byte2String;
        }

    }
}
