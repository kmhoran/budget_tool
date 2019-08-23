using System;

namespace MonthSheet.Common.Models
{
    public class IncomeTransaction
    {
        public bool Save { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal NetGain { get; set; }
        public string Detail { get; set; }
        public string For { get; set; }
        public string Category { get; set; }
    }
}