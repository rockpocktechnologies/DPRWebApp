using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DPR_CrossPlatformApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Shell.SetNavBarIsVisible(this, false); // Hide default navigation bar
            Shell.SetNavBarIsVisible(this, true);  // Show custom navigation bar

            // For Xamarin.Forms non-Shell
            MainPage = new NavigationPage(new LoginPage())
            {
                BarBackgroundColor = Color.White,
                BarTextColor = Color.Black
                // Padding = new Thickness(0, 0, 0, 0) // Adjust as needed
            };
            //MainPage = new NavigationPage(new UpdateMaterialsPage());
            ////MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
