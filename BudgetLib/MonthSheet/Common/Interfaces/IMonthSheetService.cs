using System;
using System.Collections.Generic;
using Common.Core.Models;
using MonthSheet.Common.Models;

namespace MonthSheet.Common.Interfaces
{
    public interface IMonthSheetService
    {
        Transactions LoadTransactions();
        // bool CommitThisMonthsImbalance();
        // bool UpdateMonthStartBalance();
        // bool SaveHighlightedTransactionsToFreshLedger(Transactions transactions, Categories categories);
        WrappedResponse<MonthCloseResponse> CloseMonth();
        Categories LoadCategories();
    }
}