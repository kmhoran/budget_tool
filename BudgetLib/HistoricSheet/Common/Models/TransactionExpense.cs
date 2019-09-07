using System;

namespace HistoricSheet.Common.Models
{
    public class TransactionExpense
    {
        public DateTime TransactionDate { get; set; }
        public decimal NetCost { get; set; }
        public string Detail { get; set; }
        public string By { get; set; }
        public string For { get; set; }
        public string Category { get; set; }

    }
}