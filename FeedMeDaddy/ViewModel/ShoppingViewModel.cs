using FeedMeDaddy.Core;
using FeedMeDaddy.Extensions;
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
			IEnumerable<UnitWeight> units = db.UnitWeight.Fetch().Clone();
			db.Dispose();

			//ShoppingList.Ingredients = FetchIngredients();
			ShoppingModel = new ShoppingModel(FetchIngredients(), units.ToArray());
		}

		public void AddToShoppingList()
		{
			FeedMeDaddyContext db = new FeedMeDaddyContext();

			db.UpdateShoppingList(ShoppingList);

			db.Dispose();
		}

		public void AddToModel(Ingredient ingredient)
		{
			IEnumerable<Ingredient> ingredients = FetchIngredients(ingredient);

			ShoppingModel.Ingredients = ingredients.Clone();
			ShoppingList.Ingredients.Add(ingredient);
			ShoppingModel.UpdateExpirationToMenuDate();
		}

		public void RemoveFromModel(Ingredient ingredient)
		{
			WeightConverter converter = new WeightConverter();

			ShoppingModel.Ingredients = ShoppingModel.Ingredients.Where(i => i != ingredient);

			ShoppingList.Ingredients = ShoppingList.Ingredients.Where(i => !(i.Name == ingredient.Name && converter.TypeFor(i.Unit.Unit) == converter.TypeFor(ingredient.Unit.Unit))).ToList();
			ShoppingModel.UpdateExpirationToMenuDate();
		}

		private List<Ingredient> FetchIngredients(params Ingredient[] addIngredient)
		{
			if (ShoppingList?.Ingredients == null) throw new ArgumentException("ShoppingList is not declared");

			FeedMeDaddyContext db = new FeedMeDaddyContext();
			List<Ingredient> ingredients = new List<Ingredient>();
			WeightConverter converter = new WeightConverter();

			foreach (Ingredient ing in ShoppingList.Ingredients.Concat(addIngredient).Clone())
			{
				if (ing.Category == FoodCategory.ShouldNotFigureOnShoppingList) continue;

				if (ingredients.Any(i => i.Name == ing.Name))
				{
					Ingredient possibleIngredient = ingredients.Where(i => i.Name == ing.Name)
						.Clone()
						.FirstOrDefault(i => converter.TypeFor(i.Unit.Unit) == converter.TypeFor(ing.Unit.Unit));

					if (possibleIngredient != null)
					{
						UnitType typeIng = converter.TypeFor(ing.Unit.Unit);

						UnitWeight.EUnit strdUnit = converter.StandardUnit(typeIng);
						UnitWeight strdWeight = db.UnitWeight.Fetch().FirstOrDefault(u => u.Unit == strdUnit);

						double result = converter.Add(ing.Unit, ing.Quantity, possibleIngredient.Unit, possibleIngredient.Quantity, strdWeight);

						Ingredient ingToUpdate = ingredients.First(i => i.Equals(possibleIngredient));
						ingToUpdate.Quantity = result;
						ingToUpdate.Unit = strdWeight;
					}
					else
					{
						ingredients.Add(new Ingredient(ing));
					}
				}
				else
				{
					ingredients.Add(new Ingredient(ing));
				}
			}

			db.Dispose();
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
