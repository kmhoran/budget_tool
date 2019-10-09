using System;
using Common.Core.Enums;
using Common.Core.Models;
using Common.Utils;
using Extentions.Common;
using MonthSheet.Common.Models;
using YearSheet.Common.Interfaces;

namespace YearSheet.Services
{
    public class YearSheetService : IYearSheetService
    {
        private IYearSheetRepositoryFactory _repositoryFactory;

        public YearSheetService(IYearSheetRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        public WrappedResponse<Categories> SaveCategoriesToYear(Categories categories)
        {
            var effectiveDate = Month.GetEffectiveDate();
            // save green
            var greenProjections = SaveCategoriesToUserYearSheet(categories.Green, UserEnum.Green, effectiveDate);
            if (!greenProjections.Success)
            {
                Console.WriteLine("Error thrown while saving categories to Green Year Sheet");
                return new WrappedResponse<Categories> { Success = false, Exception = greenProjections.Exception };
            }
            
            // save red
            var redProjections = SaveCategoriesToUserYearSheet(categories.Red, UserEnum.Red, effectiveDate);
            if (!redProjections.Success)
            {
                Console.WriteLine("Error thrown while saving categories to Red Year Sheet");
                return new WrappedResponse<Categories> { Success = false, Exception = redProjections.Exception };
            }
            
            return new WrappedResponse<Categories>
            {
                Success = true,
                Data = new Categories
                {
                    Green = greenProjections.Data,
                    Red = redProjections.Data
                }
            };
        }

        private WrappedResponse<CategoriesPersonal> SaveCategoriesToUserYearSheet(CategoriesPersonal categories, UserEnum user, DateTime effectiveDate)
        {
            var repo = _repositoryFactory.GetRepository(user);
            var updates = repo.PrepExpenseUpdate(categories.Expense, effectiveDate);
            updates.AddRange(repo.PrepIncomeUpdate(categories.Income, effectiveDate));
            var updateResponse = repo.UpdateRange(updates);
            if(!updateResponse.Success){
                return (WrappedResponse<CategoriesPersonal>)updateResponse;
            }
            var projections = repo.LoadProjections();
            return new WrappedResponse<CategoriesPersonal> { Success = true, Data = projections };
        }
    }
}
