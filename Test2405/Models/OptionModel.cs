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
    
    public class OptionModel
    {
        public int OptionId { get; set; }
        public string OptionName { get; set; }
        public string OptionType { get; set; }
        public string OptionValueString { get; set; }
        public Boolean OptionValueBool { get; set; }
        public String OptionValueDate { get; set; }
    }

   /* public class UserData
    {
        public static List<UserModel> Users { get; set; } = new List<UserModel>();
    }*/
}
