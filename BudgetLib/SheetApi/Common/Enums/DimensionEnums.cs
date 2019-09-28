using System;

namespace SheetApi.Common.Enums
{
    public enum DimensionEnums
    {
        Columns = 0,
        Rows = 1
    }

    public static class DimensionTypeExtention
    {
        public static string GetString(this DimensionEnums dimension)
        {
            switch(dimension)
            {
                case DimensionEnums.Columns: return "COLUMNS";
                case DimensionEnums.Rows: return "ROWS";
                default: return null; 
            }
        }
    }
}