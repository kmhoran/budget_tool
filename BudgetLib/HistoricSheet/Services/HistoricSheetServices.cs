using System;
using Common.Core.Models;
using HistoricSheet.Common.Interfaces;
using MonthSheet.Common.Models;

namespace HistoricSheet.Services
{
    public class HistoricSheetService : IHistoricSheetService
    {
        private IHistoricSheetRepository _repo;

        public HistoricSheetService(IHistoricSheetRepository repo)
        {
            _repo = repo;
        }

        public WrappedResponse AppendRecordsToHistoricLedger(Transactions transactions)
        {
            var expenseResult = _repo.AppendExpenses(transactions.Expenses);
            if (!expenseResult.Success) return expenseResult;
            var incomeResult = _repo.AppendIncome(transactions.Income);
            if (!incomeResult.Success) return incomeResult;
            return new WrappedResponse { Success = true };
        }
    }
}
