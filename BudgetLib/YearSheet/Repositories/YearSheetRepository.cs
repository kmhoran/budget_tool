using System;
using System.Collections.Generic;
using System.Globalization;
using Common.Core.Models;
using SheetApi.Common.Enums;
using SheetApi.Common.Interfaces;
using SheetApi.Common.Models;
using SheetApi.Services;
using YearSheet.Common.Enums;
using YearSheet.Common.Interfaces;
using YearSheet.Common.Models;

namespace YearSheet.Repositories
{
    public class YearSheetRepository : SheetRepositoryBase, IYearSheetRepository
    {
        private ISheetApiService _sheetApi;
        private YearSheetDetails _sheetDetails;

        public YearSheetRepository(string sheetId, YearSheetDetails sheetDetails, ISheetApiService sheetApi)
        : base(sheetId, sheetApi)
        {
            _sheetApi = sheetApi ?? throw new ArgumentNullException(nameof(sheetApi));
            _sheetDetails = sheetDetails;
        }

        public CategoriesPersonal LoadProjections()
        {
            var rawExpense = LoadRange(_sheetDetails.ExpenseProjectionRange);

            var expense = new CategoriesExpense();
            if (rawExpense != null && rawExpense.Count > 0)
            {
                expense.DailyFood = decimal
                    .Parse(rawExpense[(int)CategoriesExpenseProjectionEnum.DailyFood][0].ToString(), NumberStyles.Currency);
                expense.Gifts = decimal
                    .Parse(rawExpense[(int)CategoriesExpenseProjectionEnum.Gifts][0].ToString(), NumberStyles.Currency);
                expense.Medical = decimal
                    .Parse(rawExpense[(int)CategoriesExpenseProjectionEnum.Medical][0].ToString(), NumberStyles.Currency);
                expense.Health = decimal
                    .Parse(rawExpense[(int)CategoriesExpenseProjectionEnum.Health][0].ToString(), NumberStyles.Currency);
                expense.Necessities = decimal
                    .Parse(rawExpense[(int)CategoriesExpenseProjectionEnum.Necessities][0].ToString(), NumberStyles.Currency);
                expense.Transportation = decimal
                    .Parse(rawExpense[(int)CategoriesExpenseProjectionEnum.Transportation][0].ToString(), NumberStyles.Currency);
                expense.Personal = decimal
                    .Parse(rawExpense[(int)CategoriesExpenseProjectionEnum.Personal][0].ToString(), NumberStyles.Currency);
                expense.Fun = decimal
                    .Parse(rawExpense[(int)CategoriesExpenseProjectionEnum.Fun][0].ToString(), NumberStyles.Currency);
                expense.Utilities = decimal
                    .Parse(rawExpense[(int)CategoriesExpenseProjectionEnum.Utilities][0].ToString(), NumberStyles.Currency);
                expense.Travel = decimal
                    .Parse(rawExpense[(int)CategoriesExpenseProjectionEnum.Travel][0].ToString(), NumberStyles.Currency);
                expense.Debt = decimal
                    .Parse(rawExpense[(int)CategoriesExpenseProjectionEnum.Debt][0].ToString(), NumberStyles.Currency);
                expense.Electronics = decimal
                    .Parse(rawExpense[(int)CategoriesExpenseProjectionEnum.Electronics][0].ToString(), NumberStyles.Currency);
                expense.Goals = decimal
                    .Parse(rawExpense[(int)CategoriesExpenseProjectionEnum.Goals][0].ToString(), NumberStyles.Currency);
                expense.Rent = decimal
                    .Parse(rawExpense[(int)CategoriesExpenseProjectionEnum.Rent][0].ToString(), NumberStyles.Currency);
                expense.Car = decimal
                    .Parse(rawExpense[(int)CategoriesExpenseProjectionEnum.Car][0].ToString(), NumberStyles.Currency);
                expense.Restaurants = decimal
                    .Parse(rawExpense[(int)CategoriesExpenseProjectionEnum.Restaurants][0].ToString(), NumberStyles.Currency);
                expense.Appartment = decimal
                    .Parse(rawExpense[(int)CategoriesExpenseProjectionEnum.Appartment][0].ToString(), NumberStyles.Currency);
                expense.Investment = decimal
                    .Parse(rawExpense[(int)CategoriesExpenseProjectionEnum.Investment][0].ToString(), NumberStyles.Currency);
                expense.Other = decimal
                    .Parse(rawExpense[(int)CategoriesExpenseProjectionEnum.Other][0].ToString(), NumberStyles.Currency);
            }

            var rawIncome = LoadRange(_sheetDetails.IncomeProjectionRange);
            var income = new CategoriesIncome();
            if (rawIncome != null && rawIncome.Count > 0)
            {
                income.Savings = decimal
                    .Parse(rawExpense[(int)CategoriesIncomeProjectionEnum.Savings][0].ToString(), NumberStyles.Currency);
                income.Paycheck = decimal
                    .Parse(rawExpense[(int)CategoriesIncomeProjectionEnum.Paycheck][0].ToString(), NumberStyles.Currency);
                income.Bonus = decimal
                    .Parse(rawExpense[(int)CategoriesIncomeProjectionEnum.Bonus][0].ToString(), NumberStyles.Currency);
                income.Personal = decimal
                    .Parse(rawExpense[(int)CategoriesIncomeProjectionEnum.Personal][0].ToString(), NumberStyles.Currency);
                income.Gifts = decimal
                    .Parse(rawExpense[(int)CategoriesIncomeProjectionEnum.Gifts][0].ToString(), NumberStyles.Currency);
                income.Other = decimal
                    .Parse(rawExpense[(int)CategoriesIncomeProjectionEnum.Other][0].ToString(), NumberStyles.Currency);
            }


            return new CategoriesPersonal
            {
                Expense = expense,
                Income = income,
                RawExpense = rawExpense,
                RawIncome = rawIncome
            };
        }

        public IList<RangeUpdateModel> PrepExpenseUpdate(CategoriesExpense expenses, DateTime effectiveDate)
        {
            var expenseRange = GetExpenseRangeString(effectiveDate);
            var update = new RangeUpdateModel();
            update.Dimension = DimensionEnums.Columns;
            update.Range = expenseRange;
            update.Values = new List<IList<object>> {
                new List<object> {
                    expenses.DailyFood,
                    expenses.Gifts,
                    expenses.Medical,
                    expenses.Health,
                    expenses.Necessities,
                    expenses.Transportation,
                    expenses.Personal,
                    expenses.Fun,
                    expenses.Utilities,
                    expenses.Travel,
                    expenses.Debt,
                    expenses.Electronics,
                    expenses.Goals,
                    expenses.Rent,
                    expenses.Car,
                    expenses.Restaurants,
                    expenses.Appartment,
                    expenses.Investment,
                    expenses.Other},
            };

            return new List<RangeUpdateModel> { update };
        }

        public IList<RangeUpdateModel> PrepIncomeUpdate(CategoriesIncome income, DateTime effectiveDate)
        {
            var incomeRange = GetIncomeRangeString(effectiveDate);
            var update = new RangeUpdateModel();
            update.Dimension = DimensionEnums.Columns;
            update.Range = incomeRange;
            var catchAllValue = income.Payment + income.Gifts + income.Refund + income.Other;
            update.Values = new List<IList<object>> {
                new List<object> {
                    income.Savings,
                    income.Paycheck,
                    income.Bonus,
                    income.Personal,
                    income.Gifts,
                    catchAllValue
                },
            };

            return new List<RangeUpdateModel> { update };
        }

        private string GetExpenseRangeString(DateTime effectiveDate)
        {
            var month = effectiveDate.Month;
            // January starts on Column `D` (68 ascii) 
            var columnChar = Convert.ToChar(month + 67);
            return _sheetDetails.ExpenseColumnTemplate.Replace('X', columnChar);
        }

        private string GetIncomeRangeString(DateTime effectiveDate)
        {
            var month = effectiveDate.Month;
            // January starts on Column `E` (69 ascii)
            var columnChar = Convert.ToChar(month + 68);
            return _sheetDetails.IncomeColumnTemplate.Replace('X', columnChar);
        }
    }


}
