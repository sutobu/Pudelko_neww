using PudelkoLibrary;
using System;

namespace PudelkoConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
             List<Pudelko> pudelka = new List<Pudelko>
             {
              new Pudelko(1, 2, 3),
              new Pudelko(5, 5, 5),
              new Pudelko(5.43, 4.32, 1.43)
             };

            Console.WriteLine("Original List of Pudelka:");
            foreach (var pudelko in pudelka)
            {
                Console.WriteLine(pudelko.ToString());
            }

            Comparison<Pudelko> comparePudelka = (p1, p2) =>
            {
                int volumeComparison = p1.Objetosc.CompareTo(p2.Objetosc);
                if (volumeComparison != 0) return volumeComparison;

                int surfaceAreaComparison = p1.Pole.CompareTo(p2.Pole);
                if (surfaceAreaComparison != 0) return surfaceAreaComparison;

                double sum1 = p1.A + p1.B + p1.C;
                double sum2 = p2.A + p2.B + p2.C;
                return sum1.CompareTo(sum2);
            };


            pudelka.Sort(comparePudelka);

            Console.WriteLine("\nSorted List of Pudelka:");
            foreach (var pudelko in pudelka)
            {
                Console.WriteLine(pudelko.ToString());
            }

            Console.WriteLine("\nKompresja:");
            var compressedBox = pudelka[0].Kompresuj();
            Console.WriteLine($"Original Box: {pudelka[0]}");
            Console.WriteLine($"Compressed Box (cube with same volume): {compressedBox}");
        }
    }
}
