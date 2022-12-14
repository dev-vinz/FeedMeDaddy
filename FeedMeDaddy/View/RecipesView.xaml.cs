using FeedMeDaddy.Services.Database;
using FeedMeDaddy.ViewModel;
using FeedMeDaddy.Windows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FeedMeDaddy.View
{
    /// <summary>
    /// Logique d'interaction pour RecipesView.xaml
    /// </summary>
    public partial class RecipesView : UserControl
    {
        private RecipesViewModel ViewModel { get; set; }

        public RecipesView()
        {
            InitializeComponent();
        }


        private void QuantityChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            ViewModel = this.DataContext as RecipesViewModel;

            int qty = (int)qtyUpDown.Value;
            newQuantity(qty);

        }

        private void newQuantity(int qty)
        {
            ingredientList?.Items.Clear();
            List<Services.DataContracts.Ingredient> list = ViewModel?.ActiveRecipe?.Ingredients.ToList() ?? new List<Services.DataContracts.Ingredient>();
            foreach (Services.DataContracts.Ingredient i in list)
            {
                i.Quantity = i.Quantity / 4 * qty;
                ingredientList.Items.Add(i);
            }
        }

        private void RecipeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel = this.DataContext as RecipesViewModel;

            int index = (sender as ListBox).SelectedIndex;

            //Prevents problems in case combobox content is deleted or change of window
            if (index >= 0)
            {
                ViewModel.ActiveRecipe = ViewModel.Recipes.ElementAt(index);
                recipeComboBox.SelectedIndex = index;
                recipeName.Text = ViewModel.ActiveRecipe.Name;
                recipeDescription.Text = ViewModel.ActiveRecipe.Description;

                newQuantity((int)qtyUpDown.Value);
            }

        }

        private void Combobox_selectionChanged(object sender, SelectionChangedEventArgs e)
        {

            ViewModel = this.DataContext as RecipesViewModel;
            int index = (sender as ComboBox).SelectedIndex;

            //Prevents problems in case combobox content is deleted or change of window
            if (index >= 0)
            {
                ViewModel.ActiveRecipe = ViewModel.Recipes.ElementAt(index);

                recipeListBox.SelectedIndex = index;
                recipeName.Text = ViewModel.ActiveRecipe.Name;
                recipeDescription.Text = ViewModel.ActiveRecipe.Description;

                newQuantity((int)qtyUpDown.Value);
            }

        }

        private void addNewRecipe(object sender, RoutedEventArgs e)
        {

            AddRecipeViewModel addRecipeViewModel = new AddRecipeViewModel();
            AddRecipeWindow addRecipeWindow = new AddRecipeWindow();
            addRecipeWindow.ShowDialog();
            //Refreshing combobox and listview to display new recipe
            ViewModel = this.DataContext as RecipesViewModel;
            ViewModel.LoadRecipes();
            ViewModel.SetupRecipes();
            recipeListBox.ItemsSource = ViewModel.RecipeList;
            recipeComboBox.ItemsSource = ViewModel.RecipeList;
        }
    }
}