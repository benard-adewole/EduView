using Estudiar.MenuItems;
using Estudiar.Models;
using Estudiar.Models.Hidden;
using Estudiar.RestClients;
using Estudiar.Views;
using Estudiar.XInterface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
//using MvvmHelpers.Commands;

namespace Estudiar.ViewModels
{
    public class MainVM : INotifyPropertyChanged
    {
        public event EventHandler<string> MaterialUploadedEvent;
        public event EventHandler UploadButtonClicked;
        public event EventHandler<bool> RefreshComplete;
        public event EventHandler SaveIDClicked;
        public event EventHandler OpenPdfEvent;
        public event EventHandler SelectedCourseChanged;
        public event EventHandler ClearUploadEvent;
        public event EventHandler MenuButtonClicked;
        public event EventHandler SelectMaterialsButtonClicked;
        public event EventHandler<MenuPageItem> MenuSelected;

        private bool _isPaymentReoccuring;

        public bool IsPaymentReoccuring
        {
            get { return Preferences.Get("IsPaymentReoccuring",false); }
            set { _isPaymentReoccuring = value;
                Preferences.Set("IsPaymentReoccuring", value);
                OnPropertyChanged();
                
            }
        }


        private MyCourses myCourses;

        public MyCourses MyCourses
        {
            get { return myCourses; }
            set 
            {
                myCourses = value;
                if (value != null)
                {
                    State = LayoutState.None;
                }
                OnPropertyChanged();
            }
        }

        private Course selectedCourse;

        public Course SelectedCourse
        {
            get 
            {
                return selectedCourse;
            }
            set
            {
                if (value == null || value.title == null)
                {
                    //SelectedCourse.ma
                    selectedCourse =new Course();
                    OnPropertyChanged();
                    return;
                }
                if (MyCourses == null)
                {
                    selectedCourse = value;
                    return;
                }
                
                selectedCourse = value;
                AppState<Course>.SerializeAndSave(SelectedCourse, "SelectedCourse");

                OnPropertyChanged();
                

            }
        }
        public ICommand GotoCourses
        {
            get
            {
                return new Command(async() =>
                {
                    await Shell.Current.GoToAsync($"///{nameof(Views.Courses)}");
                });
            }
            private set { }
        }
        public ICommand CloseThisPage
        {
            get
            {
                return new Command(() =>
                {
                    //Device.BeginInvokeOnMainThread(async () => { await Shell.Current.GoToAsync(".."); });GotoCourses
                    Device.BeginInvokeOnMainThread(async () => { await Shell.Current.Navigation.PopAsync(); });
                });
            }
            private set { }
        }
        public ICommand UploadCommand
        {
            get//
            {
                return new Command(async () =>
                {
                    await Shell.Current.GoToAsync($"{nameof(Views.UploadPage)}");
                });
            }
            private set { }
        }
        public ICommand LogOutCommand
        {
            get//UploadCommand
            {
                return new Command(async() =>
                {
                    Preferences.Set("LoginResponse","");
                    Preferences.Set("MyCourses","");
                    await Shell.Current.GoToAsync($"//Home/{nameof(Views.NewLogin)}");
                });
            }
            private set { }
        }
        public async void OpenACourse()
        {

            string x = Preferences.Get("AppNavStackTemp", "");
            if (x == ($"//{nameof(Views.Courses)}"))
            {
                await Shell.Current.GoToAsync($"{nameof(Views.CourseSample.A_Course)}");
            }
            
        }
        

        private ObservableCollection<HomeFeed> homefeed;

        public ObservableCollection<HomeFeed> Homefeed
        {
            get { return homefeed; }
            set { homefeed = value; OnPropertyChanged(); }
        }

        private ObservableCollection<MenuPageItem> _menuPageItems;
        public ObservableCollection<MenuPageItem> menuPageItems
        { 
            get { return _menuPageItems; }
            set { _menuPageItems = value; OnPropertyChanged(); }
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

        private async Task<LoginResponse> GetResponseAsync()
        {
            LoginResponse loginResponse = AppState<LoginResponse>.DeserializeAndRestore("LoginResponse");
            
            return loginResponse;
        }

        //public LoginResponse loginResponse;

        private int _totalDepartments;

        public int TotalNoOfDepartments
        {
            get { return _totalDepartments; }
            set { _totalDepartments = value; OnPropertyChanged(); }
        }
        private int _totalFaculty;

        public int TotalNoOfFaculties
        {
            get { return _totalFaculty; }
            set { _totalFaculty = value; OnPropertyChanged(); }
        }

        private int _totalCourses;

        public int TotalNoOfCourses
        {
            get { return _totalCourses; }
            set { _totalCourses = value; OnPropertyChanged(); }
        }
        private MenuPageItem selectedMenu;

        public MenuPageItem SelectedMenu
        {
            get { return selectedMenu; }
            set {

                EventHandler<MenuPageItem> handler = MenuSelected;
                if (handler != null)
                {
                    handler(this, value);
                }

                selectedMenu = value;
                Preferences.Set("SelectedMenu", menuPageItems.IndexOf(SelectedMenu));
                OnPropertyChanged();
            }
        }

        private Material selectedMaterial;

        public Material SelectedMaterial
        {
            get { return selectedMaterial; }
            set
            {
                /*if (value == null || value.uri == null)
                {
                    selectedMaterial= new Material();
                    OnPropertyChanged();
                    return;
                } */  
                if (value != null)
                {
                    if (Preferences.Get("ExternalMaterial", "") != "")
                    {
                        LoadPdf();
                        return;
                    }
                    if (JsonConvert.SerializeObject(value) == Preferences.Get("SelectedMaterial",""))
                    {
                        LoadPdf();
                        return;
                    }
                    selectedMaterial = value;
                    //Last opened pdf, before app minimizes
                    //if (Application.Current.Properties.ContainsKey("ExternalMaterial"))
                    
                    if (!isInternetConnected() )
                    {
                        DependencyService.Get<IToast>().LongToast("Please ensure a good internet connection");
                        selectedMaterial = null;
                        OnPropertyChanged();
                        return;
                    }
                    /*if ((bool)Application.Current.Properties["OnStart"])
                    {
                        EventHandler eventHandler = OpenPdfEvent;
                        if (eventHandler != null)
                        {
                            OpenPdfEvent(this, null);
                        }
                    }*/
                    

                    OnPropertyChanged();
                    OpenPdf();

                }

                //
            }
        }
        private async Task OpenPdf()//Subscribe
        {
            string token = LoginResponse.token;
            string code = SelectedCourse.code;
            string name = SelectedMaterial.name;

            RestClient<Stream> restClients = new RestClient<Stream>();
            Stream pdf = await restClients.GetMaterialStreamAsync(token, code, name);

            saveAndLoad(pdf);

            //user payment method
            /*if (LoginResponse.user.status)
            {
                RestClient<Stream> restClients = new RestClient<Stream>();
                Stream pdf = await restClients.GetMaterialStreamAsync(token, code, name);

                saveAndLoad(pdf);
            }
            else
            {
                Subscribe.Execute(null);
            }*/

            
        }
        private async void LoadPdf()
        {
            if (Preferences.Get("AppNavStackTemp","").ToString() == $"//{nameof(Views.Courses)}/{nameof(Views.CourseSample.A_Course)}" ||
                Preferences.Get("AppNavStackTemp", "").ToString() == $"//{nameof(Views.Home)}")
            {
                await Shell.Current.GoToAsync($"{nameof(Views.PDFViewer)}");
            }
        }
        private async void saveAndLoad(Stream pdf)
        {
            if (pdf == null)
            {
                //Close_Clicked(null, null);
                return;
            }

            using (var fileStream = File.Create(Path.Combine(FileSystem.AppDataDirectory, "Estudiar.pdf")))
            {

                pdf.Seek(0, SeekOrigin.Begin);
                await pdf.CopyToAsync(fileStream);
            }
            AppState<Material>.SerializeAndSave(SelectedMaterial, "SelectedMaterial");
            LoadPdf();
            
            

        }
        


        /// <summary>
        private ObservableCollection<Model_Fac_dept> _Faculty_Department;

        public ObservableCollection<Model_Fac_dept> Faculty_Department
        {
            get { return _Faculty_Department; }
            set { _Faculty_Department = value; }
        }

        private Model_Fac_dept selectedFac;

        public Model_Fac_dept SelectedFac
        {
            get { return selectedFac; }//AppState<Model_Fac_dept>.DeserializeAndRestore("SelectedFac"); }
            set
            {
                selectedFac = value;
                //AppState<Model_Fac_dept>.SerializeAndSave(value, "SelectedFac");
                UploadModel.faculty = value.Faculty;

                OnPropertyChanged();
            }
        }

        private int selectedNumber;

        public int SelectedNumber
        {
            get { return selectedNumber; }
            set
            {
                selectedNumber = value;
                UploadModel.year = value;
                OnPropertyChanged();
            }
        }


        private IList<int> level;

        public IList<int> Level
        {
            get { return level; }
            set { level = value; }
        }

        

        private string uploadFileName;

        public string UploadFileName
        {
            get { return uploadFileName; }
            set { uploadFileName = value; OnPropertyChanged(); }
        }

        private DeptSelect selectedDept;

        public DeptSelect SelectedDept
        {
            get { return selectedDept; }
            set
            {
                selectedDept = value;
                UploadModel.dept = value.Department;
                OnPropertyChanged();
            }
        }
        private LayoutState state;

        public LayoutState State
        {
            get { return AppState<LayoutState>.DeserializeAndRestore("State");}
            set 
            {
                state = value;
                OnPropertyChanged();
                AppState<LayoutState>.SerializeAndSave(value, "State");
            }
        }

        private UploadModel _uploadModel;

        public UploadModel UploadModel
        {
            get { return _uploadModel; }
            set
            {
                _uploadModel = value;
                OnPropertyChanged();
            }
        }
        public ICommand FacultySelectCommand
        {
            get
            {
                return new Command(() =>
                {
                    string a = "";
                });
            }
            private set { }
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
        public ICommand Subscribe
        {
            get
            {
                return new Command(async () =>
                {
                    RestClient<payUrl> paymenturl = new RestClient<payUrl>();
                    var url = await paymenturl.PostSubscribeAsync(LoginResponse.token);
                    if (url != null)
                    {
                        await Shell.Current.Navigation.PushAsync(new Payment(url.pay_url));
                    }
                });
            }
            private set { }
        }
        public async Task UpdateSubscription()
        {
            RestClient<Stream> paymenturl = new RestClient<Stream>();
            var url = await paymenturl.PostVerifySubscriptionAsync(LoginResponse.token);
            //if user paid update status
            

            if (url != null && url.pay_status && url.msg.Trim().ToLower() != "technical issues please try again later!")
            {
                DependencyService.Get<IToast>().LongToast("Subcription Confirmed");
                IsPaymentReoccuring = true;
                loginResponse.user.status = true;// = new LoginResponse { exp =loginResponse.exp,token = loginResponse.token,user= user};
                //LoginResponse newresponse = LoginResponse;
                LoginResponse = loginResponse;
            }
        }
        public ICommand UpdateSucriptionStatus
        {
            get
            {
                return new Command(async() =>
                {
                    await UpdateSubscription();
                });
            }
            private set { }
        }
        public ICommand Unsubscribe
        {
            get
            {
                return new Command(async () =>
                {
                    if (IsPaymentReoccuring && await Shell.Current.DisplayAlert("Alert","Do you want to stop reocurring payment?","Yes","No"))
                    {
                        RestClient<Stream> paymenturl = new RestClient<Stream>();
                        var url = await paymenturl.PostUnSubscribeAsync(LoginResponse.token);
                        if (url)
                        {
                            IsPaymentReoccuring = false;
                            await Shell.Current.DisplayAlert("Success", "You have succcessfully unsubscribed", "OK");
                            UpdateSucriptionStatus.Execute(null);
                        }
                    }
                });
            }
            private set { }
        }
        public ICommand PrivacyCommand
        {
            get
            {
                return new Command(async() =>
                {
                    
                });
            }
            private set { }
        }
        public ICommand OpenNote
        {
            get
            {
                return new Command(async() =>
                {
                    await Shell.Current.Navigation.PushModalAsync(new Note());
                });
            }
            private set { }
        }
        public ICommand SaveID
        {
            get
            {
                return new Command(() =>
                {
                    EventHandler handler = SaveIDClicked;
                    if (handler != null)
                    {
                        handler(this, null);
                    }
                });
            }
            private set { }
        }
        public ICommand SendUsMail
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        var message = new EmailMessage
                        {
                            Subject = "Estudiar Student Request",
                            Body = "Brief description of the resource you would like to upload or question to be solved:\n ",
                            To = new List<string> { "estudiarhelp@gmail.com" },
                            //Cc = ccRecipients,
                            //Bcc = bccRecipients
                        };
                        await Email.ComposeAsync(message);
                    }
                    catch (FeatureNotSupportedException fbsEx)
                    {
                        // Email is not supported on this device
                        DependencyService.Get<IToast>().LongToast("Email is not supported on this device");
                    }
                    catch (Exception ex)
                    {
                        // Some other exception occurred
                        DependencyService.Get<IToast>().LongToast("Error occured");
                    }
                });
            }
            private set { }
        }
        public ICommand MenuClicked
        {
            get
            {
                return new Command(() =>
                {
                    EventHandler handler = MenuButtonClicked;
                    if (handler != null)
                    {
                        handler("Menu", null);
                    }
                });
            }
            private set { }
        }
        public ICommand CustomButtonClicked
        {
            get
            {
                return new Command<string>((button) =>
                {
                    if (button == "select materials")
                    {
                        EventHandler handler = SelectMaterialsButtonClicked;
                        if (handler != null)
                        {
                            handler(UploadModel, null);
                        }
                    }
                    else
                    {
                        EventHandler handler = UploadButtonClicked;
                        if (handler != null)
                        {
                            handler(UploadModel, null);
                        }
                    }
                });
            }
            private set { }
        }

        public ICommand CourseSelectedCommand
        {
            get
            {
                return new Command<string>((code) =>
                {
                    foreach (var item in MyCourses.courses)
                    {
                        if (item.code == code)
                        {
                            selectedCourse = item;
                            break;
                        }
                    }
                    
                });
            }
            private set { }
        }
        public ICommand ClearUploadModel
        {
            get
            {
                return new Command(() =>
                {
                    //clear upload  fields
                    UploadModel = new UploadModel();
                    SelectedFac = new Model_Fac_dept();
                    EventHandler handler = ClearUploadEvent;
                    if (handler != null)
                    {
                        handler("Menu", null);
                    }
                });
            }

            private set { }
        }
        public ICommand UploadNow
        {
            get
            {
                return new Command(() =>
                {
                    //ClearUploadModel
                    if (string.IsNullOrEmpty(UploadModel.code))
                    {
                        DependencyService.Get<IToast>().LongToast("Enter course code");
                    }
                    else if (string.IsNullOrEmpty(UploadModel.title))
                    {
                        DependencyService.Get<IToast>().LongToast("Enter course title");
                    }
                    else if (UploadModel.Material == null || UploadModel.Material.Stream == null)
                    {
                        DependencyService.Get<IToast>().LongToast("Pick a file");
                    }
                    else if (string.IsNullOrEmpty(UploadModel.Material.fileName))
                    {
                        DependencyService.Get<IToast>().LongToast("Enter File name");
                    }
                    
                    else if (string.IsNullOrEmpty(UploadModel.faculty))
                    {
                        DependencyService.Get<IToast>().LongToast("Enter course faculty");
                    }
                    else if (string.IsNullOrEmpty(UploadModel.dept))
                    {
                        if (!FacultyHasNoDepartment(UploadModel.faculty))
                        {
                            DependencyService.Get<IToast>().LongToast("Enter course department");
                        }
                        //its a faculty without department
                        else
                        {
                            UploadAsync();
                        }
                    }
                    else
                    {
                        UploadAsync();
                    }
                    
                });
            }
            private set { }
        }
        private bool FacultyHasNoDepartment(string faculty)
        {
            foreach (var item in Faculty_Department)
            {
                if (item.Faculty.ToLower() == faculty.ToLower() && item.Departments.Count != 0)
                {
                    return false;
                }
            }
            return true;
        }

        
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public MainVM()
        {


            GetResponse();

            UploadModel = new UploadModel();
            

            InitializeLevel();
            InitializeFaculties_Department();
            
           // restoreCoursePage();
            
            //SelectedMenu = menuPageItems[0];
        }

        private async Task GetResponse()
        {
            LoginResponse = AppState<LoginResponse>.DeserializeAndRestore("LoginResponse");
        }
        private void InitializeLevel()
        {
            Level = new List<int> { 100, 200, 300, 400, 500,600 };
            
        }
        private void InitializeFaculties_Department()
        {
            Faculty_Department = FacDeptImg.RetrieveData();
            
            foreach (var item in Faculty_Department)
            {
                item.Departments.Add(new DeptSelect { Department= "Shared" ,Selected=false});
            }
            Faculty_Department.Insert(0, new Model_Fac_dept
            {
                Faculty = "Shared",Image="general.png", Departments = new ObservableCollection<DeptSelect>()
            });

            Homefeed = new ObservableCollection<HomeFeed>();
            Homefeed.Add(new HomeFeed { Key = "Faculties", Value = Faculty_Department.Count-1 });
            int num = 0;
            foreach (var item in Faculty_Department)
            {
                num += item.Departments.Count;
            }
            Homefeed.Add(new HomeFeed { Key = "Departments", Value = num });
            Homefeed.Add(new HomeFeed { Key = "Courses", Value = 412 });

        }

        public async Task GetCourses(string home)
        {
            State = LayoutState.Loading;
            State = LayoutState.Loading;
            State = LayoutState.Loading;
            RestClient<MyCourses> restClient = new RestClient<MyCourses>();
            
            MyCourses newMyCourses = await restClient.GetCourses(LoginResponse.token);
            
            if (newMyCourses == null)
            {
                State = LayoutState.Error;
                State = LayoutState.Error;
                EventHandler<bool> handler = RefreshComplete;
                if (handler != null)
                {
                    handler(this, false);
                }
                return;
            }
            else
            {
                MyCourses = newMyCourses;
                AppState<MyCourses>.SerializeAndSave(MyCourses, "MyCourses");
                if (home == "home")
                {
                    HomeFilterByCode();
                }
                else
                {
                    CoursesFilterByCode();
                }
            }
            
            //SortCourses();
            //AppState<MyCourses>.SerializeAndSave(MyCourses, "MyCourses");
            
        }
        public void CoursesFilterByCode()
        {
            if (MyCourses == null || MyCourses.courses == null || MyCourses.courses.Count == 0)
            {
                State = LayoutState.None;
                return;
            }

            foreach (var item in MyCourses.courses)
            {
                foreach (var material in item.materials)
                {
                    material.title = item.title;
                    material.year = item.year;
                    material.code = item.code;
                    material.dept = item.dept;
                }
            }
            ObservableCollection<Course> courses = new ObservableCollection<Course>();

            //put first course in the list
            courses.Add(MyCourses.courses[0]);
            foreach (var item in MyCourses.courses)
            {
                if (item == courses[courses.Count - 1])
                {
                    continue;
                }
                if (item.code == courses[courses.Count - 1].code)
                {
                    foreach (var material in item.materials)//add the  new materials to previous material with same course code
                    {
                        courses[courses.Count - 1].materials.Add(material);
                    }
                    //MyCourses.courses.Remove(item);
                }
                else
                {
                    bool filtered = false;
                    for (int i = 0; i < courses.Count; i++)
                    {
                        if (courses[i].code == item.code)
                        {
                            filtered = true;
                            foreach (var material in item.materials)//add the  new materials to previous material with same course code
                            {
                                courses[i].materials.Add(material);
                            }
                        }
                    }
                    if (!filtered)
                    {
                        courses.Add(item);
                    }
                }

            }
            MyCourses = new MyCourses { courses = courses };
            State = LayoutState.None;
            SelectedCourse = MyCourses.courses[0];
            EventHandler<bool> handler = RefreshComplete;
            if (handler != null)
            {
                handler(this, false);
            }
        }
        public void HomeFilterByCode()
        {
            if (MyCourses == null || MyCourses.courses == null || MyCourses.courses.Count == 0)
            {
                State = LayoutState.None;
                return;
            }

            foreach (var item in MyCourses.courses)
            {
                foreach (var material in item.materials)
                {
                    material.title = item.title;
                    material.year = item.year;
                    material.code = item.code;
                    material.dept = item.dept;
                }
            }
            ObservableCollection<Course> courses = new ObservableCollection<Course>();

            //put first course in the list
            
            courses.Add(MyCourses.courses[0]);

            foreach (var item in MyCourses.courses)
            {
                //if (item.code.Substring(0, 3) == courses[courses.Count - 1].code.Substring(0, 3))
                if (item == courses[courses.Count - 1])
                {
                    continue;
                }
                if (item.code.Substring(0, 3) == courses[courses.Count - 1].code.Substring(0, 3))
                {
                    foreach (var material in item.materials)//add the  new materials to previous material with same course code
                    {
                        courses[courses.Count - 1].materials.Add(material);
                    }
                }
                else
                {
                    bool filtered = false;
                    for (int i = 0; i < courses.Count; i++)
                    {
                        if (courses[i].code.Substring(0, 3) == item.code.Substring(0, 3))
                        {
                            filtered = true;
                            foreach (var material in item.materials)//add the  new materials to previous material with same course code
                            {
                                courses[i].materials.Add(material);
                            }
                        }
                    }
                    if (!filtered)
                    {
                        courses.Add(item);
                    }
                }

            }
            MyCourses = new MyCourses { courses = courses };
            State = LayoutState.None;
            State = LayoutState.None;
            SelectedCourse = MyCourses.courses[0];
            EventHandler<bool> handler = RefreshComplete;
            if (handler != null)
            {
                handler(this, false);
            }

        }
        public void SortCourses()
        {

            
            
            
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

            SignUpResponse signUpResponse = await restClient.EditProfile(key,value,LoginResponse.token);

            if (signUpResponse == null)
            {
                return null;
            }
            else if(signUpResponse.msg == "Error Updating Profile")
            {
                DependencyService.Get<IToast>().LongToast($"Error Updating Profile");
            }
            else
            {
                DashBoard user = await GetDashBoardAsync();
                loginResponse.user = user.user;
                LoginResponse = loginResponse;
            }
            /*EventHandler<string> handler = MaterialUploadedEvent;
            if (handler != null)
            {
                handler(UploadModel, signUpResponse.msg);
            }*/
            //await GetCourses("courses");

            return null;
        }

        public async Task<SignUpResponse> UploadAsync()
        {
            //Application.Current.MainPage = new Views.MainAppPage(new LoginResponse());
            //return null;
            
            
            RestClient<SignUpResponse> restClient = new RestClient<SignUpResponse>();

            SignUpResponse signUpResponse = await restClient.PostUploadAsync(UploadModel,LoginResponse.token);

            if (signUpResponse == null)
            {
                return null;
            }
            EventHandler<string> handler = MaterialUploadedEvent;
            if (handler != null)
            {
                handler(UploadModel, signUpResponse.msg);
            }
            await GetCourses("courses");
            
            return null;
        }

        public void AnimateTotalFaculty()
        {
            
            Device.BeginInvokeOnMainThread(() =>
            {
                int OriginalTotalFaculty = Homefeed[0].Value;
                Homefeed[0].Value = 1;
                Device.StartTimer(TimeSpan.FromSeconds(0.2), () =>
                {
                    if (Homefeed[0].Value == OriginalTotalFaculty)
                    {
                        return false;
                    }
                    Homefeed[0].Value++;
                    return true;
                });
            });
            
        }
        public void AnimateTotalCourses()
        {
            
            Device.BeginInvokeOnMainThread(() =>
            {
                int OriginalTotalNoOfCourses = Homefeed[2].Value;
                Homefeed[2].Value = 300;
                Device.StartTimer(TimeSpan.FromMilliseconds(0.5), () =>
                {
                    if (Homefeed[2].Value == OriginalTotalNoOfCourses)
                    {
                        return false;
                    }
                    Homefeed[2].Value++;
                    return true;
                });
            });

        }
        public void AnimateTotalDepartment()
        {
            
            Device.BeginInvokeOnMainThread(() =>
            {
                int OriginalTotalDepartment = Homefeed[1].Value; //gets the departmet properties
                Homefeed[1].Value = 12;
                Device.StartTimer(TimeSpan.FromSeconds(0.07), () =>
                {
                    if (Homefeed[1].Value == OriginalTotalDepartment)
                    {
                        return false;
                    }
                    Homefeed[1].Value++;
                    return true;
                });
            });
            
        }
        private bool isInternetConnected()
        {
            var internetAccess = Xamarin.Essentials.Connectivity.NetworkAccess;
            if (internetAccess == NetworkAccess.Internet || Connectivity.ConnectionProfiles.Contains(ConnectionProfile.WiFi) || Connectivity.ConnectionProfiles.Contains(ConnectionProfile.Bluetooth))
            {
                return true;
            }
            return false;
        }

        public void sleep()
        {
            
        }
        public async void restoreCoursePage()
        {
            MyCourses myCourses = AppState<MyCourses>.DeserializeAndRestore("MyCourses");
            if (myCourses != null)
            {
                MyCourses = myCourses;
                //SortCourses();
                //SelectedCourse
                
            }

            /*MyCourses myCourses = AppState<MyCourses>.DeserializeAndRestore("MyCourses");
            if (myCourses != null)
            {
                MyCourses = myCourses;
                SortCourses();
                //SelectedCourse
                Course myCourse = AppState<Course>.DeserializeAndRestore("SelectedCourse");
                if (myCourse != null)
                {
                    await Task.Delay(10);
                    SelectedCourse = myCourse;
                    //SelectedMaterial
                    Material material = AppState<Material>.DeserializeAndRestore("SelectedMaterial");
                    if (material != null)
                    {
                        await Task.Delay(10);
                        SelectedMaterial = material;
                    }
                }
            }*/
        }

        
    }
}
