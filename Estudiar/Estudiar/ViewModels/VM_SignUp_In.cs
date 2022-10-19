using System;
using System.Collections.Generic;
using System.Text;
using Estudiar.Models;
using Estudiar.RestClients;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Estudiar.XInterface;
using Estudiar.Toolkit;
using Xamarin.Essentials;
using Estudiar.Views;

namespace Estudiar.ViewModels
{
    public class VM_SignUp_In:INotifyPropertyChanged
    {
        public event EventHandler<string> SignUpResponseReceived;
        public event EventHandler<string> FailedLogin;
        public event EventHandler<LoginResponse> ObtainenedMainVM;
        public event EventHandler CustomButtomClicked;
        public event EventHandler<LoginResponse> LoginResponseReceived;
        public string Default { get; set; }
        private ObservableCollection<Model_Fac_dept> _Faculty_Department;

        public ObservableCollection<Model_Fac_dept> Faculty_Department
        {
            get { return _Faculty_Department; }
            set { _Faculty_Department = value; }
        }

        private Model_Fac_dept previousSelectedFac;

        private Model_Fac_dept selectedFac;

        public Model_Fac_dept SelectedFac
        {
            get { return selectedFac; }
            set {
                
                selectedFac = value;
                signUpModel.faculty = value.Faculty;
                SelectedDept = new DeptSelect();
                if (previousSelectedFac != null && previousSelectedFac == selectedFac)
                {
                    return;
                }
                int index = 0;
                foreach (var item in Faculty_Department)
                {
                    if (item == selectedFac)
                    {
                        Model_Fac_dept temp = selectedFac;
                        Faculty_Department.Remove(selectedFac);

                        //modify it
                        temp.Selected = true;
                        Faculty_Department.Insert(index, temp);
                        break;
                    }
                    index++;
                }
                if (previousSelectedFac == null)
                {
                    previousSelectedFac = SelectedFac;
                }
                else
                {

                    index = 0;
                    foreach (var item in Faculty_Department)
                    {
                        if (item == previousSelectedFac)
                        {
                            Model_Fac_dept temp = previousSelectedFac;
                            Faculty_Department.Remove(previousSelectedFac);

                            //modify it
                            temp.Selected = false;
                            Faculty_Department.Insert(index, temp);
                            break;
                        }
                        index++;
                    }
                    previousSelectedFac = selectedFac;
                }
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ContentView> contentViews;

        public ObservableCollection<ContentView> ContentViews
        {
            get { return contentViews; }
            set { contentViews = value;OnPropertyChanged(); }
        }

        private DeptSelect previousDeptSelect;

        private DeptSelect selectedDept;

        public DeptSelect SelectedDept
        {
            get { return selectedDept; }
            set 
            {
                if (value.Department == null)
                {
                    return;
                }
                selectedDept = value;
                signUpModel.dept = value.Department;
                if (previousDeptSelect != null && previousDeptSelect == selectedDept)
                {
                    return;
                }
                int index = 0;
                foreach (var item in SelectedFac.Departments)
                {
                    if (item == selectedDept)
                    {
                        DeptSelect temp = selectedDept;
                        SelectedFac.Departments.Remove(selectedDept);

                        //modify it
                        temp.Selected = true;
                        SelectedFac.Departments.Insert(index, temp);
                        break;
                    }
                    index++;
                }
                if (previousDeptSelect == null)
                {
                    previousDeptSelect = selectedDept;
                }
                else
                {

                    index = 0;
                    foreach (var item in selectedFac.Departments)
                    {
                        if (item == previousDeptSelect)
                        {
                            DeptSelect temp = previousDeptSelect;
                            selectedFac.Departments.Remove(previousDeptSelect);

                            //modify it
                            temp.Selected = false;
                            selectedFac.Departments.Insert(index, temp);
                            break;
                        }
                        index++;
                    }
                    previousDeptSelect = selectedDept;
                }
                OnPropertyChanged(); 
            }
        }

        //----------Sign up data------------------------
        ////////////////////////////////////////////////
        private SignUpModel _signUpModel;

        public SignUpModel signUpModel
        {
            get { return _signUpModel; }
            set 
            {
                //value.email = value.email.Trim();
                //value.name = value.name.Trim();
                _signUpModel = value;
                OnPropertyChanged();
            }
        }

        public ICommand CustomButtonClicked
        {
            get
            {
                return new Command(async() =>
                {
                    await Shell.Current.Navigation.PushAsync(new FacultySelect(this));
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
        
        public ICommand SignUpNow
        {
            get
            {
                
                return new Command(() =>
                {
                    if (string.IsNullOrEmpty(signUpModel.email) || !Tools.IsEmail(signUpModel.email))
                    {
                        DependencyService.Get<IToast>().LongToast("Email is invalid");
                    }
                    else if (string.IsNullOrEmpty(signUpModel.password) && signUpModel.password != Repassword)
                    {
                        DependencyService.Get<IToast>().LongToast("Passwords don't match");
                    }
                    else if (signUpModel.year == 0)
                    {
                        DependencyService.Get<IToast>().LongToast("Select your level");
                    }
                    else if (string.IsNullOrEmpty(signUpModel.faculty))
                    {
                        DependencyService.Get<IToast>().LongToast("Select a faculty");
                    }
                    else if (string.IsNullOrEmpty(signUpModel.dept))
                    {
                        if (!FacultyHasNoDepartment(signUpModel.faculty))
                        {
                            DependencyService.Get<IToast>().LongToast("Select a department");
                        }
                        else
                        {
                            signUpAsync();
                        }
                    }
                    
                    else
                    {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                        signUpAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
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
        ////////////////////////////////////////////////
        //---------------------------------------------

        public ICommand GoBack
        {
            get
            {
                return new Command(async () =>
                {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    await Shell.Current.Navigation.PopAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

                });
            }
            private set { }
        }

        //----------------Login data------------------
        public ICommand LogInNow
        {
            get
            {
                return new Command(async() =>
                {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                   await logInAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

                });
            }
            private set { }
        }
        private LoginModel loginModel;

        public LoginModel LoginModel
        {
            get { return loginModel; }
            set { 
                loginModel = value;
                AppState<LoginModel>.SerializeAndSave(value, "LoginModel");
                OnPropertyChanged();
            }
        }


        private IList<int> level;

        public IList<int> Level
        {
            get { return level; }
            set { level = value; }
        }
        
        private ImageTextModel myVar;
        public ImageTextModel PickerResult
        {
            get { return myVar; }
            set
            {
                myVar = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public VM_SignUp_In()
        {
            /*LoginView loginView = new LoginView(this);
            RegisterView registerView = new RegisterView(this);
            ContentViews = new ObservableCollection<ContentView> { registerView, loginView };*/
            InitializeLevel();
            InitializeFaculties_Department();
            isLogingIn = false;
            
            signUpModel = new SignUpModel();
            LoginModel = new LoginModel();
            signUpModel.password = "";
            Repassword = "";
            restore();
            //SelectedFacDept = Faculty_Department[0];
            
        }
        private void InitializeLevel()
        {
            Level = new List<int> { 100,200,300,400,500,600};
        }
        private void InitializeFaculties_Department()
        {
            Faculty_Department = FacDeptImg.RetrieveData();
            
        }

        public void sleep()
        {
            AppState<SignUpModel>.SerializeAndSave(signUpModel, "SignUpDb");
            AppState<LoginModel>.SerializeAndSave(loginModel, "LoginDb");
        }
        private void restore()
        {
            SignUpModel signUp = AppState<SignUpModel>.DeserializeAndRestore("SignUpDb");
            if (signUp != null)
            {
                signUpModel = signUp;
                signUpModel.faculty = "";
                signUpModel.dept = "";
            }

            LoginModel login = AppState<LoginModel>.DeserializeAndRestore("LoginDb"); 
            if (login != null)
            {
                LoginModel = login;
            }
            
        }
        private string repassword;

        public string Repassword
        {
            get { return repassword; }
            set { repassword = value; OnPropertyChanged(); }
        }

        //public void SerializeAndSave(object CurrentInfo, string DbName)//Convert C# model to json
        //{
        //    var json = JsonConvert.SerializeObject(CurrentInfo);
        //    Xamarin.Forms.Application.Current.Properties[DbName] = json;
        //}
        //public SignUpModel DeserializeAndRestoreSignUp(string DbName)//Convert json to c# model
        //{
        //    try
        //    {
        //        //string json = Xamarin.Forms.Application.Current.Properties[DbName].ToString();
        //        return AppState<SignUpModel>.DeserializeAndRestore(DbName);
        //        //var taskModels = JsonConvert.DeserializeObject<SignUpModel>(json);
        //        //return taskModels;
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}
        //public LoginModel DeserializeAndRestoreLogin(string DbName)//Convert json to c# model
        //{
        //    try
        //    {
        //        string json = Xamarin.Forms.Application.Current.Properties[DbName].ToString();
        //        var taskModels = JsonConvert.DeserializeObject<LoginModel>(json);
        //        return taskModels;
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }

        //}

        public async Task<SignUpResponse> signUpAsync()
        {
            RestClient<SignUpModel> restClient = new RestClient<SignUpModel>();
            
            SignUpResponse signUpResponse = await restClient.PostSignUpAsync(signUpModel);

            if (signUpResponse == null)
            {
                return null;
            }
            EventHandler<string> handler = SignUpResponseReceived;
            if (handler != null)
            {
                handler(signUpModel, signUpResponse.msg);
            }
            return signUpResponse;
        }

        public void DisplayAlert(string Content)
        {
            EventHandler<string> handler = SignUpResponseReceived;
            if (handler != null)
            {
                handler(signUpModel, Content);
            }
        }

        private bool isLogingIn;

        public bool IsLoginIn
        {
            get { return isLogingIn; }
            set { isLogingIn = value; OnPropertyChanged(); }
        }

        public async Task<LoginResponse> logInAsync()
        {
            //Application.Current.MainPage = new Views.MainAppPage(new LoginResponse());
            //return null;
            IsLoginIn = true;
            RestClient<LoginModel> restClient = new RestClient<LoginModel>();
            
            LoginResponse loginResponse = await restClient.PostLoginAsync(LoginModel);
            EventHandler<string> handler = SignUpResponseReceived;

            if (loginResponse == null)
            {
                IsLoginIn = false;
                handler(loginModel, "Timeout Error");
                return null;
            }
            if (handler != null && loginResponse.user == null)
            {   
                //Print the msg i replacedin the token
                IsLoginIn = false;
                handler(loginModel, loginResponse.token);
            }
            else if (loginResponse.user != null)
            {
                sleep();
                Preferences.Set("MyCourses", "");
                MainVM mainVM = new MainVM() { LoginResponse = loginResponse };
                IsLoginIn = false;
                //await Shell.Current.Navigation.PopAsync();
                //await Shell.Current.GoToAsync($"///{nameof(Views.CustomTab)}");
                Application.Current.MainPage = new Estudiar.Views.MainShell();

                //(Shell.Current.CurrentPage as CustomTab).setup();
                
                //await Shell.Current.Navigation.PushAsync(new CustomTab());

                /*EventHandler<LoginResponse> handler1 = ObtainenedMainVM;
                if (handler1 != null)
                {
                    handler1(mainVM, loginResponse);
                }*/
                
            }
            return loginResponse;
        }

        
    }
}
