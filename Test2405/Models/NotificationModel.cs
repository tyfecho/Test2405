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

    //  ID -> Posted ( Sent ) -> Platform -> Notifications -> Status -> PostedBy -> Action/Activity -> Expiry -> Priority
    //  missing : Icon

    public class NotificationModel
    {
        public int NotificationID { get; set; }
        public string NotificationPosted { get; set; }
        [DisplayName("Platform")]
        public string NotificationPlatform { get; set; }
        [DisplayName("Notification Message")]
        public string NotificationMsg { get; set; }
        [DisplayName("Status")]
        public string NotificationStatus { get; set; }
        [DisplayName("Notification By")]
        public string NotificationBy { get; set; }

        //public string 
    }

   /* public class UserData
    {
        public static List<UserModel> Users { get; set; } = new List<UserModel>();
    }*/
}
