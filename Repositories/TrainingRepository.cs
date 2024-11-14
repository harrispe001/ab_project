using ab_project.Data;
using ab_project.Models;
using Microsoft.EntityFrameworkCore;

namespace ab_project.Repositories
{
    public class TrainingRepository : ITrainingRepository
    {
        private readonly ApplicationDbContext _context;

        public TrainingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Training>> GetAllTrainingsAsync()
        {
            return await _context.Trainings
                .Include(t => t.Category)
                .ToListAsync();
        }

        // Implement other methods as needed
    }
}
