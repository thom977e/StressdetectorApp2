using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using SQLite;
using System.IO;
using System.Threading.Tasks;

namespace StressdetectorApp
{
    public partial class Sundhedsdata : ContentPage
    {
        
        private readonly DatabaseService _databaseService;
        public ObservableCollection<ReceivedData> DataList { get; set; } = new ObservableCollection<ReceivedData>();
        
        public Sundhedsdata(DatabaseService databaseService)
        {
            InitializeComponent();
            _databaseService = databaseService;
            BindingContext = this; // Sætter denne side som binding context

            
            Task.Run(async () =>
            {
                await LoadData();
            });
        }
        private async Task LoadData()
        {
            
            // Hent data asynkront fra databasen
            var dataList = await _databaseService.GetDataAsync();
            DataList.Clear();

            // Display each data item
            foreach (var data in dataList)
            {
                DataList.Add(data);
            }
        }
        
        // Opdaterer data-labelen med nye data
        public void UpdateData(ReceivedData receivedData)
        {
            Console.WriteLine($"UpdateData kaldt med: {receivedData.Data}"); // Tilføj denne linje

            MainThread.BeginInvokeOnMainThread(() =>
            {
            DataList.Add(receivedData);
            });
        }

        
        

        private async void OnTilbageButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();  // Navigerer tilbage til den forrige side
        }

        // Event handler for the Analyse button
        private async void OnAnalyseButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AnalyseRapportPage());
        }
        
    }
}
