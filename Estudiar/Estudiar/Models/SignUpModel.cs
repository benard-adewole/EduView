using System;
using System.Collections.Generic;
using System.Text;

namespace Estudiar.Models
{
    public class SignUpModel
    {
        public string email { get; set; }
        public string password { get; set; }
        public int year { get; set; }
        public string dept { get; set; }
        public string faculty { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public SignUpModel()
        {

        }
        public override string ToString()
        {
            return String.Format("{0}\n{1}\n{2}\n{3}\n{4}",email, password, year, dept, faculty);
        }
    }
}
