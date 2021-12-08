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
        public PlanningView()
        {
            InitializeComponent();
            Initialize();
            InitializeBlackout();
        }

        
        
        
        private void InitializeBlackout()
        {
            dueDate.BlackoutDates.AddDatesInPast();
            dueDate.BlackoutDates.Add(new CalendarDateRange(DateTime.Now.AddDays(-1)));
        }
        private void item_TextChanged(object sender, TextChangedEventArgs e)
        {
            item.Text = string.Concat(item.Text.Where(char.IsLetter));
            item.SelectionStart = item.Text.Length;
            item.SelectionLength = 0;
            item.Focus();
        }
        private static ShoppingList GetShoppingListSelection()
        {

            FeedMeDaddyContext db = new FeedMeDaddyContext();

            var recipies = db.ShoppingList.Fetch().FirstOrDefault(s=>s.User.Id==1);
            
            db.Dispose();
            return recipies;


        }
        private void Initialize()
        {
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
            units.SelectedIndex = 0;
            Category.SelectedIndex=5;
            var list = GetShoppingListSelection();
            foreach(Ingredient ingre in list.Ingredients)
            {
                IngredientToShoppingList(ingre);
            }
        }
        private void Concat_ComboBox()
        {
            string pattern = @"[^A-Za-z ]";
            RegexOptions options = RegexOptions.IgnoreCase;
            foreach (var item in Breakfast)
                {
                    
                    if (item.Text.Any(char.IsDigit) || item.Text.Any(char.IsPunctuation) || item.Text.Any(char.IsControl) || item.Text.Any(char.IsSymbol))
                    {
                    Console.WriteLine(Regex.Replace(item.Text, pattern, "", options));
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

            IngredientToShoppingList(ing);

        }
        private void MenuShopping_Click(object sender, RoutedEventArgs e)
        {
            string test = "";
            Recipe[] recipes = GetMenuSelection();
            foreach (ComboBox combo in Breakfast)
            {
                if (!(combo.Text == ""))
                {
                    test += combo.Name + ": " + combo.Text + "\n";
                    GetIngredientFromMenu(recipes, combo);
                }
               

            }
            foreach (ComboBox combo in Dinner)
            {
                if (!(combo.Text == ""))
                {
                    test += combo.Name + ": " + combo.Text + "\n";
                    GetIngredientFromMenu(recipes, combo);
                }

            }
            foreach (ComboBox combo in Supper)
            {
                if (!(combo.Text == ""))
                {
                    test += combo.Name + ": " + combo.Text + "\n";
                    GetIngredientFromMenu(recipes, combo);
                }
            }
            MessageBox.Show(test);
        }

        private static Recipe[] GetMenuSelection()
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
            //Si la recette fait partie des recettes il faut ajouter les ingredients dans la liste de courses
            for (int i = 0; i < recipes.Length; i++)
            {
                
                if (recipes[i].Name.Equals(combo.Text))
                {
                    foreach (Ingredient ingredient in recipes[i].Ingredients)
                    {
                        IngredientToShoppingList(ingredient);
                       
                    }

                  
                }
            }
        }
        private void IngredientToShoppingList(Ingredient ing)
        {
            if(ing.Category == FoodCategory.ShouldNotFigureOnShoppingList)
            {
                return;
            }
            if (ing.Name == "")
            {
                IngredientRequired.Visibility = Visibility.Visible;
            }
            else
            {
                addIngredientToDB(ing);
                IngredientRequired.Visibility = Visibility.Collapsed;
                //Il faut faire un controle pour savoir si il y a déjà l'ingredient dans la liste de course
                int previewSize = previewShopping.Items.Count;
                bool inList = false;
                WeightConverter converter = new WeightConverter();
                for (int i = 0; i < previewSize; i++)
                {
                    Ingredient myIng = previewShopping.Items.GetItemAt(i) as Ingredient;
                    double quantitiyMyIng = myIng.Quantity;

                    if (myIng.Name.Equals(ing.Name) && (!inList))
                    {
                    
                                //Si l'unité est la même on peut juste additionner
                                if (myIng.Unit.Shortcut.Equals(ing.Unit.Shortcut))
                                {

                                    previewShopping.Items.Remove(myIng);
                                    myIng.Quantity = quantitiyMyIng+ing.Quantity;
                                    previewShopping.Items.Add(myIng);
                                    inList = true;

                                }
                                else //si l'unité est différente il faut faire une conversion
                                {
                                    double tempQuantity = 0;
                                    bool sameType = true;
                                    try
                                    {
                                        tempQuantity = converter.Convert(myIng.Unit, ing.Quantity, ing.Unit);
                                    }
                                    catch
                                    {
                                        sameType = false;
                                    }
                                    if (sameType)
                                    {
                                        previewShopping.Items.Remove(myIng);
                                        myIng.Quantity += tempQuantity;
                                        previewShopping.Items.Add(myIng);
                                        inList = true;
                                    }

                                }
                            
                        
                    }
                }
                if (!inList)
                {

                    previewShopping.Items.Add(ing);
                }
            }

        }
        private void addIngredientToDB(Ingredient ing)
        {
            FeedMeDaddyContext db = new FeedMeDaddyContext();
            ShoppingList list = GetShoppingListSelection();
           
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
