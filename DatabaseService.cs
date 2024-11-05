namespace StressdetectorApp;

using SQLite;
using System.IO;
using System.Threading.Tasks;

public class DatabaseService
{
    private readonly SQLiteAsyncConnection _database;

    public DatabaseService()
    {
        // Bestem stien til databasen
        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "receivedData.db3");
        _database = new SQLiteAsyncConnection(dbPath);

        // Opret tabellen, hvis den ikke eksisterer
        _database.CreateTableAsync<ReceivedData>().Wait();
    }

    public Task<int> SaveDataAsync(ReceivedData data)
    {
        return _database.InsertAsync(data);
    }

    public Task<List<ReceivedData>> GetDataAsync()
    {
        return _database.Table<ReceivedData>().ToListAsync();
    }
}
