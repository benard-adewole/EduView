using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Estudiar.Toolkit
{
    public static class Tools
    {
        public static bool IsNumeric(string value)
        {
            double result;
            bool isValid = Double.TryParse(value, out result);
            return isValid;
        }
        public static bool IsEmail(string email)
        {
            string MatchEmailPattern = @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
                                                + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
                                                        [0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
                                                + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
                                                        [0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
                                                + @"([a-zA-Z0-9]+[\w-]+\.)+[a-zA-Z]{1}[a-zA-Z0-9-]{1,23})$";
            /*string MatchEmailPattern1 = "@^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|"+
                                        @"[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)" +
                                        @"(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|" +
                                        @"(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";*/
            try
            {
                if (email != null)
                {
                    return System.Text.RegularExpressions.Regex.IsMatch(
                        email,
                        MatchEmailPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase,
                        TimeSpan.FromMilliseconds(250));
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }


        }
        public static bool IsWithinRange(int min, int max, string value)
        {
            if ((max == 0 && value.Length >= min)
                || (max != 0 && value.Length <= max && value.Length >= min))
            {
                return true;
            }
            return false;
        }
    }
    public class CharLengthValidationAction : TriggerAction<Entry>
    {
        public CharLengthValidationAction()
        {
            MaxCharacter = 0;
            MinCharacter = 0;
            Valid = Color.Black;
            InValid = Color.Red;
        }
        public int MaxCharacter { get; set; }
        public int MinCharacter { get; set; }
        public Color Valid { get; set; }
        public Color InValid { get; set; }
        
        protected override void Invoke(Entry entry)
        {
            entry.TextColor = Tools.IsWithinRange(MinCharacter, MaxCharacter, entry.Text) ? Valid : InValid;
        }
    }
    public class CanEditTriggerAction : TriggerAction<Button>, INotifyPropertyChanged
    {
        public string ShowIcon { get; set; }
        public string HideIcon { get; set; }

        bool _hidePassword = false;

        public bool HidePassword
        {
            set
            {
                if (_hidePassword != value)
                {
                    _hidePassword = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HidePassword)));
                }
            }
            get => _hidePassword;
        }

        protected override void Invoke(Button sender)
        {
            sender.Text = HidePassword ? ShowIcon : HideIcon;
            HidePassword = !HidePassword;
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
    public class ShowPasswordTriggerAction : TriggerAction<ImageButton>, INotifyPropertyChanged
    {
        public string ShowIcon { get; set; }
        public string HideIcon { get; set; }

        bool _hidePassword = true;

        public bool HidePassword
        {
            set
            {
                if (_hidePassword != value)
                {
                    _hidePassword = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HidePassword)));
                }
            }
            get => _hidePassword;
        }

        protected override void Invoke(ImageButton sender)
        {
            sender.Source = HidePassword ? ShowIcon : HideIcon;
            HidePassword = !HidePassword;
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
    public class NumericValidationAction : TriggerAction<Entry>
    {
        public NumericValidationAction()
        {
            Valid = Color.Black;
            InValid = Color.Red;
        }
        public Color Valid { get; set; }
        public Color InValid { get; set; }
        protected override void Invoke(Entry entry)
        {
            double result;
            bool isValid = Double.TryParse(entry.Text, out result);
            if (isValid)
            {
                entry.TextColor = Valid;
            }
            else
            {
                entry.TextColor = InValid;
            }
        }
    }
    public class NumericAndLengthValidationAction : TriggerAction<Entry>
    {
        public NumericAndLengthValidationAction()
        {
            MaxCharacter = 0;
            MinCharacter = 0;
            Valid = Color.Black;
            InValid = Color.Red;
        }
        public int MaxCharacter { get; set; }
        public int MinCharacter { get; set; }
        public Color Valid { get; set; }
        public Color InValid { get; set; }
        protected override void Invoke(Entry entry)
        {
            double result;
            bool isValid = Double.TryParse(entry.Text, out result);
            if (isValid && Tools.IsWithinRange(MinCharacter, MaxCharacter, entry.Text))
            {
                entry.TextColor = Valid;
            }
            else
            {
                entry.TextColor = InValid;
            }
        }
    }
    public class EmailValidationAction : TriggerAction<Entry>
    {
        
        public EmailValidationAction()
        {
            Valid = Color.Black;
            InValid = Color.Red;
        }
        public Color Valid { get; set; }
        public Color InValid { get; set; }
        
        protected override void Invoke(Entry entry)
        {
            if (Tools.IsEmail(entry.Text))
            {
                entry.TextColor = Valid;
            }
            else
            {
                entry.TextColor = InValid;
            }
        }
    }
    public class ShiverAction : TriggerAction<VisualElement>
    {
        public ShiverAction()
        {
            Length = 1000;
            Angle = 15;
            Vibrations = 10;
            Easing = new Easing(t => Math.Sin(Math.PI * t) *
            Math.Sin(Math.PI * 2 * Vibrations * t));
        }
        public Easing Easing { set; get; }
        public int Length { set; get; }
        public double Angle { set; get; }
        public int Vibrations { set; get; }
        protected override void Invoke(VisualElement visual)
        {
            visual.Rotation = 0;
            visual.AnchorX = 0.5;
            visual.AnchorY = 0.5;
            visual.RotateTo(Angle, (uint)Length,Easing);
        }
    }
    public class ScaleUpAndDownAction : TriggerAction<VisualElement>
    {

        public ScaleUpAndDownAction()
        {
            Anchor = new Point(0.5, 0.5);
            Scale = 2;
            StartDelay = 0;
            EndDelay = 0;
            StartLength = 500;
            EndLength = 500;
            EasingStart = Easing.Linear;
            EasingEnd = Easing.Linear;
        }
        public int StartDelay { set; get; }
        public int EndDelay { set; get; }
        public Point Anchor { set; get; }
        public double Scale { set; get; }
        public int StartLength { set; get; }
        public int EndLength { set; get; }
        [Xamarin.Forms.TypeConverter(typeof(EasingConverter))]
        public Easing EasingStart { set; get; }
        [Xamarin.Forms.TypeConverter(typeof(EasingConverter))]
        public Easing EasingEnd { set; get; }
        protected override async void Invoke(VisualElement visual)
        {
            visual.AnchorX = Anchor.X;
            visual.AnchorY = Anchor.Y;
            await Task.Delay(StartDelay);
            await visual.ScaleTo(Scale, (uint)StartLength, EasingStart);
            await Task.Delay(EndDelay);
            await visual.ScaleTo(1, (uint)EndLength, EasingEnd);
        }
    }
    public class ScaleAction : TriggerAction<VisualElement>
    {
        //Resources = new ResourceDictionary();
        //Resources.Add("customEase", new Easing(t => -6 * t * t + 7 * t));

        /*<Entry.Triggers>
            <EventTrigger Event = "Focused" >
                < Toolkit:ScaleAction Anchor = "0.5, 0.5" Easing="{StaticResource customEase}" Scale="1.1"/>
            </EventTrigger>
            <EventTrigger Event = "Unfocused" >
                < Toolkit:ScaleAction Anchor = "0.5, 0.5" Scale="1" />
            </EventTrigger>
        </Entry.Triggers>*/

        /*<Style.Triggers>
            <Trigger TargetType = "Entry" Property="IsFocused" Value="True">
                <Trigger.EnterActions>
                    <Toolkit:ScaleAction Anchor = "0.5, 0.5" Scale="1.1"/>
                </Trigger.EnterActions>
                <Trigger.ExitActions>
                    <Toolkit:ScaleAction Anchor = "0.5, 0.5" Easing="SpringIn" Scale="1"/>
                </Trigger.ExitActions>
            </Trigger>
        </Style.Triggers>*/
        public ScaleAction()
        {
            // Set defaults.
            Anchor = new Point(0.5, 0.5);
            Scale = 1;
            Delay = 0;
            Length = 250;
            Easing = Easing.Linear;
        }
        public Point Anchor { set; get; }
        public double Scale { set; get; }
        public int Delay { set; get; }
        public int Length { set; get; }
        [Xamarin.Forms.TypeConverter(typeof(EasingConverter))]
        public Easing Easing { set; get; }
        protected override async void Invoke(VisualElement sender)
        {
            sender.AnchorX = Anchor.X;
            sender.AnchorY = Anchor.Y;
            await Task.Delay(Delay);
            await sender.ScaleTo(Scale, (uint)Length, Easing);
        }
    }
    public class EasingConverter: Xamarin.Forms.TypeConverter
    {
        public override bool CanConvertFrom(Type sourceType)
        {
            if (sourceType == null)
                throw new ArgumentNullException("EasingConverter.CanConvertFrom: sourceType");
            return (sourceType == typeof(string));
        }
#pragma warning disable CS0672 // Member overrides obsolete member
        public override object ConvertFrom(CultureInfo culture, object value)
#pragma warning restore CS0672 // Member overrides obsolete member
        {
            if (value == null || !(value is string))
                return null;
            string name = ((string)value).Trim();
            if (name.StartsWith("Easing"))
            {
                name = name.Substring(7);
            }
            FieldInfo field = typeof(Easing).GetRuntimeField(name);
            if (field != null && field.IsStatic)
            {
                return (Easing)field.GetValue(null);
            }
            throw new InvalidOperationException(
            String.Format("Cannot convert \"{0}\" into Xamarin.Forms.Easing", value));
        }
    }

    public class RoundedEntry : Entry
    {

        public static readonly BindableProperty entryBorderColorProperty =
            BindableProperty.Create(
                propertyName: "BorderColor",
                returnType: typeof(Color),
                declaringType: typeof(RoundedEntry),
                defaultValue: Color.Transparent);
        public static readonly BindableProperty EntryPaddingProperty =
            BindableProperty.Create(
                propertyName: "Padding",
                returnType: typeof(Thickness),
                declaringType: typeof(RoundedEntry),
                defaultValue: new Thickness(0));
        public static readonly BindableProperty EntryBorderWidthProperty =
            BindableProperty.Create(
                propertyName: "BorderWidth",
                returnType: typeof(int),
                declaringType: typeof(RoundedEntry),
                defaultValue: 0,
                BindingMode.TwoWay);
        public static readonly BindableProperty EntryCornerRadiusProperty =
            BindableProperty.Create(
                propertyName: "CornerRadius",
                returnType: typeof(float),
                declaringType: typeof(RoundedEntry),
                defaultValue: 0f);
        public static readonly BindableProperty EntryBackColorProperty =
            BindableProperty.Create(
                propertyName: "BackColor",
                returnType: typeof(Color),
                declaringType: typeof(RoundedEntry),
                defaultValue: Color.Transparent);
        public static readonly BindableProperty EntryFocusedStateColorProperty =
            BindableProperty.Create(
                propertyName: "FocusedBorderColor",
                returnType: typeof(Color),
                declaringType: typeof(RoundedEntry),
                defaultValue: Color.Transparent);
        public static readonly BindableProperty EntryUnocusedStateColorProperty =
            BindableProperty.Create(
                propertyName: "UnfocusedBorderColor",
                returnType: typeof(Color),
                declaringType: typeof(RoundedEntry),
                defaultValue: Color.Transparent);

        public Color FocusedBorderColor
        {
            get { return (Color)GetValue(EntryFocusedStateColorProperty); }
            set { SetValue(EntryFocusedStateColorProperty, value); }
        }
        public Color UnfocusedBorderColor
        {
            get { return (Color)GetValue(EntryUnocusedStateColorProperty); }
            set { SetValue(EntryUnocusedStateColorProperty, value); }
        }
        public Color BackColor
        {
            get { return (Color)GetValue(EntryBackColorProperty); }
            set { SetValue(EntryBackColorProperty, value); }
        }
        public Thickness Padding
        {
            get { return (Thickness)GetValue(EntryPaddingProperty); }
            set { SetValue(EntryPaddingProperty, value); }
        }
        public int BorderWidth
        {
            get { return (int)GetValue(EntryBorderWidthProperty); }
            set { SetValue(EntryBorderWidthProperty, value); }
        }
        public float CornerRadius
        {
            get { return (float)GetValue(EntryCornerRadiusProperty); }
            set { SetValue(EntryCornerRadiusProperty, value); }
        }
        /*public Color BorderColor { get; set; }
        public Color BackColor { get; set; }
        public Thickness Padding { get; set; }
        public int BorderWidth { get; set; }
        public float CornerRadius { get; set; }*/
    }

    public class XEditor : Editor
    {
        public XEditor()
        {
            TextChanged += XEditor_TextChanged;
        }

        private void XEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            InvalidateMeasure();
        }
        ~XEditor()
        {
            TextChanged -= XEditor_TextChanged;
        }
        public static readonly BindableProperty entryBorderColorProperty =
            BindableProperty.Create(
                propertyName: "BorderColor",
                returnType: typeof(Color),
                declaringType: typeof(XEditor),
                defaultValue: Color.Transparent);
        public static readonly BindableProperty EntryPaddingProperty =
            BindableProperty.Create(
                propertyName: "Padding",
                returnType: typeof(Thickness),
                declaringType: typeof(XEditor),
                defaultValue: new Thickness(0));
        public static readonly BindableProperty EntryBorderWidthProperty =
            BindableProperty.Create(
                propertyName: "BorderWidth",
                returnType: typeof(int),
                declaringType: typeof(XEditor),
                defaultValue: 0);
        public static readonly BindableProperty EntryCornerRadiusProperty =
            BindableProperty.Create(
                propertyName: "CornerRadius",
                returnType: typeof(float),
                declaringType: typeof(XEditor),
                defaultValue: 0f);
        public static readonly BindableProperty EntryBackColorProperty =
            BindableProperty.Create(
                propertyName: "BackColor",
                returnType: typeof(Color),
                declaringType: typeof(XEditor),
                defaultValue: Color.Transparent);
        public static readonly BindableProperty EntryFocusedStateColorProperty =
            BindableProperty.Create(
                propertyName: "FocusedBorderColor",
                returnType: typeof(Color),
                declaringType: typeof(XEditor),
                defaultValue: Color.Transparent);
        public static readonly BindableProperty EntryUnocusedStateColorProperty =
            BindableProperty.Create(
                propertyName: "UnfocusedBorderColor",
                returnType: typeof(Color),
                declaringType: typeof(XEditor),
                defaultValue: Color.Transparent);
        public Color FocusedBorderColor
        {
            get { return (Color)GetValue(EntryFocusedStateColorProperty); }
            set { SetValue(EntryFocusedStateColorProperty, value); }
        }
        public Color UnfocusedBorderColor
        {
            get { return (Color)GetValue(EntryUnocusedStateColorProperty); }
            set { SetValue(EntryUnocusedStateColorProperty, value); }
        }

        public Color BorderColor { get; set; }
        public Color BackColor { get; set; }
        public Thickness Padding { get; set; }
        public int BorderWidth { get; set; }
        public float CornerRadius { get; set; }

    }
}

