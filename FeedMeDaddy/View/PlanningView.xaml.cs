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
            string pattern = @"[^A-Za-zÀ-ÖØ-öø-ÿ ]";
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
            var menusBreakfast = db.Menu.Fetch().Where(m => m.User.Id == 1 && m.Type == TypeMenu.Breakfast).OrderBy(m => m.Date);
            var menusDinner = db.Menu.Fetch().Where(m => m.User.Id == 1 && m.Type == TypeMenu.Dinner).OrderBy(m => m.Date);
            var menusSupper = db.Menu.Fetch().Where(m => m.User.Id == 1 && m.Type == TypeMenu.Supper).OrderBy(m => m.Date);

            db.SaveChanges();

            //Search in the DB if the menu is already placed in the week

            //Compare each date of the breakfast in the menu and the day of week
            foreach (Menu breakfast in menusBreakfast)
            {
                int datei = 0;

                foreach (ComboBox combo in Breakfast)
                {
                    if (DateTime.Compare(breakfast.Date, GetDate(datei)) == 0)
                    {
                        combo.Text = breakfast.Recipe?.Name ?? breakfast.CustomRecipe;

                    }
                    datei++;
                }
            }
            //Compare each date of the Dinner in the menu and the day of week
            foreach (Menu dinner in menusDinner)
            {
                int datei = 0;

                foreach (var combo in Dinner)
                {

                    if (DateTime.Compare(dinner.Date, GetDate(datei)) == 0)
                    {

                        combo.Text = dinner.Recipe?.Name ?? dinner.CustomRecipe;
                    }
                    datei++;
                }
            }
            //Compare each date of the Supper in the menu and the day of week
            foreach (Menu supper in menusSupper)
            {
                int datei = 0;

                foreach (var combo in Supper)
                {

                    if (DateTime.Compare(supper.Date, GetDate(datei)) == 0)
                    {

                        combo.Text = supper.Recipe?.Name ?? supper.CustomRecipe;
                    }
                    datei++;
                }
            }

            //re-add all ingredient in shopping list
            foreach (Ingredient ingre in shoppingList.Ingredients)
            {
                IngredientToShoppingList(ingre);
            }
            db.Dispose();
        }

        private void Concat_ComboBox()
        {
            string pattern = @"[^A-Za-zÀ-ÖØ-öø-ÿ ]";
            RegexOptions options = RegexOptions.IgnoreCase;
            foreach (var item in Breakfast)
            {

                if (item.Text.Any(char.IsDigit) || item.Text.Any(char.IsPunctuation) || item.Text.Any(char.IsControl) || item.Text.Any(char.IsSymbol))
                {
                    item.Text = Regex.Replace(item.Text, pattern, "", options);
                }
            }
            foreach (var item in Dinner)
            {
                if (item.Text.Any(char.IsDigit) || item.Text.Any(char.IsPunctuation) || item.Text.Any(char.IsControl) || item.Text.Any(char.IsSymbol))
                {
                    item.Text = Regex.Replace(item.Text, pattern, "", options);
                }
            }
            foreach (var item in Supper)
            {
                if (item.Text.Any(char.IsDigit) || item.Text.Any(char.IsPunctuation) || item.Text.Any(char.IsControl) || item.Text.Any(char.IsSymbol))
                {
                    item.Text = Regex.Replace(item.Text, pattern, "", options);
                }
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
            var menusBreakfast = db.Menu.Fetch().Where(m => m.User.Id == 1 && m.Type == TypeMenu.Breakfast).OrderBy(m => m.Date);
            var menusDinner = db.Menu.Fetch().Where(m => m.User.Id == 1 && m.Type == TypeMenu.Dinner).OrderBy(m => m.Date);
            var menusSupper = db.Menu.Fetch().Where(m => m.User.Id == 1 && m.Type == TypeMenu.Supper).OrderBy(m => m.Date);
            db.SaveChanges();

            Recipe[] recipes = GetRecipeSelection();
            DateTime[] dates = GetWeekdays();



            foreach (ComboBox combo in Breakfast)
            {
                if (!(string.IsNullOrWhiteSpace(combo.Text)))
                {
                    //If the menu already existe in the DB with the same name 
                    if (menusBreakfast.Any(r => (r.Recipe?.Name ?? r.CustomRecipe).Equals(combo.Text)))
                    {
                        //If they have the same date
                        if (!(menusBreakfast.Any(r => dates.Any(d => DateTime.Compare(r.Date, d) == 0))))
                        //They are the same meal but on a different day so we add the ingredients
                        {
                            GetIngredientFromMenu(recipes, combo);
                        }
                    }
                    else
                    {
                        GetIngredientFromMenu(recipes, combo);
                    }

                }


            }


            foreach (ComboBox combo in Dinner)
            {
                if (!string.IsNullOrWhiteSpace(combo.Text))
                {
                    //If the menu already existe in the DB with the same name 
                    if (menusDinner.Any(r => (r.Recipe?.Name ?? r.CustomRecipe).Equals(combo.Text)))
                    {
                        //If they have the same date
                        if (!(menusDinner.Any(r => dates.Any(d => DateTime.Compare(r.Date, d) == 0))))
                        //They are the same meal but on a different day so we add the ingredients
                        {
                            GetIngredientFromMenu(recipes, combo);
                        }
                    }
                    else
                    {
                        GetIngredientFromMenu(recipes, combo);
                    }

                }
            }

            foreach (ComboBox combo in Supper)
            {
                if (!string.IsNullOrWhiteSpace(combo.Text))
                {

                    //If the menu already existe in the DB with the same name 
                    if (menusSupper.Any(r => (r.Recipe?.Name ?? r.CustomRecipe).Equals(combo.Text)))//deux ont le meme noms
                    {
                        //do they have the same date
                        if (!(menusSupper.Any(r => dates.Any(d => DateTime.Compare(r.Date, d) == 0))))
                        //They are the same meal but on a different day so we add the ingredients
                        {
                            GetIngredientFromMenu(recipes, combo);
                        }
                    }
                    else
                    {
                        GetIngredientFromMenu(recipes, combo);
                    }

                }
            }
            GetMenuFromCombobox();
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
            var menus = GetMenuSelection();
            int b = 0;
            foreach (ComboBox combo in Breakfast)
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
                        Type = TypeMenu.Breakfast
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


                    if ((menu.Recipe?.Name ?? menu.CustomRecipe).Equals(combo.Text))//same Name
                    {

                        if (!(menus.Any(da => DateTime.Compare(menu.Date, da.Date) == 0))) //Same Date
                        {

                            addMenuToDB(menu);

                        }
                    }
                    else
                    {
                        addMenuToDB(menu);
                    }

                }

                b++;
            }

            int d = 0;
            foreach (ComboBox combo in Dinner)
            {

                if (!string.IsNullOrWhiteSpace(combo.Text))
                {
                    var menu = new Menu
                    {
                        User = new User
                        {
                            Id = 1
                        },
                        Date = GetDate(d),
                        Type = TypeMenu.Dinner
                    };

                    if (recipes.Any(r => r.Name == combo.Text))
                    {
                        menu.Recipe = recipes.FirstOrDefault(r => r.Name == combo.Text);

                    }
                    else
                    {
                        menu.CustomRecipe = combo.Text;
                    }


                    if ((menu.Recipe?.Name ?? menu.CustomRecipe).Equals(combo.Text))
                    {

                        if (!(menus.Any(da => DateTime.Compare(menu.Date, da.Date) == 0)))
                        {
                            addMenuToDB(menu);
                        }

                    }
                    else
                    {
                        addMenuToDB(menu);
                    }

                }

                d++;
            }

            int s = 0;
            foreach (ComboBox combo in Supper)
            {

                if (!string.IsNullOrWhiteSpace(combo.Text))
                {
                    var menu = new Menu
                    {
                        User = new User
                        {
                            Id = 1
                        },
                        Date = GetDate(s),
                        Type = TypeMenu.Supper
                    };
                    if (recipes.Any(r => r.Name == combo.Text))
                    {
                        menu.Recipe = recipes.FirstOrDefault(r => r.Name == combo.Text);

                    }
                    else
                    {
                        menu.CustomRecipe = combo.Text;
                    }

                    if ((menu.Recipe?.Name ?? menu.CustomRecipe).Equals(combo.Text))
                    {

                        if (!(menus.Any(da => DateTime.Compare(menu.Date, da.Date) == 0)))
                        {
                            addMenuToDB(menu);
                        }

                    }
                    else
                    {
                        addMenuToDB(menu);
                    }

                }

                s++;
            }

        }


        private void addMenuToDB(Menu menu)
        {
            FeedMeDaddyContext db = new FeedMeDaddyContext();
            db.Menu.Add(menu);
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
