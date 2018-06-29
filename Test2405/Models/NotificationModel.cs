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
        public Guid NotificationID { get; set; }
        [DisplayName("Platform")]
        public string NotificationPlatform { get; set; }
        public string NotificationMsg { get; set; }
        [DisplayName("Status")]
        public string NotificationStatus { get; set; }
        [DisplayName("Icon")]
        public string NotificationIcon { get; set; }
        [DisplayName("Notification By")]
        public string NotificationBy { get; set; }
        [DisplayName("Expiry")]
        public string NotificationExpiry { get; set; }
        [DisplayName("Action/Activity")]
        public string NotificationActionActivity { get; set; }
        [DisplayName("Priority")]
        public string NotificationPriority { get; set; }
        [DisplayName("Send On")]
        public string NotificationSendOn { get; set; }
        [DisplayName("Created On")]
        public string NotificationCreatedOn { get; set; }
       
    }

   

   /* public class UserData
    {
        public static List<UserModel> Users { get; set; } = new List<UserModel>();
    }*/
}
