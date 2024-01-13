using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using static DPR_CrossPlatformApp.ViewModels.UpdateMaterialsViewModel;

namespace DPR_CrossPlatformApp.ViewModels
{
    public class UpdateMaterialsViewModel : BaseViewModel, INotifyPropertyChanged
    {
        // private string strAPIUrl = "https://juniortogenius.com";
        private string strAPIUrl = "https://dpr-uapi.afcons.com";
        public UpdateMaterialsViewModel()
        {
            // Call the LoadMaterialTransactionsSummaryCommand in the constructor
            LoadMaterialTransactionsSummaryCommand.Execute(null);
          
                LogoutCommand = new Command(ExecuteLogoutCommand);
            string savedUsername = SecureStorage.GetAsync("Username").Result;
            string[] usernameParts = savedUsername.Split('.');
            LoggedInUsername = usernameParts[0];
        }

        private void ExecuteLogoutCommand()
        {
            // Add logic to perform logout actions, such as clearing user credentials

            // Assuming your application uses navigation, navigate to the login page
            // You may need to adjust this based on your navigation setup
            App.Current.MainPage = new LoginPage();
        }

        private ObservableCollection<Project> _projects;

        public ObservableCollection<Project> Projects
        {
            get { return _projects; }
            set
            {
                if (_projects != value)
                {
                    _projects = value;
                    OnPropertyChanged(nameof(Projects));
                }
            }
        }

        private ObservableCollection<Block> _blocks;
        public ObservableCollection<Block> Blocks
        {
            get { return _blocks; }
            set
            {
                if (_blocks != value)
                {
                    _blocks = value;
                    OnPropertyChanged(nameof(Blocks));
                }
            }
        }
     

        public async Task LoadData()
        {
            Projects = await GetProjectsFromWebApi();
        }

        private async Task<ObservableCollection<Project>> GetProjectsFromWebApi()
        {
            try
            {
                // Use HttpClient to fetch data from the API
                using (var client = new HttpClient())
                {
                    var apiUrl = $"{strAPIUrl}/api/GetMaterialsDetails/GetJobs";
                    var response = await client.GetStringAsync(apiUrl);
                    var projects = JsonConvert.DeserializeObject<ObservableCollection<Project>>(response);

                    return projects;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new ObservableCollection<Project>();
            }
        }

        private Project _selectedProject;
        public Project SelectedProject
        {
            get { return _selectedProject; }
            set
            {
                if (_selectedProject != value)
                {
                    _selectedProject = value;
                    OnPropertyChanged(nameof(SelectedProject));

                    // Load blocks when a project is selected
                    LoadBlocksCommand.Execute(null);

                }
            }
        }

      



        private Command loadBlocksCommand;
        public Command LoadBlocksCommand => loadBlocksCommand ?? (loadBlocksCommand = new Command(async () => await ExecuteLoadBlocksCommand()));


        private async Task ExecuteLoadBlocksCommand()
        {
            // Check if a project is selected
            if (SelectedProject != null)
            {
                // Assuming you have a method named GetBlocksForProjectFromWebApi to get blocks for the selected project
                Blocks = await GetBlocksForProjectFromWebApi(SelectedProject.ProjectID);
                Components = await GetComponentsForProjectFromWebApi(SelectedProject.ProjectID);

            }
        }

        private async Task<ObservableCollection<Block>> GetBlocksForProjectFromWebApi(int projectId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var apiUrl = $"{strAPIUrl}/api/GetMaterialsDetails/GetBlocksBasedOnProject?projectId={projectId}";
                    var response = await client.GetStringAsync(apiUrl);
                    var blocks = JsonConvert.DeserializeObject<ObservableCollection<Block>>(response);

                    return blocks;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new ObservableCollection<Block>();
            }
        }

        // Add a new property for locations
        private ObservableCollection<Location> _locations;
        public ObservableCollection<Location> Locations
        {
            get { return _locations; }
            set
            {
                if (_locations != value)
                {
                    _locations = value;
                    OnPropertyChanged(nameof(Locations));
                }
            }
        }

        // Add a property to store the selected block
        private Block _selectedBlock;
        public Block SelectedBlock
        {
            get { return _selectedBlock; }
            set
            {
                if (_selectedBlock != value)
                {
                    _selectedBlock = value;
                    OnPropertyChanged(nameof(SelectedBlock));

                    // Load locations when a block is selected
                    LoadLocationsCommand.Execute(null);
                }
            }
        }

        // Add a new command for loading locations
        private Command loadLocationsCommand;
        public Command LoadLocationsCommand => loadLocationsCommand ?? (loadLocationsCommand = new Command(async () => await ExecuteLoadLocationsCommand()));

        private async Task ExecuteLoadLocationsCommand()
        {
            // Check if a block is selected
            if (SelectedBlock != null)
            {
                // Assuming you have a method named GetLocationsForBlockFromWebApi to get locations for the selected block
                Locations = await GetLocationsForBlockFromWebApi(SelectedBlock.Id);
            }
        }

        private async Task<ObservableCollection<Location>> GetLocationsForBlockFromWebApi(int blockId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var apiUrl = $"{strAPIUrl}/api/GetMaterialsDetails/GetLocationsByBlock?blockId={blockId}";
                    var response = await client.GetStringAsync(apiUrl);
                    var locations = JsonConvert.DeserializeObject<ObservableCollection<Location>>(response);

                    return locations;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new ObservableCollection<Location>();
            }
        }

        // Add the following properties and commands to your existing ViewModel

        private ObservableCollection<Village> _villages;
        public ObservableCollection<Village> Villages
        {
            get { return _villages; }
            set
            {
                if (_villages != value)
                {
                    _villages = value;
                    OnPropertyChanged(nameof(Villages));
                }
            }
        }

        private Location _selectedLocation;
        public Location SelectedLocation
        {
            get { return _selectedLocation; }
            set
            {
                if (_selectedLocation != value)
                {
                    _selectedLocation = value;
                    OnPropertyChanged(nameof(SelectedLocation));

                    // Load villages when a location is selected
                    LoadVillagesCommand.Execute(null);
                }
            }
        }


        private Command loadVillagesCommand;
        public Command LoadVillagesCommand => loadVillagesCommand ?? (loadVillagesCommand = new Command(async () => await ExecuteLoadVillagesCommand()));

        private async Task ExecuteLoadVillagesCommand()
        {
            // Check if a location is selected
            if (SelectedLocation != null)
            {
                // Assuming you have a method named GetVillagesForLocationFromWebApi to get villages for the selected location
                Villages = await GetVillagesForLocationFromWebApi(SelectedLocation.Id);
            }
        }

        private async Task<ObservableCollection<Village>> GetVillagesForLocationFromWebApi(int locationId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var apiUrl = $"{strAPIUrl}/api/GetMaterialsDetails/GetVillagesByLocation?locationId={locationId}";
                    var response = await client.GetStringAsync(apiUrl);
                    var villages = JsonConvert.DeserializeObject<ObservableCollection<Village>>(response);

                    return villages;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new ObservableCollection<Village>();
            }
        }


        private ObservableCollection<BoQHead> _boqHeads;
        public ObservableCollection<BoQHead> BoQHeads
        {
            get { return _boqHeads; }
            set
            {
                if (_boqHeads != value)
                {
                    _boqHeads = value;
                    OnPropertyChanged(nameof(BoQHeads));
                }
            }
        }

        private BoQHead _selectedBoQHead;
        public BoQHead SelectedBoQHead
        {
            get { return _selectedBoQHead; }
            set
            {
                if (_selectedBoQHead != value)
                {
                    _selectedBoQHead = value;
                    OnPropertyChanged(nameof(SelectedBoQHead));

                    // Load BOQ references when a BOQ head is selected
                  //  LoadBoQReferencesCommand.Execute(null);
                }
            }
        }

        private Command loadBoQHeadsCommand;
        public Command LoadBoQHeadsCommand => loadBoQHeadsCommand ?? (loadBoQHeadsCommand = new Command(async () => await ExecuteLoadBoQHeadsCommand()));

        private async Task ExecuteLoadBoQHeadsCommand()
        {
            // Check if a project is selected
            if (SelectedProject != null)
            {
                BoQHeads = await GetBoQHeadsForProjectFromWebApi(SelectedProject.ProjectID);
            }
        }

        private async Task<ObservableCollection<BoQHead>> GetBoQHeadsForProjectFromWebApi(int projectId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var apiUrl = $"{strAPIUrl}/api/GetMaterialsDetails/GetBoQHeadsByProject?projectId={projectId}";
                    var response = await client.GetStringAsync(apiUrl);
                    var boqHeads = JsonConvert.DeserializeObject<ObservableCollection<BoQHead>>(response);

                    return boqHeads;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new ObservableCollection<BoQHead>();
            }
        }


        private ObservableCollection<BoQReference> _boqReferences;
        public ObservableCollection<BoQReference> BoQReferences
        {
            get { return _boqReferences; }
            set
            {
                if (_boqReferences != value)
                {
                    _boqReferences = value;
                    OnPropertyChanged(nameof(BoQReferences));
                }
            }
        }

        private Command loadBoQReferencesCommand;
        public Command LoadBoQReferencesCommand => loadBoQReferencesCommand ?? (loadBoQReferencesCommand = new Command(async () => await ExecuteLoadBoQReferencesCommand()));

        private async Task ExecuteLoadBoQReferencesCommand()
        {
            // Check if a BoQ Head is selected
            if (SelectedBoQHead != null)
            {
                // Assuming you have a method named GetBoQReferencesByBoQHeadFromWebApi to get BoQ References for the selected BoQ Head
                BoQReferences = await GetBoQReferencesByBoQHeadFromWebApi(SelectedBoQHead.Id);
                JTDQuantity = "60";
                FTDQuantityText = "40";
                FTMQuantityText = "60";
            
            }
        }

        private async Task<ObservableCollection<BoQReference>> GetBoQReferencesByBoQHeadFromWebApi(int boQHeadId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var apiUrl = $"{strAPIUrl}/api/GetMaterialsDetails/GetBoQReferencesByBoQHead?boQHeadId={boQHeadId}";
                    var response = await client.GetStringAsync(apiUrl);
                    var boqReferences = JsonConvert.DeserializeObject<ObservableCollection<BoQReference>>(response);

                    return boqReferences;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new ObservableCollection<BoQReference>();
            }
        }


        private BoQReference _selectedBoQReference;
        public BoQReference SelectedBoQReference
        {
            get { return _selectedBoQReference; }
            set
            {
                if (_selectedBoQReference != value)
                {
                    _selectedBoQReference = value;
                    OnPropertyChanged(nameof(SelectedBoQReference));

                    // Load BOQ details when a reference is selected
                    LoadBoQDetailsCommand.Execute(null);
                }
            }
        }

        private BoQReferenceDetail _boqReferenceDetails;
        public BoQReferenceDetail BoQReferenceDetails
        {
            get { return _boqReferenceDetails; }
            set
            {
                if (_boqReferenceDetails != value)
                {
                    _boqReferenceDetails = value;
                    OnPropertyChanged(nameof(BoQReferenceDetails));
                }
            }
        }

        public Command LoadBoQDetailsCommand => new Command(async () => await ExecuteLoadBoQDetailsCommand());

        private async Task ExecuteLoadBoQDetailsCommand()
        {
            // Check if a BOQ reference is selected
            if (SelectedBoQReference != null)
            {
                // Load BOQ details based on the selected reference
                BoQReferenceDetails = await GetBoQReferenceDetailsFromWebApi(SelectedBoQReference.Id);

                TypeOfPipe = BoQReferenceDetails.TypeOfPipeClass;
                DiaOfPipe = BoQReferenceDetails.Diameter;
                Uom = BoQReferenceDetails.UOM;
                BlockQuantity = BoQReferenceDetails.BlockQuantity.ToString();
                //JTDQuantity = BoQReferenceDetails.JTD.ToString();
            }
        }

        private async Task<BoQReferenceDetail> GetBoQReferenceDetailsFromWebApi(int boQReferenceId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var apiUrl = $"{strAPIUrl}/api/GetMaterialsDetails/GetBoQDetailsByReference?boQReferenceId={boQReferenceId}";
                    var response = await client.GetStringAsync(apiUrl);
                    var boqDetails = JsonConvert.DeserializeObject<BoQReferenceDetail>(response);

                    return boqDetails;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return null;
            }
        }

        private string _typeOfPipe;
        public string TypeOfPipe
        {
            get => _typeOfPipe;
            set
            {
                if (_typeOfPipe != value)
                {
                    _typeOfPipe = value;
                    OnPropertyChanged(nameof(TypeOfPipe));
                }
            }
        }

        private string _diaOfPipe;
        public string DiaOfPipe
        {
            get => _diaOfPipe;
            set
            {
                if (_diaOfPipe != value)
                {
                    _diaOfPipe = value;
                    OnPropertyChanged(nameof(DiaOfPipe));
                }
            }
        }


        private string _uom;
        public string Uom
        {
            get => _uom;
            set
            {
                if (_uom != value)
                {
                    _uom = value;
                    OnPropertyChanged(nameof(Uom));
                }
            }
        }

        private string _blockQuantity;
        public string BlockQuantity
        {
            get => _blockQuantity;
            set
            {
                if (_blockQuantity != value)
                {
                    _blockQuantity = value;
                    OnPropertyChanged(nameof(BlockQuantity));
                }
            }
        }

        private string _jtdQuantity;
        public string JTDQuantity
        {
            get => _jtdQuantity;
            set
            {
                if (_jtdQuantity != value)
                {
                    _jtdQuantity = value;
                    OnPropertyChanged(nameof(JTDQuantity));
                }
            }
        }
        private string _dayQuantityText;
        public string DayQuantityText
        {
            get { return _dayQuantityText; }
            set
            {
                if (_dayQuantityText != value)
                {
                    _dayQuantityText = value;
                    OnPropertyChanged(nameof(DayQuantityText));
                }
            }
        }


        private string _FTMQuantityText;
        public string FTMQuantityText
        {
            get { return _FTMQuantityText; }
            set
            {
                if (_FTMQuantityText != value)
                {
                    _FTMQuantityText = value;
                    OnPropertyChanged(nameof(FTMQuantityText));
                }
            }
        }

        private string _FTDQuantityText;
        public string FTDQuantityText
        {
            get { return _FTDQuantityText; }
            set
            {
                if (_FTDQuantityText != value)
                {
                    _FTDQuantityText = value;
                    OnPropertyChanged(nameof(FTDQuantityText));
                }
            }
        }

        private string _JTDQuantityText;
        public string JTDQuantityText
        {
            get { return _JTDQuantityText; }
            set
            {
                if (_JTDQuantityText != value)
                {
                    _JTDQuantityText = value;
                    OnPropertyChanged(nameof(JTDQuantityText));
                }
            }
        }


        //private int _dayQuantity;
        //public int DayQuantity
        //{
        //    get { return _dayQuantity; }
        //    set
        //    {
        //        if (_dayQuantity != value)
        //        {
        //            _dayQuantity = value;
        //            OnPropertyChanged(nameof(DayQuantity));
        //        }
        //    }
        //}

        private string _loggedInUsername;
        public string LoggedInUsername
        {
            get { return _loggedInUsername; }
            set
            {
                if (_loggedInUsername != value)
                {
                    _loggedInUsername = value;
                    OnPropertyChanged(nameof(LoggedInUsername));
                }
            }
        }


        private DateTime _selectedDate = DateTime.Now.Date;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
            }
        }


        private bool isListViewVisible;
        public bool IsListViewVisible
        {
            get { return isListViewVisible; }
            set
            {
                if (isListViewVisible != value)
                {
                    isListViewVisible = value;
                    OnPropertyChanged(nameof(IsListViewVisible));
                }
            }
        }


        private string wbsNumber;
        public string WBSNumber
        {
            get { return wbsNumber; }
            set
            {
                if (wbsNumber != value)
                {
                    wbsNumber = value;
                    OnPropertyChanged(nameof(WBSNumber));
                }
            }
        }

        //private string dayQuantityEntry;
        //public string DayQuantityEntry
        //{
        //    get { return dayQuantityEntry; }
        //    set
        //    {
        //        if (dayQuantityEntry != value)
        //        {
        //            dayQuantityEntry = value;
        //            OnPropertyChanged(nameof(DayQuantityEntry));
        //        }
        //    }
        //}

        private Village _selectedVillage;
        public Village SelectedVillage
        {
            get { return _selectedVillage; }
            set
            {
                if (_selectedVillage != value)
                {
                    _selectedVillage = value;
                    OnPropertyChanged(nameof(SelectedVillage));

                    // Add any additional logic or commands you want to execute when the Village is selected
                }
            }
        }

        private Component _selectedComponent;
        public Component SelectedComponent
        {
            get { return _selectedComponent; }
            set
            {
                if (_selectedComponent != value)
                {
                    _selectedComponent = value;
                    OnPropertyChanged(nameof(SelectedComponent));

                    // Load BOQ references when a BOQ head is selected
                    //  LoadBoQReferencesCommand.Execute(null);
                }
            }
        }

        private ObservableCollection<Component> _components;
        public ObservableCollection<Component> Components
        {
            get { return _components; }
            set
            {
                if (_components != value)
                {
                    _components = value;
                    OnPropertyChanged(nameof(Components));
                }
            }
        }

        private Command loadComponentsCommand;
        public Command LoadComponentsCommand => loadComponentsCommand ?? (loadComponentsCommand = new Command(async () => await ExecuteLoadComponentsCommand()));

        private async Task ExecuteLoadComponentsCommand()
        {
            // Check if a project is selected
            if (SelectedProject != null)
            {
                // Assuming you have a method named GetComponentsForProjectFromWebApi to get components for the selected project
                Components = await GetComponentsForProjectFromWebApi(SelectedProject.ProjectID);
            }
        }

        private async Task<ObservableCollection<Component>> GetComponentsForProjectFromWebApi(int projectId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var apiUrl = $"{strAPIUrl}/api/GetMaterialsDetails/GetComponentsByProjectId?projectId={projectId}";
                    var response = await client.GetStringAsync(apiUrl);
                    var components = JsonConvert.DeserializeObject<ObservableCollection<Component>>(response);

                    return components;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new ObservableCollection<Component>();
            }
        }



        private Command saveCommand;
        public Command SaveCommand => saveCommand ?? (saveCommand = new Command(OnSave));

        private Command submitCommand;
        public Command SubmitCommand => submitCommand ?? (submitCommand = new Command(OnSubmit));

        private void ShowValidationError(string errorMessage)
        {
            MessagingCenter.Send(this, "ValidationError", errorMessage);
        }

        
              private void ShowUnSubmittedDataError(string errorMessage)
        {
            MessagingCenter.Send(this, "ShowUnSubmittedDataError", errorMessage);
        }
        private void ShowSaveMessage(string errorMessage)
        {
            MessagingCenter.Send(this, "DataSaved", errorMessage);
        }

        private void DataSaveError(string errorMessage)
        {
            MessagingCenter.Send(this, "DataSaveError", errorMessage);
        }
        private void OnSave()
        {

            SaveToDatabase(false);


        }

        private void OnSubmit()
        {
            SaveToDatabase(true);
        }
        public ICommand ViewCommand => new Command(async () => await OnView());
        public ICommand EditCommand => new Command(async () => await OnEdit());
        public ICommand DeleteCommand => new Command(async () => await OnDelete());

        private async Task OnView()
        {
            await Application.Current.MainPage.DisplayAlert("View", "View icon clicked", "OK");
        }

        private async Task OnEdit()
        {
            await Application.Current.MainPage.DisplayAlert("Edit", "Edit icon clicked", "OK");
        }

        private async Task OnDelete()
        {
            await Application.Current.MainPage.DisplayAlert("Delete", "Delete icon clicked", "OK");
        }


        public async void SaveToDatabase(bool isSubmit)
        {
            // Validate required fields before saving
            string validationMessage = ValidateFields();

            if (!string.IsNullOrEmpty(validationMessage))
            {
                // Display validation error using MessagingCenter
                ShowValidationError(validationMessage);
                return;
            }

            string savedUsername = SecureStorage.GetAsync("Username").Result;

            if (isSubmit == true)
            {

                string unsubmittedDataMessage = await CheckUnsubmittedData(savedUsername, SelectedDate);
                if (!string.IsNullOrEmpty(unsubmittedDataMessage))
                {
                    ShowUnSubmittedDataError(unsubmittedDataMessage);
                    // await DisplayAlert("Unsubmitted Data Found", unsubmittedDataMessage, "OK");
                    return;
                }

            }

            // Create MaterialTransactionDto with the selected values
            var materialTransactionDto = new MaterialTransactionDto
            {
                Date = SelectedDate,
                JobCodeID = SelectedProject?.ProjectID ?? 0,
                BlockID = SelectedBlock?.Id ?? 0,
                ComponentID = SelectedComponent.Id,
                LocationID = SelectedLocation.Id,
                VillageNameID = SelectedVillage.Id,
                BOQHeadID = SelectedBoQHead.Id,
                BOQReferenceID = SelectedBoQReference?.Id ?? 0,
                ActivityID = 1,
                BlockQuantity = Convert.ToInt32(BlockQuantity),
                TypeOfPipe = TypeOfPipe,
                DiaOfPipe = DiaOfPipe,
                UOM = Uom,
                JTDQuantity = Convert.ToInt32(JTDQuantity),
                DayQuantity = Convert.ToInt32(DayQuantityText),
                Username = savedUsername,
                IsSubmitted = isSubmit,
                WBSNumber = WBSNumber,
                token = "token"
            };

            SaveMaterialTransaction(materialTransactionDto);
        }
        public ICommand LogoutCommand { get; private set; }



        private string ValidateFields()
        {
            List<string> missingFields = new List<string>();

            // Check if the selected project is valid
            if (SelectedProject == null || SelectedProject.ProjectName == "Select")
                missingFields.Add("Project");

            // Check if the selected block is valid
            if (SelectedBlock == null || SelectedBlock.BlockName == "Select")
                missingFields.Add("Block");

            // Check if the selected component is valid
            if (SelectedComponent == null || SelectedComponent.ComponentName == "Select")
                missingFields.Add("Component");

            // Check if the selected location is valid
            if (SelectedLocation == null || SelectedLocation.LocationName == "Select")
                missingFields.Add("Location");

            // Check if the selected village is valid
            if (SelectedVillage == null || SelectedVillage.VillageName == "Select")
                missingFields.Add("Village");

            // Check if the selected BOQ head is valid
            if (SelectedBoQHead == null || SelectedBoQHead.BoQHeadName == "Select")
                missingFields.Add("BOQ Head");

            // Check if the selected BOQ reference is valid
            if (SelectedBoQReference == null || SelectedBoQReference.BOQNo == "Select")
                missingFields.Add("BOQ Reference");

            // Check if the WBSNumber is not empty
            if (string.IsNullOrEmpty(WBSNumber))
                missingFields.Add("WBS Number");

            // Check if the DayQuantityEntry is not empty
            if (string.IsNullOrEmpty(DayQuantityText))
                missingFields.Add("Day Quantity");

            // Add similar checks for other fields

            if (missingFields.Count > 0)
            {
                string errorMessage = $"Please provide values for the following fields:\n\n{string.Join(",\n\n", missingFields)}";
                return errorMessage;
            }

            return null; // No missing fields
        }


        private async Task<string> CheckUnsubmittedData(string username, DateTime date)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var apiUrl = $"{strAPIUrl}/api/CheckUnsubmittedData?username={username}&date={date.ToString("yyyy-MM-dd")}";
                    var response = await client.GetStringAsync(apiUrl);

                    // Assuming the API returns a string message indicating unsubmitted data
                    return response;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return "An error occurred while checking unsubmitted data.";
            }
        }


        private async void SaveMaterialTransaction(MaterialTransactionDto materialTransactionDto)
        {
            try
            {
                // Convert MaterialTransactionDto to JSON
                var jsonBody = JsonConvert.SerializeObject(materialTransactionDto);

                // Create HttpClient
                using (var client = new HttpClient())
                {
                    // Set the API endpoint
                    var apiUrl = $"{strAPIUrl}/api/GetMaterialsDetails/SaveMaterialTransaction";

                    // Create StringContent with JSON body
                    var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                    // Call the API
                    var response = await client.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        ShowSaveMessage("DataSaved");
                    }
                    else
                    {
                        DataSaveError("DataSaveError");
                    }                    
                }
            }
            catch (Exception ex)
            {
                DataSaveError("DataSaveError");
            }

        }

        private ObservableCollection<MaterialTransactionSummary> _materialTransactionsSummary;
        public ObservableCollection<MaterialTransactionSummary> MaterialTransactionsSummary
        {
            get { return _materialTransactionsSummary; }
            set
            {
                if (_materialTransactionsSummary != value)
                {
                    _materialTransactionsSummary = value;
                    OnPropertyChanged(nameof(MaterialTransactionsSummary));
                }
            }
        }

        private Command loadMaterialTransactionsSummaryCommand;
        public Command LoadMaterialTransactionsSummaryCommand => loadMaterialTransactionsSummaryCommand ?? (loadMaterialTransactionsSummaryCommand = new Command(async () => await ExecuteLoadMaterialTransactionsSummaryCommand()));

        public async Task ExecuteLoadMaterialTransactionsSummaryCommand()
        {
            try
            {

                DateTime parsedDate = DateTime.ParseExact(SelectedDate.ToShortDateString(), "M/d/yyyy", CultureInfo.InvariantCulture);

                // Format the date as "d/M/yyyy"
                //string formattedDate = parsedDate.ToString("d/M/yyyy");

                string savedUsername = SecureStorage.GetAsync("Username").Result;
                // Assuming you have a method named GetMaterialTransactionsSummaryByDateAndUsernameFromWebApi
                

                
                MaterialTransactionsSummary = 
                    await GetMaterialTransactionsSummaryByDateAndUsernameFromWebApi(
                        SelectedDate, 
                        savedUsername);

                IsListViewVisible = MaterialTransactionsSummary.Count > 0;

            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
            }
        }

        private async Task<ObservableCollection<MaterialTransactionSummary>> GetMaterialTransactionsSummaryByDateAndUsernameFromWebApi(DateTime date, string username)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var apiUrl = $"{strAPIUrl}/api/GetMaterialsDetails/GetMaterialTransactionsSummaryByDateAndUsername?date={date}&username={username}";
                    var response = await client.GetStringAsync(apiUrl);
                    var transactionsSummary = JsonConvert.DeserializeObject<ObservableCollection<MaterialTransactionSummary>>(response);

                    return transactionsSummary;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                return new ObservableCollection<MaterialTransactionSummary>();
            }
        }

        public DateTime ConvertDateFormatddmmyytommddyy(string inputDate)
        {
            if (inputDate != null)
            {
                // Split the input date by "/", assuming the format could be dd/mm/yy or mm/dd/yy
                string[] dateParts = inputDate.Split('/');
                if (dateParts.Length == 3)
                {
                    // Try parsing in dd/mm/yy format
                    if (int.TryParse(dateParts[0], out int day) &&
                        int.TryParse(dateParts[1], out int month) &&
                        int.TryParse(dateParts[2], out int year))
                    {
                        // Create a DateTime instance with the parsed values
                        DateTime date = new DateTime(year, month, day);

                        // Format the date as mm/dd/yy
                        string convertedDate = date.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);

                      return  DateTime.ParseExact(convertedDate,
                        "MM/dd/yyyy", CultureInfo.InvariantCulture);

                      
                    }
                }
            }
            // If the input format is not recognized, handle the error or return the input date as-is
            return DateTime.Now;
           
        }

        //public string ConvertDateFormatmmddyytoddmmyyDuringDisplay(string inputDate)
        //{

        //    if (inputDate != null)
        //    {
        //        string[] dateFormats = new string[]
        //       {
        //    "MMM dd yyyy hh:mmtt",   // Format 1
        //    "MMM  dd yyyy hh:mmtt",   // Format 1
        //    "MMMM dd yyyy hh:mmtt",  // Format 2
        //    "MMMM  dd yyyy hh:mmtt",  // Format 2
        //    "dd/MM/yy",              // Format 3 (desired output format)
        //    "dd-MM-yyyy HH:mm:ss",   // Format 4 (if needed)
        //    "MMM d yyyy hh:mmtt",     // Custom Format for "Jul 5 2024 12:00AM"
        //    "MMM  d yyyy hh:mmtt",
        //    "dd/MM/yyyy",
        //    "dd-MM-yyyy"
            
        //           // Add more date formats here as needed
        //       };

        //        if (inputDate.Trim() == "Jan 1 1900 12:00AM" || inputDate.Trim() == "Jan  1 1900 12:00AM"
        //            || inputDate.Trim() == "" ||
        //            inputDate.Trim() == "01-01-1900 00:00:00" ||
        //            inputDate.Trim() == "01-01-1900")
        //        {
        //            // Handle null date or the default date as needed
        //            return ""; // Or any other representation you prefer for null/default dates
        //        }
        //        else
        //        {
        //            DateTime date = DateTime.ParseExact(inputDate,
        //                dateFormats, CultureInfo.InvariantCulture);

        //            // Format the date as dd/mm/yy
        //            string convertedDate = date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

        //            return convertedDate;
        //        }
        //    }
        //    else
        //    {
        //        return "";
        //    }

        //}


        public class MaterialTransactionSummary
        {
            public string Block { get; set; }
            public string Village { get; set; }
            public string BoQReference { get; set; }
            public int DayQuantity { get; set; }
        }

        

        private void OnDateTapped(object sender, EventArgs e)
        {
            // Handle the tap event here
            // You can show/hide the DatePicker or handle date selection as needed
        }

        public class MaterialTransactionDto
        {
            public int Id { get; set; }
            public DateTime Date { get; set; }
            public int JobCodeID { get; set; }
            public int BlockID { get; set; }
            public int ComponentID { get; set; }
            public int LocationID { get; set; }
            public int VillageNameID { get; set; }
            public int BOQHeadID { get; set; }
            public int BOQReferenceID { get; set; }
            public int ActivityID { get; set; }
            public int BlockQuantity { get; set; }
            public string TypeOfPipe { get; set; }
            public string DiaOfPipe { get; set; }
            public string UOM { get; set; }
            public int JTDQuantity { get; set; }
            public int DayQuantity { get; set; }
            public string Username { get; set; }

            public bool IsSubmitted { get; set; }

            public string token { get; set; }

            public string WBSNumber { get; set; }
        }


        public class BoQReferenceDetail
        {
            public string BOQHead { get; set; }
            public string BOQNo { get; set; }
            public string WBSNumber { get; set; }
            public string BOQDescription { get; set; }
            public string UOM { get; set; }
            public string Length { get; set; }
            public string TypeOfPipeClass { get; set; }
            public string Diameter { get; set; }
            public int BlockQuantity { get; set; }
            public DateTime CreatedDate { get; set; }
            public string CreatedBy { get; set; }
            public DateTime? UpdatedDate { get; set; }
            public string UpdatedBy { get; set; }

            // Add more properties as needed
        }

        public class BoQReference
        {
            public int Id { get; set; }
            public string BOQNo { get; set; }

           
        }


        public class BoQHead
        {
            public int Id { get; set; }
            public string BoQHeadName { get; set; }
            // Add more properties as needed
        }


        public class Project
        {
            public int ProjectID { get; set; }
            public string ProjectName { get; set; }
            public string ProjectShortName { get; set; }
        }

        public class Block
        {
            public int Id { get; set; }
            public string BlockName { get; set; }
        }

        public class Component
        {
            public int Id { get; set; }
            public string ComponentName { get; set; }
            // Add more properties as needed
        }

        public class Village
        {
            public int Id { get; set; }
            public string VillageName { get; set; }
            // Add more properties as needed
        }


        public class Location
        {
            public int Id { get; set; }
            public string LocationName { get; set; }
            // Add more properties as needed
        }

    }
}
