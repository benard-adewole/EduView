using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Estudiar.CustomViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyCustomPicker : ContentView
    {
        private int FirstItemIndex;
        private int LastItemIndex;
        private int MyItemSourceCount = 6;
        public event EventHandler<bool> OnToggleChanged;
        public event EventHandler<object> OnSelectedItemChanged;

        public static readonly BindableProperty ToggleProperty =
            BindableProperty.Create(
                propertyName: "Toggle",
                returnType: typeof(bool),
                declaringType: typeof(MyCustomPicker),
                defaultValue: false,
                propertyChanged:(bindable,oldvalue,newValue)=>
                {
                    MyCustomPicker customPicker = (MyCustomPicker)bindable;
                    EventHandler<bool> eventHandler = customPicker.OnToggleChanged;
                    if (eventHandler != null)
                    {
                        eventHandler(customPicker, (bool)newValue);
                    }
                });
        public static readonly BindableProperty SelectedItemProperty =
            BindableProperty.Create(
                propertyName: "SelectedItem",
                returnType: typeof(object),
                declaringType: typeof(MyCustomPicker),
                defaultValue: null,
                defaultBindingMode: BindingMode.TwoWay,
                propertyChanged: (bindable, oldvalue, newValue) =>
                {
                    MyCustomPicker customPicker = (MyCustomPicker)bindable;
                    EventHandler<object> eventHandler = customPicker.OnSelectedItemChanged;
                    if (eventHandler != null)
                    {
                        eventHandler(customPicker, newValue);
                    }
                });
        public static readonly BindableProperty BlinderColorProperty =
            BindableProperty.Create(
                propertyName: "BlinderColor",
                returnType: typeof(Color),
                declaringType: typeof(MyCustomPicker),
                defaultValue: Color.White);
        public static readonly BindableProperty OpenDurationProperty =
            BindableProperty.Create(
                propertyName: "OpenDuration",
                returnType: typeof(uint),
                declaringType: typeof(MyCustomPicker),
                defaultValue: 2000u);
        public static readonly BindableProperty CloseDurationProperty =
            BindableProperty.Create(
                propertyName: "CloseDuration",
                returnType: typeof(uint),
                declaringType: typeof(MyCustomPicker),
                defaultValue: 2000u);
        public static readonly BindableProperty MyDataTemplateProperty =
            BindableProperty.Create(
                propertyName: "MyDataTemplate",
                returnType: typeof(DataTemplate),
                declaringType: typeof(MyCustomPicker),
                defaultValue: new DataTemplate(() =>
                {
                    Grid grid = new Grid();
                    Frame frame = new Frame() { VerticalOptions = LayoutOptions.Center, Padding = 5 };
                    StackLayout stackLayout = new StackLayout() { VerticalOptions = LayoutOptions.Center };
                    stackLayout.Children.Add(new Image {Source = "Hamburger.png", HeightRequest = 50 });

                    Label label = new Label {HorizontalOptions = LayoutOptions.Center };
                    label.SetBinding(Label.TextProperty, ".");

                    stackLayout.Children.Add(label);

                    frame.Content = stackLayout;
                    grid.Children.Add(frame);
                    return grid;
                })
                );
        public static readonly BindableProperty MyItemSourceProperty =
            BindableProperty.Create(
                propertyName: "MyItemSource",
                returnType: typeof(object),
                declaringType: typeof(MyCustomPicker),
                defaultValue: new ObservableCollection<string> { "num", "fun", "bun","dun","hum","sun" });
        public bool Toggle
        {
            get { return (bool)GetValue(ToggleProperty); }
            set 
            {
                SetValue(ToggleProperty, value);
                if (value)
                {
                    OpenView();
                }
                else
                {
                    CloseView();
                }
            }
        }
        public Color BlinderColor
        {
            get { return (Color)GetValue(BlinderColorProperty); }
            set
            {
                SetValue(BlinderColorProperty, value);
                OnPropertyChanged();
            }
        }
        public uint OpenDuration
        {
            get { return (uint)GetValue(OpenDurationProperty); }
            set
            {
                SetValue(OpenDurationProperty, value);
            }
        }
        public uint CloseDuration
        {
            get { return (uint)GetValue(CloseDurationProperty); }
            set
            {
                SetValue(CloseDurationProperty, value);
            }
        }
        public DataTemplate MyDataTemplate
        {
            get { return (DataTemplate)GetValue(MyDataTemplateProperty); }
            set
            {
                SetValue(MyDataTemplateProperty, value);
            }
        }
        public object MyItemSource
        {
            get { return (object)GetValue(MyItemSourceProperty); }
            set
            {
                SetValue(MyItemSourceProperty, value);
                var property = typeof(ICollection).GetProperty("Count");
                MyItemSourceCount = (int)property.GetValue(MyItemSource, null);
            }
        }
        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set
            {
                SetValue(SelectedItemProperty, value);
                //MyCollectionView.SelectedItem = value;
            }
        }
        public MyCustomPicker()
        {
            InitializeComponent();
            BindingContext = this;

            
            //MyCollectionView.ScrollTo(3);
        }
        private void OpenView()
        {
            Task t = Frame1.TranslateTo(0, -thisView.HeightRequest/2, OpenDuration);
            Task t1 = Frame2.TranslateTo(0, thisView.HeightRequest/2, OpenDuration);
        }
        private void CloseView()
        {
            Task t = Frame1.TranslateTo(0, 0, CloseDuration);
            Task t1 = Frame2.TranslateTo(0, 0, CloseDuration);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (FirstItemIndex - 1 <0)
            {
                return;
            }
            MyCollectionView.ScrollTo(FirstItemIndex-1);
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            
            if (LastItemIndex + 1 >= MyItemSourceCount)
            {
                return;
            }
            MyCollectionView.ScrollTo(LastItemIndex + 1);
        }

        private void MyCollectionView_Scrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            FirstItemIndex = e.FirstVisibleItemIndex;
            LastItemIndex = e.LastVisibleItemIndex;
        }
    }
}