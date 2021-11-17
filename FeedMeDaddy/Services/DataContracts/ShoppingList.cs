using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedMeDaddy.Services.DataContracts
{
    public class ShoppingList
    {
        public User User { get; set; }
        public IEnumerable<Ingredient> Ingredients { get; set; }
    }
}
