using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.View.CustomControls;
using Todo.WinPhone.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinRT;

[assembly: ExportRenderer(typeof(CustomPicker), typeof(CustomPickerRenderer))]
namespace Todo.WinPhone.CustomRenderers
{
    class CustomPickerRenderer : PickerRenderer
    {
        private CustomPicker picker;

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                picker = (CustomPicker) e.NewElement;

                //Indexes of priority items starts at 0. Values of priority items starts at 1.
                //SetSelection get value of item but sets index of item.
                Control.SelectedIndex = Int32.Parse(picker.PickerValue)-1;
            }
          
        }
    }
}
