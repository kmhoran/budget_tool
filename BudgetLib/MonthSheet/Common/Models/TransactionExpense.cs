using System;

namespace MonthSheet.Common.Models
{
    public class TransactionExpense
    {
        private bool? _Save { get; set; }
        public bool Save
        {
            get
            {
                return _Save ?? false;
            }
            set
            {
                _Save = value;
            }
        }
        public string SaveNote { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal NetCost { get; set; }
        public string Detail { get; set; }
        public string By { get; set; }
        public string For { get; set; }
        public string Category { get; set; }

    }
}