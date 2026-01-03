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
        _logger.LogInformation("[QuantifierService.RunQuantifierExerciseAsync] - Basic Any Example");
        var hasLongTrackTitles = _dbContext.Tracks.Any(t => t.Name!.Length > 50);
        var anyTable = new Spectre.Console.Table().Title("[green]Any Quantifier Result[/]").Border(Spectre.Console.TableBorder.Rounded)
            .AddColumn("Has Track Titles Longer Than 50 Characters");
        anyTable.AddRow(hasLongTrackTitles.ToString());
        AnsiConsole.Write(anyTable);
        AnsiConsole.MarkupLine("[bold italic]Press any key to continue to the next Quantifier example...[/]");
        Console.ReadKey();
        
        // Basic All
        _logger.LogInformation("[QuantifierService.RunQuantifierExerciseAsync] - Basic All Example");
        var allTracksHaveComposer = _dbContext.Tracks.All(t => !string.IsNullOrEmpty(t.Composer));
        var allTable = new Spectre.Console.Table().Title("[green]All Quantifier Result[/]").Border(Spectre.Console.TableBorder.Rounded)
            .AddColumn("Do All Tracks Have a Composer?");
        allTable.AddRow(allTracksHaveComposer.ToString());
        AnsiConsole.Write(allTable);
        AnsiConsole.MarkupLine("[bold italic]Press any key to continue to the next Quantifier example...[/]");
        Console.ReadKey();

        // Basic Contains
        _logger.LogInformation("[QuantifierService.RunQuantifierExerciseAsync] - Basic Contains Example");
        var trackNames = _dbContext.Tracks.Select(t => t.Name).ToList();
        var containsSpecificTrack = trackNames.Contains("Imagine");
        var containsTable = new Spectre.Console.Table().Title("[green]Contains Quantifier Result[/]").Border(Spectre.Console.TableBorder.Rounded)
            .AddColumn("Does the Track List Contain 'Imagine'?");
        containsTable.AddRow(containsSpecificTrack.ToString());
        AnsiConsole.Write(containsTable);
        AnsiConsole.MarkupLine("[bold italic]Press any key to continue to the next Quantifier example...[/]");
        Console.ReadKey();
        
        // Concat
        _logger.LogInformation("[QuantifierService.RunQuantifierExerciseAsync] - Concat Example");
        var playlist1TrackIds = _dbContext.PlaylistTracks
            .Where(pt => pt.PlaylistId == 1)
            .Select(pt => pt.TrackId);
        var playlist2TrackIds = _dbContext.PlaylistTracks   
            .Where(pt => pt.PlaylistId == 2)
            .Select(pt => pt.TrackId);
        var combinedTrackIds = playlist1TrackIds.Concat(playlist2TrackIds).Distinct();
        var concatTable = new Spectre.Console.Table().Title("[green]Combined Track IDs from Playlists 1 and 2[/]").Border(Spectre.Console.TableBorder.Rounded)
            .AddColumn("Track ID");
        combinedTrackIds.ToList().ForEach(trackId => concatTable.AddRow(trackId.ToString()));        
        Spectre.Console.AnsiConsole.Write(concatTable);        
        AnsiConsole.MarkupLine("[bold italic]Press any key to continue to the next Quantifier example...[/]");
        Console.ReadKey();
        
        // ToDictionary
        _logger.LogInformation("[QuantifierService.RunQuantifierExerciseAsync] - ToDictionary Example");
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
        AnsiConsole.MarkupLine("[bold italic]Press Enter to continue to finish Quantifier Exercise...[/]");
        Console.ReadKey();
        _logger.LogInformation("[QuantifierService.RunQuantifierExerciseAsync] - Completed quantifier exercises.");
    }
}