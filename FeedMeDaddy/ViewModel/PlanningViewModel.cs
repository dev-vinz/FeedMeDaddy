using FeedMeDaddy.Core;
using FeedMeDaddy.Services;
using FeedMeDaddy.Services.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FeedMeDaddyContext = FeedMeDaddy.Services.Database.FeedMeDaddyContext;
[System.Serializable]
[System.Runtime.InteropServices.ComVisible(true)]
public enum DayOfWeek
{
    Sunday = 0,
    Monday = 1,
    Tuesday = 2,
    Wednesday = 3,
    Thursday = 4,
    Friday=5,
    Saturday=6
};

namespace FeedMeDaddy.ViewModel
{
    class PlanningViewModel : ObservableObject
    {

        private static DateTime[] GetWeekdays()
        {
            DateTime dateTime = DateTime.Now;
            DateTime[] weekdays = new DateTime[7];
            for (int i = 0; i < 7; i++)
            {
                weekdays[i] = dateTime;
                dateTime = dateTime.AddDays(1);
            }
            return weekdays;
        }
        private static string GetDay(int current)
        {
            DateTime[] weekdays = GetWeekdays();
            return weekdays[current].DayOfWeek.ToString();
        }
        private static string GetDate(int current)
        {
            DateTime[] weekdays = GetWeekdays();
            return weekdays[current].ToString("dd/MM/yyyy");
        }
        private static UnitWeight[] GetUnitsSelection()
        {
            FeedMeDaddyContext db = new FeedMeDaddyContext();

            var units = db.UnitWeight.Fetch();
            var unitWeight = new UnitWeight[units.Count()];

            Array.Copy(units.ToArray(), unitWeight, units.Count());
            db.Dispose();
            return unitWeight;
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

        private static FoodCategory[] GetCategories()
        {
            return Enum.GetValues(typeof(FoodCategory)).Cast<FoodCategory>().ToArray();
        }
        public UnitWeight[] UnitsSelection { get; set;} = GetUnitsSelection();
        public FoodCategory[] CategorySelection { get; set; } = GetCategories();
        public Recipe[] MenuSelection { get; set; } = GetMenuSelection();
        public string DateZeo { get; set; } = $"{GetDate(0)}";
        public string DayZero { get; set; } = $"{GetDay(0)} {GetDate(0)}";
        public string DayOne { get; set; } = $"{GetDay(1)} {GetDate(1)}";
        public string DayTwo { get; set; } = $"{GetDay(2)} {GetDate(2)}";
        public string DayThree { get; set; } = $"{GetDay(3)} {GetDate(3)}";
        public string DayFour { get; set; } = $"{GetDay(4)} {GetDate(4)}";
        public string DayFive { get; set; } = $"{GetDay(5)} {GetDate(5)}";
        public string DaySix { get; set; } = $"{GetDay(6)} {GetDate(6)}";







        public PlanningViewModel()
        {
        }
    }
}
