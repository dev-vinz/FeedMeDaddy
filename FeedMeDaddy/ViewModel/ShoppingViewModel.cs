using FeedMeDaddy.Core;
using FeedMeDaddy.Model;
using FeedMeDaddy.Services;
using FeedMeDaddy.Services.DataContracts;
using FeedMeDaddy.Services.Units;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FeedMeDaddyContext = FeedMeDaddy.Services.Database.FeedMeDaddyContext;

namespace FeedMeDaddy.ViewModel
{
    class ShoppingViewModel : ObservableObject
    {
        private ShoppingModel _shoppingModel;
        private ShoppingList _shoppingList;

        public ShoppingModel ShoppingModel
        {
            get { return _shoppingModel; }
            set
            {
                _shoppingModel = value;
                OnPropertyChanged("ShoppingModel");
            }
        }

        public ShoppingList ShoppingList
        {
            get { return _shoppingList; }
            set
            {
                _shoppingList = value;
                OnPropertyChanged("ShoppingList");
            }
        }

        public ShoppingViewModel()
        {
            FeedMeDaddyContext db = new FeedMeDaddyContext();
            ShoppingList = GetShoppingList();
            IEnumerable<UnitWeight> units = db.UnitWeight.Fetch();

            UnitWeight[] unitWeights = new UnitWeight[units.Count()];
            Array.Copy(units.ToArray(), unitWeights, units.Count());

            db.Dispose();

            ShoppingList.Ingredients = FetchIngredients();
            ShoppingModel = new ShoppingModel(ShoppingList.Ingredients, unitWeights);
        }

        public void AddToShoppingList(Ingredient ingredient)
        {
        }

        public void AddToModel(Ingredient ingredient)
        {
            List<Ingredient> ingredients = FetchIngredients(ingredient);

            ShoppingModel.Ingredients = ingredients;
            ShoppingList.Ingredients = ingredients;
        }

        private List<Ingredient> FetchIngredients(params Ingredient[] addIngredient)
        {
            if (ShoppingList?.Ingredients == null) throw new ArgumentException("ShoppingList is not declared");

            List<Ingredient> ingredients = new List<Ingredient>();
            WeightConverter converter = new WeightConverter();

            foreach (Ingredient ing in ShoppingList.Ingredients.Concat(addIngredient))
            {
                if (ingredients.Any(i => i.Name == ing.Name))
                {

                }
                else
                {
                    ingredients.Add(ing);
                }
            }

            return ingredients;
        }

        private ShoppingList GetShoppingList()
        {
            FeedMeDaddyContext db = new FeedMeDaddyContext();
            ShoppingList shoppingList = db.ShoppingList.Fetch().FirstOrDefault(s => s.User.Id == 1);
            db.Dispose();

            return shoppingList;
        }
    }
}
