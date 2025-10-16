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
    public class Formula
    {
        public double Amplitude { get; set; }
        public double Frequency { get; set; }
        public double PhaseShift { get; set; }

        public Formula(double amplitude, double frequency, double phaseShift)
        {
            Amplitude = amplitude;
            Frequency = frequency;
            PhaseShift = phaseShift;
        }

        public double Calculate(double x)
        {
            return Amplitude * Math.Sin(Frequency * x + PhaseShift);
        }

        public double GetAmplitude()
        {
            return Math.Abs(Amplitude);
        }

        public override string ToString()
        {
            string sign = PhaseShift < 0 ? " - " : " + ";
            double absPhase = Math.Abs(PhaseShift);

            return string.Format(
                CultureInfo.InvariantCulture,
                "y = {0} * sin({1}x{2}{3})",
                Amplitude, Frequency, sign, absPhase
            );
        }
    }

    class Program
    {
        static void Main()
        {
            TestBlock();

            Console.Write("Enter the number of formulas: ");
            if (!int.TryParse(Console.ReadLine(), out int n) || n <= 0)
            {
                Console.WriteLine("Invalid number.");
                return;
            }

            Formula[] formulas = new Formula[n];
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"Enter Amplitude(A), Frequency(a), PhaseShift(b) (in radians) for formula #{i + 1} y = A * sin(ax + b):");
                var input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Empty input. Try again.");
                    i--;
                    continue;
                }

                string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

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
                double amplitude = formula.GetAmplitude();

                bool isGreater = value > bestValue + epsilon;
                bool isEqual = Math.Abs(value - bestValue) <= epsilon;

                if (isGreater || (isEqual && amplitude > bestFormula.GetAmplitude()))
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

        static void TestBlock()
        {
            // --- Manual test block ---
            Console.WriteLine("\n=== Manual test block ===");

            Formula f1 = new Formula(2, 1, 0);   // y = 2 * sin(x)
            Formula f2 = new Formula(3, 1, 0);   // y = 3 * sin(x)
            double testX = Math.PI / 2;          // sin(pi/2) = 1

            double v1 = f1.Calculate(testX);     // 2
            double v2 = f2.Calculate(testX);     // 3

            Console.WriteLine($"{f1} -> {v1}");
            Console.WriteLine($"{f2} -> {v2}");
            Console.WriteLine("Expected: formula 2 should be chosen (higher amplitude).");

            Formula[] testFormulas = { f1, f2 };
            Formula best = testFormulas[0];
            double bestVal = best.Calculate(testX);
            const double eps = 1e-9;

            foreach (var f in testFormulas)
            {
                double val = f.Calculate(testX);
                bool greater = val > bestVal + eps;
                bool equal = Math.Abs(val - bestVal) <= eps;

                if (greater || (equal && f.GetAmplitude() > best.GetAmplitude()))
                {
                    bestVal = val;
                    best = f;
                }
            }

            Console.WriteLine($"Chosen: {best}");
            Console.WriteLine($"Value: {bestVal}");
        }
    }
}
