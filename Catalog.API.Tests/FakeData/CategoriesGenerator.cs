using Bogus;
using Catalog.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.API.Tests.FakeData
{
    internal static class CategoriesGenerator
    {
        public static List<Category> GetFakeCategories(int count)
        {
            var random = new Random();
            var status = new bool[2] { true, false };
            var testCategories = new Faker<Category>()
                .RuleFor(c => c.Name, f => f.Commerce.Categories(random.Next(1, 9))[0])
                .RuleFor(c => c.Description, f => f.Lorem.Text())
                .RuleFor(c => c.Status, status[random.Next(0, 1)]);

            return testCategories.Generate(count);
        }
    }
}
