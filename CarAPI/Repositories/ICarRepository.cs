using CarAPI.Models;

namespace CarAPI.Repositories
{
    public interface ICarRepository : IRepository<Car>
    {
        Task<IEnumerable<Car>> GetAllWithDetailsAsync();
        Task<Car?> GetByIdWithDetailsAsync(int id);
    }
}