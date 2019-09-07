using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using MonthSheet.Common.Interfaces;
using MonthSheet.Common.Models;
using Sheet.Common.Enums;
using Sheet.Common.Interfaces;
using Sheet.Common.Models;

namespace MonthSheet.Repositories
{
    public partial class MonthSheetRepository : IMonthSheetRepository
    {
        private MonthSheetDetails _sheetDetails;
        private ISheetRepository _sheetApi;

        public MonthSheetRepository(IOptions<MonthSheetDetails> sheetDetails, ISheetRepository sheetApi)
        {
            _sheetDetails = sheetDetails.Value;
            _sheetApi = sheetApi ?? throw new ArgumentNullException(nameof(sheetApi));

            var missingDetail = _sheetDetails.GetType().GetProperties()
            .Where(p => p.PropertyType == typeof(string))
            .Select(p => (name: p.Name, value: (string)p.GetValue(_sheetDetails)))
            .FirstOrDefault(r => string.IsNullOrEmpty(r.value));
            if (!string.IsNullOrEmpty(missingDetail.name)) throw new ArgumentNullException($"Missing Month detail: {missingDetail.name}");
        }

        private IList<IList<Object>> LoadRange(string range)
        {
            return _sheetApi.LoadRange(_sheetDetails.SheetId, range);
        }

        private string LoadSingle(string range)
        {
            var result = LoadRange(range);
            return result[0][0].ToString();
        }

        private bool UpdateRange(IList<IList<Object>> values, string range, DimensionType dimension = DimensionType.Columns)
        {
            var updateModel = new RangeUpdateModel
            {
                Range = range,
                Values = values,
                Dimension = dimension
            };

            return _sheetApi.UpdateRange(_sheetDetails.SheetId, updateModel);
        }

        private bool UpdateRange(IList<RangeUpdateModel> models)
        {
            return _sheetApi.UpdateRange(_sheetDetails.SheetId, models);
        }

        private bool UpdateSingle(Object value, string range)
        {
            var values = new List<IList<Object>>
            {
                new List<Object> {value}
            };

            return UpdateRange(values, range);
        }
        // private bool AppendToRange(RangeUpdateModel model)
        // {
        //     return _sheetApi.AppendToRange(_sheetDetails.SheetId, model);
        // }
    }
}
