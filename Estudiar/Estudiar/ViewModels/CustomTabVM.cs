using Estudiar.Models;
using Estudiar.RestClients;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;
using Estudiar.Views;
using Estudiar.XInterface;
using Estudiar.Models.Hidden;
using Xamarin.CommunityToolkit.UI.Views;

namespace Estudiar.ViewModels
{
    public class CustomTabVM : INotifyPropertyChanged
    {
        private string[] sourceItems = {"sss"};
        public ObservableCollection<Cours> Cours;

        private VM_SignUp_In profileEditVM;

        public VM_SignUp_In ProfileEditVM
        {
            get { return profileEditVM; }
            set { profileEditVM = value;OnPropertyChanged("ProfileEditVM"); }
        }


        //public VM_SignUp_In ProfileEditVM = new VM_SignUp_In() { SelectedFac = new Model_Fac_dept(), SelectedDept = new DeptSelect(), };

        private bool _IsProfileEditable;

        public bool IsProfileEditable
        {
            get { return _IsProfileEditable; }
            set 
            {
                _IsProfileEditable = value;
                

                OnPropertyChanged("IsProfileEditable"); 
            }
        }
        private LayoutState state = LayoutState.Error;

        public LayoutState State
        {
            get { return state; }
            set
            {
                state = value;
                OnPropertyChanged("State");
            }
        }
        private LayoutState newestCoursestate = LayoutState.Error;

        public LayoutState NewestCoursestate
        {
            get { return newestCoursestate; }
            set
            {
                newestCoursestate = value;
                OnPropertyChanged("NewestCoursestate");
            }
        }

        private LayoutState recommendedCoursestate = LayoutState.Error;

        public LayoutState RecommendedCoursestate
        {
            get { return recommendedCoursestate; }
            set
            {
                recommendedCoursestate = value;
                OnPropertyChanged("RecommendedCoursestate");
            }
        }
        private LoginResponse loginResponse;

        public LoginResponse LoginResponse
        {
            get
            {
                return AppState<LoginResponse>.DeserializeAndRestore("LoginResponse");
            }
            set
            {
                loginResponse = value;

                if (loginResponse == null)
                {
                    return;
                }
                AppState<LoginResponse>.SerializeAndSave(value, "LoginResponse");
                OnPropertyChanged("LoginResponse");
            }
        }
        public ICommand CustomButtonClicked
        {
            get
            {
                return new Command(async () =>
                {

                    await Shell.Current.Navigation.PushAsync(new FacultySelect(ProfileEditVM));
                    /*EventHandler handler = CustomButtomClicked;
                    if (handler != null)
                    {
                        handler(loginModel,null);
                        //throw new NotImplementedException();
                    }*/
                });
            }
            private set { }
        }
        private IList<int> level;

        public IList<int> Level
        {
            get { return level; }
            set { level = value; }
        }

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
                
                Tempcours = new ObservableCollection<Cours>();

                if (Cours.Count == 0)
                {
                    OnPropertyChanged("SearchKey");
                    return;
                }

                if (string.IsNullOrEmpty(value))
                {
                    foreach (var currentCourse in Cours)
                    {
                        if (Tempcours.Where(c => c.course == currentCourse.course && c.level == currentCourse.level).FirstOrDefault() == null)
                        {
                            Tempcours.Add(currentCourse);
                        }
                    }
                }
                else
                {
                    foreach (var currentCourse in Cours.Where(aCourse => aCourse.course.ToLower().Contains(value.ToLower()) || 
                    aCourse.title.ToLower().Contains(value.ToLower()) || 
                    aCourse.level.ToLower().Contains(value.ToLower())).ToList())
                    {
                        if (!Tempcours.Contains(currentCourse) &&
                            Tempcours.Where(c => c.course == currentCourse.course && c.level == currentCourse.level).FirstOrDefault() == null)
                        {
                            Tempcours.Add(currentCourse);
                        }
                        
                    }
                }


                OnPropertyChanged("SearchKey");
            }
        }



        private ObservableCollection<Cours> _tempcours;

        public ObservableCollection<Cours> Tempcours
        {
            get { return _tempcours; }
            set { _tempcours = value; OnPropertyChanged("Tempcours"); }
        }
        private ObservableCollection<Cours> _tempSearchedcours;

        public ObservableCollection<Cours> TempSearchedcours
        {
            get { return _tempSearchedcours; }
            set { _tempSearchedcours = value; OnPropertyChanged("TempSearchedcours"); }
        }
        private ObservableCollection<Cours> _recommendedCourse;

        public ObservableCollection<Cours> RecommendedCourse
        {
            get { return _recommendedCourse; }
            set { _recommendedCourse = value; OnPropertyChanged("RecommendedCourse"); }
        }

        private ObservableCollection<MyFileNewStructure> _newResources;

        public ObservableCollection<MyFileNewStructure> NewResources
        {
            get { return _newResources; }
            set { _newResources = value; OnPropertyChanged("NewResources"); }
        }

        public ICommand SelectedCourse
        {
            get
            {
                return new Command<Cours>(async (param) =>
                {
                    //show cgpa, sgpa, number of courses, number of units, total score
                    //string Content = JsonConvert.SerializeObject(param);
                    AppState<Cours>.SerializeAndSave(param, "SelectedCourse");
                    await Shell.Current.GoToAsync($"{nameof(Views.CourseInfo)}");
                });
            }
            private set { }
        }
        public ICommand LogOutCommand
        {
            get//UploadCommand
            {
                return new Command(async () =>
                {
                    Preferences.Set("LoginResponse", "");
                    Preferences.Set("MyCourses", "");
                    //await Shell.Current.GoToAsync($"//{nameof(Views.NewLogin)}");
                    Application.Current.MainPage = new Estudiar.Views.AppShell();
                });
            }
            private set { }
        }
        public ICommand OpenCGPAdashboard
        {
            get
            {
                return new Command(async () =>
                {
                    //show cgpa, sgpa, number of courses, number of units, total score
                    //string Content = JsonConvert.SerializeObject(param);
                    //AppState<Cours>.SerializeAndSave(param, "SelectedCourse");
                    await Shell.Current.Navigation.PushAsync(new CGPA.DashBoard());
                });
            }
            private set { }
        }
        public ICommand SelectedMaterial
        {
            get
            {
                return new Command<MyFileNewStructure>(async (param) =>
                {
                    AppState<MyFileNewStructure>.SerializeAndSave(param, "SelectedMaterial");
                    await Shell.Current.GoToAsync($"{nameof(Views.BPDFview)}");
                });
            }
            private set { }
        }
        private ObservableCollection<Model_Fac_dept> _Faculty_Department;

        public ObservableCollection<Model_Fac_dept> Faculty_Department
        {
            get { return _Faculty_Department; }
            set { _Faculty_Department = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CustomTabVM()
        {
            InitializeLevel();
            InitializeFaculties_Department();
            Task.Run(async() => { await GetCourses(""); });
        }

        public async Task setup()
        {
            
        }

        private void InitializeFaculties_Department()
        {
            if (LoginResponse == null)
                return;
            Faculty_Department = FacDeptImg.RetrieveData();
            ProfileEditVM = new VM_SignUp_In();
            ProfileEditVM.Faculty_Department = Faculty_Department;
            foreach (var faculty in ProfileEditVM.Faculty_Department)
            {
                if (faculty.Faculty.ToLower() == LoginResponse.user.faculty.ToLower())
                {
                    ProfileEditVM.SelectedFac = faculty;
                    foreach (var depts in ProfileEditVM.SelectedFac.Departments)
                    {
                        if (depts.Department.ToLower() == LoginResponse.user.dept.ToLower())
                        {
                            ProfileEditVM.SelectedDept = depts;
                            break;
                        }
                    }
                    break;
                }
            }
            //ProfileEditVM.SelectedDept = new DeptSelect();
            
        }
        private void InitializeLevel()
        {
            Level = new List<int> { 100, 200, 300, 400, 500, 600 };
        }
        public ICommand RefreshCourses
        {
            get
            {
                return new Command(async () =>
                {
                    await GetCourses("courses");
                });
            }
            private set { }
        }
        public async Task GetCourses(string home)
        {
            State = LayoutState.Loading;
            RecommendedCoursestate = LayoutState.Loading;
            NewestCoursestate = LayoutState.Loading;
            Cours = new ObservableCollection<Cours>();
            Tempcours = new ObservableCollection<Cours>();
            RecommendedCourse = new ObservableCollection<Cours>();
            NewResources = new ObservableCollection<MyFileNewStructure>();
            RestClient<ObservableCollection<Faculties>> restClient = new RestClient<ObservableCollection<Faculties>>();

            ObservableCollection<Faculties> schoolDb = await restClient.GetCourses("");
            if (schoolDb == null)
            {
                State = LayoutState.Error;
                RecommendedCoursestate = LayoutState.Error;
                NewestCoursestate = LayoutState.Error;
                return;
            }
            else if(schoolDb.Count == 0)
            {
                State = LayoutState.Empty;
            }
            
            foreach (var item in schoolDb)
            {
                foreach (var level in item.levels)
                {
                    foreach (var dept in level.departments)
                    {
                        foreach (var course in dept.courses)
                        {
                            Cours cours = course;
                            cours.department = dept.department;
                            cours.level = level.level;
                            cours.faculty = item.faculty;
                            
                            Cours.Add(cours);
                        }
                    }

                }
            }

            var levelFilter = "500";//LoginResponse.user.year.ToString()
            var courseFilter = "systems";//LoginResponse.user.dept.ToLower()

            if (LoginResponse != null)
            {
                levelFilter = LoginResponse.user.year.ToString();
                courseFilter = LoginResponse.user.dept.ToLower();
            }

            

            //Filters courses based on department and level
            //After filtering, select the newest resources
            foreach (var item in Cours.Where(aCourse => aCourse.department.ToLower() == courseFilter && aCourse.level == levelFilter).ToList())
            {
                RecommendedCourse.Add(item);
                foreach (var type in item.types)
                {
                    foreach (var resource in type.resources)
                    {
                        foreach (var file in resource.files)
                        {
                            MyFileNewStructure myFile = new MyFileNewStructure
                            {
                                dateCreated = file.dateCreated,
                                course = item.course,
                                desscription = item.description,
                                dp = item.dp,
                                filesize = file.filesize,
                                foldername = resource.foldername,
                                title = item.title,
                                type = type.type,
                                url = file.url,
                            };
                            if (NewResources.Count == 5)
                            {
                                //start filtering
                                //check for the least(outdated) in the list
                                DateTime Outdated = NewResources[0].dateCreated;

                                for (int i = 1; i < NewResources.Count; i++)
                                {
                                    if (Outdated > NewResources[i].dateCreated)
                                    {
                                        //current index is more outdated
                                        //swap

                                        Outdated = NewResources[i].dateCreated;
                                    }
                                }

                                //remove the most outdated
                                foreach (var newres in NewResources)
                                {
                                    if (newres.dateCreated == Outdated)
                                    {
                                        NewResources.Remove(newres);
                                        break;
                                    }
                                }

                                //add the new date
                                NewResources.Add(myFile);

                            }
                            else
                            {
                                NewResources.Add(myFile);
                            }

                        }
                    }
                }
                
            }

            if (RecommendedCourse == null)
            {
                RecommendedCoursestate = LayoutState.Error;
            }
            else if (RecommendedCourse.Count == 0)
            {
                RecommendedCoursestate = LayoutState.Empty;
            }
            else
            {
                RecommendedCoursestate = LayoutState.None;
            }

            if (NewResources == null)
            {
                NewestCoursestate = LayoutState.Error;
            }
            else if (NewResources.Count == 0)
            {
                NewestCoursestate = LayoutState.Empty;
            }
            else
            {
                NewestCoursestate = LayoutState.None;
            }
            if (schoolDb.Count != 0)
            {
                State = LayoutState.None;
            }
            //NewResources = NewResources.OrderBy(x => x.dateCreated).Select(x=> new MyFileNewStructure(){});
            //reverses the time date order.....so newer materials of the 5 selected come first
            //NewResources = NewResources.Reverse().to;
            SearchKey = "";
            //SortCourses();
            //AppState<MyCourses>.SerializeAndSave(MyCourses, "MyCourses");

        }
        public async Task<DashBoard> GetDashBoardAsync()
        {
            RestClient<DashBoard> restClient = new RestClient<DashBoard>();
            DashBoard result = await restClient.GetDashBoardAsync(LoginResponse.token);
            return result;
        }
        public async Task<SignUpResponse> EditProfileAsync(string key, string value)
        {

            RestClient<SignUpResponse> restClient = new RestClient<SignUpResponse>();

            SignUpResponse signUpResponse = await restClient.EditProfile(key, value, LoginResponse.token);

            if (signUpResponse == null)
            {
                return null;
            }
            else if (signUpResponse.msg == "Error Updating Profile")
            {
                DependencyService.Get<IToast>().LongToast($"Error Updating Profile");
            }
            else
            {
                DashBoard user = await GetDashBoardAsync();
                LoginResponse newResponse = new LoginResponse();
                newResponse.exp = LoginResponse.exp;
                newResponse.token = LoginResponse.token;
                newResponse.user = user.user;
                //loginResponse.user = user.user;
                LoginResponse = newResponse;
            }
            /*EventHandler<string> handler = MaterialUploadedEvent;
            if (handler != null)
            {
                handler(UploadModel, signUpResponse.msg);
            }*/
            //await GetCourses("courses");

            return null;
        }
    }
}
