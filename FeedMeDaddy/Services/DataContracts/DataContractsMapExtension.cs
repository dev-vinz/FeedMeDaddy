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

        public static Menu ToDataContract(this Database.Menu menu) => new Menu
        {
            User = menu.UserNavigation.ToDataContract(),
            Date = menu.Date,
            Type = (TypeMenu)menu.Type,
            Recipe = menu.RecipeNavigation?.ToDataContract(),
            CustomRecipe = menu.CustomRecipe
        };

        public static Recipe ToDataContract(this Database.Recipe recipe) => new Recipe
        {
            Id = recipe.Id,
            User = recipe.UserNavigation.ToDataContract(),
            Name = recipe.Name,
            Description = recipe.Description,
            NbPersons = recipe.NbPersons,
            Ingredients = recipe.RecipeIngredient.Select(ri => ri.Ingredient.ToDataContract())
        };

        public static ShoppingList ToDataContract(this Database.ShoppingList shoppingList) => new ShoppingList
        {
            User = shoppingList.UserNavigation.ToDataContract(),
            Ingredients = shoppingList.ShoppingIngredient.Select(si => si.Ingredient.ToDataContract())
        };

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
    }
}
