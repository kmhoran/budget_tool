using System;

namespace MonthSheet.Common.Models
{
    public class ExpenseTransaction
    {
        public bool Save { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal NetCost { get; set; }
        public string Detail { get; set; }
        public string By { get; set; }
        public string For { get; set; }
        public string Category { get; set; }
    }
}