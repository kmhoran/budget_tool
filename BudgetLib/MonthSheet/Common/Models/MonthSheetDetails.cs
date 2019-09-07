using System;

namespace MonthSheet.Common.Models
{

    public class MonthSheetDetails
    {
        public string SheetId { get; set; }
        public string TransactionsExpenseRange { get; set; }
        public string TransactionsIncomeRange { get; set; }
        public string GreenExpenseCategoriesRange { get; set; }
        public string GreenIncomeCategoriesRange { get; set; }

        public string RedExpenseCategoriesRange { get; set; }
        public string RedIncomeCategoriesRange { get; set; }
        public string GreenStartingBalance { get; set; }
        public string GreenCurrentBalance { get; set; }
        public string ImbalanceIndexRange { get; set; }
        public string ImbalanceValueRange { get; set; }
        public string ImbalanceStartDate { get; set; }
        public string RedStartingBalance { get; set; }
        public string RedCurrentBalance { get; set; }
        public string TransactionsRange { get; set; }
        public string ImbalanceColumn { get; set; }
        public string RentTotal { get; set; }
        public string GreenName { get; set; }
        public string RedName { get; set; }
        public string GreenMainPageId { get; set; }
        public string RedMainPageId { get; set; }
        public string DataPageId { get; set; }
        public string TransactionPageId { get; set; }
    }
}