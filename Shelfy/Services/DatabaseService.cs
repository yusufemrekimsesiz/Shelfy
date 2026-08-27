using Microsoft.Extensions.Logging;
using SQLite;
using Shelfy.Core;

namespace Shelfy.Services;

public class DatabaseService : IPantryRepository
{
    private readonly ILogger<DatabaseService> _logger;
    private SQLiteAsyncConnection? _connection;

    public DatabaseService(ILogger<DatabaseService> logger)
    {
        _logger = logger;
    }

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
        try
        {
            await InitAsync();
            return await _connection!.Table<PantryItem>()
                .OrderBy(x => x.ExpirationDate)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürünler yüklenirken hata oluştu");
            return new List<PantryItem>();
        }
    }

    public async Task<int> SaveAsync(PantryItem item)
    {
        try
        {
            await InitAsync();
            if (item.Id != 0)
                return await _connection!.UpdateAsync(item);

            return await _connection!.InsertAsync(item);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürün kaydedilirken hata oluştu: {ProductName}", item.ProductName);
            return 0;
        }
    }

    public async Task<int> DeleteAsync(PantryItem item)
    {
        try
        {
            await InitAsync();
            return await _connection!.DeleteAsync(item);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ürün silinirken hata oluştu: {ProductName}", item.ProductName);
            return 0;
        }
    }
}