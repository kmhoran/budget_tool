using System;
using System.Collections.Generic;
using MonthSheet.Common.Interfaces;
using Sheet.Common.Models;

namespace MonthSheet.Repositories
{
    public partial class MonthSheetRepository : IMonthSheetRepository
    {
        public decimal LoadCurrentImbalanceValue()
        {
            var result = LoadSingle(_sheetDetails.ImbalanceValueRange);
            return decimal.Parse(result);
        }

        public bool UpdateImbalance(decimal value, int index)
        {
            var range = _sheetDetails.ImbalanceColumn+index;
            return UpdateSingle(value, range);
        }

        // for backwards compatability
        public bool UpdateImbalanceIndex(int index)
        {
            return UpdateSingle(index, _sheetDetails.ImbalanceIndexRange);
        }
    }
}
