using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MedReminders
{
    public partial class App : Application
    {
        public static class AppKeys
        {
            public static string CheckBoxCheckedStateKey = "CheckBoxCheckedStateKey";
            public static string CheckedStateTimeStamp = "CheckedStateTimeStamp";
        }

        public bool CheckBoxCheckedState
        {
            get
            {
                if (CurrentApp.Properties.ContainsKey(AppKeys.CheckBoxCheckedStateKey))
                    return (bool)CurrentApp.Properties[AppKeys.CheckBoxCheckedStateKey];
                return false;
            }
            set
            {
                CurrentApp.Properties[AppKeys.CheckBoxCheckedStateKey] = value;
            }
        }

        public DateTime CheckedStateTimeStamp
        {
            get
            {
                if (CurrentApp.Properties.ContainsKey(AppKeys.CheckedStateTimeStamp))
                    return (DateTime)CurrentApp.Properties[AppKeys.CheckedStateTimeStamp];
                return DateTime.MinValue;
            }
            set
            {
                CurrentApp.Properties[AppKeys.CheckedStateTimeStamp] = value;
            }
        }

        /// <summary>
        /// Call this method to retrieve the current application instance.
        /// </summary>
        public static App CurrentApp
        {
            get
            {
                if (Application.Current != null)
                    return (App)Application.Current;
                return null;
            }
        }

        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected async override void OnSleep()
        {
            await CurrentApp.SavePropertiesAsync();
        }

        protected override void OnResume()
        {
        }

        /// <summary>
        /// Call this method to change the state of the Checkbox CheckedState, since this method will always 
        /// handle updating the timestamp properly as well.
        /// </summary>
        /// <param name="newState"></param>
        public void UpdateCheckedState(bool newState)
        {
            if (CurrentApp.CheckBoxCheckedState != newState)
            {
                CurrentApp.CheckBoxCheckedState = newState;
                if (newState)
                {
                    CurrentApp.CheckedStateTimeStamp = DateTime.Now;
                }
            }
        }
    }
}
