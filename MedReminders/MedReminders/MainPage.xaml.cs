using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MedReminders
{
    public partial class MainPage : ContentPage
    {
        private bool _checkedState;
        public bool CheckedState
        {
            get { return _checkedState; }
            set
            {
                _checkedState = value;
                OnPropertyChanged("CheckedState");
            }
        }

        private DateTime _previousTimeStamp;
        public DateTime PreviousTimeStamp
        {
            get { return _previousTimeStamp; }
            set
            {
                _previousTimeStamp = value;
                OnPropertyChanged("PreviousTimeStamp");
                OnPropertyChanged("FormattedPreviousTimeStamp");
            }
        }

        public string FormattedPreviousTimeStamp
        {
            get
            {
                if (_previousTimeStamp == DateTime.MinValue)
                {
                    return "N/A";
                }
                else
                {
                    return _previousTimeStamp.ToString("MM/dd/yyyy HH:mm");
                }
            }
        }

        private string _formattedRefreshTimeStamp;
        public string FormattedRefreshTimeStamp
        {
            get { return _formattedRefreshTimeStamp; }
            set
            {
                _formattedRefreshTimeStamp = value;
                OnPropertyChanged("FormattedRefreshTimeStamp");
            }
        }

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        protected override void OnAppearing()
        {
            RefreshPage();
        }

        public void RefreshPage()
        {
            var timeStamp = App.CurrentApp.CheckedStateTimeStamp;
            var nextTimeStamp = timeStamp.AddDays(1);
            var now = DateTime.Now;
            
            PreviousTimeStamp = timeStamp;
            FormattedRefreshTimeStamp = now.ToString("MM/dd/yyyy HH:mm");

            if (now >= timeStamp && now < nextTimeStamp)
            {
                //inside this half open interval we want the checked state to persist
                CheckedState = App.CurrentApp.CheckBoxCheckedState;
            }
            else if (now >= nextTimeStamp)
            {
                //inside this closed ray we want the checked state to be false
                CheckedState = false;
                App.CurrentApp.UpdateCheckedState(false);
            }
        }

        private async void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            var newState = e.Value;
            if (!newState)
            {
                bool response = await DisplayAlert("Alert",
                    "Are you sure you want to uncheck the checked state?",
                    "Yes",
                    "No");
                if (!response)
                {
                    CheckedState = true; //suppress the toggle action
                    return;
                }
            }
            App.CurrentApp.UpdateCheckedState(newState);
            RefreshPage();
        }

        private async void buttSave_Clicked(object sender, EventArgs e)
        {
            var msg = "";
            try
            {
                await App.CurrentApp.SavePropertiesAsync();
                msg = "Timestamp has successfully been saved.";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            await DisplayAlert("Save Receipt", msg, "OK");
        }
    }
}