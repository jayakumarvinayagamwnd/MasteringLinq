using LinqExercises.Shared.Data;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;

namespace LinqExercises.Features.Partition
{
    public class PartitionService : IPartitionService
    {
        private readonly ILogger<PartitionService> _logger;
        private readonly ChinookDbContext _dbContext;
        public PartitionService(ILogger<PartitionService> logger, ChinookDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        public async Task RunPartitionExerciseAsync()
        {
            _logger.LogInformation("[PartitionService.RunPartitionExerciseAsync] - Running partition exercise.");
            // Skip, Take, SkipWhile, TakeWhile operations can be demonstrated here.
            var allTracks = await _dbContext.Tracks.Take(10).ToListAsync();
            var trackTable = new Table().Title("[green]First 10 Tracks[/]").Border(TableBorder.Rounded)
                .AddColumn("Track ID")
                .AddColumn("Track Name");
            allTracks.ForEach(track =>
                trackTable.AddRow(
                    Markup.Escape(track.TrackId.ToString()),
                    Markup.Escape(track.Name ?? "Unknown"))
            );
            AnsiConsole.Write(trackTable);
            AnsiConsole.MarkupLine("[yellow]Press any key to continue to next partition example...[/]");
            Console.ReadKey();
            var skippedTracks = await _dbContext.Tracks.Skip(5).Take(10).ToListAsync();
            var skippedTrackTable = new Table().Title("[green]Tracks after Skipping 5[/]").Border(TableBorder.Rounded)
                .AddColumn("Track ID")
                .AddColumn("Track Name");
            skippedTracks.ForEach(track =>
                skippedTrackTable.AddRow(
                    Markup.Escape(track.TrackId.ToString()),
                    Markup.Escape(track.Name ?? "Unknown"))
            );
            AnsiConsole.Write(skippedTrackTable);
            AnsiConsole.MarkupLine("[yellow]Press any key to continue to next partition example...[/]");
            Console.ReadKey();
            // EF Core cannot translate SkipWhile/TakeWhile to SQL; use a server-side range filter instead.
            var conditionalTracks = await _dbContext.Tracks
                .Where(t => t.TrackId >= 5 && t.TrackId < 15)
                .OrderBy(t => t.TrackId)
                .ToListAsync();
            var conditionalTrackTable = new Table().Title("[green]Tracks with Conditional SkipWhile and TakeWhile[/]").Border(TableBorder.Rounded)
                .AddColumn("Track ID")
                .AddColumn("Track Name");
            conditionalTracks.ForEach(track =>
                conditionalTrackTable.AddRow(
                    Markup.Escape(track.TrackId.ToString()),
                    Markup.Escape(track.Name ?? "Unknown"))
            );
            AnsiConsole.Write(conditionalTrackTable);

            AnsiConsole.MarkupLine("[yellow]Press any key to finish the Group Exercise...[/]");
            Console.ReadKey();
            _logger.LogInformation("[PartitionService.RunPartitionExerciseAsync] - Partition exercise completed.");
        }
    }
}