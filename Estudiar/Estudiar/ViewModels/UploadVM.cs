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

namespace Estudiar.ViewModels
{
    public class UploadVM
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public UploadVM()
        {

        }
    }
}
