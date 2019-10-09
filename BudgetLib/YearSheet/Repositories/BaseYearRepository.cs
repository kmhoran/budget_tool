using System;
using Microsoft.Extensions.Options;
using SheetApi.Common.Interfaces;
using SheetApi.Services;
using YearSheet.Common.Interfaces;
using YearSheet.Common.Models;

namespace YearSheet.Repositories
{
    public class BaseYearRepository: SheetRepositoryBase
    {
        private ISheetApiService _sheetApi;
        public BaseYearRepository(string sheetId, ISheetApiService sheetApi)
        :base(sheetId, sheetApi){
            _sheetApi = sheetApi ?? throw new ArgumentNullException(nameof(sheetApi));
        }
    }

    
}
