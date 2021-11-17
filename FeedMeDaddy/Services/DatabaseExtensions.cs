using FeedMeDaddy.Services.Database;
using FeedMeDaddy.Services.DataContracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedMeDaddy.Services
{
    public static class DatabaseExtensions
    {
        public static void Load(this FeedMeDaddyContext db)
        {
            db.FoodCategory.Load();
            db.Fridge.Load();
            db.Ingredient.Load();
            db.Menu.Load();
            db.Recipe.Load();
            db.RecipeIngredient.Load();
            db.ShoppingIngredient.Load();
            db.ShoppingList.Load();
            db.TypeMenu.Load();
            db.UnitWeight.Load();
            db.User.Load();
        }

        public static IEnumerable<DataContracts.Ingredient> Fetch(this DbSet<Database.Ingredient> ingredients)
        {
            return ingredients.Select(i => i.ToDataContract()).AsEnumerable();
        }

        public static IEnumerable<DataContracts.Menu> Fetch(this DbSet<Database.Menu> menus)
        {
            return menus.Select(m => m.ToDataContract()).AsEnumerable();
        }

        public static IEnumerable<DataContracts.Recipe> Fetch(this DbSet<Database.Recipe> recipes)
        {
            return recipes.Select(r => r.ToDataContract()).AsEnumerable();
        }

        public static IEnumerable<DataContracts.ShoppingList> Fetch(this DbSet<Database.ShoppingList> shoppingLists)
        {
            return shoppingLists.Select(s => s.ToDataContract()).AsEnumerable();
        }

        public static IEnumerable<DataContracts.UnitWeight> Fetch(this DbSet<Database.UnitWeight> unitWeights)
        {
            return unitWeights.Select(u => u.ToDataContract()).AsEnumerable();
        }

        public static IEnumerable<DataContracts.User> Fetch(this DbSet<Database.User> users)
        {
            return users.Select(u => u.ToDataContract()).AsEnumerable();
        }
    }
}
