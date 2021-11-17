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

        private void IngredientToShopping_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(item.Text==""? "Nothing" : "You added " + String.Format("{0:#,#.0}", quantity.Value)+" "+ units.Text + " of "+ item.Text + (dueDate.Text == "" ? " " :" for the date : " + dueDate.Text));
            //previewShopping.Items.Add(item.Text == "" ? "Nothing" : "You added " + String.Format("{0:#,#.0}", quantity.Value) + " " + units.Text + " of " + item.Text + (dueDate.Text == "" ? " " : " for the date : " + dueDate.Text));
            var row = new { Ingredient = item.Text, Quantity = String.Format("{0:#,#.0}", quantity.Value), Units = units.Text, Expiration_date = dueDate.Text == "" ? "" : dueDate.Text };
            previewShopping.Items.Add(row);
        }

        private void InitializeBlackout()
        {
            dueDate.BlackoutDates.AddDatesInPast();
            dueDate.BlackoutDates.Add(new CalendarDateRange(DateTime.Now.AddDays(-1)));
        }
        private void item_TextChanged(object sender, TextChangedEventArgs e)
        {
            item.Text = string.Concat(item.Text.Where(char.IsLetter));
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
            var row = new{Ingredient = "product_1", Quantity="100", Units= "g", Expiration_date="12.12.2012" };
            previewShopping.Items.Add(row);

        }
        private void Concat_ComboBox()
        {
            foreach (var item in Breakfast)
            {
                item.Text = string.Concat(item.Text.Where(char.IsLetter));
            }
            foreach (var item in Dinner)
            {
                item.Text = string.Concat(item.Text.Where(char.IsLetter));
            }
            foreach (var item in Supper)
            {
                item.Text = string.Concat(item.Text.Where(char.IsLetter));
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {            
            string test = "";
            foreach (var item in Breakfast)
            {
                if(item.Text=="")
                {

                }
                else
                {
                    test += item.Name + ": "+item.Text + " ";
                }
                
            }
            foreach (var item in Dinner)
            {
                if (item.Text == "")
                {

                }
                else
                {
                    test += item.Name + ": " + item.Text + " ";
                }
            }
            foreach (var item in Supper)
            {
                if (item.Text == "")
                {

                }
                else
                {
                    test += item.Name + ": " + item.Text + " ";
                }


                
            }
            MessageBox.Show(test);
        }
        private void Combobox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Concat_ComboBox();
        }
    }
}
