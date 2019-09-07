using System;

namespace Sheet.Common.Enums
{
    public enum DimensionType
    {
        Columns = 0,
        Rows = 1
    }

    public static class DimensionTypeExtention
    {
        public static string GetString(this DimensionType dimension)
        {
            switch(dimension)
            {
                case DimensionType.Columns: return "COLUMNS";
                case DimensionType.Rows: return "ROWS";
                default: return null; 
            }
        }
    }
}