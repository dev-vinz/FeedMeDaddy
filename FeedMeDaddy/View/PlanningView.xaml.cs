using FeedMeDaddy.Services.DataContracts;
using FeedMeDaddy.Services.Units;
using FeedMeDaddy.Core;
using FeedMeDaddy.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using FeedMeDaddyContext = FeedMeDaddy.Services.Database.FeedMeDaddyContext;
using System.Windows.Media;
using Menu = FeedMeDaddy.Services.DataContracts.Menu;
using System.Globalization;

namespace FeedMeDaddy.View
{
    /// <summary>
    /// Logique d'interaction pour PlanningView.xaml
    /// </summary>
    public partial class PlanningView : UserControl
    {
        List<ComboBox> Breakfast = new List<ComboBox>();
        List<ComboBox> Dinner = new List<ComboBox>();
        List<ComboBox> Supper = new List<ComboBox>();
        ShoppingList shoppingList = GetShoppingListSelection();

        public PlanningView()
        {
            InitializeComponent();
            Initialize();
            InitializeBlackout();
        }




        private void InitializeBlackout()
        {
            //remove all date in the past forme the datepicker
            dueDate.BlackoutDates.AddDatesInPast();
            dueDate.BlackoutDates.Add(new CalendarDateRange(DateTime.Now.AddDays(-1)));
        }
        private void item_TextChanged(object sender, TextChangedEventArgs e)
        {
            string pattern = @"[^A-Za-zÀ-ÖØ-öø-ÿ-' ]";
            RegexOptions options = RegexOptions.IgnoreCase;
            item.Text = Regex.Replace(item.Text, pattern, "", options);
            item.SelectionStart = item.Text.Length;
            item.SelectionLength = 0;
            item.Focus();
        }
        private static ShoppingList GetShoppingListSelection()
        {

            FeedMeDaddyContext db = new FeedMeDaddyContext();

            var recipies = db.ShoppingList.Fetch().FirstOrDefault(s => s.User.Id == 1);

            db.Dispose();
            return recipies;


        }

        private void Initialize()
        {
            //We put all the combobox in a List, that correspond with when in the day is the meal
            //it's easier to find them, the offset in the list, is the same for the date
            Breakfast.Add(B0);
            Breakfast.Add(B1);
            Breakfast.Add(B2);
            Breakfast.Add(B3);
            Breakfast.Add(B4);
            Breakfast.Add(B5);
            Breakfast.Add(B6);

            Dinner.Add(D0);
            Dinner.Add(D1);
            Dinner.Add(D2);
            Dinner.Add(D3);
            Dinner.Add(D4);
            Dinner.Add(D5);
            Dinner.Add(D6);

            Supper.Add(S0);
            Supper.Add(S1);
            Supper.Add(S2);
            Supper.Add(S3);
            Supper.Add(S4);
            Supper.Add(S5);
            Supper.Add(S6);

            //Set default values for the combobox units and Category
            units.SelectedIndex = 0;
            Category.SelectedIndex = 5;

            FeedMeDaddyContext db = new FeedMeDaddyContext();

            //Delete all past menus
            var menusToDelete = db.Menu.Fetch().Where(m => m.User.Id == 1 && m.Date < DateTime.Today);
            db.Menu.RemoveRange(menusToDelete.ToArray());
            db.SaveChanges();

            //Fetch all breakfast, dinner and supper
            IEnumerable<Menu> menusBreakfast = db.Menu.Fetch().Where(m => m.User.Id == 1 && m.Type == TypeMenu.Breakfast).OrderBy(m => m.Date);
            IEnumerable<Menu> menusDinner = db.Menu.Fetch().Where(m => m.User.Id == 1 && m.Type == TypeMenu.Dinner).OrderBy(m => m.Date);
            IEnumerable<Menu> menusSupper = db.Menu.Fetch().Where(m => m.User.Id == 1 && m.Type == TypeMenu.Supper).OrderBy(m => m.Date);

            db.SaveChanges();

            //Search in the DB if the menu is already placed in the week

            addInitialMenuType(menusBreakfast, Breakfast);

            addInitialMenuType(menusDinner, Dinner);

            addInitialMenuType(menusSupper, Supper);

            //re-add all ingredient in shopping list
            foreach (Ingredient ingre in shoppingList.Ingredients)
            {
                IngredientToShoppingList(ingre);
            }
            db.Dispose();
        }
        private void addInitialMenuType(IEnumerable<Menu> MenuType, List<ComboBox> ListType)
        {
            //Compare each date of the breakfast in the menu and the day of week
            foreach (Menu type in MenuType)
            {
                int datei = 0;

                foreach (ComboBox combo in ListType)
                {
                    if (DateTime.Compare(type.Date, GetDate(datei)) == 0)
                    {
                        combo.Text = type.Recipe?.Name ?? type.CustomRecipe;
                    }
                    datei++;
                }
            }
        }
        private void Concat_ComboBox()
        {
            foreach (var item in Breakfast)
            {
                Concat_Text(item);
            }
            foreach (var item in Dinner)
            {
                Concat_Text(item);
            }
            foreach (var item in Supper)
            {
                Concat_Text(item);
            }
        }
        private void Concat_Text(ComboBox item)
        {
            string pattern = @"[^A-Za-zÀ-ÖØ-öø-ÿ-' ]";
            RegexOptions options = RegexOptions.IgnoreCase;
            if (item.Text.Any(char.IsDigit) || item.Text.Any(char.IsPunctuation) || item.Text.Any(char.IsControl) || item.Text.Any(char.IsSymbol))
            {
                item.Text = Regex.Replace(item.Text, pattern, "", options);
            }
        }
        private void IngredientToShopping_Click(object sender, RoutedEventArgs e)
        {

            var ing = new Ingredient
            {
                Name = item.Text,
                Quantity = (double)quantity.Value,
                Unit = (UnitWeight)units.SelectedItem,
                ExpirationDate = dueDate.SelectedDate,
                Category = (FoodCategory)Category.SelectedItem,
            };
            //Adding ingredient to the shopping list and the database
            IngredientToShoppingList(ing);
            addIngredientToDB(ing);

        }

        private void MenuShopping_Click(object sender, RoutedEventArgs e)
        {
            //Fetching all the menus from the DB, filtered by typeMenu
            FeedMeDaddyContext db = new FeedMeDaddyContext();
            IEnumerable<Menu> menusBreakfast = db.Menu.Fetch().Where(m => m.User.Id == 1 && m.Type == TypeMenu.Breakfast).OrderBy(m => m.Date);
            IEnumerable<Menu> menusDinner = db.Menu.Fetch().Where(m => m.User.Id == 1 && m.Type == TypeMenu.Dinner).OrderBy(m => m.Date);
            IEnumerable<Menu> menusSupper = db.Menu.Fetch().Where(m => m.User.Id == 1 && m.Type == TypeMenu.Supper).OrderBy(m => m.Date);
            db.SaveChanges();

            Recipe[] recipes = GetRecipeSelection();
            //           DateTime[] dates = GetWeekdays();

            CheckIngredientInDB(Breakfast, menusBreakfast, recipes, TypeMenu.Breakfast);

            CheckIngredientInDB(Dinner, menusDinner, recipes, TypeMenu.Dinner);

            CheckIngredientInDB(Supper, menusSupper, recipes, TypeMenu.Supper);

            GetMenuFromCombobox();
        }
        private void CheckIngredientInDB(List<ComboBox> MenuType, IEnumerable<Menu> menu, Recipe[] recipes, TypeMenu typeMenu)
        {
            int b = 0;
            foreach (ComboBox combo in MenuType)
            {
                if (!string.IsNullOrWhiteSpace(combo.Text))
                {
                    //If the menu already existe in the DB with the same name and the same type
                    if (menu.Any(r => (r.Recipe?.Name ?? r.CustomRecipe).Equals(combo.Text)&& r.Type == typeMenu && r.Date == GetDate(b) ))
                    {
                        //They are the same so we must not add the ingredients
                    }
                    else
                    {
                        GetIngredientFromMenu(recipes, combo);
                    }

                }

                b++;

            }
        }
        private static Recipe[] GetRecipeSelection()
        {

            FeedMeDaddyContext db = new FeedMeDaddyContext();

            var recipies = db.Recipe.Fetch();
            var recipiesName = new Recipe[recipies.Count()];

            Array.Copy(recipies.ToArray(), recipiesName, recipies.Count());
            db.Dispose();
            return recipiesName;

        }
        private void GetIngredientFromMenu(Recipe[] recipes, ComboBox combo)
        {
            //If the recipe is from the DB we need to add the ingredients to the shopping list, else its a custom recipe with no ingredients
            for (int i = 0; i < recipes.Length; i++)
            {

                if (recipes[i].Name.Equals(combo.Text))
                {
                    foreach (Ingredient ingredient in recipes[i].Ingredients)
                    {
                        //Adding ingredient to the DB and to the shopping list
                        IngredientToShoppingList(ingredient);
                        addIngredientToDB(ingredient);
                    }


                }
            }
        }
        private void IngredientToShoppingList(Ingredient ing)
        {
            if (ing.Category == FoodCategory.ShouldNotFigureOnShoppingList)
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(ing.Name))
            {
                IngredientRequired.Visibility = Visibility.Visible;
                item.BorderBrush = Brushes.Red;
            }
            else
            {
                IngredientRequired.Visibility = Visibility.Collapsed;
                item.BorderBrush = Brushes.Gray;
                //Need to check if ingredient already existe in the shopping list
                int previewSize = previewShopping.Items.Count;
                bool inList = false;
                WeightConverter converter = new WeightConverter();
                for (int i = 0; i < previewSize; i++)
                {
                    Ingredient myIng = previewShopping.Items.GetItemAt(i) as Ingredient;
                    double quantitiyMyIng = myIng.Quantity;

                    if (myIng.Name.Equals(ing.Name) && (!inList)) //They have the same name
                    {
                        //If the unit is the same we can just add them together before adding them in the shopping list
                        if (myIng.Unit.Shortcut.Equals(ing.Unit.Shortcut))
                        {

                            previewShopping.Items.Remove(myIng);
                            myIng.Quantity = quantitiyMyIng + ing.Quantity;
                            previewShopping.Items.Add(myIng);
                            inList = true;

                        }
                        else //if the unit is different but still in the same Weight (kg -> g or g -> Kg) we can convert
                        {
                            double tempQuantity = 0;
                            bool sameType = true;
                            try
                            {
                                tempQuantity = converter.Add(ing.Unit, ing.Quantity, myIng.Unit, myIng.Quantity, myIng.Unit);
                            }
                            catch
                            {
                                sameType = false;
                            }
                            if (sameType)
                            {
                                previewShopping.Items.Remove(myIng);
                                myIng.Quantity = tempQuantity;
                                previewShopping.Items.Add(myIng);
                                inList = true;
                            }

                        }


                    }
                }
                if (!inList) //if it's not in the list we can simply add it
                {

                    previewShopping.Items.Add(ing);
                }
            }

        }
        private void addIngredientToDB(Ingredient ing)
        {
            if (!string.IsNullOrWhiteSpace(ing.Name))
            {
                FeedMeDaddyContext db = new FeedMeDaddyContext();
                var list = db.ShoppingList.Fetch().FirstOrDefault(s => s.User.Id == 1);
                list.Ingredients.Add(ing);
                db.UpdateShoppingList(list);

                db.Dispose();
            }
        }
        private static DateTime[] GetWeekdays()
        {
            DateTime dateTime = DateTime.Now.Date;
            DateTime[] weekdays = new DateTime[7];
            for (int i = 0; i < 7; i++)
            {
                weekdays[i] = dateTime;
                dateTime = dateTime.AddDays(1);
            }
            return weekdays;
        }
        private static DateTime GetDate(int current)
        {
            DateTime[] weekdays = GetWeekdays();
            return weekdays[current];
        }
        private Menu[] GetMenuSelection()
        {
            FeedMeDaddyContext db = new FeedMeDaddyContext();

            var dbMenus = db.Menu.Fetch();

            Menu[] menus = new Menu[dbMenus.Count()];
            Array.Copy(dbMenus.ToArray(), menus, dbMenus.Count());

            db.Dispose();

            return menus;
        }
        private void GetMenuFromCombobox()
        {

            Recipe[] recipes = GetRecipeSelection();
            Menu[] menuSelection = GetMenuSelection();


            CheckMenuFromCombobox(recipes, menuSelection, Breakfast, TypeMenu.Breakfast);
            CheckMenuFromCombobox(recipes, menuSelection, Dinner, TypeMenu.Dinner);
            CheckMenuFromCombobox(recipes, menuSelection, Supper, TypeMenu.Supper);
        }

        private void CheckMenuFromCombobox(IEnumerable<Recipe> recipes, Menu[] menuSelection, List<ComboBox> MenuType, TypeMenu typeMenu)
        {
            int b = 0;
            foreach (ComboBox combo in MenuType)
            {
                if (!string.IsNullOrWhiteSpace(combo.Text))
                {
                    var menu = new Menu
                    {
                        User = new User
                        {
                            Id = 1
                        },
                        Date = GetDate(b),
                        Type = typeMenu
                    };
                    //If the recipe is known it's a recipe, if it's not known it's a customRecipe
                    if (recipes.Any(r => r.Name == combo.Text))
                    {
                        menu.Recipe = recipes.FirstOrDefault(r => r.Name == combo.Text);
                    }
                    else
                    {
                        menu.CustomRecipe = combo.Text;
                    }


                    if (menuSelection.Any(da => DateTime.Compare(menu.Date, da.Date) == 0 && da.Type == menu.Type))
                    {
                        // On récupère le menu avec la même date et le même type
                        Menu dateMenu = menuSelection.FirstOrDefault(da => DateTime.Compare(menu.Date, da.Date) == 0 && da.Type == menu.Type);

                        // On sait que les dates et les types sont exacts, donc on regarde si le nom est le même
                        if ((dateMenu.Recipe?.Name ?? dateMenu.CustomRecipe) != (menu.Recipe?.Name ?? menu.CustomRecipe))
                        {
                            // Si les noms de recette sont pas pareils, alors on modifie
                            UpdateMenuToDB(menu);
                        }

                        // Sinon rien, donc pas besoin de else
                    }
                    else
                    {
                        // Il n'y a pas 2 dates égales de même type donc on ajoute
                        addMenuToDB(menu);
                    }


                }

                b++;

            }
        }
        private void addMenuToDB(Menu menu)
        {
            FeedMeDaddyContext db = new FeedMeDaddyContext();
            db.Menu.Add(menu);
            db.SaveChanges();
            db.Dispose();
        }
        private void UpdateMenuToDB(Menu updateMenu)
        {
            FeedMeDaddyContext db = new FeedMeDaddyContext();
            db.Menu.Update(updateMenu);
            db.SaveChanges();
            db.Dispose();
        }

        private void Combobox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Concat_ComboBox();
            /* ComboBox MyCombo = sender as ComboBox;

             if (MyCombo != null)
             {
                 TextBox text = MyCombo.Template.FindName("PART_EditableTextBox", MyCombo) as TextBox;

                 if (null != text)
                 {
                     text.Select(text.Text.Length, 0);
                 }
             }
             */
        }
        private void DatePicker_Loaded(object sender, RoutedEventArgs e)
        {
            DatePicker datePicker = sender as DatePicker;
            if (datePicker != null)
            {
                System.Windows.Controls.Primitives.DatePickerTextBox datePickerTextBox = FindVisualChild<System.Windows.Controls.Primitives.DatePickerTextBox>(datePicker);
                if (datePickerTextBox != null)
                {

                    ContentControl watermark = datePickerTextBox.Template.FindName("PART_Watermark", datePickerTextBox) as ContentControl;
                    if (watermark != null)
                    {
                        watermark.Content = string.Empty;
                    }
                }
            }
        }

        private T FindVisualChild<T>(DependencyObject depencencyObject) where T : DependencyObject
        {
            if (depencencyObject != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depencencyObject); ++i)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depencencyObject, i);
                    T result = (child as T) ?? FindVisualChild<T>(child);
                    if (result != null)
                        return result;
                }
            }

            return null;
        }
    }
}
