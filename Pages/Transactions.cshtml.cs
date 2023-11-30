using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Budgetiir.Pages
{
  public class TransactionsModel : PageModel
  {
    private readonly ILogger<TransactionsModel> _logger;

    public TransactionsModel(ILogger<TransactionsModel> logger)
    {
      _logger = logger;
    }

    public void OnGet()
    {
    }
  }
}
