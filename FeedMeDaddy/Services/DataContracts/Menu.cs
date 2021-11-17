using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedMeDaddy.Services.DataContracts
{
    public class Menu
    {
        public User User { get; set; }
        public DateTime Date { get; set; }
        public TypeMenu Type { get; set; }
        public Recipe Recipe { get; set; }
        public string CustomRecipe { get; set; }
    }
}
