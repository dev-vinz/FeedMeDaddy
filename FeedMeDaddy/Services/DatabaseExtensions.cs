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

        public static void UpdateMenu(this FeedMeDaddyContext db, DataContracts.Menu menu)
        {
            // First delete the old Menu
            db.DeleteMenu(menu);
            // Add the new updated menu
            db.AddMenu(menu);

        }

        public static EntityEntry<Database.Menu> AddMenu(this FeedMeDaddyContext db, DataContracts.Menu menu)
        {
            EntityEntry<Database.Menu> menuAdded = db.Menu.Add(menu.ToDatabase());


            db.SaveChanges();

           
            return menuAdded;
        }
        public static EntityEntry<Database.Recipe> AddRecipe(this FeedMeDaddyContext db, DataContracts.Recipe recipe)
        {
            EntityEntry<Database.Recipe> recipeAdded = db.Recipe.Add(recipe.ToDatabase());

            /*int nbIngredients = db.Ingredient.OrderByDescending(i => i.Id).FirstOrDefault().Id;
            int nbRecipes = db.Recipe.OrderByDescending(r => r.Id).FirstOrDefault().Id;
            db.Database.ExecuteSqlRaw($"DBCC CHECKIDENT ('Ingredient', RESEED, {nbIngredients})");
            db.Database.ExecuteSqlRaw($"DBCC CHECKIDENT ('Recipe', RESEED, {nbRecipes})");*/

            db.SaveChanges();

            foreach (DataContracts.Ingredient ingredient in recipe.Ingredients)
            {
                EntityEntry<Database.Ingredient> ingredientAdded = db.Ingredient.Add(ingredient.ToDatabase());
                db.SaveChanges();

                RecipeIngredient entity = new RecipeIngredient
                {
                    RecipeId = recipeAdded.Entity.Id,
                    IngredientId = ingredientAdded.Entity.Id
                };

                db.RecipeIngredient.Add(entity);
                db.SaveChanges();
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
        public static EntityEntry<Database.Menu> Update(this DbSet<Database.Menu> table, DataContracts.Menu entity)
        {
            Database.Menu menu = entity.ToDatabase(table);
            menu.Recipe = entity.Recipe?.Id;
            menu.CustomRecipe = entity.CustomRecipe;

            return table.Update(menu);
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
        public static void DeleteMenu(this FeedMeDaddyContext db, DataContracts.Menu menu)
        {
            Database.Menu oldmenu = db.Menu.FirstOrDefault(m => (m.Date == menu.Date && (m.Type.Equals(menu.Type))));
            db.Menu.Remove(oldmenu);

            db.SaveChanges();

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

		#region Export Shopping List Extensions

        public static string[] ExportToFile(this IEnumerable<DataContracts.Ingredient> ingredients)
		{
            IEnumerable<DataContracts.Ingredient> ingredientsOrdered = ingredients.OrderBy(i => i.Name).ThenBy(i => i.Unit.Shortcut);

            List<string> lines = new List<string>();

            IEnumerable<DataContracts.Ingredient> vegetable = ingredientsOrdered.Where(i => i.Category == DataContracts.FoodCategory.Vegetable);
            IEnumerable<DataContracts.Ingredient> meat = ingredientsOrdered.Where(i => i.Category == DataContracts.FoodCategory.Meat);
            IEnumerable<DataContracts.Ingredient> fish = ingredientsOrdered.Where(i => i.Category == DataContracts.FoodCategory.Fish);
            IEnumerable<DataContracts.Ingredient> fruit = ingredientsOrdered.Where(i => i.Category == DataContracts.FoodCategory.Fruit);
            IEnumerable<DataContracts.Ingredient> dairy = ingredientsOrdered.Where(i => i.Category == DataContracts.FoodCategory.Dairy);
            IEnumerable<DataContracts.Ingredient> other = ingredientsOrdered.Where(i => i.Category == DataContracts.FoodCategory.Other);

            IEnumerable<DataContracts.Ingredient>[] finalTab = new IEnumerable<DataContracts.Ingredient>[]
			{
                vegetable,
                meat,
                fish,
                fruit,
                dairy,
                other,
			};

            foreach (IEnumerable<DataContracts.Ingredient> list in finalTab)
			{
                if (list.Count() < 1) continue;

                DataContracts.FoodCategory category = list.ElementAt(0).Category;
                string value = $"{category} :";

                foreach (DataContracts.Ingredient ing in list)
				{
                    value += $"\n\t{ing.Name} {ing.FullUnit}";
				}

                value += "\n";

                lines.Add(value);
			}

            return lines.ToArray();
		}

		#endregion
	}
}
