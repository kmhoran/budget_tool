using System;
using System.Collections.Generic;
using Common.Core.Enums;
using Common.Core.Models;
using MonthSheet.Common.Models;
using SheetApi.Common.Models;

namespace MonthSheet.Common.Interfaces
{
    public interface IMonthSheetRepository
    {
        Transactions LoadTransactions();
        WrappedResponse ClearTransactions();
        IList<RangeUpdateModel> PrepSaveHighlightedTransactions(List<TransactionExpense> expenses, List<TransactionIncome> incomes);
        Categories LoadCategories();
        IList<RangeUpdateModel> PrepIncomeCategoryProjectionUpdate(CategoriesIncome income, UserEnum user);
        IList<RangeUpdateModel> PrepExpenseCategoryProjectionUpdate(CategoriesExpense expenses, UserEnum user);
        decimal LoadCurrentImbalanceValue();
        IList<RangeUpdateModel> PrepUpdateImbalance(decimal value, int index);
        IList<RangeUpdateModel> PrepUpdateImbalanceIndex(int index);
        decimal LoadPersonalCurrentBalance(UserEnum user);
        IList<RangeUpdateModel> PrepUpdatePersonalStartingBalance(decimal value, UserEnum user);
        decimal LoadNextMonthRentTotal();
        WrappedResponse UpdateRange(IList<RangeUpdateModel> models);
    }
}