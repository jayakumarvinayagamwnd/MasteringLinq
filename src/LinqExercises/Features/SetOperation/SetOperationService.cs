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
            // Distinct
            _logger.LogInformation("[SetOperationService.RunSetOperationExerciseAsync] - Distinct Example");    
            var customerNames = await _chinookDbContext.Customers
                .Select(c => c.FirstName)
                .Distinct()
                .ToListAsync();
            var distinctTable = new Table().Title("Distinct Customer First Names");
            distinctTable.AddColumn("First Name");
            customerNames.ForEach(name => distinctTable.AddRow(Markup.Escape(name!)));
            AnsiConsole.Write(distinctTable);
            AnsiConsole.MarkupLine($"[bold yellow]Results: {customerNames.Count}[/]");
            AnsiConsole.MarkupLine("[bold italic]Press any key to continue to the next Set Operation example...[/]");
            Console.ReadKey();

            // Except Example
            _logger.LogInformation("[SetOperationService.RunSetOperationExerciseAsync] - Except Example");
            var trackIdsInPlaylist1 = await _chinookDbContext.PlaylistTracks
                .Where(pt => pt.PlaylistId == 1)
                .Select(pt => pt.TrackId)
                .ToListAsync();
            var trackIdsInPlaylist2 = await _chinookDbContext.PlaylistTracks
                .Where(pt => pt.PlaylistId == 2)
                .Select(pt => pt.TrackId)
                .ToListAsync();
            var exceptTrackIds = trackIdsInPlaylist1.Except(trackIdsInPlaylist2).ToList();
            var exceptTable = new Table().Title("Tracks in Playlist 1 but not in Playlist 2");
            exceptTable.AddColumn("Track ID");
            exceptTrackIds.ForEach(id => exceptTable.AddRow(Markup.Escape(id.ToString())));
            AnsiConsole.Write(exceptTable);
            AnsiConsole.MarkupLine($"[bold yellow]Results: {exceptTrackIds.Count}[/]");
            AnsiConsole.MarkupLine("[bold italic]Press any key to continue to the next Set Operation example...[/]");
            Console.ReadKey();
            
            // Intersect Example
            _logger.LogInformation("[SetOperationService.RunSetOperationExerciseAsync] - Intersect Example");
            var intersectTrackIds = trackIdsInPlaylist1.Intersect(trackIdsInPlaylist2).ToList();
            var intersectTable = new Table().Title("Tracks in both Playlist 1 and Playlist 2");
            intersectTable.AddColumn("Track ID");
            intersectTrackIds.ForEach(id => intersectTable.AddRow(Markup.Escape(id.ToString())));
            AnsiConsole.Write(intersectTable);
            AnsiConsole.MarkupLine($"[bold yellow]Results: {intersectTrackIds.Count}[/]");
            AnsiConsole.MarkupLine("[bold italic]Press any key to continue to the next Set Operation example...[/]");
            Console.ReadKey();
            
            // Union Example
            _logger.LogInformation("[SetOperationService.RunSetOperationExerciseAsync] - Union Example");
            var unionTrackIds = trackIdsInPlaylist1.Union(trackIdsInPlaylist2).ToList();
            var unionTable = new Table().Title("All Unique Tracks in Playlist 1 and Playlist 2");
            unionTable.AddColumn("Track ID");
            unionTrackIds.ForEach(id => unionTable.AddRow(Markup.Escape(id.ToString())));
            AnsiConsole.Write(unionTable);
            AnsiConsole.MarkupLine($"[bold yellow]Results: {unionTrackIds.Count}[/]");
            AnsiConsole.MarkupLine("[bold italic]Press Enter to continue to finish Set Operation Exercise...[/]");
            Console.ReadKey();
            _logger.LogInformation("[SetOperationService.RunSetOperationExerciseAsync] - Set Operation Exercise Completed.");
        }
    }
}