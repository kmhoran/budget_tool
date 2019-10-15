using System;
using Common.Core.Enums;
using Microsoft.Extensions.Options;
using SheetApi.Common.Interfaces;
using YearSheet.Common.Interfaces;
using YearSheet.Common.Models;

namespace YearSheet.Repositories
{
    public class YearSheetRepositoryFactory: IYearSheetRepositoryFactory
    {
        private YearSheetDetails _sheetDetails;
        private ISheetApiService _sheetApi;

        public YearSheetRepositoryFactory(IOptions<YearSheetDetails> sheetDetails, ISheetApiService sheetApi)
        {
            _sheetDetails = sheetDetails.Value;
            _sheetApi = sheetApi;
        }
        public IYearSheetRepository GetRepository(UserEnum user)
        {
                        switch (user)
            {
                case UserEnum.Green:
                    return new YearSheetRepository(_sheetDetails.GreenSheetId, _sheetDetails, _sheetApi);
                case UserEnum.Red:
                    return new YearSheetRepository(_sheetDetails.RedSheetId, _sheetDetails, _sheetApi);
                default:
                    throw new ArgumentException("Unknown User Enum");
            }
        }
    }
}