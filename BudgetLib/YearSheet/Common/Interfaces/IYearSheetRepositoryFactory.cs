using System;
using Common.Core.Enums;

namespace YearSheet.Common.Interfaces
{
    public interface IYearSheetRepositoryFactory
    {
        IYearSheetRepository GetRepository(UserEnum user);
    }
}