using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Todo.View.CustomControls;
using Todo.WinPhone.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinRT;
using Thickness = Windows.UI.Xaml.Thickness;

[assembly: ExportRenderer(typeof(CustomSwitch), typeof(CustomSwitchRenderer))]
namespace Todo.WinPhone.CustomRenderers
{
    class CustomSwitchRenderer : SwitchRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Switch> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.Margin = new Thickness(-15, -5, -15, -5);
                Control.Padding = new Thickness(0);
                Control.BorderThickness = new Thickness(0);
                Control.OffContent = "";
                Control.OnContent = "";
            }
        }
    }
}
