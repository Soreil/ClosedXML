using System;
using System.Text;

namespace ClosedXML.Excel.CalcEngine.Functions
{
    public static class XLMath
    {
        public static double DegreesToRadians(double degrees)
        {
            return (Math.PI / 180.0) * degrees;
        }

        public static double RadiansToDegrees(double radians)
        {
            return (180.0 / Math.PI) * radians;
        }

        public static double GradsToRadians(double grads)
        {
            return (grads / 200.0) * Math.PI;
        }

        public static double RadiansToGrads(double radians)
        {
            return (radians / Math.PI) * 200.0;
        }

        public static double DegreesToGrads(double degrees)
        {
            return (degrees / 9.0) * 10.0;
        }

        public static double GradsToDegrees(double grads)
        {
            return (grads / 10.0) * 9.0;
        }

        public static double Asinh(double x)
        {
            return (Math.Log(x + Math.Sqrt(x * x + 1.0)));
        }

        public static double ACosh(double x)
        {
            return (Math.Log(x + Math.Sqrt((x * x) - 1.0)));
        }

        public static double ATanh(double x)
        {
            return (Math.Log((1.0 + x) / (1.0 - x)) / 2.0);
        }

        public static double ACoth(double x)
        {
            //return (Math.Log((x + 1.0) / (x - 1.0)) / 2.0);
            return (ATanh(1.0 / x));
        }

        public static double ASech(double x)
        {
            return (ACosh(1.0 / x));
        }

        public static double ACsch(double x)
        {
            return (Asinh(1.0 / x));
        }

        public static double Sech(double x)
        {
            return (1.0 / Math.Cosh(x));
        }

        public static double Csch(double x)
        {
            return (1.0 / Math.Sinh(x));
        }

        public static double Coth(double x)
        {
            return (Math.Cosh(x) / Math.Sinh(x));
        }

        internal static OneOf<double, XLError> CombinChecked(double number, double numberChosen)
        {
            if (number < 0 || numberChosen < 0)
                return XLError.NumberInvalid;

            var n = Math.Floor(number);
            var k = Math.Floor(numberChosen);

            // Parameter doesn't fit into int. That's how many multiplications Excel allows.
            if (n >= int.MaxValue || k >= int.MaxValue)
                return XLError.NumberInvalid;

            if (n < k)
                return XLError.NumberInvalid;

            var combinations = Combin(n, k);
            if (double.IsInfinity(combinations) || double.IsNaN(combinations))
                return XLError.NumberInvalid;

            return combinations;
        }

        internal static double Combin(double n, double k)
        {
            if (k == 0) return 1;

            // Don't use recursion, malicious input could exhaust stack.
            // Don't calculate directly from factorials, could overflow.
            double result = 1;
            for (var i = 1; i <= k; i++, n--)
            {
                result *= n;
                result /= i;
            }

            return result;
        }

        internal static double Factorial(double n)
        {
            n = Math.Truncate(n);
            var factorial = 1d;
            while (n > 1)
            {
                factorial *= n--;

                // n can be very large, stop when we reach infinity.
                if (double.IsInfinity(factorial))
                    return factorial;
            }

            return factorial;
        }

        public static Boolean IsEven(Int32 value)
        {
            return Math.Abs(value % 2) == 0;
        }

        public static Boolean IsEven(double value)
        {
            // Check the number doesn't have any fractions and that it is even.
            // Due to rounding after division, only checking for % 2 could fail
            // for numbers really close to whole number.
            var hasNoFraction = value % 1 == 0;
            var isEven = value % 2 == 0;
            return hasNoFraction && isEven;
        }

        public static Boolean IsOdd(Int32 value)
        {
            return Math.Abs(value % 2) != 0;
        }

        public static Boolean IsOdd(double value)
        {
            var hasNoFraction = value % 1 == 0;
            var isOdd = value % 2 != 0;
            return hasNoFraction && isOdd;
        }

        public static int RomanToArabic(string text)
        {
            if (text.Length == 0)
                return 0;
            if (text.StartsWith("M", StringComparison.InvariantCultureIgnoreCase))
                return 1000 + RomanToArabic(text.Substring(1));
            if (text.StartsWith("CM", StringComparison.InvariantCultureIgnoreCase))
                return 900 + RomanToArabic(text.Substring(2));
            if (text.StartsWith("D", StringComparison.InvariantCultureIgnoreCase))
                return 500 + RomanToArabic(text.Substring(1));
            if (text.StartsWith("CD", StringComparison.InvariantCultureIgnoreCase))
                return 400 + RomanToArabic(text.Substring(2));
            if (text.StartsWith("C", StringComparison.InvariantCultureIgnoreCase))
                return 100 + RomanToArabic(text.Substring(1));
            if (text.StartsWith("XC", StringComparison.InvariantCultureIgnoreCase))
                return 90 + RomanToArabic(text.Substring(2));
            if (text.StartsWith("L", StringComparison.InvariantCultureIgnoreCase))
                return 50 + RomanToArabic(text.Substring(1));
            if (text.StartsWith("XL", StringComparison.InvariantCultureIgnoreCase))
                return 40 + RomanToArabic(text.Substring(2));
            if (text.StartsWith("X", StringComparison.InvariantCultureIgnoreCase))
                return 10 + RomanToArabic(text.Substring(1));
            if (text.StartsWith("IX", StringComparison.InvariantCultureIgnoreCase))
                return 9 + RomanToArabic(text.Substring(2));
            if (text.StartsWith("V", StringComparison.InvariantCultureIgnoreCase))
                return 5 + RomanToArabic(text.Substring(1));
            if (text.StartsWith("IV", StringComparison.InvariantCultureIgnoreCase))
                return 4 + RomanToArabic(text.Substring(2));
            if (text.StartsWith("I", StringComparison.InvariantCultureIgnoreCase))
                return 1 + RomanToArabic(text.Substring(1));

            throw new ArgumentOutOfRangeException("text is not a valid roman number");
        }

        public static string ChangeBase(long number, int radix)
        {
            if (number < 0)
                throw new ArgumentOutOfRangeException("number must be greater or equal to 0");
            if (radix < 2)
                throw new ArgumentOutOfRangeException("radix must be greater or equal to 2");
            if (radix > 36)
                throw new ArgumentOutOfRangeException("radix must be smaller than or equal to 36");

            StringBuilder sb = new StringBuilder();
            long remaining = number;

            if (remaining == 0)
            {
                sb.Insert(0, '0');
            }

            while (remaining > 0)
            {
                var nextDigitDecimal = remaining % radix;
                remaining = remaining / radix;

                if (nextDigitDecimal < 10)
                    sb.Insert(0, nextDigitDecimal);
                else
                    sb.Insert(0, (char)(nextDigitDecimal + 55));
            }

            return sb.ToString();
        }
    }
}
