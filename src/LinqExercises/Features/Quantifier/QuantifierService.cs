using LinqExercises.Shared.Data;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace LinqExercises.Features.Quantifier;

public class QuantifierService : IQuantifierService
{
    private readonly ILogger<IQuantifierService> _logger;
    private readonly ChinookDbContext _dbContext;
    public QuantifierService(ILogger<IQuantifierService> logger, ChinookDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
    public async Task RunQuantifierExerciseAsync()
    {
        _logger.LogInformation("[QuantifierService.RunQuantifierExerciseAsync] - Running quantifier exercises...");
        // Basic Any
        var hasLongTrackTitles = _dbContext.Tracks.Any(t => t.Name!.Length > 50);
        _logger.LogInformation($"Are there any tracks with titles longer than 50 characters? {hasLongTrackTitles}");
        // Basic All
        var allTracksHaveComposer = _dbContext.Tracks.All(t => !string.IsNullOrEmpty(t.Composer));
        _logger.LogInformation($"Do all tracks have a composer? {allTracksHaveComposer}");
        // Basic Contains
        var trackNames = _dbContext.Tracks.Select(t => t.Name).ToList();
        var containsSpecificTrack = trackNames.Contains("Imagine");
        _logger.LogInformation($"Does the track list contain 'Imagine'? {containsSpecificTrack}");
        // Concat
        var playlist1TrackIds = _dbContext.PlaylistTracks
            .Where(pt => pt.PlaylistId == 1)
            .Select(pt => pt.TrackId);
        var playlist2TrackIds = _dbContext.PlaylistTracks   
            .Where(pt => pt.PlaylistId == 2)
            .Select(pt => pt.TrackId);
        var combinedTrackIds = playlist1TrackIds.Concat(playlist2TrackIds).Distinct();
        _logger.LogInformation($"Combined track IDs from playlists 1 and 2 contains {combinedTrackIds.Count()} unique tracks.");

        _logger.LogInformation("Press any key to continue to the next quantifier example..."); 
        Console.ReadKey();
        // ToDictionary
        var trackDictionary = _dbContext.Tracks
            .Where(t => combinedTrackIds.Contains(t.TrackId))
            .ToDictionary(t => t.TrackId, t => t.Name);
        _logger.LogInformation($"Combined track dictionary from playlists 1 and 2 contains {trackDictionary.Count} tracks.");
        var trackDictionaryTable = new Spectre.Console.Table().Title("[green]Combined Track Dictionary[/]").Border(Spectre.Console.TableBorder.Rounded)
            .AddColumn("Track ID")
            .AddColumn("Track Name");
        var trackKeys = trackDictionary.Keys;
        foreach (var key in trackKeys)
        {
            trackDictionaryTable.AddRow(key.ToString(), Spectre.Console.Markup.Escape(trackDictionary[key] ?? string.Empty));
        }
        Spectre.Console.AnsiConsole.Write(trackDictionaryTable);
        _logger.LogInformation("Press any key to finish the quantifier exercises...");
        Console.ReadKey();
        _logger.LogInformation("[QuantifierService.RunQuantifierExerciseAsync] - Completed quantifier exercises.");
    }
}