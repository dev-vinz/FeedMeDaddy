using FeedMeDaddy.Model;
using FeedMeDaddy.Services;
using FeedMeDaddy.Services.DataContracts;
using FeedMeDaddy.ViewModel;
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
        public List<Ingredient> ingredients { get; set; }
        private RecipesViewModel ViewModel { get; set; }

        public AddRecipeWindow()
        {
            ingredients = new List<Ingredient>();

            InitializeComponent();

            FillCombobox();
        }

        private void FillCombobox()
        {
            AddRecipeViewModel addRecipeViewModel = new AddRecipeViewModel();
            foreach (var category in addRecipeViewModel.addRecipeModel.Categories)
            {
                CategoryCombobox.Items.Add(category);
            }
            foreach (UnitWeight unit in addRecipeViewModel.addRecipeModel.Units)
            {
                UnitCombobox.Items.Add(unit);
            }
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

            foreach (Ingredient i in ingredients)
            {
                IngredientListView.Items.Add(i);
            }

        }

        private void RemoveIngredient(object sender, RoutedEventArgs e)
        {
            /*int i = IngredientListView.SelectedIndex;

            ingredients.RemoveAt(i);*/
            if (IngredientListView.SelectedItem != null)
            {
                ingredients.RemoveAt(IngredientListView.Items.IndexOf(IngredientListView.SelectedItem));
                IngredientListView.Items.RemoveAt(IngredientListView.Items.IndexOf(IngredientListView.SelectedItem));

            }
            else
            {
                MessageBox.Show("No item selected");
            }
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
            Recipe recipe = new Recipe
            {
                Id = 0,
                User = new User { Id = 1 },
                Name = NameTextBox.Text,
                Description = DescriptionTextBox.Text,
                NbPersons = 4,
                Ingredients = ingredients
            };
            Services.Database.FeedMeDaddyContext db = new Services.Database.FeedMeDaddyContext();
            db.AddRecipe(recipe);
            db.SaveChanges();
            db.Dispose();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

		private void Border_MouseDown(object sender, MouseButtonEventArgs e)
		{
            if (e.LeftButton == MouseButtonState.Pressed)
			{
                this.DragMove();
			}
		}
	}


}
