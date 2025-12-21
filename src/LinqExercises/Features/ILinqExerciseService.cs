using Microsoft.Extensions.Logging;

namespace LinqExercises.Features
{
    public interface ILinqExerciseService
    {
        Task RunAllExercises();
    }

    public class LinqExerciseService : ILinqExerciseService
    {
        private readonly ILogger<LinqExerciseService> _logger;
        private readonly Features.Projection.IProjectionService _projectionService;
        public LinqExerciseService( ILogger<LinqExerciseService> logger, Features.Projection.IProjectionService projectionService)
        {
            _logger = logger;
            _projectionService = projectionService;
        }
        public async Task RunAllExercises()
        {
            // Implementation of exercises would go here
            _logger.LogInformation("[LinqExerciseService.RunAllExercises] - Running all LINQ exercises...");
            await _projectionService.RunProjectionExerciseAsync();
            _logger.LogInformation("[LinqExerciseService.RunAllExercises] - Completed all LINQ exercises.");
        }
    }
}