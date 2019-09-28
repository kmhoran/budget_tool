using System;
using System.Collections.Generic;
using Common.Core.Models;
using Common.Utils;
using Extentions.Common;
using Microsoft.Extensions.Options;
using MonthSheet.Common.Enums;
using MonthSheet.Common.Interfaces;
using MonthSheet.Common.Models;
using SheetApi.Common.Models;

namespace MonthSheet.Services
{
    public class MonthSheetService : IMonthSheetService
    {
        private MonthSheetDetails _sheetDetails;
        private IMonthSheetRepository _repo;

        public MonthSheetService(IOptions<MonthSheetDetails> sheetDetails, IMonthSheetRepository repo)
        {
            _sheetDetails = sheetDetails.Value;
            _repo = repo;
        }

        public Transactions LoadTransactions()
        {
            return _repo.LoadTransactions();
        }

        public Categories LoadCategories()
        {
            return _repo.LoadCategories();
        }


        #region Close Month
        public WrappedResponse<MonthCloseResponse> CloseMonth()
        {
            // load transactions and categories into memory
            var transactions = LoadTransactions();
            var categories = LoadCategories();

            var updates = new List<RangeUpdateModel>();

            // update imbalance log
            updates.AddRange(PrepSaveThisMonthsImbalance());

            // update month starting balance
            updates.AddRange(PrepUpdateMonthStartBalance());

            // save highlighted
            updates.AddRange(PrepSaveHighlightedTransactions(transactions, categories));

            // the service expects to write highlighted transactions to a blank ledger. Let's clear it out.
            var tableClearResult = _repo.ClearTransactions();
            if (!tableClearResult.Success)
            {
                // TODO : get some real error handling
                Console.WriteLine("COULD NOT CLEAR TRANSACTION TABLE");
                return (WrappedResponse<MonthCloseResponse>)tableClearResult;
            }

            return new WrappedResponse<MonthCloseResponse>
            {
                Success = true,
                Data = new MonthCloseResponse
                {
                    Transactions = transactions,
                    Categories = categories
                }
            };
        }

        private IList<RangeUpdateModel> PrepSaveThisMonthsImbalance()
        {
            var imbalanceValue = _repo.LoadCurrentImbalanceValue();
            var imbalanceIndex = GetImbalanceIndex();
            var updateImbalancePrep = _repo.PrepUpdateImbalance(
                value: imbalanceValue,
                index: imbalanceIndex);

            // for backwards compatability
            var updateIndexPrep = _repo.PrepUpdateImbalanceIndex(imbalanceIndex);
            updateImbalancePrep.AddRange(updateIndexPrep);

            return updateImbalancePrep;
        }

        private int GetImbalanceIndex()
        {
            // imbalance index is determined by number of months since the start date
            // TODO: improve imbalance implementation on sheet
            var start = DateTime.Parse(_sheetDetails.ImbalanceStartDate.ToString());
            var now = Month.GetEffectiveDate();
            return ((now.Year - start.Year) * 12) + (now.Month - start.Month);
        }

        private IList<RangeUpdateModel> PrepUpdateMonthStartBalance()
        {
            // update Green
            var greenBalance = _repo.LoadPersonalCurrentBalance(UserEnum.Green);
            var greenUpdate = _repo.PrepUpdatePersonalStartingBalance(greenBalance, UserEnum.Green);

            // update Red
            var redBalance = _repo.LoadPersonalCurrentBalance(UserEnum.Red);
            var redUpdate = _repo.PrepUpdatePersonalStartingBalance(redBalance, UserEnum.Red);

            greenUpdate.AddRange(new List<RangeUpdateModel>());
            return greenUpdate;
        }

        // TODO: explain
        private IList<RangeUpdateModel> PrepSaveHighlightedTransactions(Transactions transactions, Categories categories)
        {
            var (expenseToSave, incomeToSave) = FindHighlightedTransctions(transactions, categories);
            return _repo.PrepSaveHighlightedTransactions(expenseToSave, incomeToSave);
        }

        private (List<TransactionExpense> expenseToSave, List<TransactionIncome> incomeToSave) FindHighlightedTransctions(
    Transactions transactions,
    Categories categories)
        {
            var expenseToSave = new List<TransactionExpense>();
            var incomeToSave = new List<TransactionIncome>();

            // add next month rent
            expenseToSave.AddRange(CalculateNextMonthRent(categories));

            var saveNote = "saved";

            // expense
            foreach (var expense in transactions.Expenses)
            {
                if (expense.Save)
                {
                    var toSave = expense.DeepCopy();
                    toSave.TransactionDate = expense.TransactionDate.AddMonths(1);
                    toSave.SaveNote = saveNote;
                    expenseToSave.Add(toSave);
                }
            }

            // income
            foreach (var income in transactions.Income)
            {
                if (income.Save)
                {
                    var toSave = income.DeepCopy();
                    toSave.TransactionDate = income.TransactionDate.AddMonths(1);
                    toSave.SaveNote = saveNote;
                    incomeToSave.Add(toSave);
                }
            }

            return (expenseToSave, incomeToSave);

        }

        private List<TransactionExpense> CalculateNextMonthRent(Categories categories)
        {
            var nextRentTotal = _repo.LoadNextMonthRentTotal();
            var thisMonthIncome = categories.Green.Income.Total + categories.Red.Income.Total;

            // TODO: explain
            var redProportion = (nextRentTotal / thisMonthIncome) * categories.Red.Income.Total;
            var redRent = redProportion - (redProportion % 5);
            var greenRent = nextRentTotal - redRent;

            var nextMonth = Month.GetEffectiveDate().AddMonths(1);
            var rentDueDay = 15;
            var rentDate = new DateTime(nextMonth.Year, nextMonth.Month, rentDueDay);

            var saveNote = "!!!";
            var detail = "rent (*calculated*)";

            return new List<TransactionExpense>
            {
                // Red Rent
                new TransactionExpense
                {
                    SaveNote = saveNote,
                    TransactionDate = rentDate,
                    NetCost = redRent,
                    Detail = detail,
                    By = _sheetDetails.RedName,
                    For = _sheetDetails.RedName,
                    Category = CategoriesExpenseRowEnum.Rent.GetString()
                },
                // Green Rent 
                new TransactionExpense
                {
                    SaveNote = saveNote,
                    TransactionDate = rentDate,
                    NetCost = redRent,
                    Detail = detail,
                    By = _sheetDetails.GreenName,
                    For = _sheetDetails.GreenName,
                    Category = CategoriesExpenseRowEnum.Rent.GetString()
                }
            };
        }
        #endregion
    }
}
