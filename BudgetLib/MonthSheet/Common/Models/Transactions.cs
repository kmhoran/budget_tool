using System;
using System.Collections.Generic;

namespace MonthSheet.Common.Models
{
    public class Transactions
    {
        public List<TransactionExpense> Expenses { get; set; }
        public IList<IList<Object>> RawExpenses { get; set; }
        public List<TransactionIncome> Income { get; set; }
        public IList<IList<Object>> RawIncome { get; set; }
    }
}