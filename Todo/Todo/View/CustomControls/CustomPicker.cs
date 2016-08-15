using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Models;
using Xamarin.Forms;

namespace Todo.View.CustomControls
{
    public class CustomPicker : Picker
    {
        public static readonly BindableProperty PickerValueProperty =
            BindableProperty.Create("PickerValue", typeof(string), typeof(CustomPicker), null);

        public string PickerValue
        {
            get { return (string)GetValue(PickerValueProperty); }
            set { SetValue(PickerValueProperty, value); }
        }
    }
}
