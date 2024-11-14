using ab_project.Models;

namespace ab_project.Services
{
    public interface ITrainingService
    {
        Task<IEnumerable<Training>> GetAllTrainingsAsync();
        // Add other methods as needed
    }
}
