using System;
using System.Collections.Generic;
using System.Globalization;
using MonthSheet.Common.Enums;
using MonthSheet.Common.Interfaces;
using MonthSheet.Common.Models;
using Sheet.Common.Enums;
using Sheet.Common.Models;

namespace MonthSheet.Repositories
{
    public partial class MonthSheetRepository : IMonthSheetRepository
    {

        // TODO: put this in the app config
        private readonly string SAVE_ENTRY_KEYWORD = "save";

        public Transactions LoadTransactions()
        {
            // load expenses
            var rawExpenses = LoadRange(_sheetDetails.TransactionsExpenseRange);
            var expenses = new List<TransactionExpense>();
            if (rawExpenses != null && rawExpenses.Count > 0)
            {
                foreach (var row in rawExpenses)
                {
                    // TODO: handle mal-formed inputs more grcefully
                    // let's just skip incomplete rows
                    if (row.Count <= (int)TransactionExpenseColumnEnum.Category)
                    {
                        continue;
                    }
                    var expense = new TransactionExpense()
                    {
                        Save = row[(int)TransactionExpenseColumnEnum.SaveNote]
                            .ToString().ToLower() == SAVE_ENTRY_KEYWORD,
                        SaveNote = row[(int)TransactionExpenseColumnEnum.SaveNote]
                            .ToString(),
                        TransactionDate = DateTime
                            .Parse(row[(int)TransactionExpenseColumnEnum.DateString].ToString()),
                        NetCost = decimal.Parse(
                            row[(int)TransactionExpenseColumnEnum.DollarAmount].ToString(),
                            NumberStyles.Currency),
                        Detail = row[(int)TransactionExpenseColumnEnum.Detail].ToString(),
                        By = row[(int)TransactionExpenseColumnEnum.By].ToString(),
                        For = row[(int)TransactionExpenseColumnEnum.For].ToString(),
                        Category = row[(int)TransactionExpenseColumnEnum.Category].ToString(),

                    };
                    expenses.Add(expense);
                }
            }

            // load income
            var rawIncome = LoadRange(_sheetDetails.TransactionsIncomeRange);
            var incomes = new List<TransactionIncome>();
            if (rawIncome != null && rawIncome.Count > 0)
            {
                foreach (var row in rawIncome)
                {
                    if (row.Count <= (int)TransactionIncomeColumnEnum.Category)
                    {
                        continue;
                    }
                    var income = new TransactionIncome()
                    {
                        Save = row[(int)TransactionIncomeColumnEnum.SaveNote]
                            .ToString().ToLower() == SAVE_ENTRY_KEYWORD,
                        TransactionDate = DateTime
                            .Parse(row[(int)TransactionIncomeColumnEnum.DateString].ToString()),
                        NetGain = decimal.Parse(
                            row[(int)TransactionIncomeColumnEnum.DollarAmount].ToString(),
                            NumberStyles.Currency),
                        Detail = row[(int)TransactionIncomeColumnEnum.Detail].ToString(),
                        For = row[(int)TransactionIncomeColumnEnum.For].ToString(),
                        Category = row[(int)TransactionIncomeColumnEnum.Category].ToString(),

                    };
                    incomes.Add(income);
                }
            }

            return new Transactions()
            {
                Expenses = expenses,
                RawExpenses = rawExpenses,
                Income = incomes,
                RawIncome = rawIncome
            };
        }

        public bool ClearTransactions()
        {
            return _sheetApi.ClearRange(_sheetDetails.SheetId, _sheetDetails.TransactionsRange);
        }

        /*
        This method is responsible for updating the (hopefully empty) transaction columns with 
         */
        public bool SaveHighlightedTransactionsToFreshLedger(List<TransactionExpense> expenses, List<TransactionIncome> incomes)
        {
            // the service expects to write highlighted transactions to a blank ledger. Let's clear it out.
            var tableClearSuccess = ClearTransactions();
            if (!tableClearSuccess)
            {
                // TODO : get some real error handling
                Console.WriteLine("COULD NOT CLEAR TRANSACTION TABLE");
                return false;
            }

            // expense update
            var expenseUpdateModel = new RangeUpdateModel();
            expenseUpdateModel.Range = _sheetDetails.TransactionsExpenseRange;
            expenseUpdateModel.Dimension = DimensionType.Rows;
            expenseUpdateModel.Values = new List<IList<object>>();

            foreach (var expense in expenses)
            {
                var row = new List<object>();
                row.Insert((int)TransactionExpenseColumnEnum.SaveNote, expense.SaveNote);
                row.Insert((int)TransactionExpenseColumnEnum.DateString, expense.TransactionDate.ToString("d"));
                row.Insert((int)TransactionExpenseColumnEnum.DollarAmount, expense.NetCost);
                row.Insert((int)TransactionExpenseColumnEnum.Detail, expense.Detail);
                row.Insert((int)TransactionExpenseColumnEnum.By, expense.By);
                row.Insert((int)TransactionExpenseColumnEnum.For, expense.For);
                row.Insert((int)TransactionExpenseColumnEnum.Category, expense.Category);
                expenseUpdateModel.Values.Add(row);
            }

            // expense update
            var incomeUpdateModel = new RangeUpdateModel();
            incomeUpdateModel.Range = _sheetDetails.TransactionsIncomeRange;
            incomeUpdateModel.Dimension = DimensionType.Rows;
            incomeUpdateModel.Values = new List<IList<object>>();

            foreach (var income in incomes)
            {
                var row = new List<object>();
                row.Insert((int)TransactionIncomeColumnEnum.SaveNote, income.SaveNote);
                row.Insert((int)TransactionIncomeColumnEnum.DateString, income.TransactionDate.ToString("d"));
                row.Insert((int)TransactionIncomeColumnEnum.DollarAmount, income.NetGain);
                row.Insert((int)TransactionIncomeColumnEnum.Detail, income.Detail);
                row.Insert((int)TransactionIncomeColumnEnum.For, income.For);
                row.Insert((int)TransactionIncomeColumnEnum.Category, income.Category);
                incomeUpdateModel.Values.Add(row);
            }

            var toSave = new List<RangeUpdateModel> {
                expenseUpdateModel,
                incomeUpdateModel
            };

            return UpdateRange(toSave);
        }
    }
}
