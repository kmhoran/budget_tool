using System;
using System.Collections.Generic;
using Common.Core.Models;
using MonthSheet.Common.Models;

namespace HistoricSheet.Common.Interfaces
{
    public interface IHistoricSheetRepository
    {
        WrappedResponse AppendExpenses(IList<TransactionExpense> expenses);
        WrappedResponse AppendIncome(IList<TransactionIncome> incomes);
    }
}