using SQLite;
using Shelfy.Models;

namespace Shelfy.Services;

public class DatabaseService
{
    private SQLiteAsyncConnection? _connection;

    private async Task InitAsync()
    {
        if (_connection is not null)
            return;

        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "shelfy.db3");
        _connection = new SQLiteAsyncConnection(dbPath);
        await _connection.CreateTableAsync<PantryItem>();
    }

    public async Task<List<PantryItem>> GetAllAsync()
    {
        await InitAsync();
        return await _connection!.Table<PantryItem>()
            .OrderBy(x => x.ExpirationDate)
            .ToListAsync();
    }

    public async Task<int> SaveAsync(PantryItem item)
    {
        await InitAsync();
        if (item.Id != 0)
            return await _connection!.UpdateAsync(item);

        return await _connection!.InsertAsync(item);
    }

    public async Task<int> DeleteAsync(PantryItem item)
    {
        await InitAsync();
        return await _connection!.DeleteAsync(item);
    }
}