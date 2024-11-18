using ab_project.Models;

namespace ab_project.Services
{
    public interface ITrainingService
    {
        Task<IEnumerable<Training>> GetAllTrainingsAsync();
        Task<Training> GetTrainingByIdAsync(int id);
        Task AddTrainingAsync(Training training);
        Task UpdateTrainingAsync(Training training);
        Task DeleteTrainingAsync(int id);
    }
}
