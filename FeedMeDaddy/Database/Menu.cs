using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FeedMeDaddy.Database
{
    public partial class Menu
    {
        public int User { get; set; }
        public DateTime Date { get; set; }
        public int Type { get; set; }
        public int? Recipe { get; set; }
        public string CustomRecipe { get; set; }

        public virtual Recipe RecipeNavigation { get; set; }
        public virtual TypeMenu TypeNavigation { get; set; }
        public virtual User UserNavigation { get; set; }
    }
}
