using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Test2405.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    
    public class NotificationModel
    {
        public int NotificationID { get; set; }
        public string NotificationPosted { get; set; }
        public string NotificationPlatform { get; set; }
        public string NotificationMsg { get; set; }
        public string NotificationStatus { get; set; }
        public string NotificationBy { get; set; }

        //public string 
    }

   /* public class UserData
    {
        public static List<UserModel> Users { get; set; } = new List<UserModel>();
    }*/
}
