using LinqExercises.Shared.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace LinqExercises.Features.AdvancedLINQPattern
{
    public class AdvancedLINQPatternService : IAdvancedLINQPatternService
    {
        private readonly ILogger<AdvancedLINQPatternService> _logger;
        private readonly ChinookDbContext _dbContext;
        public AdvancedLINQPatternService(ILogger<AdvancedLINQPatternService> logger, ChinookDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        public async Task RunAdvancedLINQPatternExerciseAsync()
        {
            _logger.LogInformation("[AdvancedLINQPattern.RunAdvancedLINQPatternExerciseAsync] - Running advanced LINQ pattern exercise.");
            // Implement advanced LINQ patterns here.
            // Example: Using SelectMany to flatten collections.
            var playlistsWithTracks = await _dbContext.Playlists
                .Include(p => p.PlaylistTracks)
                .ThenInclude(pt => pt.Track)
                .ToListAsync();
            var trackTable = new Table().Title("[green]Playlists and their Tracks[/]").Border(TableBorder.Rounded)
                .AddColumn("Playlist Name")
                .AddColumn("Track Name");
            foreach (var playlist in playlistsWithTracks)
            {
                foreach (var track in playlist.PlaylistTracks.Select(pt => pt.Track))
                {
                    trackTable.AddRow(
                        Markup.Escape(playlist.Name ?? "Unknown"),
                        Markup.Escape(track!.Name ?? "Unknown"));
                }
            }
            AnsiConsole.Write(trackTable);
            AnsiConsole.MarkupLine("[yellow]Press any key to continue to next Advanced LINQ Pattern example...[/]");
            Console.ReadKey();
            // Example: Using Parallel LINQ (PLINQ) for parallel processing
            var tracks = await _dbContext.Tracks.ToListAsync();
            var parallelGrouped = tracks
                .AsParallel()
                .Where(t => t.Milliseconds > 180000)
                .GroupBy(t => t.Album?.Title ?? "Unknown")
                .OrderByDescending(g => g.Count())
                .Take(10)
                .ToList();

            var parallelTable = new Table().Title("[blue]Top 10 Albums by Long Tracks (Parallel)[/]").Border(TableBorder.Rounded)
                .AddColumn("Album")
                .AddColumn("Track Count");
            parallelGrouped.ForEach(g => parallelTable.AddRow(Markup.Escape(g.Key), g.Count().ToString()));
            AnsiConsole.Write(parallelTable);
            AnsiConsole.MarkupLine("[yellow]Press any key to continue to next Advanced LINQ Pattern example...[/]");
            Console.ReadKey();
            // Complex Joins and Grouping
            var complexJoin = _dbContext.PlaylistTracks
                .Join(_dbContext.Tracks, pt => pt.TrackId, t => t.TrackId, (pt, t) => t)
                .Join(_dbContext.Albums, t => t.AlbumId, a => a.AlbumId, (t, a) => new { t, a })
                .GroupBy(x => x.a.Title)
                .OrderByDescending(g => g.Count())
                .Select(g => new
                {
                    AlbumTitle = g.Key,
                    TrackCount = g.Count()
                });
            var complexJoinTable = new Table().Title("[blue]Complex Join and Grouping Results[/]").Border(TableBorder.Rounded)
                .AddColumn("Album Title")
                .AddColumn("Track Count");
            complexJoin.ToList().ForEach(result => complexJoinTable.AddRow(Markup.Escape(result.AlbumTitle!), result.TrackCount.ToString()));
            AnsiConsole.Write(complexJoinTable);
            // AsNoTracking()
            AnsiConsole.MarkupLine("[yellow]Press any key to continue to next Advanced LINQ Pattern example...[/]");
            Console.ReadKey();
            var noTrackingQuery = _dbContext.Albums.AsNoTracking();
            var noTrackingTable = new Table().Title("[blue]Albums with AsNoTracking[/]").Border(TableBorder.Rounded)
                .AddColumn("Album ID")
                .AddColumn("Album Title");
            noTrackingQuery.ToList().ForEach(album =>
                noTrackingTable.AddRow(
                    Markup.Escape(album.AlbumId.ToString()),
                    Markup.Escape(album.Title ?? "Unknown")));
            AnsiConsole.Write(noTrackingTable);

            AnsiConsole.MarkupLine("[yellow]Press any key to finish the Advanced LINQ Pattern Exercise...[/]");
            Console.ReadKey();
            _logger.LogInformation("[AdvancedLINQPattern.RunAdvancedLINQPatternExerciseAsync] - Completed advanced LINQ pattern exercise.");
        }
    }
}