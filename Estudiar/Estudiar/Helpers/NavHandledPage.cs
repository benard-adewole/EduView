﻿using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Estudiar.Helpers
{
    public class NavHandledPage:ContentPage
    {
        /// <summary> 
        /// /// Gets or Sets the Back button click overriden custom action 
        /// /// </summary> 
        public Action CustomBackButtonAction { get; set; }
        public static readonly BindableProperty EnableBackButtonOverrideProperty = BindableProperty.Create(
            nameof(EnableBackButtonOverride),
            typeof(bool),
            typeof(NavHandledPage),
            false);
        /// <summary> 
        /// Gets or Sets Custom Back button overriding state 
        /// </summary> 
        public bool EnableBackButtonOverride 
        {
            get 
            {
                return (bool)GetValue(EnableBackButtonOverrideProperty);
            }
            set
            {
                SetValue(EnableBackButtonOverrideProperty, value);
            }
        }
    }
}
