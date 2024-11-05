
using System.Net;
using System.Net.Sockets;

namespace StressdetectorApp;


public class WiFiServer
{
    private ReceivedData _receivedData;
    private readonly Sundhedsdata _sundhedsdata;
    private readonly DatabaseService _databaseService;
    
    private readonly TcpListener _listener  = new TcpListener(
        localaddr: IPAddress.Parse("0.0.0.0"),
        port: 8080
    );

    // Event til at sende modtagne data tilbage til UI
    public event Action<string> DataReceived;

    public WiFiServer()
    {
        _listener.Start();
        Console.WriteLine("Serveren kører og lytter...");

        // Accepter klientforbindelser asynkront
        _listener.BeginAcceptTcpClient(new AsyncCallback(AcceptClient), _listener);
    }

    // Metode til at acceptere nye klientforbindelser (Arduinoen)
    private void AcceptClient(IAsyncResult ar)
    {
        // Hent TcpListener og accepter klienten (Arduino)
      
        TcpClient client = _listener.EndAcceptTcpClient(ar);
        Console.WriteLine("Klient forbundet!");

        // Start en ny læseoperation til at modtage data fra klienten
        OnDataReceived(client);
        // Accepter næste klientforbindelse (hvis der er flere forbindelser)
        _listener.BeginAcceptTcpClient(new AsyncCallback(AcceptClient), _listener);
    }

    private async void OnDataReceived(TcpClient client)
    {
        using (NetworkStream stream = client.GetStream())
        using (StreamReader reader = new StreamReader(stream))
        using (StreamWriter writer = new StreamWriter(stream))
        {
            var allDataAsText = await reader.ReadToEndAsync();
            Console.WriteLine($"Data received: {allDataAsText}");

            // Send en simpel respons tilbage til klienten
            await writer.WriteLineAsync("Data modtaget");
            await writer.FlushAsync();
        }
        client.Close();
        
    }
        
    }

    
    



// _receivedData = new ReceivedData();
// Console.WriteLine($"Data received: {_receivedData.Data}");
// NetworkStream stream = client.GetStream(); 
// using var reader = new StreamReader(stream);
// var allDataAsText = reader.ReadToEnd();
//        
// Console.WriteLine("Received raw data: " + allDataAsText);
// DataReceived(allDataAsText);




    //gamle on data received:::::
    
   


   /*NetworkStream stream = client.GetStream();
   using var reader = new StreamReader(stream);
   var allDataAsText = reader.ReadToEnd();

   Console.WriteLine($"Raw data received: {allDataAsText}");

   // Split dataen i tal baseret på mellemrum
   var dataParts = allDataAsText.Split(' ');
   if (dataParts.Length == 2)
   {
       var pulse = dataParts[0]; // Første del er pulsen
       var conductance = dataParts[1]; // Anden del er konduktans

       // Gemmer data som en samlet streng eller opdaterer UI
       var receivedData = new ReceivedData
       {
           Data = $"Pulse: {pulse}, Conductance: {conductance}",
           Timestamp = DateTime.Now
       };

       _sundhedsdata.UpdateData(receivedData);
       _databaseService.SaveDataAsync(receivedData);
   }
   else
   {
       Console.WriteLine("Data format ikke genkendt");
   }*/
    
    
    
    
    
    
    // // Metode til at modtage data fra klienten (Arduino)
    // private void ReceiveData(IAsyncResult ar)
    // {
    //     // Dekonstruktion af tilstanden for at få adgang til klienten og buffer
    //     object[] state = (object[])ar.AsyncState;
    //     TcpClient client = (TcpClient)state[0];
    //     byte[] buffer = (byte[])state[1];
    //
    //     // Læs data fra strømmen
    //     NetworkStream stream = client.GetStream();
    //     int bytesRead = stream.EndRead(ar);
    //
    //     if (bytesRead > 0)
    //     {
    //         // Konverter data fra byte-array til en læselig tekststreng
    //         string receivedData = Encoding.ASCII.GetString(buffer, 0, bytesRead);
    //         Console.WriteLine("Data modtaget: " + receivedData);
    //
    //         // Trigger event med modtagne data
    //         DataReceived?.Invoke(receivedData);
    //
    //         // Start en ny læseoperation for at holde forbindelsen åben og modtage flere data
    //         stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(ReceiveData), new object[] { client, buffer });
    //     }
    //     else
    //     {
    //         // Luk forbindelsen, hvis der ikke er mere data
    //         client.Close();
    //         Console.WriteLine("Klient afbrudt.");
    //     }
    // }

    // Metode til at stoppe serveren og frigøre ressourcer
    //public void StopServer()
    //{
     //   _listener.Stop();
    //}
