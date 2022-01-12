using FeedMeDaddy.Core;
using FeedMeDaddy.Services;
using FeedMeDaddy.Services.Database;
using FeedMeDaddy.Services.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedMeDaddy.ViewModel
{
    class RecipesViewModel : ObservableObject
    {

        public Services.DataContracts.Recipe[] Recipes { get; set; }
        public List<string> RecipeList { get; set; }
        public string RecipeName { get; set; }
        public string RecipeDescription { get; set; }

        public Services.DataContracts.Ingredient[] RecipeIngredients { get; set; }
        public Services.DataContracts.Recipe ActiveRecipe { get; set; }

        public RecipesViewModel()
        {
            LoadRecipes();
            SetupRecipes();
        }

        public void LoadRecipes()
        {
            FeedMeDaddyContext db = new FeedMeDaddyContext();

            var recipes = db.Recipe.Fetch().Where(r => r.User.Id == 1);

            Recipes = new Services.DataContracts.Recipe[recipes.Count()];
            Array.Copy(recipes.ToArray(), Recipes, recipes.Count());

            db.Dispose();
        }
        public void SetupRecipes()
        {
            //ActiveRecipe = Recipes.ElementAt(0);

            RecipeName = "";
            RecipeDescription ="";

            RecipeList = new List<string>();
            //RecipeIngredients = new Services.DataContracts.Ingredient[ActiveRecipe.Ingredients.Count()];
            //Array.Copy(ActiveRecipe.Ingredients.ToArray(), RecipeIngredients, ActiveRecipe.Ingredients.Count());
            foreach (var r in Recipes)
            {
                RecipeList.Add(r.Name);
            }


        }

    }
}
