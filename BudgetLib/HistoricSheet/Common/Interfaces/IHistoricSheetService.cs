using System;
using Common.Core.Models;
using MonthSheet.Common.Models;

namespace HistoricSheet.Common.Interfaces
{
    public interface IHistoricSheetService
    {
        WrappedResponse AppendRecordsToHistoricLedger(Transactions transactions);
    }
}