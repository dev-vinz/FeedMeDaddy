using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FeedMeDaddy.Database
{
    public partial class Fridge
    {
        public int User { get; set; }
        public string Ingredients { get; set; }

        public virtual User UserNavigation { get; set; }
    }
}
