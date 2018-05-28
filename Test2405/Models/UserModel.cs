using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test2405.Models
{
    public class UserModel
    {
        public string Username { get; set; }
        public string Domain { get; set; }
        public string Password { get; set; }
    }

    public class UserData
    {
        public static List<UserModel> Users { get; set; } = new List<UserModel>();
    }
}
