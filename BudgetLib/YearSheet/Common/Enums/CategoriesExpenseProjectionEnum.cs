using System;

namespace YearSheet.Common.Enums
{
    public enum CategoriesExpenseProjectionEnum
    {
        DailyFood,
        Gifts,
        Medical,
        Health,
        Necessities,
        Transportation,
        Personal,
        Fun,
        Utilities,
        Travel,
        Debt,
        Electronics,
        Goals,
        Rent,
        Car,
        Restaurants,
        Appartment,
        Investment,
        Other,
    }

    public static class CategoriesExpenseProjectionEnumExtentions
    {
        public static string GetString(this CategoriesExpenseProjectionEnum category)
        {
            switch (category)
            {
                case CategoriesExpenseProjectionEnum.DailyFood: return "Daily Food";
                case CategoriesExpenseProjectionEnum.Gifts: return "Gifts";
                case CategoriesExpenseProjectionEnum.Medical: return "Medical";
                case CategoriesExpenseProjectionEnum.Health: return "Health";
                case CategoriesExpenseProjectionEnum.Necessities: return "Necessities";
                case CategoriesExpenseProjectionEnum.Transportation: return "Transportaion";
                case CategoriesExpenseProjectionEnum.Personal: return "Personal";
                case CategoriesExpenseProjectionEnum.Fun: return "Fun";
                case CategoriesExpenseProjectionEnum.Utilities: return "Utilities";
                case CategoriesExpenseProjectionEnum.Travel: return "Travel";
                case CategoriesExpenseProjectionEnum.Debt: return "Debt";
                case CategoriesExpenseProjectionEnum.Electronics: return "Electronics";
                case CategoriesExpenseProjectionEnum.Goals: return "Goals";
                case CategoriesExpenseProjectionEnum.Rent: return "Rent";
                case CategoriesExpenseProjectionEnum.Car: return "Car";
                case CategoriesExpenseProjectionEnum.Restaurants: return "Restaurants";
                case CategoriesExpenseProjectionEnum.Appartment: return "Appartment";
                case CategoriesExpenseProjectionEnum.Investment: return "Investment";
                case CategoriesExpenseProjectionEnum.Other: return "Other";
                default: return null;
            }
        }
    }
}