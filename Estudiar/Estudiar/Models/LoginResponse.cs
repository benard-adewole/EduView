using System;
using System.Collections.Generic;
using System.Text;

namespace Estudiar.Models
{

    public class User
    {
        public bool status { get; set; }
        public bool privileges { get; set; }
        public DateTime period_end { get; set; }
        public string _id { get; set; }
        public string name { get; set; }

        public string email { get; set; }
        public string password { get; set; }
        public int year { get; set; }
        public string dept { get; set; }
        public string faculty { get; set; }
        public int __v { get; set; }
        public string pay_reference { get; set; }
    }

    public class LoginResponse
    {
        public int exp { get; set; }
        public string token { get; set; }
        public User user { get; set; }
    }
}
