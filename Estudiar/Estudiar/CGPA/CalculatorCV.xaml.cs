using Estudiar.Models;
using Estudiar.ViewModels;
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
using Xamarin.CommunityToolkit.UI.Views.Options;
using Xamarin.CommunityToolkit.UI.Views;
using System.Collections.ObjectModel;
using Estudiar.CustomViews;
using Xamarin.CommunityToolkit.Extensions;

namespace Estudiar.CGPA
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalculatorCV : ContentView
    {
        calcVm calcVm;
        SemesterGroup semesterGroup;
        Color[] colors;
        private double collectionItemtemplateHeight = 0;
        public CalculatorCV()
        {

        }
        public CalculatorCV(SemesterGroup semesterGroup)
        {
            Init(semesterGroup);
        }
        public void Init(SemesterGroup semesterGroup)
        {
            colors = SetGradients((Color)Application.Current.Resources["Primary"], Color.Red, 100).ToArray();
            Xamarin.Forms.Application.Current.RequestedThemeChanged += Current_RequestedThemeChanged;
            InitializeComponent();
            hideMenu();
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
                SaveEditToolbar.IsVisible = true;
                cgpaCanvas.InvalidateSurface();

            }
            else
            {
                SaveEditToolbar.IsVisible = false;
                closeChart();
                newCalVM.Universitygrades.Add(new universitygrade() { semester = 1 });
                newCalVM.FileName = "unsaved";
            }

            BindingContext = newCalVM;
            calcVm = newCalVM;

            var entries = new[]
            {
                new ChartEntry(212)
                {
                    Label = "UWP",
                    ValueLabel = "112",
                    Color = SKColor.Parse("#2c3e50")
                },
                new ChartEntry(248)
                {
                    Label = "Android",
                    ValueLabel = "648",
                    Color = SKColor.Parse("#77d065")
                },
                new ChartEntry(128)
                {
                    Label = "iOS",
                    ValueLabel = "428",
                    Color = SKColor.Parse("#b455b6")
                },
                new ChartEntry(514)
                {
                    Label = "Forms",
                    ValueLabel = "214",
                    Color = SKColor.Parse("#3498db")
                }
            };
        }

        private void Current_RequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
        {
            cgpaCanvas.InvalidateSurface();
        }

        private void CalcVm_CGPAchanged(object sender, EventArgs e)
        {
            cgpaCanvas.InvalidateSurface();
        }

        protected void OnAppearing()
        { 

            if (App.Current.RequestedTheme == Xamarin.Forms.OSAppTheme.Dark)
            {
                Settings.ChangeStatus(System.Drawing.Color.Black, false);
            }
            else
            {
                Settings.ChangeStatus((Color)Application.Current.Resources["BackgroundColor"], true);
            }

            //base.OnAppearing();

        }
        private async void hideMenu()
        {
            await Task.Delay(100);

            //closing menu that was  initially opened by xaml
            MoreMenu_Clicked(null, null);
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            //await DisplayAlert("unit", (BindingContext as calcVm).Universitygrades[0].GetCourseDetails[0].unit.ToString(), "Ok");
            await Shell.Current.DisplayAlert("unit", (BindingContext as calcVm).Universitygrades[0].GetCourseDetails[0].overall.ToString(), "Ok");
            //await DisplayAlert("height", course, "Ok");
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            foreach (var semester in calcVm.Universitygrades)
            {
                semester.calculateGPA();
                await Shell.Current.DisplayAlert("gpa", semester.gpa.ToString(), "Ok");

                var temp = semester;
                calcVm.Universitygrades.Remove(semester);
                calcVm.Universitygrades.Insert(0, temp);

                break;
            }
        }



        private async void OnDeleteUpdateExpannderHeight_Clicked(object sender, EventArgs e)
        {
            ImageButton button = (sender as ImageButton);
            CourseDetails commandParameter = button.CommandParameter as CourseDetails;
            int index = calcVm.DeletedIndex;
            ObservableCollection<CourseDetails> AllCourses = null;
            foreach (var item in calcVm.Universitygrades)
            {
                if (item.semester == commandParameter.semester)
                {
                    AllCourses = item.GetCourseDetails;
                    break;
                }
            }

            int coursesCount = calcVm.SemesterCourseCount(commandParameter);

            var element = button.Parent;
            var height = (element as Grid).Height + (element as Grid).Margin.Top + (element as Grid).Margin.Bottom;
            collectionItemtemplateHeight = height;
            element = element.Parent;

            CollectionView collectionview = (element as CollectionView);
            GridItemsLayout gridItemsLayout = collectionview.ItemsLayout as GridItemsLayout;
            Grid header = (collectionview.Header as Grid);
            var headerHeight = header.Height + header.Margin.Top + header.Margin.Bottom;
            StackLayout footer = (collectionview.Footer as StackLayout);
            var footerHeight = (footer.Children[0] as Button).Height + (footer.Children[0] as Button).Margin.Top + (footer.Children[0] as Button).Margin.Bottom + 10;


            element = element.Parent;
            element = element.Parent;
            Expander expander = element.Parent as Expander;


            //if course wasn't found in list set count to 0
            collectionview.HeightRequest = (height * ((-1 == coursesCount) ? 0 : coursesCount)) + footerHeight + headerHeight + gridItemsLayout.VerticalItemSpacing * (coursesCount + 1);
            expander.ForceUpdateSize();

            //calculate cgpa
            Frame expheader = (expander.Header as Frame);
            StackLayout stack = (expheader.Content as StackLayout).Children[0] as StackLayout;
            Label x = (stack.Children[1] as Label);

            foreach (var semester in calcVm.Universitygrades)
            {
                if (semester.semester == (commandParameter as CourseDetails).semester)
                {
                    semester.calculateGPA();

                    x.Text = string.Format("GPA: {0:F2}", semester.gpa);

                    calcVm.CalcOtherParams();
                    break;
                }
            }
            cgpaCanvas.InvalidateSurface();


            //Snack bar call back
            SnackBarActionOptions actionOptions = new SnackBarActionOptions
            {
                Action = () =>
                {
                    AllCourses.Insert(index, commandParameter);
                    coursesCount = calcVm.SemesterCourseCount(commandParameter);


                    //if course wasn't found in list set count to 0
                    collectionview.HeightRequest = (height * ((-1 == coursesCount) ? 0 : coursesCount)) + footerHeight + headerHeight + gridItemsLayout.VerticalItemSpacing * (coursesCount + 1);
                    expander.ForceUpdateSize();


                    foreach (var semester in calcVm.Universitygrades)
                    {
                        if (semester.semester == (commandParameter as CourseDetails).semester)
                        {
                            semester.calculateGPA();

                            x.Text = string.Format("GPA: {0:F2}", semester.gpa);

                            calcVm.CalcOtherParams();
                            cgpaCanvas.InvalidateSurface();
                            break;
                        }
                    }
                    return Task.CompletedTask;
                },
                Text = "Undo",
                //BackgroundColor = Color.Red,
                ForegroundColor = Color.White,
            };
            SnackBarOptions barOptions = new SnackBarOptions
            {
                MessageOptions = new MessageOptions { Message = "Restore deleted course" },
                Actions = new[] { actionOptions },
                BackgroundColor = (Color)Application.Current.Resources["Primary"]
            };
            var result = await Shell.Current.DisplaySnackBarAsync(barOptions);

        }
        private async void OnAddUpdateExpannderHeight_Clicked(object sender, EventArgs e)
        {
            Button button = (sender as Button);
            ObservableCollection<CourseDetails> commandParameter = button.CommandParameter as ObservableCollection<CourseDetails>;
            int coursesCount = commandParameter.Count;
            var element = button.Parent;
            element = element.Parent;


            // collection view
            CollectionView collectionview = (element as CollectionView);
            GridItemsLayout gridItemsLayout = collectionview.ItemsLayout as GridItemsLayout;
            Grid header = (collectionview.Header as Grid);
            var headerHeight = header.Height + header.Margin.Top + header.Margin.Bottom;
            StackLayout footer = (collectionview.Footer as StackLayout);
            var footerHeight = (footer.Children[0] as Button).Height + (footer.Children[0] as Button).Margin.Top + (footer.Children[0] as Button).Margin.Bottom + 10;

            element = element.Parent;
            element = element.Parent;
            Expander expander = element.Parent as Expander;

            double height = (collectionItemtemplateHeight == 0) ? headerHeight + 14 : collectionItemtemplateHeight;
            collectionview.HeightRequest = (height * ((-1 == coursesCount) ? 0 : coursesCount)) +
                footerHeight + headerHeight + gridItemsLayout.VerticalItemSpacing * (coursesCount + 1);
            expander.ForceUpdateSize();

            //calculate cgpa
            Frame expheader = (expander.Header as Frame);
            StackLayout stack = (expheader.Content as StackLayout).Children[0] as StackLayout;
            Label x = (stack.Children[1] as Label);

            foreach (var semester in calcVm.Universitygrades)
            {
                if (semester.semester == (commandParameter as ObservableCollection<CourseDetails>)[0].semester)
                {
                    MainCollectionView.ScrollTo(semester.semester + 1, animate: true);
                    semester.calculateGPA();

                    x.Text = string.Format("GPA: {0:F2}", semester.gpa);

                    calcVm.CalcOtherParams();
                    break;
                }
            }
            cgpaCanvas.InvalidateSurface();
        }

        private async void Dropdown_ItemSelected(object sender, CustomViews.Dropdown.ItemSelectedEventArgs e)
        {
            Dropdown dropdown = (sender as Dropdown);

            var element = dropdown.Parent;
            CourseDetails commandParameter = ((element as Grid).Children[3] as ImageButton).CommandParameter as CourseDetails;
            var height = (element as Grid).Height + (element as Grid).Margin.Top + (element as Grid).Margin.Bottom; ;
            element = element.Parent;

            CollectionView collectionview = (element as CollectionView);
            Grid header = (collectionview.Header as Grid);
            //collectionItemtemplateHeight = header.Height + header.Margin.Top + header.Margin.Bottom;


            StackLayout footer = (collectionview.Footer as StackLayout);
            var footerHeight = (footer.Children[0] as Button).Height + (footer.Children[0] as Button).Margin.Top + (footer.Children[0] as Button).Margin.Bottom;


            element = element.Parent;
            element = element.Parent;
            Expander expander = element.Parent as Expander;

            Frame expheader = (expander.Header as Frame);
            StackLayout stack = (expheader.Content as StackLayout).Children[0] as StackLayout;
            Label x = (stack.Children[1] as Label);



            foreach (var semester in calcVm.Universitygrades)
            {

                if (semester.semester == (commandParameter as CourseDetails).semester)
                {
                    semester.calculateGPA();

                    calcVm.CalcOtherParams();
                    if (semester.semester != 1)
                    {
                        x.Text = string.Format("GPA: {0:F2}", semester.gpa);
                    }
                    else
                    {
                        x.Text = string.Format("GPA: {0:F2}", semester.gpa);
                    }
                    //sgpa
                    break;
                }
            }
            cgpaCanvas.InvalidateSurface();
            //if course wasn't found in list set count to 0
        }

        private void MoreMenu_Clicked(object sender, EventArgs e)
        {
            MenuOptionGrid.IsVisible ^= true;
            MenuExpander.IsExpanded ^= true;
        }

        private void MenuOptionGrid_Tapped(object sender, EventArgs e)
        {
            MoreMenu_Clicked(null, null);
        }

        private void AddSemester_Clicked(object sender, EventArgs e)
        {
            MainCollectionView.ScrollTo(calcVm.Universitygrades.Count, animate: true);
            cgpaCanvas.InvalidateSurface();
            //unitPassed.SetBinding(Label.TextProperty, "TotalUnits");
        }
        private float previousCGPA = 0f;
        private async void SKCanvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            previousCGPA = calcVm.cgpa;
            String TEXT = String.Format("{0:F2}", previousCGPA);


            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;
            canvas.Clear();

            int padding = 20;
            SKRect rect = new SKRect(padding, padding, info.Width - padding, info.Height - padding);
            float startAngle = 140f;//(float)startAngleSlider.Value;
            float sweepAngle = (previousCGPA * 260 / 5);// calculates what the sweep angle should be

            double GuageEndPointColor = 0f;
            //float stopindex = (260 * colors.Length / 360);
            if (calcVm.cgpa > 2.5)
            {
                GuageEndPointColor = colors.Length - ((calcVm.cgpa - 2.5f) * colors.Length / 5);
            }
            else
            {
                GuageEndPointColor = calcVm.cgpa * colors.Length / 5;
            }
            SKPaint kPaint = new SKPaint();
            kPaint.Shader = SKShader.CreateLinearGradient(
                        new SKPoint(rect.Right, rect.Top),
                        new SKPoint(rect.Left, rect.Bottom),
                        new SKColor[] { SKColors.Red, ((Color)Application.Current.Resources["Primary"]).ToSKColor() },
                        new float[] { 0, 1 },
                        SKShaderTileMode.Repeat);
            kPaint.Style = SKPaintStyle.Stroke;
            kPaint.StrokeWidth = 10;
            kPaint.Color = colors[(int)GuageEndPointColor].ToSKColor();

            SKPaint arcPaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Red,
                StrokeCap = SKStrokeCap.Round,
                // Create linear gradient from upper-left to lower-right
                Shader = SKShader.CreateLinearGradient(
                        new SKPoint(rect.Right, rect.Top),
                        new SKPoint(rect.Left, rect.Bottom),
                        new SKColor[] { SKColors.Red, SKColors.Blue },
                        new float[] { 0, 1 },
                        SKShaderTileMode.Repeat),
                StrokeWidth = 30
            };

            SKPaint outlinePaint = new SKPaint();

            if (App.Current.RequestedTheme == Xamarin.Forms.OSAppTheme.Dark)
            {
                outlinePaint.Style = SKPaintStyle.Stroke;
                outlinePaint.Color = ((Color)Application.Current.Resources["BackgroundColorDark"]).ToSKColor();
                outlinePaint.StrokeCap = SKStrokeCap.Round;
                outlinePaint.StrokeWidth = 30;
            }
            else
            {
                outlinePaint.Style = SKPaintStyle.Stroke;
                outlinePaint.Color = SKColors.LightGray;
                outlinePaint.StrokeCap = SKStrokeCap.Round;
                outlinePaint.StrokeWidth = 30;
            }


            //canvas.DrawOval(rect, outlinePaint);

            //bg full arc
            using (SKPath path = new SKPath())
            {
                path.AddArc(rect, startAngle, 260f);
                canvas.DrawPath(path, outlinePaint);
            }


            using (SKPath path = new SKPath())
            {
                path.AddArc(rect, startAngle, sweepAngle);
                canvas.DrawPath(path, arcPaint);
            }

            SKPaint linePaint = new SKPaint() { Style = SKPaintStyle.Stroke, StrokeWidth = 10, Color = SKColors.Black, StrokeCap = SKStrokeCap.Round };

            //draw text
            using (SKPaint paint = new SKPaint())
            {

                // Set text color to blue
                if (App.Current.RequestedTheme == Xamarin.Forms.OSAppTheme.Dark)
                {
                    paint.Color = SKColors.White;
                }
                else
                {
                    paint.Color = SKColors.Black;
                }
                
                // Set text size to fill 90% of width
                paint.TextSize = 10;
                float width = paint.MeasureText(TEXT);
                float scale = 0.6f * info.Width / width;
                paint.TextSize *= scale;
                // Get text bounds
                SKRect valuetextBounds = new SKRect();
                paint.MeasureText(TEXT, ref valuetextBounds);

                // Calculate offsets to position text above center
                float xText = info.Width / 2 - valuetextBounds.MidX;
                float yText = info.Height / 2 - valuetextBounds.MidY +20;
                // Draw unreflected text
                canvas.DrawText(TEXT, xText, yText, paint);
                //textPaint.Color = SKColors.Black;

                linePaint.Color = colors[(int)GuageEndPointColor].ToSKColor();
                canvas.DrawLine((info.Width / 2) - 100, 140, (info.Width / 2) + 100, 140, linePaint);

                TEXT = "CGPA";
                paint.Color = colors[(int)GuageEndPointColor].ToSKColor();

                // Set text size to fill 90% of width
                paint.TextSize = 10;
                width = paint.MeasureText(TEXT);
                scale = 0.3f * info.Width / width;
                paint.TextSize *= scale;
                // Get text bounds
                SKRect textBounds = new SKRect();
                paint.MeasureText(TEXT, ref textBounds);

                // Calculate offsets to position text above center
                xText = info.Width / 2 - textBounds.MidX;
                yText = info.Height / 2 + valuetextBounds.MidX +20;
                // Draw unreflected text
                canvas.DrawText(TEXT, xText, yText, paint);

                TEXT = String.Format("{0:D2}/{1:D2}", (int)calcVm.TotalScore, (int)calcVm.OverAllAttainableScore);
                if (App.Current.RequestedTheme == Xamarin.Forms.OSAppTheme.Dark)
                {
                    paint.Color = SKColors.White;
                }
                else
                {
                    paint.Color = SKColors.Black;
                }
                // Set text size to fill 90% of width
                paint.TextSize = 10;
                width = paint.MeasureText(TEXT);
                scale = 0.25f * info.Width / width;
                paint.TextSize *= scale;
                // Get text bounds
                textBounds = new SKRect();
                paint.MeasureText(TEXT, ref textBounds);

                // Calculate offsets to position text above center
                xText = info.Width / 2 - textBounds.MidX;
                yText = info.Height / 2 - textBounds.MidY;
                // Draw unreflected text
                canvas.DrawText(TEXT, xText, 120, paint);

            }



        }

        private void CollectionView_Scrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            double x = e.VerticalDelta;
            double y = e.VerticalOffset;
        }
        public static IEnumerable<Color> SetGradients(Color start, Color end, int steps)
        {
            var colorList = new List<Color>();

            double aStep = ((end.A * 255) - (start.A * 255)) / steps;
            double rStep = ((end.R * 255) - (start.R * 255)) / steps;
            double gStep = ((end.G * 255) - (start.G * 255)) / steps;
            double bStep = ((end.B * 255) - (start.B * 255)) / steps;

            for (int i = 0; i < 100; i++)
            {
                var a = (start.A * 255) + (int)(aStep * i);
                var r = (start.R * 255) + (int)(rStep * i);
                var g = (start.G * 255) + (int)(gStep * i);
                var b = (start.B * 255) + (int)(bStep * i);

                colorList.Add(Color.FromRgba(r / 255, g / 255, b / 255, a / 255));
            }

            return colorList;
        }

        private void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    MoreMenu_Clicked(null, null);
                    break;
                case GestureStatus.Running:
                    break;
                case GestureStatus.Completed:
                    break;
                case GestureStatus.Canceled:
                    break;
                default:
                    break;
            }
        }

        private void GraphsView_Tapped(object sender, EventArgs e)
        {
            closeChart();
        }
        private void openChart()
        {
            GraphView.IsVisible = true;
        }
        private void closeChart()
        {
            GraphView.IsVisible = false;
        }
        private void GraphsView_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    closeChart();
                    break;
                case GestureStatus.Running:
                    break;
                case GestureStatus.Completed:
                    break;
                case GestureStatus.Canceled:
                    break;
                default:
                    break;
            }
        }

        private void InfoBtn_Clicked(object sender, EventArgs e)
        {
            openChart();
        }
    }
}