using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Text;
using Todo.Models;
using Xamarin.Forms;
using static System.Int32;

namespace Todo.Views
{
    public class ListPage : ContentPage
    {
        private ListView _todoItemListView;

        public ListPage()
        {
            Title = "Todo";
            Padding = new Thickness(10);

            var stackLayout = new StackLayout { Spacing = 20 };

            var dataTemplate = new DataTemplate(() =>
            {
                var priorityLabel = new Label
                {
                    FontSize = 30,
                    HeightRequest = 40,
                    WidthRequest = 40
                };

                var nameLabel = new Label { HorizontalOptions = LayoutOptions.StartAndExpand, FontSize = 20};
                var checkImg = new Image
                {
                    Source = ImageSource.FromFile("check.png"),
                    HorizontalOptions = LayoutOptions.End
                };

                priorityLabel.SetBinding(Label.TextProperty, "Priority");
                nameLabel.SetBinding(Label.TextProperty, "Name");
                checkImg.SetBinding(IsVisibleProperty, "Done");

                var mainLayout = new StackLayout { Orientation = StackOrientation.Horizontal };
                var subLayout = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.Center
                };
                mainLayout.Children.Add(priorityLabel);
                mainLayout.Children.Add(subLayout);

                subLayout.Children.Add(nameLabel);
                subLayout.Children.Add(checkImg);

                return new ViewCell { View = mainLayout };
            });

            _todoItemListView = new ListView
            {
                ItemsSource = App.Database.GetItems(),
                ItemTemplate = dataTemplate
            };

            _todoItemListView.ItemSelected += (sender, e) =>
            {
                var todoItem = (TodoItem)e.SelectedItem;
                var todoPage = new ItemPage { BindingContext = todoItem };

                ((App)App.Current).ResumeAtTodoId = todoItem.Id;
                Debug.WriteLine("setting ResumeAtTodoId = " + todoItem.Id);


                Navigation.PushAsync(todoPage);
            };

            #region toolbar
            ToolbarItem tbi = null;

            if (Device.OS == TargetPlatform.Android)
            { // BUG: Android doesn't support the icon being null
                tbi = new ToolbarItem("+", "plus", () =>
                {
                    var todoItem = new TodoItem();
                    var todoPage = new ItemPage { BindingContext = todoItem };
                    Navigation.PushAsync(todoPage);
                }, 0, 0);
            }
            if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
            {
                tbi = new ToolbarItem("Add", "add.png", () =>
                {
                    var todoItem = new TodoItem();
                    var todoPage = new ItemPage { BindingContext = todoItem };
                    Navigation.PushAsync(todoPage);
                }, 0, 0);
            }

            ToolbarItems.Add(tbi);
            #endregion

            //var scrollView = new ScrollView { Content = _todoItemListView };

            //stackLayout.Children.Add(scrollView);

            //Content = stackLayout;

            stackLayout.Children.Add(_todoItemListView);
            var scrollView = new ScrollView() { Content = stackLayout };
            Content = scrollView;

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // reset the 'resume' id, since we just want to re-start here
            ((App)App.Current).ResumeAtTodoId = -1;
            _todoItemListView.ItemsSource = App.Database.GetItems();
        }
    }
}
