using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FeedMeDaddy.Services.Database
{
    public partial class Ingredient
    {
        public Ingredient()
        {
            RecipeIngredient = new HashSet<RecipeIngredient>();
            ShoppingIngredient = new HashSet<ShoppingIngredient>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
        public int Category { get; set; }
        public int Unit { get; set; }
        public DateTime? LimitDate { get; set; }

        public virtual FoodCategory CategoryNavigation { get; set; }
        public virtual UnitWeight UnitNavigation { get; set; }
        public virtual ICollection<RecipeIngredient> RecipeIngredient { get; set; }
        public virtual ICollection<ShoppingIngredient> ShoppingIngredient { get; set; }
    }
}
