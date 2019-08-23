using System;
using System.Collections.Generic;

namespace MonthSheet.Common.Models
{
    public class Transactions
    {
        public List<ExpenseTransaction> Expenses { get; set; }
        public List<IncomeTransaction> Income { get; set; }
    }
}