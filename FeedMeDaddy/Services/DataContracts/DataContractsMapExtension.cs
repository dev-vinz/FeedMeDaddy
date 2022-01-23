using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedMeDaddy.Services.DataContracts
{
    public static class DataContractsMapExtension
    {
        public static Ingredient ToDataContract(this Database.Ingredient ingredient) => new Ingredient
        {
            Id = ingredient.Id,
            Name = ingredient.Name,
            Quantity = ingredient.Quantity,
            Category = (FoodCategory)ingredient.Category,
            Unit = ingredient.UnitNavigation.ToDataContract(),
            ExpirationDate = ingredient.LimitDate
        };

        public static Database.Ingredient ToDatabase(this Ingredient ingredient) => new Database.Ingredient
        {
            Id = ingredient.Id,
            Name = ingredient.Name,
            Quantity = ingredient.Quantity,
            Category = (int)ingredient.Category,
            Unit = (int)ingredient.Unit.Unit,
            LimitDate = ingredient.ExpirationDate
        };

        public static Database.Ingredient ToDatabase(this Ingredient ingredient, DbSet<Database.Ingredient> ingredients)
        {
            return ingredients.FirstOrDefault(i => i.Id == ingredient.Id);
        }

        public static Menu ToDataContract(this Database.Menu menu) => new Menu
        {
            User = menu.UserNavigation.ToDataContract(),
            Date = menu.Date,
            Type = (TypeMenu)menu.Type,
            Recipe = menu.RecipeNavigation?.ToDataContract(),
            CustomRecipe = menu.CustomRecipe
        };

        public static Database.Menu ToDatabase(this Menu menu) => new Database.Menu
        {
            User = menu.User.Id,
            Date = menu.Date,
            Type = (int)menu.Type,
            Recipe = menu.Recipe?.Id,
            CustomRecipe = menu.CustomRecipe
        };

        public static Database.Menu ToDatabase(this Menu menu, DbSet<Database.Menu> menus)
        {
            return menus.FirstOrDefault(m => m.User == menu.User.Id && DateTime.Compare(m.Date, menu.Date) == 0 && m.Type == (int)menu.Type);
        }

        public static Recipe ToDataContract(this Database.Recipe recipe) => new Recipe
        {
            Id = recipe.Id,
            User = recipe.UserNavigation.ToDataContract(),
            Name = recipe.Name,
            Description = recipe.Description,
            NbPersons = recipe.NbPersons,
            Ingredients = recipe.RecipeIngredient.Select(ri => ri.Ingredient.ToDataContract())
        };

        public static Database.Recipe ToDatabase(this Recipe recipe) => new Database.Recipe
        {
            Id = recipe.Id,
            User = recipe.User.Id,
            Name = recipe.Name,
            Description = recipe.Description,
            NbPersons = recipe.NbPersons
        };

        public static Database.Recipe ToDatabase(this Recipe recipe, DbSet<Database.Recipe> recipes)
        {
            return recipes.FirstOrDefault(r => r.Id == recipe.Id && r.User == recipe.User.Id);
        }

        public static ShoppingList ToDataContract(this Database.ShoppingList shoppingList) => new ShoppingList
        {
            User = shoppingList.UserNavigation.ToDataContract(),
            Ingredients = shoppingList.ShoppingIngredient.Select(si => si.Ingredient.ToDataContract()).ToList()
        };

        public static Database.ShoppingList ToDatabase(this ShoppingList shoppingList) => new Database.ShoppingList
        {
            Id = shoppingList.User.Id,
            User = shoppingList.User.Id
        };

        public static Database.ShoppingList ToDatabase(this ShoppingList shoppingList, DbSet<Database.ShoppingList> shoppingLists)
        {
            return shoppingLists.FirstOrDefault(s => s.User == shoppingList.User.Id);
        }

        public static UnitWeight ToDataContract(this Database.UnitWeight unitWeight) => new UnitWeight
        {
            Unit = (UnitWeight.EUnit)unitWeight.Id,
            Shortcut = unitWeight.Shortcut
        };

        public static User ToDataContract(this Database.User user) => new User
        {
            Id = user.Id,
            Username = user.Username,
            Password = user.Password
        };

        public static Database.User ToDatabase(this User user) => new Database.User
        {
            Id = user.Id,
            Username = user.Username,
            Password = user.Password
        };

        public static Database.User ToDatabase(this User user, DbSet<Database.User> users)
        {
            return users.FirstOrDefault(u => u.Id == user.Id);
        }
    }
}
