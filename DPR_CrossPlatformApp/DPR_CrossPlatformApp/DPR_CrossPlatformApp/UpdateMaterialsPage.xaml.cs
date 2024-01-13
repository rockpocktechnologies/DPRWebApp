using DPR_CrossPlatformApp.ViewModels;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using static DPR_CrossPlatformApp.ViewModels.UpdateMaterialsViewModel;

namespace DPR_CrossPlatformApp
{
    public partial class UpdateMaterialsPage : ContentPage
    {
        public UpdateMaterialsPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false); // Hide the navigation bar

            // Create an instance of your view model
            var viewModel = new UpdateMaterialsViewModel();
            MessagingCenter.Subscribe<UpdateMaterialsViewModel, string>(this, "ValidationError", async (sender, message) =>
            {
                await DisplayAlert("Validation Error", message, "OK");
            });

            MessagingCenter.Subscribe<UpdateMaterialsViewModel,
                string>(this, "DataSaved", async (sender, message) =>
            {
                await DisplayAlert("Data Saved Successfully", message, "OK");
            });

            MessagingCenter.Subscribe<UpdateMaterialsViewModel,
                string>(this, "DataSaveError", async (sender, message) =>
                {
                    await DisplayAlert("Error Occurred", message, "OK");
                });

            MessagingCenter.Subscribe<UpdateMaterialsViewModel,
               string>(this, "ShowUnSubmittedDataError", async (sender, message) =>
               {
                   await DisplayAlert("Unsubmitted Data Found", message, "OK");
               });
            // Set the view model as the BindingContext for the page
            BindingContext = viewModel;

            // Load data
            LoadData();
            ProjectPicker.SelectedIndexChanged += ProjectPicker_SelectedIndexChanged;
            BlockPicker.SelectedIndexChanged += BlockPicker_SelectedIndexChanged;
            LocationPicker.SelectedIndexChanged += LocationPicker_SelectedIndexChanged;
            VillagePicker.SelectedIndexChanged += VillagePicker_SelectedIndexChanged;            // Event handlers for dropdowns
            BoQHeadPicker.SelectedIndexChanged += BoQHeadPicker_SelectedIndexChanged;
            BoQReferencePicker.SelectedIndexChanged += BoQReferencePicker_SelectedIndexChanged; 
            
        }


        private void OnDateSelected(object sender, DateChangedEventArgs e)
        {
            if (BindingContext is UpdateMaterialsViewModel viewModel)
            {
                // Reload material transactions when the date changes
                viewModel.ExecuteLoadMaterialTransactionsSummaryCommand();
            }
        }

        public void HideListWhenNoData()
        {
            materialTransactionsListView.IsVisible = false;

        }

           public void ShowListWhenData()
        {
            materialTransactionsListView.IsVisible = true;

        }
        private async void BoQReferencePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            var viewModel = (UpdateMaterialsViewModel)BindingContext;

            if (picker.SelectedItem != null)
            {
                // Assuming you have a method named LoadBoQDetailsCommand.Execute() to load BoQ details
                viewModel.LoadBoQDetailsCommand.Execute(null);
                
            }
        }


        private void VillagePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            var viewModel = (UpdateMaterialsViewModel)BindingContext;
            if (picker.SelectedItem != null)
            {
                viewModel.LoadBoQHeadsCommand.Execute(null);
            }
        }


        private void BoQHeadPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            var viewModel = (UpdateMaterialsViewModel)BindingContext;
            if (picker.SelectedItem != null)
            {
                viewModel.LoadBoQReferencesCommand.Execute(null);
            }
        }

        private void LocationPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            var viewModel = (UpdateMaterialsViewModel)BindingContext;
            if (picker.SelectedItem != null)
            {
                viewModel.LoadVillagesCommand.Execute(null);
            }
        }

        private void BlockPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            var viewModel = (UpdateMaterialsViewModel)BindingContext;
            if (picker.SelectedItem != null)
            {
                // Assuming you have a method named LoadLocationsCommand.Execute() to load locations
                viewModel.LoadLocationsCommand.Execute(null);
            }
        }
        private void ProjectPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            var viewModel = new UpdateMaterialsViewModel();
            if (picker.SelectedItem != null)
            {
                // Assuming you have a method named LoadBlocksCommand.Execute() to load blocks
                // You might need to adjust this based on your actual implementation
                viewModel.LoadBlocksCommand.Execute(null);
                //viewModel.LoadComponentsCommand.Execute(null);
                //viewModel.LoadBoQHeadsCommand.Execute(null);
            }
        }



        private async void LoadData()
        {
            // Load any initial data or perform actions on page load
            // You can call methods from your view model if needed
            await (BindingContext as UpdateMaterialsViewModel)?.LoadData();
        }

        // Other methods or event handlers as needed
    }
}
