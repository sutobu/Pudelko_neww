using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using PudelkoLib;

namespace PudelkoLibrary
{

    public class Pudelko : IFormattable, IEquatable<Pudelko>, IEnumerable<double>
    {
        private const double DefaultDimension = 0.1; // 10 cm in meters
        private const double MaxDimension = 10.0; // 10 meters

        private readonly double a;
        private readonly double b;
        private readonly double c;

        public double A => Math.Round(a, 3);
        public double B => Math.Round(b, 3);
        public double C => Math.Round(c, 3);
        public UnitOfMeasure Unit { get; }

        public Pudelko(double a = 0.1, double b = 0.1, double c = 0.1, UnitOfMeasure unit = UnitOfMeasure.meter)
        {
            this.a = Math.Round(ConvertToMeters(a, unit), 3);
            this.b = Math.Round(ConvertToMeters(b, unit), 3);
            this.c = Math.Round(ConvertToMeters(c, unit), 3);

            ValidateDimensions(this.a, this.b, this.c);
            Unit = unit;
        }
        private double ConvertToMeters(double value, UnitOfMeasure unit)
        {
            if(UnitOfMeasure.milimeter == unit)
            {
                return value / 1000;
            }
            else if (UnitOfMeasure.centimeter == unit)
            {
                return value / 100;
            }
            else if (UnitOfMeasure.meter == unit)
            {
                return value;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(unit), "Invalid unit of measure");
            }
        }

        private void ValidateDimensions(params double[] dimensions)
        {
            foreach (var dimension in dimensions)
            {
                if (dimension <= 0 || dimension > MaxDimension)
                {
                    throw new ArgumentOutOfRangeException(nameof(dimensions), "Dimensions must be positive and less than or equal to 10 meters");
                }
            }
        }

        public override string ToString()
        {
            return ToString("m", null);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format)) format = "m";

            if (format == "m")
            {
                return $"{A} m × {B} m × {C} m";
            }
            else if (format == "cm")
            {
                return $"{A * 100} cm × {B * 100} cm × {C * 100} cm";
            }
            else if (format == "mm")
            {
                return $"{A * 1000} mm × {B * 1000} mm × {C * 1000} mm";
            }
            else
            {
                throw new FormatException($"The format '{format}' is not supported.");
            }
        }

        public double Objetosc => Math.Round(a * b * c, 9);
        public double Pole => Math.Round(2 * (a * b + b * c + a * c), 6);


        public bool Equals(Pudelko other)
        {
            if (other == null) return false;

            var dimensions = new[] { A, B, C };
            var otherDimensions = new[] { other.A, other.B, other.C };

            Array.Sort(dimensions);
            Array.Sort(otherDimensions);

            return dimensions.SequenceEqual(otherDimensions);
        }

        public override bool Equals(object obj)
        {
            if (obj is Pudelko other)
            {
                return Equals(other);
            }
            return false;
        }

        public override int GetHashCode()
        {
            var dimensions = new[] { A, B, C };
            Array.Sort(dimensions);
            return HashCode.Combine(dimensions[0], dimensions[1], dimensions[2]);
        }

        public static bool operator ==(Pudelko left, Pudelko right)
        {
            if (left is null) return right is null;
            return left.Equals(right);
        }

        public static bool operator !=(Pudelko left, Pudelko right)
        {
            return !(left == right);
        }

        public static Pudelko operator +(Pudelko p1, Pudelko p2)
        {
            double newA = p1.A + p2.A;
            double newB = p1.B + p2.B;
            double newC = p1.C + p2.C;

            return new Pudelko(newA, newB, newC, UnitOfMeasure.meter);
        }

        public static explicit operator double[](Pudelko p)
        {
            return new[] { p.A, p.B, p.C };
        }

        public static implicit operator Pudelko((int a, int b, int c) dimensions)
        {
            return new Pudelko(dimensions.a / 1000.0, dimensions.b / 1000.0, dimensions.c / 1000.0, UnitOfMeasure.meter);
        }

        public double this[int index]
        {
            get
            {
                if(index < 0 || index > 2)
                    throw new IndexOutOfRangeException("Index must be 0, 1, or 2");
                if (index == 0)
                    return A;
                else if (index == 1)
                    return B;
                else
                    return C;
            }
        }

        public IEnumerator<double> GetEnumerator()
        {
            yield return A;
            yield return B;
            yield return C;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static Pudelko Parse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentNullException(nameof(input), "Input string cannot be null or empty");

            var parts = input.Split('×', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 3)
                throw new FormatException("Input string is not in the correct format");

            double[] dimensions = new double[3];
            UnitOfMeasure unit = UnitOfMeasure.meter;

            for (int i = 0; i < parts.Length; i++)
            {
                var part = parts[i].Trim();
                var valueUnit = part.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (valueUnit.Length != 2)
                    throw new FormatException("Input string is not in the correct format");

                if (!double.TryParse(valueUnit[0], NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
                    throw new FormatException("Input string is not in the correct format");

                if (valueUnit[1] == "cm")
                    unit = UnitOfMeasure.centimeter;
                else if (valueUnit[1] == "mm")
                    unit = UnitOfMeasure.milimeter;
                else if (valueUnit[1] == "m")
                    unit = UnitOfMeasure.meter;
                else
                    throw new FormatException("Input string is not in the correct format");

                if (unit == UnitOfMeasure.milimeter)
                    dimensions[i] = value / 1000;
                else if (unit == UnitOfMeasure.centimeter)
                    dimensions[i] = value / 100;
                else if (unit == UnitOfMeasure.meter)
                    dimensions[i] = value;
                else
                    throw new FormatException("Input string is not in the correct format");

            }
                return new Pudelko(dimensions[0], dimensions[1], dimensions[2], UnitOfMeasure.meter);
            
        }
    }
}