using SQLite;


namespace StressdetectorApp;

public class ReceivedData
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Data { get; set; }
    public DateTime Timestamp { get; set; } // Tilføj en tidsstempel for hver post
}