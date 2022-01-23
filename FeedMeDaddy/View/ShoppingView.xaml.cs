using FeedMeDaddy.Services;
using FeedMeDaddy.Services.DataContracts;
using FeedMeDaddy.Services.Units;
using FeedMeDaddy.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FeedMeDaddyContext = FeedMeDaddy.Services.Database.FeedMeDaddyContext;

namespace FeedMeDaddy.View
{
    /// <summary>
    /// Logique d'interaction pour ShoppingView.xaml
    /// </summary>
    public partial class ShoppingView : System.Windows.Controls.UserControl
    {
        public ShoppingView()
        {
            InitializeComponent();
            InitializeSettings();
        }

        private void InitializeSettings()
        {
            ingredientNameError.Text = "";
            borderIngredientName.BorderBrush = Brushes.White;
            ingredientQuantityError.Text = "";
            borderIngredientQuantity.BorderBrush = Brushes.White;
            ingredientCategoryError.Text = "";
            borderIngredientCategory.BorderBrush = Brushes.White;
            dateIngredientExpiration.BlackoutDates.AddDatesInPast();
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

        private void BtnAddIngredient_Click(object sender, RoutedEventArgs e)
        {
            ingredientNameError.Text = "";
            borderIngredientName.BorderBrush = Brushes.White;
            ingredientQuantityError.Text = "";
            borderIngredientQuantity.BorderBrush = Brushes.White;
            ingredientCategoryError.Text = "";
            borderIngredientCategory.BorderBrush = Brushes.White;

            string ingredientName = boxIngredientName.Text;
            double? ingredientQuantity = upDownIngredientQuantity.Value;
            UnitWeight ingredientUnit = boxIngredientUnit.SelectedItem as UnitWeight;
            DateTime? expirationDate = dateIngredientExpiration.SelectedDate;

            if (string.IsNullOrEmpty(ingredientName))
            {
                ingredientNameError.Text = "Required";
                borderIngredientName.BorderBrush = Brushes.Red;
                boxIngredientName.Focus();
                return;
            }

            if (ingredientQuantity == null || ingredientQuantity == 0)
            {
                ingredientQuantityError.Text = "Required";
                borderIngredientQuantity.BorderBrush = Brushes.Red;
                upDownIngredientQuantity.Focus();
                return;
            }

            if (!Enum.TryParse(boxIngredientCategory.SelectedItem?.ToString(), out FoodCategory ingredientCategory))
            {
                ingredientCategoryError.Text = "Required";
                borderIngredientCategory.BorderBrush = Brushes.Red;
                boxIngredientCategory.Focus();
                return;
            }

            Ingredient ingredient = new Ingredient
            {
                Name = ingredientName,
                Quantity = ingredientQuantity.Value,
                Category = ingredientCategory,
                Unit = ingredientUnit,
                ExpirationDate = expirationDate
            };

            ShoppingViewModel viewModel = DataContext as ShoppingViewModel;

            viewModel.AddToModel(ingredient);
            viewModel.AddToShoppingList();

            Refresh_ShoppingList(viewModel);
        }

        private void BtnRemoveIngredient_Click(object sender, RoutedEventArgs e)
        {
            Ingredient ingredient = shoppingList.SelectedItem as Ingredient;
            WeightConverter converter = new WeightConverter();

            if (ingredient == null) return;

            UnitType typeIng = converter.TypeFor(ingredient.Unit.Unit);

            FeedMeDaddyContext db = new FeedMeDaddyContext();

            IEnumerable<Ingredient> allIngredients = db.Ingredient.Fetch().Where(i => i.Name == ingredient.Name && converter.TypeFor(i.Unit.Unit) == typeIng);
            IEnumerable<Recipe> allRecipes = db.Recipe.Fetch().Where(r => r.User.Id == 1);

            IEnumerable<Ingredient> ingredientToDelete = allIngredients.Where(i => !allRecipes.Any(r => r.Ingredients.Any(ri => ri.Id == i.Id)));

            db.Ingredient.RemoveRange(ingredientToDelete);
            db.SaveChanges();
            db.Dispose();

            ShoppingViewModel viewModel = DataContext as ShoppingViewModel;

            viewModel.RemoveFromModel(ingredient);

            Refresh_ShoppingList(viewModel);
        }

        private void BtnRemoveAllIngredient_Click(object sender, RoutedEventArgs e)
        {
            int count = shoppingList.Items.Count;
            for (int k = 0; k < count; k++)
            {
                 
                    shoppingList.SelectedIndex = 0;
                    BtnRemoveIngredient_Click(sender, e);
                
            }
        }

        private void ShoppingList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnRemoveIngredient.IsEnabled = true;
        }

        private void Refresh_ShoppingList(ShoppingViewModel viewModel)
        {
            shoppingList.ItemsSource = viewModel.ShoppingModel.Ingredients;
            boxIngredientName.Clear();
            upDownIngredientQuantity.Value = 0;
            boxIngredientUnit.SelectedIndex = 0;
            boxIngredientCategory.SelectedItem = null;
        }

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            ShoppingViewModel viewModel = DataContext as ShoppingViewModel;

            IEnumerable<Ingredient> ingredients = viewModel.ShoppingModel.Ingredients;
            string[] exportList = ingredients.ExportToFile();

            SaveFileDialog dialog = new SaveFileDialog();

            dialog.Filter = "Text Files|*.txt";
            dialog.Title = "Save your shopping list";
            dialog.FileName = $"ShoppingList_{DateTime.Now:ddMMyy}";
            dialog.ShowDialog();

            if (!string.IsNullOrWhiteSpace(dialog.FileName))
            {
                File.WriteAllLines(dialog.FileName, exportList);
            }
        }
    }
}
