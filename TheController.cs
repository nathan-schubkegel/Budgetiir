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
  }
}
