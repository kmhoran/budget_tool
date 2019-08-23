using System;
using System.Collections.Generic;
using MonthSheet.Common.Models;

namespace MonthSheet.Common.Interfaces
{
    public interface IMonthSheetRepository
    {
        Transactions LoadTransactions();
    }
}