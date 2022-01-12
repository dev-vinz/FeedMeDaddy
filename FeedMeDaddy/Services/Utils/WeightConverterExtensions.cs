using FeedMeDaddy.Services.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedMeDaddy.Services.Units
{
    public static class WeightConverterExtensions
    {
        public static double Add(this IWeightConverter converter, UnitWeight unit1, double value1, UnitWeight unit2, double value2, UnitWeight destUnit)
        {
            return converter.Convert(unit1, value1, destUnit) + converter.Convert(unit2, value2, destUnit);
        }
    }
}
