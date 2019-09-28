using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using MonthSheet.Common.Interfaces;
using MonthSheet.Common.Models;
using SheetApi.Common.Interfaces;
using SheetApi.Services;

namespace MonthSheet.Repositories
{
    public partial class MonthSheetRepository : SheetRepositoryBase, IMonthSheetRepository
    {
        private MonthSheetDetails _sheetDetails;
        private ISheetApiService _sheetApi;

        public MonthSheetRepository(IOptions<MonthSheetDetails> sheetDetails, ISheetApiService sheetApi)
        :base(sheetDetails.Value.SheetId, sheetApi)
        {
            _sheetDetails = sheetDetails.Value;
            _sheetApi = sheetApi ?? throw new ArgumentNullException(nameof(sheetApi));

            var missingDetail = _sheetDetails.GetType().GetProperties()
            .Where(p => p.PropertyType == typeof(string))
            .Select(p => (name: p.Name, value: (string)p.GetValue(_sheetDetails)))
            .FirstOrDefault(r => string.IsNullOrEmpty(r.value));
            if (!string.IsNullOrEmpty(missingDetail.name)) throw new ArgumentNullException($"Missing Month detail: {missingDetail.name}");
        }
    }
}
