using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace Budgetiir
{
  [ApiController]
  [Route("api")]
  public class TheController : ControllerBase
  {
    private readonly ILogger<TheController> _logger;

    public TheController(ILogger<TheController> logger)
    {
      _logger = logger;
    }
    
    public class GetEnvelopesPoco
    {
      public List<EnvelopePoco> Envelopes { get; set; }
    }
    
    public class EnvelopePoco
    {
      private DateTime _budgetCycleStart;
      private DateTime _nextBudgetCycleStart;
      private Envelope _envelope;
      
      public EnvelopePoco(DateTime budgetCycleStart, DateTime nextBudgetCycleStart, Envelope envelope)
      {
        _budgetCycleStart = budgetCycleStart;
        _nextBudgetCycleStart = nextBudgetCycleStart;
        _envelope = envelope;
      }
      
      public string Name => _envelope.Name;
      public string SavedAmount => _envelope.SavedAmount.ToString();
      public string GoalAmount => _envelope.Goal.GetTargetAmountThisBudgetCycle(_budgetCycleStart, _nextBudgetCycleStart)?.ToString() ?? "";
      public string GoalDate => _envelope.Goal.GetDateNeededThisBudgetCycle(_budgetCycleStart, _nextBudgetCycleStart)?.ToString("o") ?? "";
    }
    
    [HttpGet("envelopes")]
    public ActionResult<GetEnvelopesPoco> GetEnvelopes()
    {
      var now = DateTime.Now.ToUniversalTime();
      var budgetCycleStart = new DateTime(now.Year, now.Month, 1, 0, 0, 0, 0, DateTimeKind.Utc);
      var nextBudgetCycleStart = new DateTime(now.Month == 12 ? now.Year + 1 : now.Year, now.Month == 12 ? 1 : now.Month + 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
      return new GetEnvelopesPoco
      {
        Envelopes = new []
        {
          new Envelope { Name = "Auto Insurance", SavedAmount = 0m, MonthlyGoal = new MonthlyGoal { DayOfMonth = 30, GoalAmount = 100m } },
          new Envelope { Name = "Timothy Grass Purchase", SavedAmount = 100m, TargetDateGoal = new TargetDateGoal { StartDate = DateTime.Parse("2023-09-01T00:00:00Z"), TargetDate = DateTime.Parse("2024-03-01T00:00:00Z"), GoalAmount = 1500m } },
          new Envelope { Name = "Amazon Prime Subscription", SavedAmount = 50m, YearlyGoal = new YearlyGoal { DayOfYear = 100, GoalAmount = 170m } },
          new Envelope { Name = "Emergency Fund", SavedAmount = 600m, NoDeadlineGoal = new NoDeadlineGoal { GoalAmount = 1000m } },
          new Envelope { Name = "Kid's College Fund", SavedAmount = 0, NoGoal = new NoGoal() },
        }.Select(x => new EnvelopePoco(budgetCycleStart, nextBudgetCycleStart, x)).ToList()
      };
    }
  }
}
