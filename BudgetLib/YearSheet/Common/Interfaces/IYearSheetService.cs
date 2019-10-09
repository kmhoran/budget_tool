using System;
using Common.Core.Models;

namespace YearSheet.Common.Interfaces
{
    public interface IYearSheetService
    {
        WrappedResponse<Categories> SaveCategoriesToYear(Categories categories);
    }
}