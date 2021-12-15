using FeedMeDaddy.Services.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FeedMeDaddy.Windows
{
    /// <summary>
    /// Logique d'interaction pour AddRecipeWindow.xaml
    /// </summary>
    public partial class AddRecipeWindow : Window
    {
        public List<Ingredient> ingredients {get;set;}


        public AddRecipeWindow()
        {
            List<Ingredient> ingredients = new List<Ingredient>();
            InitializeComponent();
        }

        private void AddIngredient(object sender, RoutedEventArgs e)
        {
            IngredientTextBox.BorderBrush = Brushes.Gray;
            qtyUpDown.BorderBrush = Brushes.Gray;
            UnitCombobox.BorderBrush = Brushes.Gray;
            CategoryCombobox.BorderBrush = Brushes.Gray;
            IngredientListView.BorderBrush = Brushes.Gray;

            string ingredientName = IngredientTextBox.Text;
            double? ingredientQuantity = qtyUpDown.Value;
            UnitWeight ingredientUnit = UnitCombobox.SelectedItem as UnitWeight;

            if (string.IsNullOrEmpty(ingredientName))
            {
                IngredientTextBox.BorderBrush = Brushes.Red;
                IngredientTextBox.Focus();
                return;
                
            }

            if (ingredientQuantity == null || ingredientQuantity == 0)
            {
                qtyUpDown.BorderBrush = Brushes.Red;
                qtyUpDown.Focus();
                return;
               
            }

            if (!Enum.TryParse(CategoryCombobox.SelectedItem?.ToString(), out FoodCategory ingredientCategory))
            {
                CategoryCombobox.BorderBrush = Brushes.Red;
                CategoryCombobox.Focus();
                return;
            }

            Ingredient ingredient = new Ingredient
            {
                Name = ingredientName,
                Quantity = ingredientQuantity.Value,
                Category = ingredientCategory,
                Unit = ingredientUnit,
                ExpirationDate = null
            };

            ingredients.Add(ingredient);
            IngredientListView.Items.Clear();
            foreach(Ingredient i in ingredients)
            {
                IngredientListView.Items.Add(i);
            }

        }

        private void RemoveIngredient(object sender, RoutedEventArgs e)
        {
            int i = IngredientListView.SelectedIndex;

            ingredients.RemoveAt(i);
        }

        private void AddRecipe(object sender, RoutedEventArgs e)
        {
            NameTextBox.BorderBrush = Brushes.Gray;
            IngredientListView.BorderBrush = Brushes.Gray;
            DescriptionTextBox.BorderBrush = Brushes.Gray;

            if (string.IsNullOrEmpty(NameTextBox.Text))
            {
                NameTextBox.BorderBrush = Brushes.Red;
                NameTextBox.Focus();
                return;
            }
            if (string.IsNullOrEmpty(DescriptionTextBox.Text))
            {
                DescriptionTextBox.BorderBrush = Brushes.Red;
                DescriptionTextBox.Focus();
                return;

            }
            if (IngredientListView.Items.Count == 0)
            {
                IngredientListView.BorderBrush = Brushes.Red;
                IngredientTextBox.Focus();
                return;
            }


        }
    }


}
