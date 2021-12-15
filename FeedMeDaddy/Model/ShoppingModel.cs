using FeedMeDaddy.Core;
using FeedMeDaddy.Services;
using FeedMeDaddy.Services.DataContracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FeedMeDaddyContext = FeedMeDaddy.Services.Database.FeedMeDaddyContext;

namespace FeedMeDaddy.Model
{
    public class ShoppingModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Ingredient[] _ingredients;
        private UnitWeight[] _units;
        private DateTime[] _dateMenus;
        public FoodCategory[] _categories;

        public IEnumerable<Ingredient> Ingredients
        {
            get { return _ingredients; }
            set
            {
                _ingredients = value.ToArray();
                RaisePropertyChanged("Ingredients");
            }
        }

        public UnitWeight[] Units
        {
            get { return _units; }
            set
            {
                _units = value;
                RaisePropertyChanged("Units");
            }
        }

        public IEnumerable<DateTime> DateMenus
        {
            get { return _dateMenus; }
            set
            {
                _dateMenus = value.ToArray();
                RaisePropertyChanged("DateMenus");
            }
        }

        public FoodCategory[] Categories
        {
            get { return _categories; }
            set
            {
                _categories = value;
                RaisePropertyChanged("Categories");
            }
        }

        public ShoppingModel(IEnumerable<Ingredient> ingredients, UnitWeight[] units)
        {
            Ingredients = ingredients;
            Units = units;
            Categories = GetCategories();

            UpdateExpirationToMenuDate();
        }

        private void UpdateExpirationToMenuDate()
        {
            FeedMeDaddyContext db = new FeedMeDaddyContext();

            IEnumerable<Menu> menus = db.Menu.Fetch();
            Menu[] copyMenu = new Menu[menus.Count()];

            Array.Copy(menus.ToArray(), copyMenu, menus.Count());

            db.Dispose();

            foreach (Ingredient ingredient in Ingredients)
            {
                DateTime? maxDate = null;

                foreach (Menu menu in copyMenu)
                {
                    DateTime? menuDate = menu.Date;

                    // Chercher si l'ingrédient se trouve dans ce menu
                    if (menu.Recipe?.Ingredients.Any(i => i.Name == ingredient.Name) ?? false)
                    {
                        if (menuDate > maxDate || maxDate == null)
                        {
                            maxDate = menuDate;
                        }
                    }
                }

                ingredient.ExpirationDate = maxDate;
            }
        }

        private FoodCategory[] GetCategories()
        {
            return Enum.GetValues(typeof(FoodCategory)).Cast<FoodCategory>().ToArray();
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
