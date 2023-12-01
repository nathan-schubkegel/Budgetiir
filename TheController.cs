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
      public string Name { get; set; }
      public string SavedAmount { get; set; }
      public string GoalAmount { get; set; }
      public string GoalDate { get; set; }
    }
    
    [HttpGet("envelopes")]
    public ActionResult<GetEnvelopesPoco> GetEnvelopes()
    {
      return new GetEnvelopesPoco
      {
        Envelopes = new()
        {
          new EnvelopePoco { Name = "Animal Feed", SavedAmount = "100", GoalAmount = "500", GoalDate = DateTime.Now.AddDays(1 - DateTime.Now.Day) /* first day of month */ .AddDays(14) /* fifteenth day */ .ToString("o") },
          new EnvelopePoco { Name = "New Tooth", SavedAmount = "0", GoalAmount = "2000", GoalDate = DateTime.Now.AddDays(1 - DateTime.Now.Day) /* first day of month */ .AddDays(27) /* 28th day */ .ToString("o") },
          new EnvelopePoco { Name = "Corn Dogs", SavedAmount = "5", GoalAmount = "11.75", GoalDate = DateTime.Now.ToString("o") },
        }
      };
    }
  }
}
