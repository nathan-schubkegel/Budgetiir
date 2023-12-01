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
      public List<Envelope> Envelopes { get; set; }
    }
    
    [HttpGet("envelopes")]
    public ActionResult<GetEnvelopesPoco> GetEnvelopes()
    {
      return new GetEnvelopesPoco
      {
        Envelopes = new()
        {
          new Envelope { Name = "Auto Insurance", SavedAmount = 0m, MonthlyGoal = new MonthlyGoal { DayOfMonth = 30, GoalAmount = 100m } },
          new Envelope { Name = "Timothy Grass Purchase", SavedAmount = 100m, TargetDateGoal = new TargetDateGoal { StartDate = DateTime.Parse("2023-09-01T00:00:00Z"), TargetDate = DateTime.Parse("2024-03-01T00:00:00Z"), GoalAmount = 1500m } },
          new Envelope { Name = "Amazon Prime Subscription", SavedAmount = 50m, YearlyGoal = new YearlyGoal { DayOfYear = 100, GoalAmount = 170m } },
          new Envelope { Name = "Emergency Fund", SavedAmount = 600m, NoDeadlineGoal = new NoDeadlineGoal { GoalAmount = 1000m } },
          new Envelope { Name = "Kid's College Fund", SavedAmount = 0, NoGoal = new NoGoal() },
        }
      };
    }
  }
}
