using LinqExercises.Shared.Data;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;

namespace LinqExercises.Features.Aggregation
{
    public class AggregationService : IAggregationService
    {
        private readonly ILogger<AggregationService> _logger;
        private readonly ChinookDbContext _dbContext;
        public AggregationService(ILogger<AggregationService> logger, ChinookDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        public async Task RunAggregationExerciseAsync()
        {
            _logger.LogInformation("[AggregationService.RunAggregationExerciseAsync] - Running aggregation exercises...");
            //Count, Sum, Min, Max, Average, and Aggregate.
            var totalTracks = _dbContext.Tracks.Count();
            var totalTrackLength = _dbContext.Tracks.Sum(t => t.Milliseconds);
            var shortestTrack = _dbContext.Tracks.Min(t => t.Milliseconds);
            var longestTrack = _dbContext.Tracks.Max(t => t.Milliseconds);
            var averageTrackLength = _dbContext.Tracks.Average(t => t.Milliseconds);
            var concatenatedTrackNames = _dbContext.Tracks
                .Select(t => t.Name)
                .AsEnumerable()
                .Aggregate((current, next) => current + ", " + next);
            var aggregationtable = new Table().Title("[green]Track Aggregations[/]").Border(TableBorder.Rounded)
                .AddColumn("Aggregation")
                .AddColumn("Value");
            aggregationtable.AddRow("Total Tracks", totalTracks.ToString());
            aggregationtable.AddRow("Total Track Length (ms)", totalTrackLength.ToString());
            aggregationtable.AddRow("Shortest Track (ms)", shortestTrack.ToString());
            aggregationtable.AddRow("Longest Track (ms)", longestTrack.ToString());
            aggregationtable.AddRow("Average Track Length (ms)", averageTrackLength.ToString());
            aggregationtable.AddRow("Concatenated Track Names", Markup.Escape(concatenatedTrackNames ?? string.Empty));
            AnsiConsole.Write(aggregationtable);    
            // Async versions of the above aggregations
            _logger.LogInformation("Press any key to continue to async aggregation examples...");
            Console.ReadKey();
            var asyncTotalTracks = await _dbContext.Tracks.CountAsync();
            var asyncTotalTrackLength = await _dbContext.Tracks.SumAsync(t => t.Milliseconds);
            var asyncShortestTrack = await _dbContext.Tracks.MinAsync(t => t.Milliseconds);
            var asyncLongestTrack = await _dbContext.Tracks.MaxAsync(t => t.Milliseconds);
            var asyncAverageTrackLength = await _dbContext.Tracks.AverageAsync(t => t.Milliseconds);
            
            // Async aggregate - concatenate track names
            var asyncConcatenatedTrackNames = await _dbContext.Tracks
                .Select(t => t.Name)
                .AsAsyncEnumerable()
                .AggregateAsync((current, next, ct) => 
                    ValueTask.FromResult<string?>((current ?? string.Empty) + ", " + (next ?? string.Empty)));
            
            var aggregationAsyncTable = new Table().Title("[green]Async Track Aggregations[/]").Border(TableBorder.Rounded)
                .AddColumn("Aggregation")
                .AddColumn("Value");
            aggregationAsyncTable.AddRow("Total Tracks", asyncTotalTracks.ToString());
            aggregationAsyncTable.AddRow("Total Track Length (ms)", asyncTotalTrackLength.ToString());
            aggregationAsyncTable.AddRow("Shortest Track (ms)", asyncShortestTrack.ToString());
            aggregationAsyncTable.AddRow("Longest Track (ms)", asyncLongestTrack.ToString()); 
            aggregationAsyncTable.AddRow("Average Track Length (ms)", asyncAverageTrackLength.ToString());
            aggregationAsyncTable.AddRow("Concatenated Track Names", Markup.Escape(asyncConcatenatedTrackNames ?? string.Empty));
            AnsiConsole.Write(aggregationAsyncTable);

            _logger.LogInformation("Press any key to finish the aggregation exercises...");
            Console.ReadKey();
            _logger.LogInformation("[AggregationService.RunAggregationExerciseAsync] - Completed aggregation exercises.");
        }
    }   
}