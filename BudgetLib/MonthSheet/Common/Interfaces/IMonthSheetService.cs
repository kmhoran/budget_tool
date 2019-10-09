using System;
using System.Collections.Generic;
using Common.Core.Models;
using MonthSheet.Common.Models;

namespace MonthSheet.Common.Interfaces
{
    public interface IMonthSheetService
    {
        Transactions LoadTransactions();
        WrappedResponse<MonthCloseResponse> CloseMonth();
        WrappedResponse UpdateCategoryProjections(Categories categories);
        Categories LoadCategories();
    }
}