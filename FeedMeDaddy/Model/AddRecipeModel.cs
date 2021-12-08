using FeedMeDaddy.Services.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedMeDaddy.Model
{
    public class AddRecipeModel
    {
        public UnitWeight[] Units { get; set; }

        public FoodCategory[] Categories { get; set; }

        public AddRecipeModel(UnitWeight[] units)
        {
            Units = units;
            Categories = GetCategories();
        }


        private FoodCategory[] GetCategories()
        {
            return Enum.GetValues(typeof(FoodCategory)).Cast<FoodCategory>().ToArray();
        }
    }
}