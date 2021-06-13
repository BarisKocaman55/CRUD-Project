﻿using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IUserService
    {
        //IResult Add(User user);
        IDataResult<User> Register(User user);
        IDataResult<User> Login(User user);
    }
}