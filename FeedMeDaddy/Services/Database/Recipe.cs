using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FeedMeDaddy.Services.Database
{
    public partial class Recipe
    {
        public Recipe()
        {
            RecipeIngredient = new HashSet<RecipeIngredient>();
        }

        public int Id { get; set; }
        public int User { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int NbPersons { get; set; }
        public string Ingredients { get; set; }

        public virtual User UserNavigation { get; set; }
        public virtual ICollection<RecipeIngredient> RecipeIngredient { get; set; }
    }
}
