using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estudiar.Models
{
    public class CourseDetails
    {
        public string code { get; set; } = "";
        
        public string title { get; set; } 
        public int gradeIndex { get; set; }= 0; //e.g A B C D E F
        //public List<string> possibleunit { get; set; } = new List<string>() { "0", "1", "2", "3", "4","5","6","7" };
        //public List<string> possiblegrades { get; set; } = new List<string>() { "A", "B", "C", "D", "E","F" };
        public int unit { get; set; } = 2;
        public int score { get; set; } = 0;
        public int overall { get; set; } = 0;
        public int semester { get; set; } = 1;
        public int index { get; set; }
    }
    public class gradeInfo
    {
        public int value { get; set; }
        public int index { get; set; }
    }
}
