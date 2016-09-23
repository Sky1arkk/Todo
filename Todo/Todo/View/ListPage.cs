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
            Padding = new Thickness(App.ScreenWidth / 40);

            var dataTemplate = new DataTemplate(() =>
            {
                var priorityLabel = new Label
                {
                    VerticalOptions = LayoutOptions.Center,
                    FontSize = App.ScreenHeight / 20,
                    HeightRequest = App.ScreenWidth / 10,
                    WidthRequest = App.ScreenWidth / 10
                };

                var nameLabel = new Label
                {
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.Center,
                    FontSize = App.ScreenHeight / 35,
                    WidthRequest = App.ScreenWidth / 1.4,
                    LineBreakMode = LineBreakMode.TailTruncation
                };

                var checkImg = new Image
                {
                    Source = ImageSource.FromFile("check.png"),
                    HorizontalOptions = LayoutOptions.EndAndExpand
                };

                priorityLabel.SetBinding(Label.TextProperty, "Priority");
                nameLabel.SetBinding(Label.TextProperty, "Name");
                checkImg.SetBinding(IsVisibleProperty, "Done");

                var mainLayout = new StackLayout { Orientation = StackOrientation.Horizontal };
                mainLayout.Children.Add(priorityLabel);
                mainLayout.Children.Add(nameLabel);
                mainLayout.Children.Add(checkImg);

                return new ViewCell { View = mainLayout };
            });

            _todoItemListView = new ListView
            {
                ItemsSource = App.Database.GetItems(),
                ItemTemplate = dataTemplate,
                RowHeight = (int) (App.ScreenWidth / 10)
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
            var tbAddItem = new ToolbarItem("Add task", "null", () =>
            {
                var todoItem = new TodoItem();
                var todoPage = new ItemPage { BindingContext = todoItem };
                Navigation.PushAsync(todoPage);
            }, ToolbarItemOrder.Secondary, 0);

            var tbSortItemsByPriority = new ToolbarItem("Sort by priority", "null", () =>
            {
                var todoItemsList = (List<TodoItem>)App.Database.GetItems();
                todoItemsList = todoItemsList.OrderBy(item => item.Priority).ToList();
                _todoItemListView.ItemsSource = todoItemsList;
            }, ToolbarItemOrder.Secondary, 0);

            var tbRemoveCompletedItem = new ToolbarItem("Remove completed tasks", "null", () =>
                {
                    foreach (var todoItem in App.Database.GetCompletedItems())
                    {
                        App.Database.DeleteItem(todoItem.Id);
                    }

                    //Refresh page
                    var listPage = new ListPage();
                    Navigation.PushAsync(listPage);

                }, ToolbarItemOrder.Secondary, 0);

            ToolbarItems.Add(tbAddItem);
            ToolbarItems.Add(tbSortItemsByPriority);
            ToolbarItems.Add(tbRemoveCompletedItem);
            #endregion

            var scrollView = new ScrollView() { Content = _todoItemListView };
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
