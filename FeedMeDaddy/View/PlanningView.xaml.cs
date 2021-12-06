using FeedMeDaddy.Services.DataContracts;
using FeedMeDaddy.Services.Units;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;



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

            var ing = new Ingredient
            {
                Name = item.Text,
                Quantity = (double)quantity.Value,
                Unit = (UnitWeight)units.SelectedItem,
                ExpirationDate = dueDate.SelectedDate
            };

            //Il faut faire un controle pour savoir si il y a déjà l'ingredient dans la liste de course
            int previewSize = previewShopping.Items.Count;
            bool inList = false;
            WeightConverter converter = new WeightConverter();
            for (int i = 0; i < previewSize; i++)
            {
                var myIng = previewShopping.Items.GetItemAt(i) as Ingredient;

                if (myIng.Name.Equals(ing.Name))
                {
                    if (myIng.ExpirationDate == ing.ExpirationDate)
                    {
                        //Si l'unité est la même on peut juste additionner
                        if (myIng.Unit.Shortcut.Equals(ing.Unit.Shortcut))
                        {

                            previewShopping.Items.Remove(myIng);
                            myIng.Quantity += ing.Quantity;
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
            }
            if (!inList)
            {

                previewShopping.Items.Add(ing);
            }


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
            units.SelectedIndex = 0;
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
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string test = "";
            foreach (var item in Breakfast)
            {
                if (item.Text == "")
                {

                }
                else
                {
                    test += item.Name + ": " + item.Text + " ";
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
