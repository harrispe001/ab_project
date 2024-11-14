using ab_project.Models;
using ab_project.Repositories;

namespace ab_project.Services
{
    public class TrainingService : ITrainingService
    {
        private readonly ITrainingRepository _trainingRepository;

        public TrainingService(ITrainingRepository trainingRepository)
        {
            _trainingRepository = trainingRepository;
        }

        public async Task<IEnumerable<Training>> GetAllTrainingsAsync()
        {
            return await _trainingRepository.GetAllTrainingsAsync();
        }

        // Implement other methods as needed
    }
}

    