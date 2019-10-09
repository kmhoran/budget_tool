using System;
using System.Collections.Generic;
using Common.Core.Models;
using MonthSheet.Common.Models;
using SheetApi.Common.Models;

namespace YearSheet.Common.Interfaces
{
    public interface IYearSheetRepository
    {
        CategoriesPersonal LoadProjections();
        IList<RangeUpdateModel> PrepExpenseUpdate(CategoriesExpense expenses, DateTime effectiveDate);
        IList<RangeUpdateModel> PrepIncomeUpdate(CategoriesIncome income, DateTime effectiveDate);
        WrappedResponse UpdateRange(IList<RangeUpdateModel> models);
    }
}