using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Estudiar.ViewModels
{
    public class LoadingVM: INotifyPropertyChanged
    {
        public event EventHandler<string> DismissReceived;
        private bool dismiss;

        public bool Dismiss
        {
            get { return dismiss; }
            set 
            {
                dismiss = value;
                if (dismiss)
                {
                    EventHandler<string> handler = DismissReceived;
                    if (handler != null)
                    {
                        handler(this, "done");
                    }
                }
                
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
