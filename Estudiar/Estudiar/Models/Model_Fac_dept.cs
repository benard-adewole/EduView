using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace Estudiar.Models
{
    public class Model_Fac_dept
    {
        public string Faculty { get; set; }
        public string Image { get; set; }
        public bool Selected { get; set; }

        public ObservableCollection<DeptSelect> Departments { get; set; }
    }
}
