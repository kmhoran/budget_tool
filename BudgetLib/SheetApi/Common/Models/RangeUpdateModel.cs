using System;
using System.Collections.Generic;
using SheetApi.Common.Enums;

namespace SheetApi.Common.Models
{
    public class RangeUpdateModel
    {
        public string Range { get; set; }
        public DimensionEnums Dimension { get; set; }
        public IList<IList<Object>> Values { get; set; }
    }
}