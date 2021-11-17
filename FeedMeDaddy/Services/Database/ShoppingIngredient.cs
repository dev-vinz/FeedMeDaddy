using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FeedMeDaddy.Services.Database
{
    public partial class ShoppingIngredient
    {
        public int ShoppingId { get; set; }
        public int IngredientId { get; set; }

        public virtual Ingredient Ingredient { get; set; }
        public virtual ShoppingList Shopping { get; set; }
    }
}
