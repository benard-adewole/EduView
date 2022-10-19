using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Estudiar.Models
{
    public class Material
    {
        public string _id { get; set; }
        public string name { get; set; }
        public string uri { get; set; }
        public string uploader { get; set; }
        public string title { get; set; }
        public int year { get; set; }
        public string code{ get; set; }
        public string dept{ get; set; }

    }

    public class Course
    {
        public string _id { get; set; }
        public int year { get; set; }
        public string dept { get; set; }
        public string faculty { get; set; }

        public string code { get; set; }
        public string title { get; set; }
        public ObservableCollection<Material> materials { get; set; }
        public int __v { get; set; }
    }

    public class MyCourses
    {
        public ObservableCollection<Course> courses { get; set; }
    }
}
