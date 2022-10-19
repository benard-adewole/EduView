using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Estudiar.Models
{
    public class DeptSelect
    {
        public string Department { get; set; }
        public bool Selected { get; set; } = false;
        public string Color { get; set; } = "#A29CF4";
    }
}
