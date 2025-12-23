using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace LinqExercises.Features
{
    public class LinqExerciseService : ILinqExerciseService
    {
        private readonly ILogger<LinqExerciseService> _logger;
        private readonly Features.Projection.IProjectionService _projectionService;
        private readonly Features.Filtering.IFilterService _filterService;
        private readonly Features.Sorting.ISortService _sortService;
        private readonly Features.Joining.IJoinService _joinService;
        public LinqExerciseService( ILogger<LinqExerciseService> logger, Features.Projection.IProjectionService projectionService, 
            Features.Filtering.IFilterService filterService, Features.Sorting.ISortService sortService, Features.Joining.IJoinService joinService )
        {
            _logger = logger;
            _projectionService = projectionService;
            _filterService = filterService;
            _sortService = sortService;
            _joinService = joinService;
        }
        public async Task RunAllExercises()
        {
            // Implementation of exercises would go here
            _logger.LogInformation("[LinqExerciseService.RunAllExercises] - Running all LINQ exercises...");
            int choice = 14;
            do
            {
                var optiontable = new Table().Title("[green]Linq Exercise Options[/]").Border(TableBorder.Rounded)
                    .AddColumn("Option Number").AddColumn("Exercise Type");
                optiontable.AddRow("1", "Projection Exercises");
                optiontable.AddRow("2", "Filtering Exercises");
                optiontable.AddRow("3", "Joining Exercises");
                optiontable.AddRow("4", "Grouping Exercises");
                optiontable.AddRow("5", "Aggregation Exercises");
                optiontable.AddRow("6", "Set Operations Exercises");
                optiontable.AddRow("7", "Ordering Exercises");
                optiontable.AddRow("8", "Quantifiers Exercises");
                optiontable.AddRow("9", "Partitioning Exercises");
                optiontable.AddRow("10", "Element Operators Exercises");
                optiontable.AddRow("11", "Conversion Exercises");
                optiontable.AddRow("12", "Generation Exercises");
                optiontable.AddRow("13", "Miscellaneous Exercises");
                optiontable.AddRow("14", "Sorting Exercises");
                optiontable.AddRow("0", "Exit");
                AnsiConsole.Write(optiontable);
                choice = AnsiConsole.Ask<int>("Please select the exercise number to run?");
                switch (choice)
                {
                    case 1:
                        await _projectionService.RunProjectionExerciseAsync();
                        break;
                    case 2:
                        await _filterService.RunFilteringExerciseAsync();
                        break;
                    case 3:
                        await _joinService.RunJoiningExerciseAsync();
                        break;
                    case 14:
                        await _sortService.RunSortingExerciseAsync();
                        break;
                    default:
                        _logger.LogInformation("Exiting LINQ Exercises.");
                        break;
                }
            }while(choice<15);
            
            _logger.LogInformation("[LinqExerciseService.RunAllExercises] - Completed all LINQ exercises.");
        }
    }
}