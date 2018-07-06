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
    
    public class CombinedOptionModel
    {

        public CombinedOptionModel()
        {
        }

        //  Constructor
        public CombinedOptionModel(String file,String version,String description, String size, String publishedOn)
        {
            CombinedFile = file;
            CombinedVersion = version;
            CombinedDescription = description;
            CombinedSize = size;
            CombinedPublishedOn = publishedOn;
        }

        //public Guid NotificationID { get; set; }
        [DisplayName("File")]
        public string CombinedFile { get; set; }
        [DisplayName("Version")]
        public string CombinedVersion { get; set; }
        [DisplayName("Change Description")]
        public string CombinedDescription { get; set; }
        [DisplayName("Size(Read Only)")]
        public string CombinedSize { get; set; }
        [DisplayName("Published On")]
        public string CombinedPublishedOn { get; set; }
        


    }

   /* public class UserData
    {
        public static List<UserModel> Users { get; set; } = new List<UserModel>();
    }*/
}
