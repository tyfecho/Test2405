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

    public class AnnouncementModel
    {
        public Guid AnnouncementID { get; set; }
        [DisplayName("Announcement Platform")]
        public string AnnouncementPlatform { get; set; }
        [DisplayName("Announcement Send On")]
        public string AnnouncementSendOn { get; set; }
        [DisplayName("Announcement Message")]
        public string AnnouncementMessage { get; set; }
        [DisplayName("Announcement Stop On")]
        public string AnnouncementStopOn { get; set; }
        [DisplayName("Announcement Status")]
        public string AnnouncementStatus { get; set; }
        [DisplayName("Announcement By")]
        public string AnnouncementBy { get; set; }
        [DisplayName("Announcement Type")]
        public AnnouncementType AnnouncementType { get; set; }
        
        [DisplayName("Created On")]
        public string AnnouncementCreatedOn { get; set; }
       
    }

    public enum AnnouncementType
    {
        Type1,
        Type2,
        Type3
    }

    /* public class UserData
     {
         public static List<UserModel> Users { get; set; } = new List<UserModel>();
     }*/
}
