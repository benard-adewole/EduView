using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SkiaSharp.Views.Forms;
using SkiaSharp;
using Estudiar.Helpers;
using Microcharts.Forms;
using Microcharts;
using Estudiar.Models;
using Estudiar.ViewModels;

namespace Estudiar.CGPA
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Calculator : ContentPage
    {
        calcVm calcVm;
        SemesterGroup semesterGroup;
        public Calculator()
        {
            
        }
        protected override void OnAppearing()
        {
            //sfPdfViewVM.GetStream();
            base.OnAppearing();
        }
        private async void LoadPageAsync()
        {
            await Task.Delay(500);
            await MyLazyView.LoadViewAsync();
            MyLazyView.Content.BindingContext = new calcVm();
            (MyLazyView.Content as CalculatorCV).Init(semesterGroup);
        }
        public Calculator(SemesterGroup semesterGroup)
        {
            LoadPageAsync();
            InitializeComponent();
            this.semesterGroup = semesterGroup;
            //BindingContext = new calcVm();
            calcVm newCalVM = new calcVm();
            if (semesterGroup != null)
            {
                newCalVM.Universitygrades = semesterGroup.universitygrades;
                newCalVM.CalcOtherParams();
                newCalVM.UpdateGradeFreqGraph();
                newCalVM.UpdateGPAcomparisonGraph();
                newCalVM.FileName = semesterGroup.Name;
                //SaveEditToolbar.IsVisible = true;
                //cgpaCanvas.InvalidateSurface();

            }
            else
            {
                //SaveEditToolbar.IsVisible = false;
                //closeChart();
                newCalVM.Universitygrades.Add(new universitygrade() { semester = 1 });
                newCalVM.FileName = "unsaved";
            }

            BindingContext = newCalVM;
            calcVm = newCalVM;



        }
    }
}