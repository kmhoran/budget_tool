using System;
using System.Collections.Generic;
using Common.Core.Models;
using SheetApi.Common.Models;

namespace SheetApi.Common.Interfaces
{
    public interface ISheetApiService
    {
        IList<IList<Object>> LoadRange(string spreadsheetId, string range);
        WrappedResponse UpdateRange(string spreadsheetId, RangeUpdateModel model);
        WrappedResponse UpdateRange(string spreadsheetId, IList<RangeUpdateModel> models);
        WrappedResponse ClearRange(string spreadsheetId, string range);
        WrappedResponse AppendToRange(string spreadsheetId, RangeUpdateModel model);
    }
}