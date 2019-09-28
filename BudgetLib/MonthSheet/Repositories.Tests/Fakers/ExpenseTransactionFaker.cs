using System;
using System.Collections.Generic;
using Bogus;
using MonthSheet.Common.Models;

namespace MonthSheet.Repositories.Tests.Fakers
{
    public sealed class RawExpenseTransactionFaker: Faker<List<Object>>
    {
        public RawExpenseTransactionFaker() => 
        CustomInstantiator(c => new List<object>(){
            new Bogus.Faker().Lorem.Word(), // save note
            new Bogus.Faker().Date.Recent().ToShortDateString(), // transaction date
            $"${new Bogus.Faker().Random.Decimal(min:0, max:1000)}", // net cost
            new Bogus.Faker().Lorem.Sentence(), // detail
            new Bogus.Faker().Lorem.Word(), // by
            new Bogus.Faker().Lorem.Word(), // for
            new Bogus.Faker().Lorem.Word(), // category
        });
    }
}