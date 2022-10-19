using Estudiar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Estudiar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class savepopup : Popup
    {
        public savepopup(string initialName)
        {
            InitializeComponent();
            filename.Text = initialName;
            FileExist();
        }

        private void FileExist()
        {
            FileExists.Opacity = 0;
            SaveBtn.IsEnabled = true;
            try
            {
                var semestergroup = AppState<AllSavedData>.DeserializeAndRestore("AllSavedData").SemesterGroup;
                foreach (var item in semestergroup)
                {
                    if (filename.Text.Trim().ToLower() == "unsaved")
                    {
                        FileExists.Text = "*Cannot save file as 'unsaved'";
                        FileExists.Opacity = 1;
                        SaveBtn.IsEnabled = false;
                        break;
                    }
                    if (item.Name.ToLower().Trim() == filename.Text.Trim().ToLower())
                    {
                        FileExists.Text = "*File with same name already exists";
                        FileExists.Opacity = 1;
                        SaveBtn.IsEnabled = false;
                        break;
                    }
                }
            }
            catch
            {
                if (filename.Text.Trim().ToLower() == "unsaved")
                {
                    FileExists.Text = "*Cannot save file as 'unsaved'";
                    FileExists.Opacity = 1;
                    SaveBtn.IsEnabled = false;
                }
            }
        }
        private void SaveBtn_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(filename.Text))
            {
                return;
            }
            filename.Unfocus();
            this.Dismiss(filename.Text.Trim().ToLower());
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            filename.Unfocus();
            this.Dismiss(null);
        }

        private void filename_TextChanged(object sender, TextChangedEventArgs e)
        {
            FileExist();
        }
    }
}