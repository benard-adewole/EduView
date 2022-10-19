using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Estudiar.DataTemplates
{
    public class CourseViewDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ListView { get; set; }
        public DataTemplate GridView { get; set; }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return GridView;
            //return ((Monkey)item).Location.Contains("America") ? ListView : GridView;
        }
    }
}
