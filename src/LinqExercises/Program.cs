// Configure the logger from appsettings.json and demonstrate simple DI usage
using Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using LinqExercises.Extensions;
using Microsoft.Extensions.DependencyInjection.Extensions;
using LinqExercises.Features;

// Build configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();
// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)    
    .CreateLogger();
// Setup Dependency Injection
var services = new ServiceCollection();
services.TryAddSingleton(Log.Logger);
services.TryAddSingleton<IConfiguration>(configuration);
// Register Microsoft.Extensions.Logging so ILogger<T> can be resolved
// Route Microsoft.Extensions.Logging `ILogger<T>` to Serilog
services.AddLogging(builder => builder.ClearProviders().AddSerilog(Log.Logger, dispose: false));
services.AddLinqExercisesServices(configuration);

try
{
    // Run the LINQ exercises
    using var provider = services.BuildServiceProvider();
    var linqExerciseService = provider.GetRequiredService<ILinqExerciseService>();
    await linqExerciseService.RunAllExercises();
}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandled exception occurred");
}
finally
{
    Log.CloseAndFlush();
}