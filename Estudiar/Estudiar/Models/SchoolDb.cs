using Syncfusion.XForms.Expander;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace Estudiar.Models
{
    public class MyFileNewStructure
    {
        public string dp { get; set; }
        public string desscription { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public string course { get; set; }
        public double filesize { get; set; }
        public string url { get; set; }
        public string foldername { get; set; }
        public DateTime dateCreated { get; set; }
    }
    public class MyFile
    {
        public string type { get; set; }
        public DateTime dateCreated { get; set; }
        public double filesize { get; set; }
        public string url { get; set; }
    }

    public class Resource
    {
        public string foldername { get; set; }
        public ObservableCollection<MyFile> files { get; set; }
    }

    public class ResourceType : INotifyPropertyChanged
    {
        public string type { get; set; }
        private bool isExpanded;
        public AnimationEasing animation;
        public AnimationEasing Animation
        {
            get { return animation; }
            set
            {
                animation = value;
                this.RaisedOnPropertyChanged("Animation");
            }
        }

        public bool IsExpanded
        {
            get { return isExpanded; }
            set
            {
                isExpanded = value;
                this.RaisedOnPropertyChanged("IsExpanded");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisedOnPropertyChanged(string _PropertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(_PropertyName));
            }
        }

        public ObservableCollection<Resource> resources { get; set; }
    }

    public class Cours
    {
        public string course { get; set; }
        public string dp { get; set; }
        public string description { get; set; }
        public string title { get; set; }
        public ObservableCollection<ResourceType> types { get; set; }
        public string department { get; set; }
        public string faculty { get; set; }
        public string level { get; set; }

    }

    public class Department
    {
        public string department { get; set; }
        public ObservableCollection<Cours> courses { get; set; }
    }

    public class Level
    {
        public string level { get; set; }
        public ObservableCollection<Department> departments { get; set; }
    }
    

    public class Faculties
    {
        public string faculty { get; set; }
        public ObservableCollection<Level> levels { get; set; }
    }
    
}
