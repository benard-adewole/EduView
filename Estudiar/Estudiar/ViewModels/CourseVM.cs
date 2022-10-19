using Estudiar.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using System.Linq;
using Xamarin.Forms;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Estudiar.ViewModels
{
    //[QueryProperty(nameof(Content), nameof(Content))]
    public class CourseVM : INotifyPropertyChanged
    {
        public event EventHandler<Resource> MaterialChangedEvent;
        string content = "";
        public string Content
        {
            get { return content; }
            set
            {
                content = Uri.UnescapeDataString(value ?? string.Empty);
                OnPropertyChanged();
                InitializeData(content);
            }
        }
        public ObservableCollection<MyFileNewStructure> CourseMaterials;
        private string searchKey;

        public string SearchKey
        {
            get { return searchKey; }
            set
            {
                if (value == null)
                {
                    return;
                }

                searchKey = value;

                TempSelectedFolder = new ObservableCollection<MyFile>();

                if (SelectedResource.files.Count == 0)
                {
                    OnPropertyChanged("SearchKey");
                    return;
                }

                if (string.IsNullOrEmpty(value))
                {
                    foreach (var currentFile in SelectedResource.files)
                    {
                        TempSelectedFolder.Add(currentFile);
                    }
                }
                else
                {
                    foreach (var currentFile in SelectedResource.files.Where(aFile => aFile.type.ToLower().Contains(value.ToLower())).ToList())
                    {
                        if (!TempSelectedFolder.Contains(currentFile))
                        {
                            TempSelectedFolder.Add(currentFile);
                        }

                    }
                }


                OnPropertyChanged("SearchKey");
            }
        }

        private Cours _dbCourseStructure;

        public Cours DbCourseStructure
        {
            get { return _dbCourseStructure; }
            set { _dbCourseStructure = value; OnPropertyChanged("DbCourseStructure"); }
        }


        private Cours cours;

        public Cours SelectedCourse
        {
            get { return cours; }
            set { cours = value; OnPropertyChanged("SelectedCourse"); }
        }

        private Resource selectedResource;

        public Resource SelectedResource
        {
            get { return selectedResource; }
            set 
            {
                selectedResource = value;
                //SelectedFolder = new ObservableCollection<MyFile>();
                selectedFolder = value.files;
                SearchKey = "";
                OnPropertyChanged("SelectedResource");
            }
        }


        private ObservableCollection<MyFile> selectedFolder;

        public ObservableCollection<MyFile> TempSelectedFolder
        {
            get { return selectedFolder; }
            set { selectedFolder = value; OnPropertyChanged("TempSelectedFolder"); }
        }

        public ObservableCollection<MyFile> sele { get; set; }


        private ObservableCollection<MyFileNewStructure> _course;

        public ObservableCollection<MyFileNewStructure> TempCourseMaterials
        {
            get { return _course; }
            set { _course = value; OnPropertyChanged("TempCourseMaterials"); }
        }
        private void InitializeData(string getContent)
        {

        }
        public ICommand SelectedResourceCommand
        {
            get
            {
                return new Command<Resource>(async (param) =>
                {
                    if (param.files.Count == 1)
                    {
                        AppState<MyFile>.SerializeAndSave(param.files[0], "SelectedMaterial");
                        await Shell.Current.GoToAsync($"{nameof(Views.BPDFview)}");
                    }
                    else
                    {
                        SelectedResource = param;
                        EventHandler<Resource> handler = MaterialChangedEvent;
                        if (handler != null)
                        {
                            handler(this, param);
                        }
                    }
                    
                    //AppState<Resource>.SerializeAndSave(param,"SelectedMaterial");
                    //await Shell.Current.GoToAsync($"{nameof(Views.BPDFview)}");
                });
            }
            private set { }
        }

        public ICommand SelectedMaterial
        {
            get
            {
                return new Command<MyFile>(async (param) =>
                {
                    
                    AppState<MyFile>.SerializeAndSave(param,"SelectedMaterial");
                    await Shell.Current.GoToAsync($"{nameof(Views.BPDFview)}");
                });
            }
            private set { }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public CourseVM()
        {
            
            CourseMaterials = new ObservableCollection<MyFileNewStructure>();
            TempCourseMaterials = new ObservableCollection<MyFileNewStructure>();
           
            DbCourseStructure = AppState<Cours>.DeserializeAndRestore("SelectedCourse");
            foreach (var type in DbCourseStructure.types)
            {
                foreach (var resource in type.resources)
                {
                    foreach (var file in resource.files)
                    {
                        CourseMaterials.Add(new MyFileNewStructure()
                        {
                            course = DbCourseStructure.course,
                            url = file.url,
                            foldername = resource.foldername,
                            filesize = file.filesize,
                            type = type.type,
                            dp = DbCourseStructure.dp,
                            desscription = DbCourseStructure.description,
                            title = DbCourseStructure.title,
                            dateCreated = file.dateCreated,
                        }) ;
                    }
                }
            };
            
        }
    }
    
    
    public class MyFileNewStructure
    {
        public string  dp { get; set; }
        public string  desscription { get; set; }
        public string  title { get; set; }
        public string type { get; set; }
        public string course { get; set; }
        public double filesize { get; set; }
        public string url { get; set; }
        public string foldername { get; set; }
        public DateTime dateCreated { get; set; }
    }
}
