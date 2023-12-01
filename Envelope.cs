using System;
using Newtonsoft.Json;

namespace Budgetiir
{
  // yo - don't forget this class is totally JSON serialized/deserialized to disk
  public class Envelope
  {
    public string Name { get; set; }
    public Decimal SavedAmount { get; set; }
    
    [JsonIgnore]
    public ISavingGoal Goal => _goal;
    private ISavingGoal _goal;
    public MonthlyGoal MonthlyGoal { get => _goal as MonthlyGoal; set => _goal = value ?? _goal; }
    public YearlyGoal YearlyGoal { get => _goal as YearlyGoal; set => _goal = value ?? _goal; }
    public TargetDateGoal TargetDateGoal { get => _goal as TargetDateGoal; set => _goal = value ?? _goal; }
    public NoDeadlineGoal NoDeadlineGoal { get => _goal as NoDeadlineGoal; set => _goal = value ?? _goal; }
    public NoGoal NoGoal { get => _goal as NoGoal; set => _goal = value ?? _goal; }
  }

  public interface ISavingGoal
  {
    DateTime? GetDateNeededThisBudgetCycle(DateTime budgetCycleStart, DateTime nextBudgetCycleStart);
    Decimal? GetTargetAmountThisBudgetCycle(DateTime budgetCycleStart, DateTime nextBudgetCycleStart);
  }

  public class MonthlyGoal : ISavingGoal
  {
    public int DayOfMonth { get; set; }
    public Decimal GoalAmount { get; set; }
    
    public DateTime? GetDateNeededThisBudgetCycle(DateTime budgetCycleStart, DateTime nextBudgetCycleStart)
    {
      return ToTargetDateGoal(budgetCycleStart, nextBudgetCycleStart)
        .GetDateNeededThisBudgetCycle(budgetCycleStart, nextBudgetCycleStart);
    }
    
    public Decimal? GetTargetAmountThisBudgetCycle(DateTime budgetCycleStart, DateTime nextBudgetCycleStart)
    {
      return ToTargetDateGoal(budgetCycleStart, nextBudgetCycleStart)
        .GetTargetAmountThisBudgetCycle(budgetCycleStart, nextBudgetCycleStart);
    }

    private TargetDateGoal ToTargetDateGoal(DateTime budgetCycleStart, DateTime nextBudgetCycleStart)
    {
      // figure out when the money is due
      var targetDate = new DateTime(budgetCycleStart.Year, budgetCycleStart.Month, DayOfMonth);
      if (targetDate < budgetCycleStart)
      {
        targetDate = new DateTime(nextBudgetCycleStart.Year, nextBudgetCycleStart.Month, DayOfMonth);
      }
      if (targetDate < budgetCycleStart)
      {
        targetDate = targetDate.AddDays(31); // not perfect, but good enough
      }

      // for simplicity, assume we've been saving since last month
      var startDate = targetDate - TimeSpan.FromDays(31);
      return new TargetDateGoal
      {
        StartDate = startDate,
        TargetDate = targetDate,
        GoalAmount = GoalAmount,
      };
    }
  }

  public class YearlyGoal : ISavingGoal
  {
    public DateTime StartDate { get; set; } // when did we first set this goal?
    public int DayOfYear { get; set; } // 1-based indexing
    public Decimal GoalAmount { get; set; }

    public DateTime? GetDateNeededThisBudgetCycle(DateTime budgetCycleStart, DateTime nextBudgetCycleStart)
    {
      return ToTargetDateGoal(budgetCycleStart, nextBudgetCycleStart)
        .GetDateNeededThisBudgetCycle(budgetCycleStart, nextBudgetCycleStart);
    }
    
    public Decimal? GetTargetAmountThisBudgetCycle(DateTime budgetCycleStart, DateTime nextBudgetCycleStart)
    {
      return ToTargetDateGoal(budgetCycleStart, nextBudgetCycleStart)
        .GetTargetAmountThisBudgetCycle(budgetCycleStart, nextBudgetCycleStart);
    }

    private TargetDateGoal ToTargetDateGoal(DateTime budgetCycleStart, DateTime nextBudgetCycleStart)
    {
      // figure out when the money is due.           year, month, day    1-based indexing
      var targetDate = new DateTime(budgetCycleStart.Year, 1, 1).AddDays(DayOfYear - 1); 
      if (targetDate < budgetCycleStart) 
      {
        targetDate = new DateTime(budgetCycleStart.Year + 1, 1, 1).AddDays(DayOfYear - 1); 
      }
      
      // figure out when we started saving
      // so if we started with only 6 months to save, then we save 2x as fast monthly
      var lastYear = targetDate - TimeSpan.FromDays(365);
      var startDate = StartDate >= lastYear ? StartDate : lastYear;
      
      return new TargetDateGoal
      {
        StartDate = startDate,
        TargetDate = targetDate,
        GoalAmount = GoalAmount,
      };
    }
  }

  public class TargetDateGoal : ISavingGoal
  {
    public DateTime StartDate { get; set; }
    public DateTime TargetDate { get; set; }
    public Decimal GoalAmount { get; set; }
    
    public DateTime? GetDateNeededThisBudgetCycle(DateTime budgetCycleStart, DateTime nextBudgetCycleStart)
    {
      if (TargetDate <= nextBudgetCycleStart)
      {
        if (TargetDate >= budgetCycleStart)
        {
          // TargetDate is in this budget cycle
          return TargetDate;
        }
        else // weird? target date is before the budget cycle?
        {
          return null;
        }
      }
      else // target date is beyond this budget cycle
      {
        return nextBudgetCycleStart - TimeSpan.FromDays(1); // assumes all budget cycles are at least a few days
      }
    }
    
    public Decimal? GetTargetAmountThisBudgetCycle(DateTime budgetCycleStart, DateTime nextBudgetCycleStart)
    {
      if (TargetDate <= nextBudgetCycleStart)
      {
        if (TargetDate >= budgetCycleStart)
        {
          // TargetDate is in this budget cycle
          return GoalAmount;
        }
        else // weird? target date is before the budget cycle?
        {
          return null;
        }
      }
      else // target date is beyond this budget cycle
      {
        var thisCycleDate = nextBudgetCycleStart - TimeSpan.FromDays(1); // assumes all budget cycles are at least a few days
        var dayCount = Math.Round((thisCycleDate - StartDate).TotalDays, MidpointRounding.AwayFromZero);
        var totalDayCount = Math.Round((TargetDate - StartDate).TotalDays, MidpointRounding.AwayFromZero);
        return (GoalAmount * (decimal)(dayCount / totalDayCount)).SetMaxDecimals(2);
      }
    }
  }

  // this one is for emergency funds
  public class NoDeadlineGoal : ISavingGoal
  {
    public Decimal GoalAmount { get; set; }

    public DateTime? GetDateNeededThisBudgetCycle(DateTime budgetCycleStart, DateTime nextBudgetCycleStart) => null;
    public Decimal? GetTargetAmountThisBudgetCycle(DateTime budgetCycleStart, DateTime nextBudgetCycleStart) => GoalAmount;
  }

  // this one is where excess money goes before it goes to stocks/crypto
  public class NoGoal : ISavingGoal
  {
    public DateTime? GetDateNeededThisBudgetCycle(DateTime budgetCycleStart, DateTime nextBudgetCycleStart) => null;
    public Decimal? GetTargetAmountThisBudgetCycle(DateTime budgetCycleStart, DateTime nextBudgetCycleStart) => null;
  }
}