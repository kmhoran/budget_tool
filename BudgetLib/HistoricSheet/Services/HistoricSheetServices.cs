using System;
using Common.Core.Models;
using HistoricSheet.Common.Interfaces;
using MonthSheet.Common.Models;

namespace HistoricSheet.Services
{
    public class HistoricSheetService: IHistoricSheetService
    {
        private IHistoricSheetRepository _repo;

        public HistoricSheetService(IHistoricSheetRepository repo)
        {
            _repo = repo;
        }

        public WrappedResponse AppendRecordsToHistoricLedger(Transactions transactions)
        {
            return new WrappedResponse { Success = false };
        }
    }
}
