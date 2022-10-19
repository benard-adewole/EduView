﻿using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Estudiar.Controls
{
    public class MenuItemIconFont:MenuItem
    {
        public static readonly BindableProperty IconGlyphProperty = BindableProperty.Create(nameof(IconGlyphProperty), typeof(string), typeof(FlyoutItemIconFont), string.Empty);
        public string IconGlyph
        {
            get { return (string)GetValue(IconGlyphProperty); }
            set { SetValue(IconGlyphProperty, value); }
        }
    }
}
