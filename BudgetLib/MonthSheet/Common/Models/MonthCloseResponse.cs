using System;

namespace MonthSheet.Common.Models
{
    public class MonthCloseResponse
    {
        public Transactions Transactions { get; set; }
        public Categories Categories { get; set; }
    }
}