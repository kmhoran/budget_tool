using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Options;
using MonthSheet.Common.Models;
using Extentions.Common;
using Xunit;
using Moq;
using MonthSheet.Common.Interfaces;
using SheetApi.Common.Interfaces;

namespace MonthSheet.Repositories.Tests
{
    public class MonthSheetRepositoryTests
    {
        public static readonly TheoryData<IOptions<MonthSheetDetails>, ISheetApiService> ConstructorTheoryData =
        new TheoryData<IOptions<MonthSheetDetails>, ISheetApiService>
        {
            {MonthSheetDetailMocker.GetPassing(), null},
            {MonthSheetDetailMocker.GetWithMissing("SheetId" ), SheetApiMocker.GetService()},
            {MonthSheetDetailMocker.GetWithMissing("TransactionsExpenseRange"), SheetApiMocker.GetService()},
            {MonthSheetDetailMocker.GetWithMissing("TransactionsIncomeRange"), SheetApiMocker.GetService()},
            {MonthSheetDetailMocker.GetWithMissing("GreenExpenseCategoriesRange"), SheetApiMocker.GetService()},
            {MonthSheetDetailMocker.GetWithMissing("GreenIncomeCategoriesRange"), SheetApiMocker.GetService()},
            {MonthSheetDetailMocker.GetWithMissing("RedExpenseCategoriesRange"), SheetApiMocker.GetService()},
            {MonthSheetDetailMocker.GetWithMissing("RedIncomeCategoriesRange"), SheetApiMocker.GetService()},
            {MonthSheetDetailMocker.GetWithMissing("GreenStartingBalance"), SheetApiMocker.GetService()},
            {MonthSheetDetailMocker.GetWithMissing("GreenExpenseProjectionRange"), SheetApiMocker.GetService()},
            {MonthSheetDetailMocker.GetWithMissing("RedExpenseProjectionRange"), SheetApiMocker.GetService()},
            {MonthSheetDetailMocker.GetWithMissing("GreenIncomeProjectionRange"), SheetApiMocker.GetService()},
            {MonthSheetDetailMocker.GetWithMissing("RedIncomeProjectionRange"), SheetApiMocker.GetService()},
            {MonthSheetDetailMocker.GetWithMissing("GreenCurrentBalance"), SheetApiMocker.GetService()},
            {MonthSheetDetailMocker.GetWithMissing("ImbalanceIndexRange"), SheetApiMocker.GetService()},
            {MonthSheetDetailMocker.GetWithMissing("ImbalanceValueRange"), SheetApiMocker.GetService()},
            {MonthSheetDetailMocker.GetWithMissing("ImbalanceStartDate"), SheetApiMocker.GetService()},
            {MonthSheetDetailMocker.GetWithMissing("RedStartingBalance"), SheetApiMocker.GetService()},
            {MonthSheetDetailMocker.GetWithMissing("RedCurrentBalance"), SheetApiMocker.GetService()},
            {MonthSheetDetailMocker.GetWithMissing("TransactionsRange"), SheetApiMocker.GetService()},
            {MonthSheetDetailMocker.GetWithMissing("ImbalanceColumn"), SheetApiMocker.GetService()},
            {MonthSheetDetailMocker.GetWithMissing("RentTotal"), SheetApiMocker.GetService()},
            {MonthSheetDetailMocker.GetWithMissing("GreenName"), SheetApiMocker.GetService()},
            {MonthSheetDetailMocker.GetWithMissing("RedName"), SheetApiMocker.GetService()},
            {MonthSheetDetailMocker.GetWithMissing("GreenMainPageId"), SheetApiMocker.GetService()},
            {MonthSheetDetailMocker.GetWithMissing("RedMainPageId"), SheetApiMocker.GetService()},
            {MonthSheetDetailMocker.GetWithMissing("DataPageId"), SheetApiMocker.GetService()},
            {MonthSheetDetailMocker.GetWithMissing("TransactionPageId"), SheetApiMocker.GetService()},
        };

        [Theory]
        [MemberData(nameof(ConstructorTheoryData))]
        public void Constructor_with_NullArguments_throws_ArgumentNullException(
            IOptions<MonthSheetDetails> details,
            ISheetApiService sheetApi) =>
            Assert.Throws<ArgumentNullException>(
                () => new MonthSheetRepository(details, sheetApi));

        [Fact]
        public void Constructor_with_NonNullArguments_return_NonNull()
        {
            var repo = GetMonthRepo();
            Assert.NotNull(repo);
            Assert.IsType<MonthSheetRepository>(repo);
        }

        [Fact]
        public void LoadTransactions_with_NullSheetApiResponses_returns_EmptyTrnsactionLists()
        {
            var sheetApiMock = SheetApiMocker.GetMock();
            sheetApiMock.Setup(x => x.LoadRange(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((IList<IList<Object>>)null);
            var repo = GetMonthRepo(sheetApi: sheetApiMock.Object);

            var result = repo.LoadTransactions();

            Assert.NotNull(result);
            Assert.NotNull(result.Expenses);
            Assert.NotNull(result.Income);
            Assert.Empty(result.Expenses);
            Assert.Empty(result.Income);
        }

        private static IMonthSheetRepository GetMonthRepo(IOptions<MonthSheetDetails> sheetDetails = null, ISheetApiService sheetApi = null)
        {
            return new MonthSheetRepository(sheetDetails ?? MonthSheetDetailMocker.GetPassing(), sheetApi ?? SheetApiMocker.GetService());
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
            GreenExpenseProjectionRange = "green!C20:C38",
            RedExpenseProjectionRange = "red!C20:C38",
            GreenIncomeProjectionRange = "green!H20:H27",
            RedIncomeProjectionRange = "red!H20:H27",
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

    public static class SheetApiMocker
    {
        public static Mock<ISheetApiService> GetMock()
        {
            return new Mock<ISheetApiService>();
        }

        public static ISheetApiService GetService()
        {
            return new Mock<ISheetApiService>().Object;
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
