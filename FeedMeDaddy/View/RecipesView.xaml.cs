using FeedMeDaddy.Services.Database;
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

            ViewModel = this.DataContext as RecipesViewModel;
        }


        private void QuantityChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            /*
            int qty = (int)qtyUpDown.Value;

            List<Ingredient> list = (List<Ingredient>)ingredientList.ItemsSource;
            foreach (Ingredient i in list)
            {
                i.Quantity = i.Quantity / 4 * qty;
            }
            */
        }

        private void RecipeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel = this.DataContext as RecipesViewModel;

            int index = (sender as ListBox).SelectedIndex;
            ViewModel.ActiveRecipe = ViewModel.Recipes.ElementAt(index);

            recipeComboBox.SelectedIndex = index;
            recipeName.Text = (string)recipeListBox.SelectedItem;
            recipeDescription.Text = ViewModel.ActiveRecipe.Description;
            ingredientList.Items.Clear();
            foreach (var i in ViewModel.ActiveRecipe.Ingredients)
            {
                ingredientList.Items.Add(i);
            }
        }

        private void Combobox_selectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = (sender as ComboBox).SelectedIndex;
            recipeListBox.SelectedIndex = index;
        }


    }
}
