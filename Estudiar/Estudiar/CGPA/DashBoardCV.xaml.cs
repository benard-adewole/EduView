using Estudiar.Helpers;
using Estudiar.Models;
using Estudiar.ViewModels;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Estudiar.CGPA
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DashBoardCV : ContentView
    {
        Color[] colors;
        calcVm calcVm;
        public DashBoardCV()
        {
            Application.Current.RequestedThemeChanged += (s, a) =>
            {
                TheTheme.setTheme();
                InitializeComponent();
            };
            InitializeComponent();
            calcVm = BindingContext as calcVm;

            hideMenu();

            colors = SetGradients((Color)Application.Current.Resources["Primary"], Color.Red, 100).ToArray();
        }
        
        private async void hideMenu()
        {
            await Task.Delay(100);

            //closing menu that was  initially opened by xaml
            MoreMenu_Clicked(null, null);
        }
        public void OnAppearing()
        {
            calcVm = BindingContext as calcVm;
            var groups = AppState<AllSavedData>.DeserializeAndRestore("AllSavedData");

            if (groups != null && groups.SemesterGroup.Count != 0)
            {
                if (!Preferences.Get("IsRecentHidden", false))
                {
                    calcVm.Universitygrades = groups.SemesterGroup.Last().universitygrades;
                }
                else
                {
                    calcVm.Universitygrades = new ObservableCollection<universitygrade>();
                }
                calcVm.CalcOtherParams();
                calcVm.UpdateGradeFreqGraph();
                calcVm.UpdateGPAcomparisonGraph();
                MoreMenu.IsVisible = true;
                OpenSavedData.IsVisible = true;
            }
            else
            {
                MoreMenu.IsVisible = false;
                OpenSavedData.IsVisible = false;
            }

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
        private void MoreMenu_Clicked(object sender, EventArgs e)
        {
            MenuOptionGrid.IsVisible ^= true;
            MenuExpander.IsExpanded ^= true;
        }

        private void MenuOptionGrid_Tapped(object sender, EventArgs e)
        {
            MoreMenu_Clicked(null, null);
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
        private float previousCGPA = 0f;
        private async void SKCanvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            previousCGPA = calcVm.cgpa;
            string TEXT = string.Format("{0:F2}", previousCGPA);


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
                float yText = info.Height / 2 - valuetextBounds.MidY+20; //the +20 moves it down a bit
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