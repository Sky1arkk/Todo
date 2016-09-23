using System;
using System.Diagnostics;
using Todo.Views;
using Xamarin.Forms;

namespace Todo
{
    public class App : Application
    {
        public static double ScreenHeight { get; set; }
        public static double ScreenWidth { get; set; }

        private static TodoItemDatabase _database;

        public App()
        {
            Resources = new ResourceDictionary
            {
                {"primaryGreen", Color.FromHex("91CA47")}
            };

            var nav = new NavigationPage(new ListPage());

            if (Device.OS != TargetPlatform.Windows)
            {
                nav.Title = "Todo";
                nav.BarBackgroundColor = (Color)Current.Resources["primaryGreen"];
                nav.BarTextColor = Color.Black;
            }

            MainPage = nav;

            Debug.WriteLine("Heigh: " + App.ScreenHeight);
            Debug.WriteLine("Width: " + App.ScreenWidth);
        }

        public static TodoItemDatabase Database
        {
            get
            {
                if (_database == null)
                {
                    _database = new TodoItemDatabase();
                }
                return _database;
            }
        }

        public int ResumeAtTodoId { get; set; }

        protected override void OnStart()
        {
            Debug.WriteLine("OnStart");

            // always re-set when the app starts
            // users expect this (usually)
            //			Properties ["ResumeAtTodoId"] = "";
            if (Properties.ContainsKey("ResumeAtTodoId"))
            {
                var rati = Properties["ResumeAtTodoId"].ToString();
                Debug.WriteLine("   rati=" + rati);
                if (!String.IsNullOrEmpty(rati))
                {
                    Debug.WriteLine("   rati = " + rati);
                    ResumeAtTodoId = int.Parse(rati);

                    if (ResumeAtTodoId >= 0)
                    {
                        var todoPage = new ItemPage();
                        todoPage.BindingContext = Database.GetItem(ResumeAtTodoId);

                        MainPage.Navigation.PushAsync(
                            todoPage,
                            false); // no animation
                    }
                }
            }
        }

        protected override void OnSleep()
        {
            Debug.WriteLine("OnSleep saving ResumeAtTodoId = " + ResumeAtTodoId);
            // the app should keep updating this value, to
            // keep the "state" in case of a sleep/resume
            Properties["ResumeAtTodoId"] = ResumeAtTodoId;
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
