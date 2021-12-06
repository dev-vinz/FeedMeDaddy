using FeedMeDaddy.Services.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedMeDaddy.Services.Units
{
    public class WeightConverter : IWeightConverter
    {
        public UnitType TypeFor(UnitWeight.EUnit value)
        {
            switch (value)
            {
                case UnitWeight.EUnit.Gram:
                case UnitWeight.EUnit.Kilogram:
                    return UnitType.Weight;
                case UnitWeight.EUnit.Liter:
                    return UnitType.Volume;
                case UnitWeight.EUnit.Piece:
                    return UnitType.Piece;
                case UnitWeight.EUnit.Pinch:
                    return UnitType.Pinch;
                case UnitWeight.EUnit.Teaspoon:
                    return UnitType.Teaspoon;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }

        public UnitWeight.EUnit StandardUnit(UnitType type)
        {
            switch (type)
            {
                case UnitType.Piece:
                    return UnitWeight.EUnit.Piece;
                case UnitType.Pinch:
                    return UnitWeight.EUnit.Pinch;
                case UnitType.Teaspoon:
                    return UnitWeight.EUnit.Teaspoon;
                case UnitType.Volume:
                    return UnitWeight.EUnit.Liter;
                case UnitType.Weight:
                    return UnitWeight.EUnit.Gram;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public bool TryParse(string value, out UnitWeight unit)
        {
            value = value?.Trim();
            switch (value?.ToLower())
            {
                case "kg":
                    unit = new UnitWeight { Shortcut = "kg", Unit = UnitWeight.EUnit.Kilogram };
                    return true;
                case "g":
                case "gr":
                    unit = new UnitWeight { Shortcut = "g", Unit = UnitWeight.EUnit.Gram };
                    return true;
                case "l":
                    unit = new UnitWeight { Shortcut = "l", Unit = UnitWeight.EUnit.Liter };
                    return true;
                case "pce":
                    unit = new UnitWeight { Shortcut = "pce", Unit = UnitWeight.EUnit.Piece };
                    return true;
                case "ts":
                    unit = new UnitWeight { Shortcut = "ts", Unit = UnitWeight.EUnit.Teaspoon };
                    return true;
                case "pinch":
                    unit = new UnitWeight { Shortcut = "pinch", Unit = UnitWeight.EUnit.Pinch };
                    return true;
                default:
                    unit = null;
                    return false;
            }
        }

        #region Converters

        public double Convert(UnitWeight srcUnit, double value, UnitWeight destUnit)
        {
            if (TypeFor(srcUnit.Unit) != TypeFor(destUnit.Unit)) throw new ArgumentException("Units are not compatibles");
            if (srcUnit == destUnit) return value;

            switch (srcUnit.Unit)
            {
                case UnitWeight.EUnit.Gram:
                    switch (destUnit.Unit)
                    {
                        case UnitWeight.EUnit.Kilogram:
                            return value * 1_000;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(destUnit.Unit), destUnit.Unit, null);
                    }
                case UnitWeight.EUnit.Kilogram:
                    switch (destUnit.Unit)
                    {
                        case UnitWeight.EUnit.Gram:
                            return value / 1_000;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(destUnit.Unit), destUnit.Unit, null);
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(destUnit.Unit), destUnit.Unit, null);
            }
        }

        #endregion
    }
}
