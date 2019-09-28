using System;
using System.Collections.Generic;
using Common.Core.Models;
using MonthSheet.Common.Interfaces;
using SheetApi.Common.Models;

namespace MonthSheet.Repositories
{
    public partial class MonthSheetRepository : IMonthSheetRepository
    {
        public decimal LoadCurrentImbalanceValue()
        {
            var result = LoadSingle(_sheetDetails.ImbalanceValueRange);
            return decimal.Parse(result);
        }

        public IList<RangeUpdateModel> PrepUpdateImbalance(decimal value, int index)
        {
            var range = _sheetDetails.ImbalanceColumn+index;
            return PrepSingleCellUpdate(value, range);
        }

        // for backwards compatability
        public IList<RangeUpdateModel> PrepUpdateImbalanceIndex(int index)
        {
            return PrepSingleCellUpdate(index, _sheetDetails.ImbalanceIndexRange);
        }
    }
}
