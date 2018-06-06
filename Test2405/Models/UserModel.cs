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
    
    public class UserModel
    {
        [DisplayName("User Name")]
        [Required(ErrorMessage ="This field is required")]
        public string Username { get; set; }
        [DisplayName("Domain")]
        [Required(ErrorMessage = "This field is required")]
        public string Domain { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "This field is required")]
        public string Password { get; set; }
        public string LoginErrorMessage { get; set; }
    }

   /* public class UserData
    {
        public static List<UserModel> Users { get; set; } = new List<UserModel>();
    }*/
}
