using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Options;
using MonthSheet.Common.Enums;
using MonthSheet.Common.Interfaces;
using MonthSheet.Common.Models;
using Sheet.Common.Interfaces;

namespace MonthSheet.Repositories
{
    public class MonthSheetRepository : IMonthSheetRepository
    {
        private MonthSheetDetails _sheetDetails;
        private ISheetRepository _sheetApi;

        // TODO: put this in the app config
        private readonly string SAVE_ENTRY_KEYWORD = "save";

        public MonthSheetRepository(IOptions<MonthSheetDetails> sheetDetails, ISheetRepository sheetApi)
        {
            _sheetDetails = sheetDetails.Value;
            _sheetApi = sheetApi ?? throw new ArgumentNullException(nameof(sheetApi));

            var missingDetail = _sheetDetails.GetType().GetProperties()
            .Where(p => p.PropertyType == typeof(string))
            .Select(p => (name: p.Name, value: (string)p.GetValue(_sheetDetails)))
            .FirstOrDefault(r => string.IsNullOrEmpty(r.value));
            if (!string.IsNullOrEmpty(missingDetail.name)) throw new ArgumentNullException($"Missing Month detail: {missingDetail.name}");
        }

        private IList<IList<Object>> LoadRange(string range)
        {
            return _sheetApi.LoadRange(_sheetDetails.SheetId, range);
        }

        public Transactions LoadTransactions()
        {
            // load expenses
            var expenseResults = LoadRange(_sheetDetails.TransactionsExpenseRange);
            var expenses = new List<ExpenseTransaction>();
            if (expenseResults != null && expenseResults.Count > 0)
            {
                foreach (var row in expenseResults)
                {
                    var expense = new ExpenseTransaction()
                    {
                        Save = row[(int)ExpenseTransactionColumnTypes.SaveNote]
                            .ToString().ToLower() == SAVE_ENTRY_KEYWORD,
                        TransactionDate = DateTime
                            .Parse(row[(int)ExpenseTransactionColumnTypes.DateString].ToString()),
                        NetCost = decimal.Parse(
                            row[(int)ExpenseTransactionColumnTypes.DollarAmount].ToString(),
                            NumberStyles.Currency),
                        Detail = row[(int)ExpenseTransactionColumnTypes.Detail].ToString(),
                        By = row[(int)ExpenseTransactionColumnTypes.By].ToString(),
                        For = row[(int)ExpenseTransactionColumnTypes.For].ToString(),
                        Category = row[(int)ExpenseTransactionColumnTypes.Category].ToString(),

                    };
                    expenses.Add(expense);
                }
            }

            // load income
            var incomeResults = LoadRange(_sheetDetails.TransactionsIncomeRange);
            var incomes = new List<IncomeTransaction>();
            if (incomeResults != null && incomeResults.Count > 0)
            {
                foreach (var row in incomeResults)
                {
                    var income = new IncomeTransaction()
                    {
                        Save = row[(int)IncomeTransactionColumnTypes.SaveNote]
                            .ToString().ToLower() == SAVE_ENTRY_KEYWORD,
                        TransactionDate = DateTime
                            .Parse(row[(int)IncomeTransactionColumnTypes.DateString].ToString()),
                        NetGain = decimal.Parse(
                            row[(int)IncomeTransactionColumnTypes.DollarAmount].ToString(),
                            NumberStyles.Currency),
                        Detail = row[(int)IncomeTransactionColumnTypes.Detail].ToString(),
                        For = row[(int)IncomeTransactionColumnTypes.For].ToString(),
                        Category = row[(int)IncomeTransactionColumnTypes.Category].ToString(),

                    };
                    incomes.Add(income);
                }
            }

            return new Transactions()
            {
                Expenses = expenses,
                Income = incomes
            };
        }
    }
}
