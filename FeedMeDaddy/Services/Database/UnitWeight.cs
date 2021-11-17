using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FeedMeDaddy.Services.Database
{
    public partial class UnitWeight
    {
        public UnitWeight()
        {
            Ingredient = new HashSet<Ingredient>();
        }

        public int Id { get; set; }
        public string Unit { get; set; }
        public string Shortcut { get; set; }

        public virtual ICollection<Ingredient> Ingredient { get; set; }
    }
}
