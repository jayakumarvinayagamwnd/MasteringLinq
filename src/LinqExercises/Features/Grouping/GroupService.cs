using LinqExercises.Shared.Data;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Microsoft.EntityFrameworkCore;

namespace LinqExercises.Features.Grouping
{
    public class GroupService : IGroupService
    {
        private readonly ILogger<GroupService> _logger;
        private readonly ChinookDbContext _dbContext;
        public GroupService(ILogger<GroupService> logger, ChinookDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }   
        public async Task RunGroupingExerciseAsync()
        {
            _logger.LogInformation("[GroupService.RunGroupingExerciseAsync] - Running grouping exercises...");
            // Example grouping query
            var genreTrackCounts = await _dbContext.Tracks
                .GroupBy(track => track.GenreId)
                .Select(group => new
                {
                    GenreId = group.Key,
                    TrackCount = group.Count()
                })
                .ToListAsync();
            var genreTrackTable = new Table().Title("[green]Genre - Track Count Grouping[/]").Border(TableBorder.Rounded)
                .AddColumn("Genre ID")
                .AddColumn("Track Count");
            genreTrackCounts.ForEach(item =>
                genreTrackTable.AddRow(Markup.Escape(item.GenreId.ToString()), Markup.Escape(item.TrackCount.ToString()))
            );
            AnsiConsole.Write(genreTrackTable);
            AnsiConsole.MarkupLine($"[bold yellow]Results: {genreTrackCounts.Count}[/]");
            AnsiConsole.MarkupLine("[yellow]Press any key to continue to the next Group example...[/]");
            Console.ReadKey();
            // GroupBy - Multiple Criteria Example
            var artistAlbumCounts = await _dbContext.Albums
                .Include(a => a.Artist)
                .GroupBy(a => new { a.ArtistId, a.Artist!.Name })
                .Select(g => new
                {
                    ArtistId = g.Key.ArtistId,
                    ArtistName = g.Key.Name,
                    AlbumCount = g.Count()
                })
                .ToListAsync();
            var artistAlbumTable = new Table().Title("[green]Artist - Album Count Grouping[/]").Border(TableBorder.Rounded)
                .AddColumn("Artist ID")
                .AddColumn("Artist Name")
                .AddColumn("Album Count");
            artistAlbumCounts.ForEach(item =>
                artistAlbumTable.AddRow(
                    Markup.Escape(item.ArtistId.ToString()),
                    Markup.Escape(item.ArtistName ?? "Unknown"),
                    Markup.Escape(item.AlbumCount.ToString()))
            );
            AnsiConsole.Write(artistAlbumTable);
            AnsiConsole.MarkupLine($"[bold yellow]Results: {artistAlbumCounts.Count}[/]");
            AnsiConsole.MarkupLine("[yellow]Press any key to continue to the next Group example...[/]");
            Console.ReadKey();
            // GroupBy - Batching Example (use server-side Count so EF Core can translate)
            var batchNumber = 5;
            var tracksByAlbum = await _dbContext.Tracks
                .GroupBy(t => t.AlbumId)
                .Select(g => new
                {
                    AlbumId = g.Key,
                    TrackCount = g.Count()
                })
                .Where(g => g.TrackCount >= batchNumber)
                .ToListAsync();
            var tracksByAlbumTable = new Table().Title("[green]Album - Tracks Grouping[/]").Border(TableBorder.Rounded)
                .AddColumn("Album ID")
                .AddColumn("Track Count");
            tracksByAlbum.ForEach(item =>
                tracksByAlbumTable.AddRow(
                    Markup.Escape(item.AlbumId.ToString()),
                    Markup.Escape(item.TrackCount.ToString()))
            );
            AnsiConsole.Write(tracksByAlbumTable);
            AnsiConsole.MarkupLine($"[bold yellow]Results: {tracksByAlbum.Count}[/]");

            AnsiConsole.MarkupLine("[yellow]Press any key to finish the Group Exercise...[/]");
            Console.ReadKey();
            _logger.LogInformation("[GroupService.RunGroupingExerciseAsync] - Group Exercise Completed.");
        }
    }
}