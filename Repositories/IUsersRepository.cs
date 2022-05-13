using Budget.Entities;

namespace Budget.Repositories
{
    public interface IUsersRepository
    {
        Task CreateUserAsync(User user);
        Task<User> GetUserAsync(Guid id);
        Task<IEnumerable<User>> GetUsersAsync();
        Task CreateBudgetItemAsync(Guid userId, BudgetItem item);
        Task<BudgetItem> GetBudgetItemAsync(Guid userId, Guid itemId);
        Task UpdateBudgetItemAsync(Guid userId, BudgetItem item);
        Task DeleteBudgetItemAsync(Guid userId, Guid itemId);
    }
}