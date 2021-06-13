using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IAdminService
    {
        IDataResult<Admin> Register(Admin admin);
        IDataResult<Admin> Login(Admin admin);
        IDataResult<List<User>> GetList();

        IDataResult<List<string>> GetListOrderByAlphabet();

        IDataResult<User> GetById(int id);

        IResult Delete(User user);
        IResult Update(User user);
    }
}
