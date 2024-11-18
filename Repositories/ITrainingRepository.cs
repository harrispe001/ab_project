using ab_project.Models;

namespace ab_project.Repositories
{
    public interface ITrainingRepository
    {
        Task<IEnumerable<Training>> GetAllTrainingsAsync();
        Task<Training> GetTrainingByIdAsync(int id);
        Task AddTrainingAsync(Training training);
        Task UpdateTrainingAsync(Training training);
        Task DeleteTrainingAsync(int id);
    }
}
