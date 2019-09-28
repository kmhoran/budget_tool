using System;
using System.Collections.Generic;
using Bogus;
using Bogus.DataSets;
using Moq;
using SheetApi.Common.Interfaces;
using Xunit;

namespace SheetApi.Services.Tests
{
    public class SheetRepositoryBaseTests
    {
        public static readonly TheoryData<string, ISheetApiService> ConstructorTheoryData =
        new TheoryData<string, ISheetApiService>
        {
            {new Faker().Lorem.Word(), null},
            {null, SheetApiMocker.GetService()},
        };

        [Theory]
        [MemberData(nameof(ConstructorTheoryData))]
        public void Constructor_with_NullArguments_throws_ArgumentNullException(
            string sheetId,
            ISheetApiService sheetApi) =>
            Assert.Throws<ArgumentNullException>(
                () => new SheetRepositoryBase(sheetId, sheetApi));

        [Fact]
        public void Constructor_with_NonNullArguments_return_NonNull()
        {
            var response = GetRepoBase();
            Assert.NotNull(response);
            Assert.IsType<SheetRepositoryBase>(response);
        }

        [Fact]
        public void LoadSingle_returns_Single_String()
        {
            var sheetApiMock = SheetApiMocker.GetMock();
            var goodResponse = "good";
            var badResponse = "bad";
            sheetApiMock.Setup(x => x.LoadRange(It.IsAny<string>(), It.IsAny<string>())).Returns(new List<IList<object>>(){
                    new List<object>(){ goodResponse, badResponse, badResponse},
                    new List<object>(){ badResponse, badResponse, badResponse }
                });
            var repo = GetRepoInstance(sheetApi: sheetApiMock.Object);

            var result = repo._LoadSingle(new Faker().Lorem.Word());

            Assert.NotNull(result);
            Assert.IsType<string>(result);
            Assert.Equal(goodResponse, result);

        }

        private static SheetRepositoryBase GetRepoBase(string sheetId = null, ISheetApiService sheetApi = null)
        {
            return new SheetRepositoryBase(sheetId ?? new Faker().Lorem.Word(), sheetApi ?? SheetApiMocker.GetService());
        }

        private static RepoInstance GetRepoInstance(string sheetId = null, ISheetApiService sheetApi = null)
        {
            return new RepoInstance(sheetId ?? new Faker().Lorem.Word(), sheetApi ?? SheetApiMocker.GetService());
        }

        // test class to expose the protected methods in SheetRepositoryBase
        private class RepoInstance : SheetRepositoryBase
        {
            public RepoInstance(string sheetId, ISheetApiService sheetApi) : base(sheetId, sheetApi){ }
            public string _LoadSingle(string range) => LoadSingle(range);
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


}

