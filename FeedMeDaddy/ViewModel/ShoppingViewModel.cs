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

		public void AddToShoppingList()
		{
			FeedMeDaddyContext db = new FeedMeDaddyContext();

			db.UpdateShoppingList(ShoppingList);

			db.Dispose();
		}

		public void AddToModel(Ingredient ingredient)
		{
			List<Ingredient> ingredients = FetchIngredients(ingredient);

			ShoppingModel.Ingredients = ingredients;
			ShoppingList.Ingredients.Add(ingredient);
		}

		private List<Ingredient> FetchIngredients(params Ingredient[] addIngredient)
		{
			if (ShoppingList?.Ingredients == null) throw new ArgumentException("ShoppingList is not declared");

			FeedMeDaddyContext db = new FeedMeDaddyContext();
			List<Ingredient> ingredients = new List<Ingredient>();
			WeightConverter converter = new WeightConverter();

			foreach (Ingredient ing in ShoppingList.Ingredients.Concat(addIngredient))
			{
				if (ing.Category == FoodCategory.ShouldNotFigureOnShoppingList) continue;

				if (ingredients.Any(i => i.Name == ing.Name))
				{
					IEnumerable<Ingredient> possibleIngredients = ingredients.Where(i => i.Name == ing.Name);

					foreach (Ingredient possIng in possibleIngredients)
					{
						UnitType typeIng = converter.TypeFor(ing.Unit.Unit);
						UnitType typePossIng = converter.TypeFor(possIng.Unit.Unit);

						if (typeIng == typePossIng)
						{
							UnitWeight.EUnit strdUnit = converter.StandardUnit(typeIng);
							UnitWeight strdWeight = db.UnitWeight.Fetch().FirstOrDefault(u => u.Unit == strdUnit);

							double result = converter.Add(ing.Unit, ing.Quantity, possIng.Unit, possIng.Quantity, strdWeight);

							Ingredient ingToUpdate = ingredients.First(i => i.Equals(possIng));
							ingToUpdate.Quantity = result;
							ingToUpdate.Unit = strdWeight;
						}
						else
						{
							ingredients.Add(ing);
						}
					}
				}
				else
				{
					ingredients.Add(ing);
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
