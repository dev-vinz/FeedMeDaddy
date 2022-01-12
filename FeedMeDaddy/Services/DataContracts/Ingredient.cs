using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedMeDaddy.Services.DataContracts
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
        public FoodCategory Category { get; set; }
        public UnitWeight Unit { get; set; }
        public DateTime? ExpirationDate { get; set; }

		public Ingredient()
		{
		}

		public Ingredient(Ingredient source)
		{
            Id = source.Id;
            Name = source.Name;
            Quantity = source.Quantity;
            Category = source.Category;
            Unit = source.Unit;
            ExpirationDate = source.ExpirationDate;
		}

        public string FullUnit
        {
            get
            {
                return $"{Quantity} {Unit.Shortcut}";
            }
        }
    }
}
