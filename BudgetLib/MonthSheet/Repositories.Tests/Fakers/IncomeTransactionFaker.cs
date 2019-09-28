using System;
using System.Collections.Generic;
using Bogus;
using MonthSheet.Common.Models;

namespace MonthSheet.Repositories.Tests.Fakers
{
    public sealed class RawIncomeTransactionFaker: Faker<List<object>>
    {
        public RawIncomeTransactionFaker() => 
        CustomInstantiator(c => new List<object>(){
            new Bogus.Faker().Lorem.Word(), // save note
            new Bogus.Faker().Date.Recent().ToShortDateString(), // transaction date
            $"${new Bogus.Faker().Random.Decimal(min:0, max:1000)}", // net gain
            new Bogus.Faker().Lorem.Sentence(), // detail
            new Bogus.Faker().Lorem.Word(), // for
            new Bogus.Faker().Lorem.Word(), // category
        });
    }
}