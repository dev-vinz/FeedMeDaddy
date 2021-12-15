using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FeedMeDaddy.Services.Database
{
    public partial class TypeMenu
    {
        public TypeMenu()
        {
            Menu = new HashSet<Menu>();
        }

        public int Id { get; set; }
        public string Type { get; set; }

        public virtual ICollection<Menu> Menu { get; set; }
    }
}
