using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;


namespace Estudiar.Actions
{
    public class ScaleAction:TriggerAction<VisualElement>
    {
        public ScaleAction()
        {
            // Set defaults.
            Anchor = new Point(0.5, 0.5);
            Scale = 1;
            Length = 250;
            Easing = Easing.Linear;
        }
        public Point Anchor { set; get; }
        public double Scale { set; get; }
        public int Length { set; get; }
        //[TypeConverter(typeof(EasingConverter))]
        public Easing Easing { set; get; }
        protected override void Invoke(VisualElement sender)
        {
            sender.AnchorX = Anchor.X;
            sender.AnchorY = Anchor.Y;
            sender.ScaleTo(Scale, (uint)Length, Easing);
        }
    }
}
