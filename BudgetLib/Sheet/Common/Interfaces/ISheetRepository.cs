using System;
using System.Collections.Generic;

namespace Sheet.Common.Interfaces
{
    public interface ISheetRepository
    {
        void QuickStart();
        IList<IList<Object>> LoadRange(string spreadsheetId, string range);
    }
}