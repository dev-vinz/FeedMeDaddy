using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedMeDaddy.Services.DataContracts
{
    public class UnitWeight
    {
        public EUnit Unit { get; set; }
        public string Shortcut { get; set; }

        public enum EUnit
        {
            Kilogram = 1,
            Gram = 2,
            Liter = 3,
            Piece = 4,
            Teaspoon = 5,
            Pinch = 6,
            Can = 7,
        };
    }
}
