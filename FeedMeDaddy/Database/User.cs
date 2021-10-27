using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FeedMeDaddy.Database
{
    public partial class User
    {
        public User()
        {
            Recipe = new HashSet<Recipe>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public virtual Fridge Fridge { get; set; }
        public virtual ShoppingList ShoppingList { get; set; }
        public virtual ICollection<Recipe> Recipe { get; set; }
    }
}
