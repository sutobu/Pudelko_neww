using System;

namespace PudelkoLibrary
{
    public static class PudelkoExtensions
    {
        public static Pudelko Kompresuj(this Pudelko pudelko)
        {
            double side = Math.Pow(pudelko.Objetosc, 1.0 / 3.0);
            return new Pudelko(side, side, side, pudelko.Unit);
        }
    }
}