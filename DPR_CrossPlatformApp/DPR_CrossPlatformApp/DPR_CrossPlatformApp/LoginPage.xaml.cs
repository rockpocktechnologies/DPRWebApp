

using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DPR_CrossPlatformApp
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            
            InitializeComponent();
            //UsernameEntry.BorderStyle = 0;
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            // Show the loading indicator
            LoadingIndicator.IsRunning = true;
            LoadingIndicator.IsVisible = true;
            await Task.Delay(1000);
            // Perform login logic
            // bool loginSuccess = await PerformLoginAsync(); // Replace with your actual login logic
            bool loginSuccess = IsValidLogin(UsernameEntry.Text,
                PasswordEntry.Text);
            // Hide the loading indicator
            LoadingIndicator.IsRunning = false;
            LoadingIndicator.IsVisible = false;

            if (loginSuccess)
            {
                SecureStorage.SetAsync("Username", UsernameEntry.Text);
                // Redirect to the next page (UpdateMaterialsPage)
                await Navigation.PushAsync(new UpdateMaterialsPage());
            }
            else
            {
                // Display an error message or handle the login failure
                DisplayAlert("Login Failed", "Invalid username or password", "OK");
            }
        }

        private async Task<bool> PerformLoginAsync()
        {
            // Simulate login logic or call your actual login API
            // Replace this with your actual login implementation
            //await Task.Delay(1000); // Simulating login for 2 seconds
            return true; // For demo purposes; replace with actual success/failure check
        }


        private bool IsValidLogin(string username, string password)
        {
           
            // Replace this with your actual authentication logic
            // For simplicity, we'll use hardcoded values for demonstration purposes
            return username == "sanjay.codescreators" &&
                password == "12345";
        }
    }
}
