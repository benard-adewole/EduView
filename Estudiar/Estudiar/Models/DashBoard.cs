using System;
using System.Collections.Generic;
using System.Text;

namespace Estudiar.Models.Hidden
{
    public class User
    {
        public bool status { get; set; }
        public bool privileges { get; set; }
        public string _id { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public int year { get; set; }
        public string dept { get; set; }
        public string faculty { get; set; }
        public int __v { get; set; }
        public string resetToken { get; set; }
        public DateTime resetTokenExp { get; set; }
    }
    public class DashBoard
    {
        public Models.User user { get; set; }
    }
}
