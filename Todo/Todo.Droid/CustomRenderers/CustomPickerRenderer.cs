using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Todo.Droid.CustomRenderers;
using Todo.View.CustomControls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomPicker), typeof(CustomPickerRenderer))]
namespace Todo.Droid.CustomRenderers
{
    class CustomPickerRenderer : ViewRenderer<CustomPicker, Spinner>
    {
        CustomPicker picker;

        protected override void OnElementChanged(ElementChangedEventArgs<CustomPicker> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null || Element == null)
            {
                return;
            }
            picker = e.NewElement;
            IList<string> scaleNames = e.NewElement.Items;
            Spinner spinner = new Spinner(this.Context);

            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);

            ArrayAdapter scaleAdapter;

            if (App.ScreenHeight > 800)
            {
                scaleAdapter = new ArrayAdapter<string>(this.Context, Resource.Layout.spinnerItem, scaleNames);
                scaleAdapter.SetDropDownViewResource(Resource.Layout.spinnerItem);
            }
            else
            {
                scaleAdapter = new ArrayAdapter<string>(this.Context, Android.Resource.Layout.SimpleSpinnerItem,
                    scaleNames);
                scaleAdapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            }

            spinner.Adapter = scaleAdapter;

            //Indexes of priority items starts at 0. Values of priority items starts at 1.
            //SetSelection get value of item but sets index of item.
            spinner.SetSelection(Int32.Parse(picker.PickerValue)-1);

            base.SetNativeControl(spinner);
        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            picker.SelectedIndex = (e.Position);
        }
    }
}