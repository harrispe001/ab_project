using ab_project.Models;

namespace ab_project.Repositories
{
    public interface ITrainingRepository
    {
        Task<IEnumerable<Training>> GetAllTrainingsAsync();
        // Add other methods as needed
    }
}
