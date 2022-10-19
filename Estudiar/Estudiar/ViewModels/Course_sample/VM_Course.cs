using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Estudiar.ViewModels.Course_sample
{
    
    public class VM_Course
    {
        public event EventHandler<string> LaunchToolBarItem;
        public ICommand ToolBarCommand
        {
            get
            {
                return new Command<string>((param) =>
                {
                    EventHandler<string> handler = LaunchToolBarItem;
                    if (handler != null)
                    {
                        handler(this, param);
                    }
                });
            }
            private set { }
        }

    }
}
