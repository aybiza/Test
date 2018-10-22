using System;
using System.Collections.Generic;
using System.Text;
using BLL.Models;

namespace BLL.Services.Users
{
    public interface IUserService
    {
        User GetUser(string Key, string type);
    }
}
