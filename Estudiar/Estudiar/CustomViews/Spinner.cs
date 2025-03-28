﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Estudiar.CustomViews
{
    public class Dropdown : View
    {
       
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
            propertyName: nameof(ItemsSource),
            returnType: typeof(List<string>),
            declaringType: typeof(List<string>),
            defaultValue: null);

        public List<string> ItemsSource
        {
            get { return (List<string>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(
            propertyName: nameof(SelectedIndex),
            returnType: typeof(int),
            declaringType: typeof(int),
            defaultValue: -1,
            defaultBindingMode: BindingMode.TwoWay);

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        public event EventHandler<ItemSelectedEventArgs> ItemSelected;

        public void OnItemSelected(int pos)
        {
            ItemSelected?.Invoke(this, new ItemSelectedEventArgs() { SelectedIndex = pos });
        }
        public class ItemSelectedEventArgs : EventArgs
        {
            public int SelectedIndex { get; set; }
        }
    }
}
