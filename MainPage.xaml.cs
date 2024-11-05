using System.Diagnostics;
using Microsoft.Maui.Controls;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.Maui;
using Microsoft.Maui.Dispatching;
using SQLite;
using System.IO;
using System.Threading.Tasks;
using AVFoundation;

namespace StressdetectorApp
{
    public partial class MainPage : ContentPage
    {
        private readonly WiFiServer _server;
        private readonly Sundhedsdata _sundhedsdataPage;
        private readonly DatabaseService _databaseService;
        
        public MainPage(DatabaseService databaseService, WiFiServer server)
        {
            InitializeComponent();
            // Initialiser Sundhedsdata-siden
            _sundhedsdataPage = new Sundhedsdata(databaseService);
            
            _server = server;
            _databaseService = databaseService;
            StartWifiServer();

            
            
        }
        private void StartWifiServer()
        {
            _server.DataReceived += OnDataReceived;
        }
        
        private async void OnDataReceived(string data)
        {
            // Opret og initialiser receivedData-objektet
            var receivedData = new ReceivedData { Data = data, Timestamp = DateTime.Now };

            // Naviger til Sundhedsdata-siden (hvis nødvendigt)
            await Navigation.PushAsync(_sundhedsdataPage);

            // Opdater Sundhedsdata-siden med de nye data
            _sundhedsdataPage.UpdateData(receivedData);

            // Gem data i databasen
            await _databaseService.SaveDataAsync(receivedData);
        }
        
        
       // protected override void OnDisappearing()
        // {
        //     // Stop serveren, når siden lukker
        //     _server?.StopServer();
        //     base.OnDisappearing();
        // }
        
        private async void VisSundhedsdataClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Sundhedsdata(_databaseService));
        }

        private void OnKalenderClicked(object sender, EventArgs e)
        {
            // kode til hvis vi vil have en kalender eller noget andet?
        }

        private async void OnIndstillingerClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new IndstillingerPage());
        }

        
        
        

    }
}
