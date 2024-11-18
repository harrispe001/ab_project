using ab_project.Models;
using ab_project.Repositories;

namespace ab_project.Services
{
    public class TrainingService : ITrainingService
    {
        private readonly ITrainingRepository _repository;

        public TrainingService(ITrainingRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Training>> GetAllTrainingsAsync()
        {
            return await _repository.GetAllTrainingsAsync();
        }

        public async Task<Training> GetTrainingByIdAsync(int id)
        {
            return await _repository.GetTrainingByIdAsync(id);
        }

        public async Task AddTrainingAsync(Training training)
        {
            await _repository.AddTrainingAsync(training);
        }

        public async Task UpdateTrainingAsync(Training training)
        {
            await _repository.UpdateTrainingAsync(training);
        }

        public async Task DeleteTrainingAsync(int id)
        {
            await _repository.DeleteTrainingAsync(id);
        }
    }
}

    