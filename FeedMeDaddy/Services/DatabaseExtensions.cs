using FeedMeDaddy.Services.Database;
using FeedMeDaddy.Services.DataContracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Linq;

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

            /*int nbIngredients = db.Ingredient.OrderByDescending(i => i.Id).FirstOrDefault().Id;
            int nbRecipes = db.Recipe.OrderByDescending(r => r.Id).FirstOrDefault().Id;

            db.Database.ExecuteSqlRaw($"DBCC CHECKIDENT ('Ingredient', RESEED, {nbIngredients})");
            db.Database.ExecuteSqlRaw($"DBCC CHECKIDENT ('Recipe', RESEED, {nbRecipes})");*/
        }

        #region Fetch Extensions

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

        #endregion

        #region Add Extensions

        public static EntityEntry<Database.Ingredient> Add(this DbSet<Database.Ingredient> dbSet, DataContracts.Ingredient ingredient)
        {
            return dbSet.Add(ingredient.ToDatabase());
        }

        public static EntityEntry<Database.Menu> Add(this DbSet<Database.Menu> dbSet, DataContracts.Menu menu)
        {
            return dbSet.Add(menu.ToDatabase());
        }

        public static EntityEntry<Database.Recipe> AddRecipe(this FeedMeDaddyContext db, DataContracts.Recipe recipe)
        {
            EntityEntry<Database.Recipe> recipeAdded = db.Recipe.Add(recipe.ToDatabase());

            /*int nbIngredients = db.Ingredient.OrderByDescending(i => i.Id).FirstOrDefault().Id;
            int nbRecipes = db.Recipe.OrderByDescending(r => r.Id).FirstOrDefault().Id;
            db.Database.ExecuteSqlRaw($"DBCC CHECKIDENT ('Ingredient', RESEED, {nbIngredients})");
            db.Database.ExecuteSqlRaw($"DBCC CHECKIDENT ('Recipe', RESEED, {nbRecipes})");*/

            foreach (DataContracts.Ingredient ingredient in recipe.Ingredients)
            {
                db.Ingredient.Add(ingredient.ToDatabase());

                RecipeIngredient entity = new RecipeIngredient
                {
                    RecipeId = recipe.Id,
                    IngredientId = ingredient.Id
                };

                db.RecipeIngredient.Add(entity);
            }

            return recipeAdded;
        }

        public static EntityEntry<Database.User> Add(this DbSet<Database.User> dbSet, DataContracts.User user)
        {
            return dbSet.Add(user.ToDatabase());
        }

        #endregion

        #region Update Extensions

        public static void UpdateRecipe(this FeedMeDaddyContext db, DataContracts.Recipe recipe)
        {
            // First delete the old recipe
            db.DeleteRecipe(recipe);

            // Add the new updated recipe
            db.AddRecipe(recipe);
        }

        public static void UpdateShoppingList(this FeedMeDaddyContext db, DataContracts.ShoppingList shoppingList)
        {
            int shoppingId = shoppingList.ToDatabase().Id;
            IEnumerable<ShoppingIngredient> currentIngredients = db.ShoppingIngredient.Where(si => si.ShoppingId == shoppingId).AsEnumerable();

            foreach (ShoppingIngredient shoppingIngredient in currentIngredients)
            {
                Database.Ingredient ingredient = db.Ingredient.FirstOrDefault(i => i.Id == shoppingIngredient.IngredientId);
                db.Ingredient.Remove(ingredient);
            }

            db.SaveChanges();

            /*int nbIngredients = db.Ingredient.OrderByDescending(i => i.Id).FirstOrDefault().Id;
            db.Database.ExecuteSqlRaw($"DBCC CHECKIDENT ('Ingredient', RESEED, {nbIngredients})");*/

            foreach (DataContracts.Ingredient ingredient in shoppingList.Ingredients)
            {
                ingredient.Id = 0;
                EntityEntry<Database.Ingredient> ingredientAdded = db.Ingredient.Add(ingredient.ToDatabase());
                
                db.SaveChanges();

                ShoppingIngredient entity = new ShoppingIngredient
                {
                    ShoppingId = shoppingId,
                    IngredientId = ingredientAdded.Entity.Id
                };

                db.ShoppingIngredient.Add(entity);
                db.SaveChanges();
            }
        }

        #endregion

        #region Delete Extensions

        public static void DeleteRecipe(this FeedMeDaddyContext db, DataContracts.Recipe recipe)
        {
            // Delete the recipe and the association (CASCADE)
            Database.Recipe oldRecipe = db.Recipe.FirstOrDefault(r => r.Id == recipe.Id);
            db.Recipe.Remove(oldRecipe);

            // But we have to delete the ingredients manually
            foreach (RecipeIngredient recipeIngredient in oldRecipe.RecipeIngredient)
            {
                Database.Ingredient entity = db.Ingredient.FirstOrDefault(i => i.Id == recipeIngredient.IngredientId);
                db.Ingredient.Remove(entity);
            }
        }

        public static void RemoveRange(this DbSet<Database.Menu> menuSet, params DataContracts.Menu[] entities)
        {
            foreach (DataContracts.Menu menu in entities)
            {
                menuSet.Remove(menu.ToDatabase(menuSet));
            }
        }

        #endregion

        #region Remove Range Extensions

        public static void RemoveRange(this DbSet<Database.Ingredient> table, IEnumerable<DataContracts.Ingredient> entities)
		{
            foreach (DataContracts.Ingredient entity in entities)
			{
                table.Remove(entity.ToDatabase(table));
			}
		}

        #endregion
    }
}
