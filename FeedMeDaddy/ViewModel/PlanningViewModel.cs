using FeedMeDaddy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
