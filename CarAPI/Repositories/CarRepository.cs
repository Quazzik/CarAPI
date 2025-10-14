using Microsoft.EntityFrameworkCore;
using CarAPI.Data;
using CarAPI.Models;

namespace CarAPI.Repositories
{
    public class CarRepository : Repository<Car>, ICarRepository
    {
        public CarRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Car>> GetAllAsync()
        {
            return await _context.Cars
                .Include(c => c.CarBrand)
                .Include(c => c.TrimLevel)
                .ToListAsync();
        }

        public override async Task<Car?> GetByIdAsync(int id)
        {
            return await _context.Cars
                .Include(c => c.CarBrand)
                .Include(c => c.TrimLevel)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Car>> GetAllWithDetailsAsync()
        {
            return await _context.Cars
                .Include(c => c.CarBrand)
                .Include(c => c.TrimLevel)
                .ToListAsync();
        }

        public async Task<Car?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Cars
                .Include(c => c.CarBrand)
                .Include(c => c.TrimLevel)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}