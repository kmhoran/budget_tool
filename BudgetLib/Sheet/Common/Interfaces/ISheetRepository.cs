using System;
using System.Collections.Generic;
using Sheet.Common.Models;

namespace Sheet.Common.Interfaces
{
    public interface ISheetRepository
    {
        IList<IList<Object>> LoadRange(string spreadsheetId, string range);
        bool UpdateRange(string spreadsheetId, RangeUpdateModel model);
        bool UpdateRange(string spreadsheetId, IList<RangeUpdateModel> models);
        bool ClearRange(string spreadsheetId, string range);
        // bool AppendToRange(string spreadsheetId, RangeUpdateModel model);
    }
}