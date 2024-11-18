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
            return await _context.Trainings.Include(t => t.Category).ToListAsync();
        }

        public async Task<Training> GetTrainingByIdAsync(int id)
        {
            return await _context.Trainings.Include(t => t.Category).FirstOrDefaultAsync(t => t.TrainingId == id);
        }

        public async Task AddTrainingAsync(Training training)
        {
            _context.Trainings.Add(training);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTrainingAsync(Training training)
        {
            _context.Trainings.Update(training);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTrainingAsync(int id)
        {
            var training = await _context.Trainings.FindAsync(id);
            if (training != null)
            {
                _context.Trainings.Remove(training);
                await _context.SaveChangesAsync();
            }
        }
    }
}
