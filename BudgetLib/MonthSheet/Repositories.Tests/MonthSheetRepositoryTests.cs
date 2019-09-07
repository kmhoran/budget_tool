using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Options;
using MonthSheet.Common.Models;
using Sheet.Common.Interfaces;
using Extentions.Common;
using Xunit;
using Moq;
using MonthSheet.Common.Interfaces;

namespace MonthSheet.Repositories.Tests
{
    public class MonthSheetRepositoryTests
    {
        public static readonly TheoryData<IOptions<MonthSheetDetails>, ISheetRepository> ConstructorTheoryData =
        new TheoryData<IOptions<MonthSheetDetails>, ISheetRepository>
        {
            {MonthSheetDetailMocker.GetPassing(), null},
            {MonthSheetDetailMocker.GetWithMissing("SheetId" ), SheetRepoMocker.GetRepository()},
            {MonthSheetDetailMocker.GetWithMissing("TransactionsExpenseRange"), SheetRepoMocker.GetRepository()},
            {MonthSheetDetailMocker.GetWithMissing("TransactionsIncomeRange"), SheetRepoMocker.GetRepository()},
            {MonthSheetDetailMocker.GetWithMissing("GreenExpenseCategoriesRange"), SheetRepoMocker.GetRepository()},
            {MonthSheetDetailMocker.GetWithMissing("GreenIncomeCategoriesRange"), SheetRepoMocker.GetRepository()},
            {MonthSheetDetailMocker.GetWithMissing( "RedExpenseCategoriesRange"), SheetRepoMocker.GetRepository()},
            {MonthSheetDetailMocker.GetWithMissing("RedIncomeCategoriesRange"), SheetRepoMocker.GetRepository()},
            {MonthSheetDetailMocker.GetWithMissing("GreenStartingBalance"), SheetRepoMocker.GetRepository()},
            {MonthSheetDetailMocker.GetWithMissing("GreenCurrentBalance"), SheetRepoMocker.GetRepository()},
            {MonthSheetDetailMocker.GetWithMissing("ImbalanceIndexRange"), SheetRepoMocker.GetRepository()},
            {MonthSheetDetailMocker.GetWithMissing("ImbalanceValueRange"), SheetRepoMocker.GetRepository()},
            {MonthSheetDetailMocker.GetWithMissing("ImbalanceStartDate"), SheetRepoMocker.GetRepository()},
            {MonthSheetDetailMocker.GetWithMissing("RedStartingBalance"), SheetRepoMocker.GetRepository()},
            {MonthSheetDetailMocker.GetWithMissing("RedCurrentBalance"), SheetRepoMocker.GetRepository()},
            {MonthSheetDetailMocker.GetWithMissing("TransactionsRange"), SheetRepoMocker.GetRepository()},
            {MonthSheetDetailMocker.GetWithMissing("ImbalanceColumn"), SheetRepoMocker.GetRepository()},
            {MonthSheetDetailMocker.GetWithMissing("RentTotal"), SheetRepoMocker.GetRepository()},
            {MonthSheetDetailMocker.GetWithMissing("GreenName"), SheetRepoMocker.GetRepository()},
            {MonthSheetDetailMocker.GetWithMissing("RedName"), SheetRepoMocker.GetRepository()},
            {MonthSheetDetailMocker.GetWithMissing("GreenMainPageId"), SheetRepoMocker.GetRepository()},
            {MonthSheetDetailMocker.GetWithMissing("RedMainPageId"), SheetRepoMocker.GetRepository()},
            {MonthSheetDetailMocker.GetWithMissing("DataPageId"), SheetRepoMocker.GetRepository()},
            {MonthSheetDetailMocker.GetWithMissing("TransactionPageId"), SheetRepoMocker.GetRepository()},
        };

        [Theory]
        [MemberData(nameof(ConstructorTheoryData))]
        public void Constructor_with_NullArguments_throws_ArgumentNullException(
            IOptions<MonthSheetDetails> details,
            ISheetRepository sheetRepo) =>
            Assert.Throws<ArgumentNullException>(
                () => new MonthSheetRepository(details, sheetRepo));

        [Fact]
        public void Constructor_with_NonNullArguments_return_NonNull()
        {
            var repo = GetMonthRepo();
            Assert.NotNull(repo);
        }


        [Fact]
        public void LoadTransactions_with_NullSheetApiResponses_returns_EmptyTrnsactionLists()
        {
            var sheetRepoMock = SheetRepoMocker.GetMock();
            sheetRepoMock.Setup(x => x.LoadRange(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((IList<IList<Object>>)null);
            var repo = GetMonthRepo(sheetRepo: sheetRepoMock.Object);

            var result = repo.LoadTransactions();

            Assert.NotNull(result);
            Assert.NotNull(result.Expenses);
            Assert.NotNull(result.Income);
            Assert.Empty(result.Expenses);
            Assert.Empty(result.Income);
        }



        private static IMonthSheetRepository GetMonthRepo(IOptions<MonthSheetDetails> sheetDetails = null, ISheetRepository sheetRepo = null)
        {
            return new MonthSheetRepository(sheetDetails ?? MonthSheetDetailMocker.GetPassing(), SheetRepoMocker.GetRepository());
        }


    }
    #region Helpers
    public static class MonthSheetDetailMocker
    {
        private static readonly MonthSheetDetails _monthDetails = new MonthSheetDetails()
        {
            SheetId = "xxxxxxxx-xxxxxxxx-xxxxxxxx",
            TransactionsExpenseRange = "transactions!A6:G207",
            TransactionsIncomeRange = "transactions!H6:M207",
            GreenExpenseCategoriesRange = "data!D3:D21",
            GreenIncomeCategoriesRange = "data!H2:H10",
            ImbalanceStartDate = "2017-06-01T01:00:00",
            RedExpenseCategoriesRange = "data!E3:E21",
            RedIncomeCategoriesRange = "data!I2:I10",
            GreenStartingBalance = "data!S2",
            GreenCurrentBalance = "data!T3",
            ImbalanceIndexRange = "data!L5",
            ImbalanceValueRange = "data!L4",
            RedStartingBalance = "data!T2",
            RedCurrentBalance = "data!T3",
            TransactionsRange = "transactions!A6:M207",
            ImbalanceColumn = "data!N",
            RentTotal = "data!R2",
            GreenName = "Green",
            RedName = "Red",
            GreenMainPageId = "3950782",
            RedMainPageId = "1938506",
            DataPageId = "194859860",
            TransactionPageId = "0"
        };


        public static IOptions<MonthSheetDetails> GetPassing()
        {
            return Options.Create<MonthSheetDetails>(_monthDetails);
        }

        public static IOptions<MonthSheetDetails> GetWithMissing(string propName)
        {
            var copy = _monthDetails.DeepCopy();
            Type classType = typeof(MonthSheetDetails);
            PropertyInfo info = classType.GetProperty(propName);
            info.SetValue(copy, null);

            return Options.Create<MonthSheetDetails>(copy);
        }
    }

    public static class SheetRepoMocker
    {
        public static Mock<ISheetRepository> GetMock()
        {
            return new Mock<ISheetRepository>();
        }

        public static ISheetRepository GetRepository()
        {
            return new Mock<ISheetRepository>().Object;
        }
    }
    public static class ClassExtentions
    {
        public static object GetProp<T>(this T obj, string propertyName)
        {
            Type classType = typeof(T);
            PropertyInfo info = classType.GetProperty(propertyName);
            return info.GetValue(obj);
        }
    }

    #endregion
}
