namespace LinqExercises.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Configuration;
    using LinqExercises.Shared.Data;
    using Microsoft.EntityFrameworkCore;
    using LinqExercises.Features;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using LinqExercises.Features.Sorting;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddLinqExercisesServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Register ChinookDbContext with connection string from configuration
            var connectionString = configuration.GetConnectionString("Chinook");
            services.AddDbContext<ChinookDbContext>(options =>
                options.UseSqlite(connectionString)
            );
            // Register LinqExerciseService
            services.AddTransient<ILinqExerciseService, LinqExerciseService>();
            // Register ProjectionService
            services.AddTransient<Features.Projection.IProjectionService, Features.Projection.ProjectionService>();
            // Register FilterService
            services.AddTransient<Features.Filtering.IFilterService, Features.Filtering.FilterService>();  
            // Register SortService 
            services.TryAddTransient<ISortService, SortService>();

            return services;
        }
    }
}