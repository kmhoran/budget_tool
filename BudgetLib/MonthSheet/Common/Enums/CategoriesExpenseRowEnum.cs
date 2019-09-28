using System;

namespace MonthSheet.Common.Enums
{
    public enum CategoriesExpenseRowEnum
    {
        Total,
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

    public static class CategoriesExpenseRowEnumExtentions
    {
        public static string GetString(this CategoriesExpenseRowEnum category)
        {
            switch (category)
            {
                case CategoriesExpenseRowEnum.Total: return "Total";
                case CategoriesExpenseRowEnum.DailyFood: return "Daily Food";
                case CategoriesExpenseRowEnum.Gifts: return "Gifts";
                case CategoriesExpenseRowEnum.Medical: return "Medical";
                case CategoriesExpenseRowEnum.Health: return "Health";
                case CategoriesExpenseRowEnum.Necessities: return "Necessities";
                case CategoriesExpenseRowEnum.Transportation: return "Transportaion";
                case CategoriesExpenseRowEnum.Personal: return "Personal";
                case CategoriesExpenseRowEnum.Fun: return "Fun";
                case CategoriesExpenseRowEnum.Utilities: return "Utilities";
                case CategoriesExpenseRowEnum.Travel: return "Travel";
                case CategoriesExpenseRowEnum.Debt: return "Debt";
                case CategoriesExpenseRowEnum.Electronics: return "Electronics";
                case CategoriesExpenseRowEnum.Goals: return "Goals";
                case CategoriesExpenseRowEnum.Rent: return "Rent";
                case CategoriesExpenseRowEnum.Car: return "Car";
                case CategoriesExpenseRowEnum.Restaurants: return "Restaurants";
                case CategoriesExpenseRowEnum.Appartment: return "Appartment";
                case CategoriesExpenseRowEnum.Investment: return "Investment";
                case CategoriesExpenseRowEnum.Other: return "Other";
                default: return null;
            }
        }
    }
}