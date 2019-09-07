using System;
using System.Collections.Generic;
using Sheet.Common.Enums;

namespace Sheet.Common.Models
{
    public class RangeUpdateModel
    {
        public string Range { get; set; }
        public DimensionType Dimension { get; set; }
        public IList<IList<Object>> Values { get; set; }
    }
}