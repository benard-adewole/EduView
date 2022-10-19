using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Estudiar.Models;
using Estudiar.XInterface;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Estudiar.ViewModels
{
    public class savedDataVm : INotifyPropertyChanged
    {

        private bool _IsRecentHidden;

        public bool IsRecentHidden
        {
            get { return _IsRecentHidden; }
            set 
            {
                _IsRecentHidden = value;
                Preferences.Set("IsRecentHidden", value);
                OnPropertyChanged("IsRecentHidden");
            }
        }


        private bool _IsResultLocked;

        public bool IsResultLocked
        {
            get { return _IsResultLocked; }
            set 
            {
                _IsResultLocked = value;
                Preferences.Set("IsResultLocked", value);
                OnPropertyChanged("IsResultLocked");
            }
        }

        private ObservableCollection<SemesterGroup> semesters;

        public ObservableCollection<SemesterGroup> SemesterGroup
        {
            get 
            {
                return semesters;
            }
            set 
            {
                semesters = value;
                AppState<AllSavedData>.SerializeAndSave(new AllSavedData { SemesterGroup = value},"AllSavedData");
                OnPropertyChanged("SemesterGroup");
            }
        }

        public ICommand DeleteAll
        {
            get
            {
                return new Command(async () =>
                {
                    if (await Shell.Current.DisplayAlert("Delete Alert","Do you want  to delete all saved items","Yes","No"))
                    {
                        SemesterGroup = new ObservableCollection<SemesterGroup>();
                        ToastOptions toastOptions = new ToastOptions()
                        {
                            MessageOptions = new MessageOptions { Message = "All items Deleted successfully" },
                            BackgroundColor = Color.Green//(Color)Application.Current.Resources["Primary"]
                        };
                        await Shell.Current.DisplayToastAsync(toastOptions);
                    }
                    
                });
            }
            private set { }
        }
        public ICommand EditSemesterGroup
        {
            get
            {
                return new Command<SemesterGroup>(async (param) =>
                {
                    /*EventHandler<SemesterGroup> handler = EditClicked;
                    if (handler != null)
                    {
                        handler(this, param);
                    }*/
                    await Shell.Current.Navigation.PushAsync(new Estudiar.CGPA.Calculator(param));


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
                    await Shell.Current.Navigation.PopAsync();
                });
            }
            private set { }
        }
        public ICommand DeleteSemesterGroup
        {
            get
            {
                return new Command<SemesterGroup>(async (param) =>
                {
                    if (await Shell.Current.DisplayAlert("Delete Alert", "Do you want  to delete this item saved as "+ param.Name, "Yes", "No"))
                    {
                        SemesterGroup.Remove(param);
                        SemesterGroup = SemesterGroup;
                    }

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
        public ICommand HideRecent
        {
            get
            {
                return new Command(async () =>
                {
                    
                    if (!IsRecentHidden)
                    {
                        IsRecentHidden = true;
                        DependencyService.Get<IToast>().LongToast("Dashboard won't display last saved result");
                    }
                    else
                    {
                        IsRecentHidden = false;
                        DependencyService.Get<IToast>().LongToast("Dashboard will display last saved result");
                    }
                });
            }
            private set { }
        }
        public ICommand ToggleFingerPrint
        {
            get
            {
                return new Command(() =>
                {
                    if(!IsResultLocked)
                    {
                        IsResultLocked = true;
                        DependencyService.Get<IToast>().LongToast("Fingerprint lock enabled");
                    }
                    else
                    {
                        IsResultLocked = false;
                        DependencyService.Get<IToast>().LongToast("Fingerprint lock disabled");
                    }
                });
            }
            private set { }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public savedDataVm()
        {
            IsResultLocked = Preferences.Get("IsResultLocked", false);
            IsRecentHidden = Preferences.Get("IsRecentHidden", false);
            GetData();
        }
        public void GetData()
        {
            var groups = AppState<AllSavedData>.DeserializeAndRestore("AllSavedData");
            if (groups != null)
            {
                SemesterGroup = groups.SemesterGroup;
            }
            else
            {
                SemesterGroup = new ObservableCollection<SemesterGroup>();
            }
        }
    }
}
