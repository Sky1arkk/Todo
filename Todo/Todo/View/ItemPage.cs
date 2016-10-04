using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Todo.Models;
using Todo.View.CustomControls;
using Xamarin.Forms;

namespace Todo.Views
{
    public class ItemPage : ContentPage
    {
        public ItemPage()
        {
            double labelFontSize = App.ScreenHeight / 20;
            double entryFontSize = App.ScreenHeight / 30;

            Padding = new Thickness(App.ScreenWidth / 40);

            var stackLayout = new StackLayout();

            this.SetBinding(TitleProperty, "Name");

            NavigationPage.SetHasNavigationBar(this, true);

            var nameLabel = new Label() { Text = "Name", FontSize = labelFontSize };
            var nameEntry = new Entry()
            {
                Placeholder = "Enter name or subject",
                FontSize = entryFontSize
            };
            nameEntry.TextChanged += (sender, e) =>
            {
                const int limit = 50;
                var text = nameEntry.Text;      //Get Current Text
                if (text.Length > limit)       //If it is more than your character restriction
                {
                    text = text.Remove(text.Length - 1);  // Remove Last character
                    nameEntry.Text = text;        //Set the Old value
                }
            };

            nameEntry.SetBinding(Entry.TextProperty, "Name");

            var noteLabel = new Label() { Text = "Note", FontSize = labelFontSize };
            var noteEditor = new Editor() { FontSize = entryFontSize };
            noteEditor.SetBinding(Editor.TextProperty, "Note");

            var priorityLabel = new Label() { Text = "Priority", FontSize = labelFontSize };
            var priorityPicker = new CustomPicker() { Title = "Select priority" };
            if (Device.OS == TargetPlatform.Windows)
            {
                priorityPicker.Title = null;
            }

            priorityPicker.SetBinding(CustomPicker.PickerValueProperty, "Priority");

            priorityPicker.Items.Add("1");
            priorityPicker.Items.Add("2");
            priorityPicker.Items.Add("3");

            int selectedPickerValue = 0;
            priorityPicker.SelectedIndexChanged += (sender, args) =>
            {
                if (priorityPicker.SelectedIndex < 0 || priorityPicker.SelectedIndex > 2)
                {
                    selectedPickerValue = 3;
                }
                else
                {
                    selectedPickerValue = Int32.Parse(priorityPicker.Items[priorityPicker.SelectedIndex]);
                }
            };
            

            var doneLabel = new Label { Text = "Done", FontSize = labelFontSize };
            var doneSwitch = new CustomSwitch() { HorizontalOptions = LayoutOptions.Start };
            doneSwitch.SetBinding(Switch.IsToggledProperty, "Done");

            var saveButton = new Button { Text = "Save", FontSize = labelFontSize };
            saveButton.Clicked += (sender, e) =>
            {
                if (nameEntry.Text != null && noteEditor.Text != null && selectedPickerValue != 0)
                {
                    var todoItem = (TodoItem) BindingContext;
                    todoItem.Priority = selectedPickerValue;
                    App.Database.SaveItem(todoItem);
                    this.Navigation.PopAsync();
                }
                else
                {
                    DisplayAlert("Something went wrong", "Some fields are empty or priority not selected", "OK");
                }
            };

            var deleteButton = new Button { Text = "Delete", FontSize = labelFontSize };
            deleteButton.Clicked += (sender, e) =>
            {
                var todoItem = (TodoItem)BindingContext;
                App.Database.DeleteItem(todoItem.Id);
                this.Navigation.PopAsync();
            };

            var cancelButton = new Button { Text = "Cancel", FontSize = labelFontSize };
            cancelButton.Clicked += (sender, e) =>
            {
                var todoItem = (TodoItem)BindingContext;
                this.Navigation.PopAsync();
            };


            stackLayout.Children.Add(nameLabel);
            stackLayout.Children.Add(nameEntry);
            stackLayout.Children.Add(noteLabel);
            stackLayout.Children.Add(noteEditor);
            stackLayout.Children.Add(priorityLabel);
            stackLayout.Children.Add(priorityPicker);
            stackLayout.Children.Add(doneLabel);
            stackLayout.Children.Add(doneSwitch);
            stackLayout.Children.Add(saveButton);
            stackLayout.Children.Add(deleteButton);
            stackLayout.Children.Add(cancelButton);

            var scrollView = new ScrollView() {Content = stackLayout};

            Content = scrollView;
        }
    }
}
