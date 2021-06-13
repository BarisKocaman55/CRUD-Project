using Business.Abstract;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text;

namespace Business.Concrete
{
    public class AdminManager : IAdminService
    {
        private IAdminDal _adminDal;
        private IUserDal _userDal;

        public AdminManager(IAdminDal adminDal, IUserDal userDal)
        {
            _adminDal = adminDal;
            _userDal = userDal;
        }


        public IDataResult<Admin> Login(Admin admin)
        {
            admin.Password = GetMD5(admin.Password);
            var data = _userDal.GetList(u => u.Email.Equals(admin.Email) && u.Password.Equals(admin.Password));

            if (data.Count() > 0)
            {
                return new SuccessDataResult<Admin>(admin, "Successfully Logined!!!");
            }

            else
            {
                return new ErrorDataResult<Admin>("Login Failed!!!");
            }
        }

        public IDataResult<Admin> Register(Admin admin)
        {
            IResult result = BusinessRules.Run(CheckIfEmailExists(admin.Email));

            if (result != null)
            {
                return new ErrorDataResult<Admin>("This Email is Already Used!!!");
            }

            admin.Password = GetMD5(admin.Password);
            _adminDal.Add(admin);
            return new SuccessDataResult<Admin>(admin, "Successfully Registered!!!");
        }


        public IResult Delete(User user)
        {
            _userDal.Delete(user);
            return new SuccessResult("User Successfully Deleted!!!");
        }

        public IResult Update(User user)
        {
            _userDal.Update(user);
            return new SuccessResult("User Successfully Updated!!!");
        }


        public IDataResult<User> GetById(int id)
        {
            return new SuccessDataResult<User>(_userDal.Get(u => u.Id == id));
        }

        public IDataResult<List<User>> GetList()
        {
            return new SuccessDataResult<List<User>>(_userDal.GetList().ToList());
        }

        public IDataResult<List<string>> GetListOrderByAlphabet()
        {
            List<User> userQuery = _userDal.GetList().ToList();
            int counter = userQuery.Count();

            //If query has no users returns Error message
            if(counter < 1)
            {
                return new ErrorDataResult<List<string>>("Error!!!");
            }

            //If query has only one user then it simply returns directly the user
            else if(counter == 1)
            {
                int i;
                string[] firstNameList = userQuery.Select(u => u.FirtstName).ToArray();
                string[] lastNameList = userQuery.Select(l => l.LastName).ToArray();

                string[] fullName = new string[counter];
                for (i = 0; i < counter; i++)
                {
                    fullName[i] = (firstNameList[i] + " " + lastNameList[i]);
                }

                List<string> users = new List<string>();

                for (i = 0; i < counter; i++)
                {
                    users.Add(fullName[i]);
                }

                return new SuccessDataResult<List<string>>(users, "Users Listed!!!");
            }

            // If the query has more than one user then we get firstname and lastname to arrays.
            // After that we find number of firstnames which is our number of records
            // Then we find length of every firstname and combine them with fullname in two-dim array
            //We sort fullname as string and after the sort we compare the sorted array with fullNameWithLength
            //At the end we substr the fullnames with the length of every single firstname.
            else
            {
                int i, j;
                string[] firstNameList = userQuery.Select(u => u.FirtstName).ToArray();
                string[] lastNameList = userQuery.Select(l => l.LastName).ToArray();

                //Get length of the every name
                int[] lengthCounter = new int[counter];
                for (i = 0; i < counter; i++)
                {
                    lengthCounter[i] = firstNameList[i].Length;
                }

                //Convert lengths of the names to string
                string[] lengtString = new string[counter];
                for (i = 0; i < counter; i++)
                {
                    lengtString[i] = lengthCounter[i].ToString();
                }

                string[] fullName = new string[counter];
                for (i = 0; i < counter; i++)
                {
                    fullName[i] = (firstNameList[i] + "" + lastNameList[i]).ToLower();
                }

                //Merge fullname and length of the firstnames to two dimensional array
                string[,] fullNameWithLength = new string[counter, 2];
                for (i = 0; i < counter; i++)
                {
                    fullNameWithLength[i, 0] = fullName[i];
                }

                for (i = 0; i < counter; i++)
                {
                    fullNameWithLength[i, 1] = lengtString[i];
                }

                //Sort fullname
                for (i = 0; i < counter; i++)
                {
                    var x = fullName[i];
                    var k = i;
                    while (k > 0 && fullName[k - 1].CompareTo(x) > 0)
                    {
                        fullName[k] = fullName[k - 1];
                        k = k - 1;
                    }
                    fullName[k] = x;
                }

                //Compare fullName array with fullNameWith array. If two string equals substring fullname.
                for (i = 0; i < counter; i++)
                {
                    for (j = 0; j < counter; j++)
                    {
                        if (fullName[i].CompareTo(fullNameWithLength[j, 0]) == 0)
                        {
                            fullName[i] = fullName[i].Substring(0, Int32.Parse(fullNameWithLength[j, 1])) + " " + fullName[i].Substring(Int32.Parse(fullNameWithLength[j, 1]));
                            break;
                        }
                    }
                }

                //Initialize a user List
                List<string> users = new List<string>();

                for (i = 0; i < counter; i++)
                {
                    users.Add(fullName[i]);
                }

                return new SuccessDataResult<List<string>>(users, "Users Listed!!!");


            }

        }

        //Checking if the mail used befor.
        private IResult CheckIfEmailExists(string email)
        {
            var result = _adminDal.GetList(a => a.Email == email).Any();
            if (result)
            {
                return new ErrorResult("This email already used!!!");
            }

            return new SuccessResult();
        }

        //Encrypt the password.
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
