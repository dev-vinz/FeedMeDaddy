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

        public List<string>recipeList { get; set; }
        public string recipeName { get; set; }
        public string recipeDescription { get; set; }
        public string recipeIngredients { get; set; }

        public RecipesViewModel()
        {
            /*
            var db = new FeedMeDaddyContext();
            var recipes = db.Recipe.Where(r => r.UserNavigation.Id == 1);
            recipeList = recipes.FirstOrDefault().Name;
            recipeName = recipes.FirstOrDefault().Name;
            recipeDescription = string.Format(recipes.FirstOrDefault().Description);
            db.Dispose();*/

            var db = new FeedMeDaddyContext();

            var recipes = db.Recipe.Fetch().Where(r => r.User.Id == 1);
            recipeName = recipes.ElementAt(0).Name;
            recipeDescription = recipes.ElementAt(0).Description;
            recipeList = new List<string>();
            
            foreach(var r in recipes)
            {
                recipeList.Add(r.Name);
            }
            
            db.Dispose();


        }


    }
}
