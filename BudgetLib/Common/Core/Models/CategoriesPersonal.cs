using System;
using System.Collections.Generic;

namespace Common.Core.Models
{
    public class CategoriesPersonal
    {
        public CategoriesExpense Expense { get; set; }
        public IList<IList<Object>> RawExpense { get; set; }
        public CategoriesIncome Income { get; set; }
        public IList<IList<Object>> RawIncome { get; set; }
    }
}