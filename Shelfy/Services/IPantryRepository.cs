using Shelfy.Core;

namespace Shelfy.Services;

public interface IPantryRepository
{
    Task<List<PantryItem>> GetAllAsync();
    Task<int> SaveAsync(PantryItem item);
    Task<int> DeleteAsync(PantryItem item);
}