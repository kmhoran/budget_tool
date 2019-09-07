using System;
using System.Collections.Generic;
using MonthSheet.Common.Enums;
using MonthSheet.Common.Models;

namespace MonthSheet.Common.Interfaces
{
    public interface IMonthSheetRepository
    {
        Transactions LoadTransactions();
        bool ClearTransactions();
        bool SaveHighlightedTransactionsToFreshLedger(List<TransactionExpense> expenses, List<TransactionIncome> incomes);
        Categories LoadCategories();
        decimal LoadCurrentImbalanceValue();
        bool UpdateImbalance(decimal value, int index);
        bool UpdateImbalanceIndex(int index);
        decimal LoadPersonalCurrentBalance(UserEnum user);
        bool UpdatePersonalStartingBalance(decimal value, UserEnum user);
        decimal LoadNextMonthRentTotal();

    }
}