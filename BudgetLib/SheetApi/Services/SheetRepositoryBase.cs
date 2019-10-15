using System;
using System.Collections.Generic;
using System.Linq;
using Common.Core.Models;
using SheetApi.Common.Enums;
using SheetApi.Common.Interfaces;
using SheetApi.Common.Models;

namespace SheetApi.Services
{
    public class SheetRepositoryBase
    {
        private string _sheetId;
        private ISheetApiService _sheetApi;

        // TODO: write tests for this base class
        public SheetRepositoryBase(string SheetId, ISheetApiService sheetApi)
        {
            _sheetId = SheetId ?? throw new ArgumentNullException("Sheet Id"); ;
            _sheetApi = sheetApi ?? throw new ArgumentNullException(nameof(sheetApi));
        }

        protected IList<IList<Object>> LoadRange(string range)
        {
            return _sheetApi.LoadRange(_sheetId, range);
        }

        protected string LoadSingle(string range)
        {
            var result = LoadRange(range);
            if (!(
                result != null
                && result.Count >= 1
                && result[0] != null
                && result[0].Count >= 1))
                return null;

            // return the first element of the IList<IList<Object>> response as a string
            return result[0][0].ToString();
        }

        public WrappedResponse UpdateRange(IList<RangeUpdateModel> models)
        {
            return _sheetApi.UpdateRange(_sheetId, models);
        }

        protected IList<RangeUpdateModel> PrepSingleCellUpdate(Object value, string range)
        {
            var values = new List<IList<Object>>
            {
                new List<Object> {value}
            };

            return new List<RangeUpdateModel>
            {
                new RangeUpdateModel
                {
                    Range = range,
                    Values = values,
                    Dimension = DimensionEnums.Columns
                }
            };
        }

        protected WrappedResponse AppendToRange(RangeUpdateModel model)
        {
            return _sheetApi.AppendToRange(_sheetId, model);
        }

    }
}