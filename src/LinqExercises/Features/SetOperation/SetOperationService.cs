using LinqExercises.Shared.Data;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;

namespace LinqExercises.Features.SetOperation
{
    public class SetOperationService : ISetOperationService
    {
        private readonly ILogger<SetOperationService> _logger;
        private readonly ChinookDbContext _chinookDbContext;
        public SetOperationService(
            ILogger<SetOperationService> logger,
            ChinookDbContext chinookDbContext)
        {
            _logger = logger;
            _chinookDbContext = chinookDbContext;
        }
        public async Task RunSetOperationExerciseAsync()
        {
            _logger.LogInformation("[SetOperationService.RunSetOperationExerciseAsync] - Running Set Operation Exercise...");
            // Distinct, Except, Intersect, Union Example
            var trackNames = await _chinookDbContext.Tracks
                .Select(t => t.Name)
                .Distinct()
                .ToListAsync();
            var albumTitle = await _chinookDbContext.Albums
                .DistinctBy(x=> x.Title)
                .ToListAsync();
            // Except
            var trackNamesInPlaylist = await _chinookDbContext.PlaylistTracks
                .Select(pt => pt.Track!.Name)
                .Distinct()
                .ToListAsync();
            var trackNamesNotInPlaylist = trackNames.Except(trackNamesInPlaylist).ToList();
            // Intersect
            var trackNamesInBoth = trackNames.Intersect(trackNamesInPlaylist).ToList();
            // Union
            var allTrackNames = trackNames.Union(trackNamesInPlaylist).ToList();
            var distinctTable = new Table().Title("Distinct Track Names");
            distinctTable.AddColumn("Track Name");
            foreach (var name in trackNames)
            {
                distinctTable.AddRow(name);
            }
            AnsiConsole.Write(distinctTable);
            AnsiConsole.MarkupLine("[yellow]Press any key to continue...[/]");
            Console.ReadKey();
            var exceptTable = new Table().Title("Track Names Not In Playlist");
            exceptTable.AddColumn("Track Name");
            foreach (var name in trackNamesNotInPlaylist)
            {
                exceptTable.AddRow(name);
            }
            AnsiConsole.Write(exceptTable);
            AnsiConsole.MarkupLine("[yellow]Press any key to continue...[/]");
            Console.ReadKey();
            var intersectTable = new Table().Title("Track Names In Both");
            intersectTable.AddColumn("Track Name");
            foreach (var name in trackNamesInBoth)
            {
                intersectTable.AddRow(name);
            }
            AnsiConsole.Write(intersectTable);
            AnsiConsole.MarkupLine("[yellow]Press any key to continue...[/]");
            Console.ReadKey();
            var unionTable = new Table().Title("All Track Names (Union)");
            unionTable.AddColumn("Track Name");
            foreach (var name in allTrackNames)
            {
                unionTable.AddRow(name);
            }
            AnsiConsole.Write(unionTable);
            AnsiConsole.MarkupLine("[yellow]Press any key to finish the Set Operation Exercise...[/]");
            Console.ReadKey();
            _logger.LogInformation("[SetOperationService.RunSetOperationExerciseAsync] - Set Operation Exercise Completed.");
        }
    }
}