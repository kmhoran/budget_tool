using System;
using System.Collections.Generic;
using Common.Core.Enums;
using Common.Core.Models;
using MonthSheet.Common.Enums;
using MonthSheet.Common.Interfaces;
using SheetApi.Common.Enums;
using SheetApi.Common.Models;

namespace MonthSheet.Repositories
{
    public partial class MonthSheetRepository : IMonthSheetRepository
    {
        public Categories LoadCategories()
        {
            var greenCategories = LoadPersonalCategories(UserEnum.Green);
            var redCategories = LoadPersonalCategories(UserEnum.Red);

            return new Categories
            {
                Green = greenCategories,
                Red = redCategories
            };
        }

        private CategoriesPersonal LoadPersonalCategories(UserEnum user)
        {
            string expenseRange = null;
            string incomeRange = null;
            switch (user)
            {
                case UserEnum.Green:
                    expenseRange = _sheetDetails.GreenExpenseCategoriesRange;
                    incomeRange = _sheetDetails.GreenIncomeCategoriesRange;
                    break;
                case UserEnum.Red:
                    expenseRange = _sheetDetails.RedExpenseCategoriesRange;
                    incomeRange = _sheetDetails.RedIncomeCategoriesRange;
                    break;
                default: break;
            }

            // load expense categories
            var rawExpense = LoadRange(expenseRange);
            var expense = new CategoriesExpense();
            if (rawExpense != null && rawExpense.Count > 0)
            {
                expense.Total = decimal
                    .Parse(rawExpense[(int)CategoriesExpenseRowEnum.Total][0].ToString());
                expense.DailyFood = decimal
                    .Parse(rawExpense[(int)CategoriesExpenseRowEnum.DailyFood][0].ToString());
                expense.Gifts = decimal
                    .Parse(rawExpense[(int)CategoriesExpenseRowEnum.Gifts][0].ToString());
                expense.Medical = decimal
                    .Parse(rawExpense[(int)CategoriesExpenseRowEnum.Medical][0].ToString());
                expense.Health = decimal
                    .Parse(rawExpense[(int)CategoriesExpenseRowEnum.Health][0].ToString());
                expense.Necessities = decimal
                    .Parse(rawExpense[(int)CategoriesExpenseRowEnum.Necessities][0].ToString());
                expense.Transportation = decimal
                    .Parse(rawExpense[(int)CategoriesExpenseRowEnum.Transportation][0].ToString());
                expense.Personal = decimal
                    .Parse(rawExpense[(int)CategoriesExpenseRowEnum.Personal][0].ToString());
                expense.Fun = decimal
                    .Parse(rawExpense[(int)CategoriesExpenseRowEnum.Fun][0].ToString());
                expense.Utilities = decimal
                    .Parse(rawExpense[(int)CategoriesExpenseRowEnum.Utilities][0].ToString());
                expense.Travel = decimal
                    .Parse(rawExpense[(int)CategoriesExpenseRowEnum.Travel][0].ToString());
                expense.Debt = decimal
                    .Parse(rawExpense[(int)CategoriesExpenseRowEnum.Debt][0].ToString());
                expense.Electronics = decimal
                    .Parse(rawExpense[(int)CategoriesExpenseRowEnum.Electronics][0].ToString());
                expense.Goals = decimal
                    .Parse(rawExpense[(int)CategoriesExpenseRowEnum.Goals][0].ToString());
                expense.Rent = decimal
                    .Parse(rawExpense[(int)CategoriesExpenseRowEnum.Rent][0].ToString());
                expense.Car = decimal
                    .Parse(rawExpense[(int)CategoriesExpenseRowEnum.Car][0].ToString());
                expense.Restaurants = decimal
                    .Parse(rawExpense[(int)CategoriesExpenseRowEnum.Restaurants][0].ToString());
                expense.Appartment = decimal
                    .Parse(rawExpense[(int)CategoriesExpenseRowEnum.Appartment][0].ToString());
                expense.Investment = decimal
                    .Parse(rawExpense[(int)CategoriesExpenseRowEnum.Investment][0].ToString());
                expense.Other = decimal
                    .Parse(rawExpense[(int)CategoriesExpenseRowEnum.Other][0].ToString());
            }

            // load income categories
            var rawIncome = LoadRange(incomeRange);
            var income = new CategoriesIncome();
            if (rawIncome != null && rawIncome.Count > 0)
            {
                income.Total = decimal
                    .Parse(rawIncome[(int)CategoriesIncomeRowEnum.Total][0].ToString());
                income.Savings = decimal
                    .Parse(rawIncome[(int)CategoriesIncomeRowEnum.Savings][0].ToString());
                income.Paycheck = decimal
                    .Parse(rawIncome[(int)CategoriesIncomeRowEnum.Paycheck][0].ToString());
                income.Bonus = decimal
                    .Parse(rawIncome[(int)CategoriesIncomeRowEnum.Bonus][0].ToString());
                income.Personal = decimal
                    .Parse(rawIncome[(int)CategoriesIncomeRowEnum.Personal][0].ToString());
                income.Gifts = decimal
                    .Parse(rawIncome[(int)CategoriesIncomeRowEnum.Gifts][0].ToString());
                income.Refund = decimal
                    .Parse(rawIncome[(int)CategoriesIncomeRowEnum.Refund][0].ToString());
                income.Payment = decimal
                    .Parse(rawIncome[(int)CategoriesIncomeRowEnum.Payment][0].ToString());
                income.Other = decimal
                    .Parse(rawIncome[(int)CategoriesIncomeRowEnum.Other][0].ToString());
            }


            return new CategoriesPersonal
            {
                Expense = expense,
                RawExpense = rawExpense,
                Income = income,
                RawIncome = rawIncome
            };
        }

        public IList<RangeUpdateModel> PrepExpenseCategoryProjectionUpdate(CategoriesExpense expenses, UserEnum user)
        {
            var expenseUpdateModel = new RangeUpdateModel();
            expenseUpdateModel.Range = user == UserEnum.Green ?
                _sheetDetails.GreenExpenseProjectionRange :
                _sheetDetails.RedExpenseProjectionRange;

            expenseUpdateModel.Dimension = DimensionEnums.Columns;

            // TODO: use order-safe enum
            expenseUpdateModel.Values = new List<IList<object>> {
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
            return new List<RangeUpdateModel> { expenseUpdateModel };
        }

        public IList<RangeUpdateModel> PrepIncomeCategoryProjectionUpdate(CategoriesIncome income, UserEnum user)
        {
            var update = new RangeUpdateModel();
            update.Range = user == UserEnum.Green ?
                            _sheetDetails.GreenIncomeProjectionRange :
                            _sheetDetails.RedIncomeProjectionRange;

            update.Dimension = DimensionEnums.Columns;

            var catchAllValue = income.Payment + income.Gifts + income.Refund + income.Other;
            update.Values = new List<IList<object>> {
                new List<object> {
                    0,
                    income.Paycheck,
                },
            };

            return new List<RangeUpdateModel> { update };
        }
    }
}
