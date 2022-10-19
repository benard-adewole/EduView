using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Estudiar.Models
{
    public class UploadModel
    {
        public int year { get; set; }
        public string title { get; set; }
        public string code { get; set; }
        public IncomingFile Material{ get;set;}
        
        public string dept { get; set; }
        public string faculty { get; set; }
        public override string ToString()
        {
            return String.Format("{0}\n{1}\n{2}\n{3}\n{4}\n{5}", title, code, year, dept, faculty,Material.fileName);
        }
    }
}
