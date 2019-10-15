using System;

namespace YearSheet.Common.Models
{
    public class YearSheetDetails
    {
        public string GreenSheetId { get; set; }
        public string RedSheetId { get; set; }
        public string ExpenseColumnTemplate { get; set; }
        public string ExpenseProjectionRange { get; set; }
        public string IncomeColumnTemplate { get; set; }
        public string IncomeProjectionRange { get; set; }
    }
}