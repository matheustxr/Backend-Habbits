using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using Habbits.Domain.Entities;

namespace CommonTestUtilities.Entities
{
    public class HabitCategoryBuilder
    {
        public static HabitCategory Build()
        {
            return new Faker<HabitCategory>()
                .RuleFor(c => c.Id, _ => 1)
                .RuleFor(c => c.Category, faker => faker.Commerce.Department());
        }
    }
}
