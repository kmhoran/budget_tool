using System;
using System.Collections.Generic;
using System.Linq;
using Common.Core.Models;
using HistoricSheet.Common.Enums;
using HistoricSheet.Common.Interfaces;
using HistoricSheet.Common.Models;
using Microsoft.Extensions.Options;
using MonthSheet.Common.Models;
using SheetApi.Common.Enums;
using SheetApi.Common.Interfaces;
using SheetApi.Common.Models;
using SheetApi.Services;

namespace HistoricSheet.Repositories
{
    public class HistoricSheetRepository : SheetRepositoryBase, IHistoricSheetRepository
    {
        private HistoricSheetDetails _sheetDetails;
        private ISheetApiService _sheetApi;

        public HistoricSheetRepository(IOptions<HistoricSheetDetails> sheetDetails, ISheetApiService sheetApi)
        : base(sheetDetails.Value.SheetId, sheetApi)
        {
            _sheetDetails = sheetDetails.Value;

            _sheetApi = sheetApi ?? throw new ArgumentNullException(nameof(sheetApi));

            var missingDetail = _sheetDetails.GetType().GetProperties()
            .Where(p => p.PropertyType == typeof(string))
            .Select(p => (name: p.Name, value: (string)p.GetValue(_sheetDetails)))
            .FirstOrDefault(r => string.IsNullOrEmpty(r.value));
            if (!string.IsNullOrEmpty(missingDetail.name)) throw new ArgumentNullException($"Missing Historic Sheet detail: {missingDetail.name}");
        }

        public WrappedResponse AppendExpenses(IList<TransactionExpense> expenses)
        {
            var expenseAppendModel = new RangeUpdateModel();
            expenseAppendModel.Range = _sheetDetails.ExpenseRange;
            expenseAppendModel.Dimension = DimensionEnums.Rows;
            expenseAppendModel.Values = new List<IList<object>>();
            var sortedExpense = expenses.ToList().OrderBy(x => x.TransactionDate).ToList();

            foreach (var expense in sortedExpense)
            {
                var row = new List<object>();
                var dateString = GetHistoricSheetDateString(expense.TransactionDate);
                row.Insert((int)HistoricExpenseColumnEnum.DateString, dateString);
                row.Insert((int)HistoricExpenseColumnEnum.DollarAmount, expense.NetCost);
                row.Insert((int)HistoricExpenseColumnEnum.Detail, expense.Detail);
                row.Insert((int)HistoricExpenseColumnEnum.By, expense.By);
                row.Insert((int)HistoricExpenseColumnEnum.For, expense.For);
                row.Insert((int)HistoricExpenseColumnEnum.Category, expense.Category);
                expenseAppendModel.Values.Add(row);
            }

            return AppendToRange(expenseAppendModel);
        }

        public WrappedResponse AppendIncome(IList<TransactionIncome> incomes)
        {
            var incomeAppendModel = new RangeUpdateModel();
            incomeAppendModel.Range = _sheetDetails.IncomeRange;
            incomeAppendModel.Dimension = DimensionEnums.Rows;
            incomeAppendModel.Values = new List<IList<object>>();
            var sortedIncome = incomes.ToList().OrderBy(x => x.TransactionDate).ToList();

            foreach (var income in sortedIncome)
            {
                var row = new List<object>();
                var dateString = GetHistoricSheetDateString(income.TransactionDate);
                row.Insert((int)HistoricIncomeColumnEnum.DateString, dateString);
                row.Insert((int)HistoricIncomeColumnEnum.DollarAmount, income.NetGain);
                row.Insert((int)HistoricIncomeColumnEnum.Detail, income.Detail);
                row.Insert((int)HistoricIncomeColumnEnum.For, income.For);
                row.Insert((int)HistoricIncomeColumnEnum.Category, income.Category);
                incomeAppendModel.Values.Add(row);
            }

            return AppendToRange(incomeAppendModel);
        }

        private string GetHistoricSheetDateString(DateTime date) =>
        string.Format("{0:yyyy-MM-dd}", date);
    }
}
