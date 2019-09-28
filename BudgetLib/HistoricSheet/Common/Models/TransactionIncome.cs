using System;

namespace HistoricSheet.Common.Models
{
    public class TransactionIncome
    {
        public DateTime TransactionDate { get; set; }
        public decimal NetGain { get; set; }
        public string Detail { get; set; }
        public string For { get; set; }
        public string Category { get; set; }
    }
}