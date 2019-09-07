using System;
using System.Collections.Generic;
using MonthSheet.Common.Enums;
using MonthSheet.Common.Interfaces;

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
        
        public bool UpdatePersonalStartingBalance(decimal value, UserEnum user)
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

            return UpdateSingle(value, balanceRange);
        }

        public decimal LoadNextMonthRentTotal()
        {
            return decimal.Parse(LoadSingle(_sheetDetails.RentTotal).ToString());
        }
        
    }
}
