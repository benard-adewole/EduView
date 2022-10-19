using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Estudiar.MenuItems
{
    public class MenuPageItem
    {
        public string Title { get; set; }
        public Xamarin.Forms.Shapes.Geometry Icon { get; set; }
        public Type TargetType { get; set; }
    }
}
