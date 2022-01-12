using FeedMeDaddy.Services.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedMeDaddy.Services.Units
{
    public interface IWeightConverter
    {
        UnitType TypeFor(UnitWeight.EUnit value);
        UnitWeight.EUnit StandardUnit(UnitType type);

        bool TryParse(string value, out UnitWeight unit);

        double Convert(UnitWeight srcUnit, double value, UnitWeight destUnit);
    }
}
