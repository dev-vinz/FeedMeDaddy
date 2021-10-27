using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Database.Models
{
    public partial class Recipe
    {
        public int Id { get; set; }
        public int User { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int NbPersons { get; set; }
        public string Ingredients { get; set; }

        public virtual User UserNavigation { get; set; }
    }
}
