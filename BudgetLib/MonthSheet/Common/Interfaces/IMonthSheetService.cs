using System;
using System.Collections.Generic;
using MonthSheet.Common.Models;

namespace MonthSheet.Common.Interfaces
{
    public interface IMonthSheetService
    {
        Transactions LoadTransactions();
    }
}