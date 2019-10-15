using System;
using System.Collections.Generic;
using Common.Core.Enums;
using MonthSheet.Common.Interfaces;
using SheetApi.Common.Models;

namespace MonthSheet.Repositories
{
    public partial class MonthSheetRepository : IMonthSheetRepository
    {
        public decimal LoadPersonalCurrentBalance(UserEnum user)
        {
            string balanceRange = null;
            switch (user)
            {
                case UserEnum.Green:
                    balanceRange = _sheetDetails.GreenCurrentBalance;
                    break;
                case UserEnum.Red:
                    balanceRange = _sheetDetails.RedCurrentBalance;
                    break;
                default: break;
            }
            return decimal.Parse(LoadSingle(balanceRange));
        }
        
        public IList<RangeUpdateModel> PrepUpdatePersonalStartingBalance(decimal value, UserEnum user)
        {
            string balanceRange = null;
            switch (user)
            {
                case UserEnum.Green:
                    balanceRange = _sheetDetails.GreenStartingBalance;
                    break;
                case UserEnum.Red:
                    balanceRange = _sheetDetails.RedStartingBalance;
                    break;
                default: break;
            }

            return PrepSingleCellUpdate(value, balanceRange);
        }

        public decimal LoadNextMonthRentTotal()
        {
            return decimal.Parse(LoadSingle(_sheetDetails.RentTotal).ToString());
        }
        
    }
}
