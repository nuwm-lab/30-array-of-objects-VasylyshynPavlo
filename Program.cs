using System;
using System.Globalization;

namespace LabWork
{
    // Даний проект є шаблоном для виконання лабораторних робіт
    // з курсу "Об'єктно-орієнтоване програмування та патерни проектування"
    // Необхідно змінювати і дописувати код лише в цьому проекті
    // Відео-інструкції щодо роботи з github можна переглянути 
    // за посиланням https://www.youtube.com/@ViktorZhukovskyy/videos 

    // Class formula for sin(ax + b)
    class Formula
    {
        private double _A;
        private double _a;
        private double _b;

        public Formula(double A, double a, double b)
        {
            _A = A;
            _a = a;
            _b = b;
        }

        public double A
        {
            get => _A;
            set => _A = value;
        }

        public double a
        {
            get => _a;
            set => _a = value;
        }

        public double b
        {
            get => _b;
            set => _b = value;
        }

        public double Calculate(double x)
        {
            return _A * Math.Sin(_a * x + _b);
        }

        public double GetAmplitude()
        {
            return Math.Abs(_A);
        }

        public override string ToString()
        {
            return $"y = {A} * sin({a}x + {b})";
        }

    }

    class Program
    {
        static void Main()
        {
            Console.Write("Enter the number of formulas: ");
            if (!int.TryParse(Console.ReadLine(), out int n) || n <= 0)
            {
                Console.WriteLine("Invalid number.");
                return;
            }

            Formula[] formulas = new Formula[n];
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"Enter A, a, b (in radians) for formula #{i + 1}:");
                string[] parts = Console.ReadLine()
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != 3 ||
                    !double.TryParse(parts[0], NumberStyles.Any, CultureInfo.InvariantCulture, out double A) ||
                    !double.TryParse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out double a) ||
                    !double.TryParse(parts[2], NumberStyles.Any, CultureInfo.InvariantCulture, out double b))
                {
                    Console.WriteLine("Invalid input. Try again.");
                    i--;
                    continue;
                }

                formulas[i] = new Formula(A, a, b);
            }

            Console.Write("Enter x (in radians): ");
            if (!double.TryParse(Console.ReadLine(), NumberStyles.Any, CultureInfo.InvariantCulture, out double x))
            {
                Console.WriteLine("Invalid x.");
                return;
            }

            const double epsilon = 1e-9;
            Formula bestFormula = formulas[0];
            double bestValue = bestFormula.Calculate(x);

            foreach (var formula in formulas)
            {
                double value = formula.Calculate(x);
                if (value > bestValue + epsilon ||
                    Math.Abs(value - bestValue) <= epsilon && formula.GetAmplitude() > bestFormula.GetAmplitude())
                {
                    bestValue = value;
                    bestFormula = formula;
                }
            }

            Console.WriteLine("\nFormula with maximum value at given x:");
            Console.WriteLine(bestFormula);
            Console.WriteLine($"Value at x = {x}: {bestValue}");
            Console.WriteLine($"Amplitude: {bestFormula.GetAmplitude()}");
        }
    }
}
