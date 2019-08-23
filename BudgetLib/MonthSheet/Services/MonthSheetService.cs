using System;
using MonthSheet.Common.Interfaces;
using MonthSheet.Common.Models;

namespace MonthSheet.Services
{
    public class MonthSheetService: IMonthSheetService
    {
        private IMonthSheetRepository _repo;

        public MonthSheetService(IMonthSheetRepository repo)
        {
            _repo = repo;
        }

        public Transactions LoadTransactions()
        {
            return _repo.LoadTransactions();
        }
    }
}
