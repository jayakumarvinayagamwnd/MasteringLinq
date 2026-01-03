using System;
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
        private readonly Features.Quantifier.IQuantifierService _quantifierService;
        private readonly Features.Aggregation.IAggregationService _aggregationService;
        private readonly Features.ElementOperator.IElementOperator _elementOperatorService;
        private readonly Features.SetOperation.ISetOperationService _setOperationService;
        private readonly Features.Grouping.IGroupService _groupService;
        private readonly Features.Partition.IPartitionService _partitionService;
        private readonly Features.AdvancedLINQPattern.IAdvancedLINQPatternService _advancedLINQPatternService;
        public LinqExerciseService(ILogger<LinqExerciseService> logger,
            Features.Projection.IProjectionService projectionService,
            Features.Filtering.IFilterService filterService,
            Features.Sorting.ISortService sortService,
            Features.Joining.IJoinService joinService,
            Features.Quantifier.IQuantifierService quantifierService,
            Features.Aggregation.IAggregationService aggregationService,
            Features.ElementOperator.IElementOperator elementOperatorService,
            Features.SetOperation.ISetOperationService setOperationService,
            Features.Grouping.IGroupService groupService,
            Features.Partition.IPartitionService partitionService,
            Features.AdvancedLINQPattern.IAdvancedLINQPatternService advancedLINQPatternService)
        {
            _logger = logger;
            _projectionService = projectionService;
            _filterService = filterService;
            _sortService = sortService;
            _joinService = joinService;
            _quantifierService = quantifierService;
            _aggregationService = aggregationService;
            _elementOperatorService = elementOperatorService;
            _setOperationService = setOperationService;
            _groupService = groupService;
            _partitionService = partitionService;
            _advancedLINQPatternService = advancedLINQPatternService;
        }
        public async Task RunAllExercises()
        {
            _logger.LogInformation("[LinqExerciseService.RunAllExercises] - Running all LINQ exercises...");
            int choice = 12;
            var envChoice = Environment.GetEnvironmentVariable("LINQ_CHOICE");
            var hasForcedChoice = int.TryParse(envChoice, out var forcedChoice);
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
                optiontable.AddRow("7", "Sorting Exercises");
                optiontable.AddRow("8", "Quantifiers Exercises");
                optiontable.AddRow("9", "Partitioning Exercises");
                optiontable.AddRow("10", "Element Operators Exercises");
                optiontable.AddRow("11", "Advanced LINQ Patterns");
                optiontable.AddRow("12", "Exit");
                AnsiConsole.Write(optiontable);
                choice = AnsiConsole.Ask<int>("[yellow]Please select the exercise number to run?[/]", hasForcedChoice ? forcedChoice : 12);
                
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
                    case 4:
                        await _groupService.RunGroupingExerciseAsync();
                        break;
                    case 5:
                        await _aggregationService.RunAggregationExerciseAsync();
                        break;
                    case 6:
                        await _setOperationService.RunSetOperationExerciseAsync();
                        break;
                    case 7:
                        await _sortService.RunSortingExerciseAsync();
                        break;
                    case 8:
                        await _quantifierService.RunQuantifierExerciseAsync();
                        break;
                    case 9:
                        await _partitionService.RunPartitionExerciseAsync();
                        break;
                    case 10:
                        await _elementOperatorService.RunElementOperatorExerciseAsync();
                        break;
                    case 11:
                        await _advancedLINQPatternService.RunAdvancedLINQPatternExerciseAsync();
                        break;
                    default:
                        _logger.LogInformation("Exiting LINQ Exercises.");
                        break;
                }

                // If we were given a forced choice, run it once and exit.
                if (hasForcedChoice)
                {
                    break;
                }
            } while (choice < 11);
            
            _logger.LogInformation("[LinqExerciseService.RunAllExercises] - Completed all LINQ exercises.");
        }
    }
}