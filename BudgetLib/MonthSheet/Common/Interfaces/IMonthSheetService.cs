using System;
using System.Collections.Generic;
using MonthSheet.Common.Models;

namespace MonthSheet.Common.Interfaces
{
    public interface IMonthSheetService
    {
        Transactions LoadTransactions();
        bool CommitThisMonthsImbalance();
        bool UpdateMonthStartBalance();
        bool SaveHighlightedTransactionsToFreshLedger(Transactions transactions, Categories categories);
        Categories LoadCategories();
    }
}