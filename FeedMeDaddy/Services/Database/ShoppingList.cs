using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FeedMeDaddy.Services.Database
{
    public partial class ShoppingList
    {
        public ShoppingList()
        {
            ShoppingIngredient = new HashSet<ShoppingIngredient>();
        }

        public int Id { get; set; }
        public int User { get; set; }

        public virtual User UserNavigation { get; set; }
        public virtual ICollection<ShoppingIngredient> ShoppingIngredient { get; set; }
    }
}
