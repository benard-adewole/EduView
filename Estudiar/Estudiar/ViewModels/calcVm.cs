using Estudiar.Models;
using Estudiar.XInterface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Xamarin.Forms;
using Microcharts.Forms;
using Microcharts;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using Xamarin.Essentials;
using Estudiar.CGPA;

namespace Estudiar.ViewModels
{

    public class calcVm:INotifyPropertyChanged
    {
        public event EventHandler ParamsChanged;

        private int deleteIndex;

        public List<string> possibleunit { get; set; } = new List<string>() { "0", "1", "2", "3", "4", "5", "6", "7" };
        public List<string> possiblegrades { get; set; } = new List<string>() { "A", "B", "C", "D", "E", "F" };

        private string fileName;

        public string FileName
        {
            get { return fileName; }
            set { fileName = value;OnPropertyChanged("FileName"); }
        }


        private SemesterGroup semesterGrouptoload;

        public SemesterGroup SelectedSavedGradeToLoad
        {
            get { return semesterGrouptoload; }
            set { semesterGrouptoload = value; OnPropertyChanged("SelectedSavedGradeToLoad"); }
        }
        private Chart _gradeFreq;

        public Chart GradeFreq
        {
            get 
            {
                return _gradeFreq; 
            }
            set 
            {
                _gradeFreq = value;
                OnPropertyChanged();
            }
        }

        private Chart _GPAcompare;

        public Chart GPAcompare
        {
            get
            {
                return _GPAcompare;
            }
            set
            {
                _GPAcompare = value;
                OnPropertyChanged();
            }
        }

        public int DeletedIndex
        {
            get { return deleteIndex; }
            set { deleteIndex = value; }
        }

        private float _cgpa;

        public float cgpa
        {
            get { return _cgpa; }
            set 
            {
                EventHandler handler = ParamsChanged;
                if (handler != null && cgpa != value)
                {
                    handler(this, null);
                }
                _cgpa = value;
                OnPropertyChanged("cgpa");
            }
        }

        private int totalunits;

        public int TotalUnits
        {
            get { return totalunits; }
            set { totalunits = value; OnPropertyChanged("TotalUnits"); }
        }

        private int numberofCursesDone;

        public int NumberofCoursesDone
        {
            get { return numberofCursesDone; }
            set { numberofCursesDone = value; OnPropertyChanged("NumberofCoursesDone"); }
        }

        private float totalScore;

        public float TotalScore
        {
            get { return totalScore; }
            set { totalScore = value; OnPropertyChanged("TotalScore"); }
        }
        private float overAllAttainableScore;

        public float OverAllAttainableScore
        {
            get { return overAllAttainableScore; }
            set { overAllAttainableScore = value; OnPropertyChanged("OverAllAttainableScore"); }
        }


        private ObservableCollection<universitygrade> _universitygrades;

        public ObservableCollection<universitygrade> Universitygrades
        {
            get { return _universitygrades; }
            set { _universitygrades = value;OnPropertyChanged("Universitygrades"); }
        }


        private ObservableCollection<CourseDetails> _coursedetails;

        public ObservableCollection<CourseDetails> Coursedetails
        {
            get { return _coursedetails; }
            set { _coursedetails = value;OnPropertyChanged("Coursedetails"); }
        }
        public ICommand InfoCardsPressed
        {
            get
            {
                return new Command<Estudiar.Controls.CardViewInfo>((param) =>
                {
                    DependencyService.Get<IToast>().ShortToast($"{param.Value} {param.Description}");
                });
            }
            private set { }
        }
        public ICommand AddCourse
        {
            get
            {
                return new Command<ObservableCollection<CourseDetails>>((param) =>
                {
                    
                    foreach (var universitygrade in Universitygrades)
                    {
                        if (universitygrade.GetCourseDetails == param)
                        {
                            param.Add(new CourseDetails() { semester = universitygrade.semester, index = universitygrade.GetCourseDetails.Count+1});
                            OnPropertyChanged();
                            break;
                        }
                    }
                    
                });
            }
            private set { }
        }
        public ICommand DeleteCourse
        {
            get
            {
                return new Command<CourseDetails>( (param) =>
                {
                    foreach (var item in Universitygrades)
                    {
                        if (item.semester == param.semester)
                        {
                            DeletedIndex = item.GetCourseDetails.IndexOf(param);
                            item.GetCourseDetails.Remove(param);
                            //DependencyService.Get<ISnackBar>().SnackbarShow("Restore deleted course");
                        }
                    }
                });
            }
            private set { }
        }
        public ICommand DeleteSemester
        {
            get
            {
                return new Command<universitygrade>(async (param) =>
                {
                    if (Universitygrades.Last<universitygrade>() == param)
                    {
                        int index = Universitygrades.IndexOf(param);

                        if (Universitygrades.Count == 1)
                        {
                            ClearInputs.Execute(null);
                            return;
                        }
                        else
                        {
                            Universitygrades.Remove(param);
                            CalcOtherParams();
                        }
                        
                        //Snack bar call back
                        SnackBarActionOptions actionOptions = new SnackBarActionOptions
                        {
                            Action = () =>
                            {
                                //
                                if (Universitygrades.Count == 0)
                                {
                                    Universitygrades.Add(param);
                                }
                                else
                                {
                                    Universitygrades.Insert(index, param);
                                }
                                
                                CalcOtherParams();
                                return Task.CompletedTask;
                            },
                            Text = "Undo",
                            //BackgroundColor = Color.Red,
                            ForegroundColor = Color.White,
                        };
                        SnackBarOptions barOptions = new SnackBarOptions
                        {
                            MessageOptions = new MessageOptions { Message = "Restore deleted semester" },
                            Actions = new[] { actionOptions },
                            BackgroundColor = (Color)Application.Current.Resources["Primary"]
                        };
                        var result = await Shell.Current.DisplaySnackBarAsync(barOptions);
                        
                        
                    }
                    else
                    {
                        ToastOptions toastOptions = new ToastOptions()
                        {
                            MessageOptions = new MessageOptions { Message = "You can only start deleting from the last semester upward" },
                            BackgroundColor = (Color)Application.Current.Resources["Primary"]
                        };
                        await Shell.Current.DisplayToastAsync(toastOptions);
                    }
                    
                });
            }
            private set { }
        }

        
        public ICommand ClearInputs
        {
            get
            {
                return new Command(async () =>
                {
                    if (await Shell.Current.DisplayAlert("Alert","Clear all grades input","Yes","No"))
                    {
                        Universitygrades = new ObservableCollection<universitygrade>();
                        NumberofCoursesDone = 0;
                        cgpa = 0f;
                        UpdateGradeFreqGraph();
                        UpdateGPAcomparisonGraph();
                        TotalUnits = 0;
                        TotalScore = 0f;
                        OverAllAttainableScore = 0f;
                        ToastOptions toastOptions = new ToastOptions()
                        {
                            MessageOptions = new MessageOptions { Message = "All courses deleted" },
                            BackgroundColor = Color.Green,//(Color)Application.Current.Resources["Primary"]
                        };
                        await Shell.Current.DisplayToastAsync(toastOptions);
                       


                    }
                    
                });
            }
            private set { }
        }
        public ICommand Rename
        {
            get
            {
                return new Command<string>(async (param) =>
                {
                    var result = await Shell.Current.Navigation.ShowPopupAsync(new Views.savepopup(param));
                    if (result is null)
                    { return; }

                    AllSavedData lastSaved = AppState<AllSavedData>.DeserializeAndRestore("AllSavedData"); // appState = (AppState<AllSavedData>)result;
                    if (lastSaved == null)
                    {
                        lastSaved = new AllSavedData { SemesterGroup = new ObservableCollection<SemesterGroup>() };
                    }
                    foreach (var semesterGroup in lastSaved.SemesterGroup)
                    {
                        if (semesterGroup.Name.ToLower() == param.ToLower())
                        {
                            lastSaved.SemesterGroup.Remove(semesterGroup);
                            break;
                        }
                    }
                    FileName = result as string;

                    lastSaved.SemesterGroup.Add(new SemesterGroup { Name = result as string, universitygrades = Universitygrades, Description = GetDescription() });
                    AppState<AllSavedData>.SerializeAndSave(lastSaved, "AllSavedData"); // appState = (AppState<AllSavedData>)result;
                    ToastOptions toastOptions = new ToastOptions()
                    {
                        MessageOptions = new MessageOptions { Message = "Result has been saved" },
                        BackgroundColor = Color.Green//(Color)Application.Current.Resources["Primary"]
                    };
                    await Shell.Current.DisplayToastAsync(toastOptions);
                });
            }
            private set { }
        }
        public ICommand OpenFreshCalc
        {
            get
            {
                return new Command(async () =>
                {
                    await Shell.Current.Navigation.PushAsync(new Estudiar.CGPA.Calculator(null));
                });
            }
            private set { }
        }
        public ICommand Pop
        {
            get
            {
                return new Command(async () =>
                {
                    if (FileName.ToLower() == "unsaved")
                    {
                        if (await Shell.Current.DisplayAlert("Unsaved Result","Discard Result","Yes","No"))
                        {
                            await Shell.Current.Navigation.PopAsync();
                        }
                    }
                    else
                    {
                        await Shell.Current.Navigation.PopAsync();
                    }
                });
            }
            private set { }
        }
        public ICommand Save
        {
            get
            {
                return new Command(async () =>
                {
                   var result =  await Shell.Current.Navigation.ShowPopupAsync(new Views.savepopup(FileName));
                    if (result is null)
                    { return; }
                    FileName = result as string;
                    AllSavedData lastSaved = AppState<AllSavedData>.DeserializeAndRestore("AllSavedData"); // appState = (AppState<AllSavedData>)result;
                    if  (lastSaved == null)
                    {
                        lastSaved = new AllSavedData { SemesterGroup = new ObservableCollection<SemesterGroup>()};
                    }
                    lastSaved.SemesterGroup.Add(new SemesterGroup { Name = result as string, universitygrades = Universitygrades, Description = GetDescription() });
                    AppState<AllSavedData>.SerializeAndSave(lastSaved, "AllSavedData"); // appState = (AppState<AllSavedData>)result;
                    ToastOptions toastOptions = new ToastOptions()
                    {
                        MessageOptions = new MessageOptions { Message = "Result has been saved" },
                        BackgroundColor = Color.Green//(Color)Application.Current.Resources["Primary"]
                    };
                    await Shell.Current.DisplayToastAsync(toastOptions); 
                });
            }
            private set { }
        }
        public ICommand SaveEdit
        {
            get
            {
                return new Command(async () =>
                {

                    AllSavedData lastSaved = AppState<AllSavedData>.DeserializeAndRestore("AllSavedData"); // appState = (AppState<AllSavedData>)result;

                    int index = 0;
                    for (int i = 0; i < lastSaved.SemesterGroup.Count; i++)
                    {
                        if (lastSaved.SemesterGroup[i].Name == FileName.ToLower())
                        {
                            lastSaved.SemesterGroup.RemoveAt(i);// (SelectedSavedGradeToLoad);
                            break;
                        }
                        index++;
                    }
                    
                    
                    //lastSaved.SemesterGroup.Add();
                    lastSaved.SemesterGroup.Insert(index, new SemesterGroup { Name = FileName, universitygrades = Universitygrades, Description = GetDescription() });
                    AppState<AllSavedData>.SerializeAndSave(lastSaved, "AllSavedData"); // appState = (AppState<AllSavedData>)result;
                    
                    ToastOptions toastOptions = new ToastOptions()
                    {
                        MessageOptions = new MessageOptions { Message = "Result Edited Successfully" },
                        BackgroundColor = Color.Green//(Color)Application.Current.Resources["Primary"]
                    };
                    await Shell.Current.DisplayToastAsync(toastOptions);
                });
            }
            private set { }
        }
        public ICommand OpenSavedData
        {
            get
            {
                return new Command(async () =>
                {
                    bool IsResultLocked = Preferences.Get("IsResultLocked", false);
                    if (IsResultLocked)
                    {
                        bool supported = await CrossFingerprint.Current.IsAvailableAsync(true);
                        if (supported)
                        {
                            AuthenticationRequestConfiguration conf = new AuthenticationRequestConfiguration("Access your result", "Access your result");
                            var result = await CrossFingerprint.Current.AuthenticateAsync(conf);

                            if (result.Status == FingerprintAuthenticationResultStatus.Succeeded)
                            {
                                await Shell.Current.Navigation.PushAsync(new SavedGrades());
                            }
                            else if (result.Status == FingerprintAuthenticationResultStatus.Denied)
                            {
                                DependencyService.Get<IToast>().LongToast("Biometric unrecognized");
                            }
                            else if (result.Status == FingerprintAuthenticationResultStatus.TooManyAttempts)
                            {
                                DependencyService.Get<IToast>().LongToast("Failed to authenticate. Too many attempts");
                            }
                            else
                            {
                                await Shell.Current.DisplayAlert("Sorry!", "Authentication failed!", "Ok");
                            }
                        }
                        else
                        {
                            await Shell.Current.DisplayAlert("Sorry!", "Biometrics are not available on your device", "Ok");
                        }
                    }
                    else
                    {
                        await Shell.Current.Navigation.PushAsync(new Estudiar.CGPA.SavedGrades());
                    }
                    
                });
            }
            private set { }
        }
        public ICommand ShowInfo
        {
            get
            {
                return new Command<universitygrade>(async (param) =>
                {
                    //show cgpa, sgpa, number of courses, number of units, total score
                    await Shell.Current.Navigation.ShowPopupAsync(new Estudiar.CGPA.ResultInfoPopUp(param, SemesterDescription(param)));

                });
            }
            private set { }
        }

        public ICommand SemesterSelected
        {
            get
            {
                return new Command<universitygrade>(async (param) =>
                {
                    var result = await Shell.Current.Navigation.ShowPopupAsync(new Estudiar.CGPA.SemesterOptionPopUp());
                    if (result is null)
                    { return; }
                    switch (result as string)
                    {
                        case "info":
                            ShowInfo.Execute(param);
                            break;
                        case "delete":
                            DeleteSemester.Execute(param);
                            break;
                        default:
                            break;
                    }


                });
            }
            private set { }
        }
        public int SemesterCourseCount(CourseDetails param)
        {
            foreach (var item in Universitygrades)
            {
                if (item.semester == param.semester)
                {
                    return item.GetCourseDetails.Count;
                }
            }
            return -1;
        }
        public ICommand AddSemester
        {
            get
            {
                return new Command(() =>
                {
                    Universitygrades.Add(new universitygrade { GetCourseDetails = new ObservableCollection<CourseDetails>() ,semester = Universitygrades.Count+1});
                    CalcOtherParams();
                });
            }
            private set { }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public calcVm()
        {
            Universitygrades = new ObservableCollection<universitygrade>();
            cgpa = 0f;
            TotalUnits = 0;
            totalScore = 0f;
            fileName = "";
            UpdateGradeFreqGraph();
            UpdateGPAcomparisonGraph();

            /*for (int i = 100;i < 400;i+= 100)
            {
                universitygrade universitygrade1 = new universitygrade() { semester = i/100};
                for (int j = 0; j < 3; j++)
                {
                    universitygrade1.GetCourseDetails.Add(new CourseDetails() { code ="ssg112",unit = j,semester= i/100, index=j+1 } );
                }
                Universitygrades.Add(universitygrade1);

            }
            CalcOtherParams();*/
        }
       
        public void UpdateGPAcomparisonGraph()
        {
            if (Universitygrades.Count == 0)
            {
                return;
            }
            List<float> GPAs = new List<float>();
            string[] pointColors = new string[] { "#77d065", "#2c3e50", "#00589C", "#FF4081", "#b455b6", "#3498db", "#2c3e50" };
            List<ChartEntry> entries = new List<ChartEntry>();
            Random random = new Random();
            int i = 0;
            foreach (var semester in Universitygrades)
            {
                GPAs.Add(semester.gpa);
                entries.Add(new ChartEntry(semester.gpa)
                {
                    Label = string.Format("Semester {0}", semester.semester),
                    ValueLabel = string.Format("{0:F2}", semester.gpa),
                    //randomly select a color
                    Color = SKColor.Parse(pointColors[i%7])
                });
                i++;
            }

            
            GPAcompare = new LineChart
            {
                Entries = entries,
                LineMode = LineMode.Spline,
                LineSize = 8,
                PointMode = PointMode.Circle,
                PointSize = 18,
                LabelTextSize = 22,
                BackgroundColor = Color.Transparent.ToSKColor(),
                ValueLabelOrientation = Orientation.Horizontal,
                //ValueLabelOption = ValueLabelOption.TopOfElement
            };
        }
        public void UpdateGradeFreqGraph()
        {
            int[] frequencyOfGrades = new int[] { 0, 0, 0, 0, 0, 0 };
            string[] pointColors = new string[] { "#77d065","#2c3e50","#00589C", "#FF4081", "#b455b6", "#3498db", "#2c3e50" };
            foreach (var semester in Universitygrades)
            {
                foreach (var coures in semester.GetCourseDetails)
                {
                    frequencyOfGrades[coures.gradeIndex] += 1;

                }
            }
            List<ChartEntry> entries = new List<ChartEntry>();
            for (int i = 0; i < possiblegrades.Count; i++)
            {
                entries.Add(new ChartEntry(frequencyOfGrades[i])
                {
                    Label = possiblegrades[i],
                    ValueLabel = frequencyOfGrades[i].ToString(),
                    Color = SKColor.Parse(pointColors[i])
                });
            }
            GradeFreq = new PointChart
            {
                Entries = entries,
                //IsAnimated = true,
                //AnimationDuration = new TimeSpan(100),
                BackgroundColor = Color.Transparent.ToSKColor(),
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                LabelTextSize = 22,
                PointSize = 22,
            };
        }
        private string GetDescription()
        {
            string result = "";
            return $"CGPA: {cgpa}";
            result += $"CGPA: {cgpa}, {TotalScore}/{OverAllAttainableScore}\n{TotalUnits} unit(s), {Universitygrades.Count} semester(s) + {NumberofCoursesDone} course(s)\n";
            int[] frequencyOfGrades = new int[]{ 0,0,0,0,0,0};
            foreach (var semester in Universitygrades)
            {
                foreach (var coures in semester.GetCourseDetails)
                {
                    frequencyOfGrades[coures.gradeIndex] += 1;

                }
            }
            for (int i = 0; i < frequencyOfGrades.Length; i++)
            {
                if (frequencyOfGrades[i] != 0)
                {
                    string terminate = frequencyOfGrades[i] == 1 ? "" : "'s, ";
                    if (i != frequencyOfGrades.Length - 1 && frequencyOfGrades.Length != 1)
                    {
                        result += $"{frequencyOfGrades[i]}{possiblegrades[i]}"+terminate;
                    }
                    else
                    {
                        result += $"{frequencyOfGrades[i]}{possiblegrades[i]}"+terminate;
                    }
                }
            }
            return result;
        }

        private string SemesterDescription(universitygrade currentSemester)
        {
            float SGPA = 0;

            int totallocalUnits = 0;
            float totallocalScore = 0f;
            float totallocalOverallAttainable = 0f;
            int localnumberofcourses = 0;
            foreach (var semester in Universitygrades)
            {
                if (semester.gpa == 0f)
                {
                    semester.calculateGPA();
                }
                localnumberofcourses += semester.GetCourseDetails.Count();
                totallocalScore += semester.scoreobtained;
                totallocalOverallAttainable += semester.overallobtainablescore;
                totallocalUnits += semester.totalunits;
                if (semester == currentSemester)
                {
                    break;
                }
            }
            
            if (totallocalOverallAttainable != 0)
            {
                SGPA = (totallocalScore * 5) / totallocalOverallAttainable;
            }

            return string.Format("Current Semester\nGPA: {0:F2}\nScore: {1}\nAttainable Score: {2}\n{3} unit(s), {4} course(s)\n\n\nSemester 1 up till Semester {5}\nSGPA: {6:F2}\nScore: {7}\nAttainable Score: {8}\n{9} unit(s), {10} course(s)",
                currentSemester.gpa,
                currentSemester.scoreobtained,
                currentSemester.overallobtainablescore,
                currentSemester.totalunits,
                currentSemester.GetCourseDetails.Count,
                currentSemester.semester,
                SGPA,
                totallocalScore,
                totallocalOverallAttainable,
                totallocalUnits,
                localnumberofcourses);
        }
        public void CalcOtherParams()
        {
            int totallocalUnits  =0;
            float totallocalScore = 0f;
            float totallocalOverallAttainable = 0f;
            int localnumberofcourses = 0;
            foreach (var semester in Universitygrades)
            {
                if (semester.gpa == 0f)
                {
                    semester.calculateGPA();
                }
                localnumberofcourses += semester.GetCourseDetails.Count();
                totallocalScore += semester.scoreobtained;
                totallocalOverallAttainable += semester.overallobtainablescore;
                totallocalUnits += semester.totalunits;
            }

            NumberofCoursesDone = localnumberofcourses;
            if (totallocalOverallAttainable == 0)
            {
                cgpa = 0f;
            }
            else
            {
                cgpa = (totallocalScore * 5) / totallocalOverallAttainable;
            }
            
            TotalUnits = totallocalUnits;
            TotalScore = totallocalScore;
            OverAllAttainableScore = totallocalOverallAttainable;
            UpdateGradeFreqGraph();
            UpdateGPAcomparisonGraph();
        }
    }
}
