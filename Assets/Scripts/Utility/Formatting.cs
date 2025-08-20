using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BreakInfinity;
using UnityEngine;

namespace DotsKiller.Utility
{
    public static class Formatting
    {
        /// <summary>
        /// Displays numbers like this:
        /// 0.00 - 1: 0.##;
        /// 0 - 999.99: 0.##;
        /// 1K - 999.99T: power of 1000 number suffix;
        /// min value - 0, 1e15 - max value: scientific notation;
        /// infinity, NaN, >= limit: "://OV#R!OAD".
        /// </summary>
        /// <param name="number">Value to format.</param>
        /// <param name="maxDecimalDigits">Maximum amount of digits shown after the comma.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when abbreviating the number but it is not covered by the abbreviations list</exception>
        public static string DefaultFormat(BigDouble number, int maxDecimalDigits = 2)
        {
            if (BigDouble.IsInfinity(number) || BigDouble.IsNaN(number))
            {
                return "://OV#R!OAD";
            }
            
            long power = number.Exponent;
            if (power >= Formulas.Limit.Exponent)
            {
                return "://OV#R!OAD";
            }

            string format = $"F{maxDecimalDigits}";
            string optionalDigitsFormat = "0." + new string('#', maxDecimalDigits);
            //0.72
            if (power is < 0 and > -3)
            {
                return number.ToDouble().ToString(optionalDigitsFormat);
            }
            
            double mantissa = Math.Truncate(number.Mantissa * 100) / 100;
            //2.31e-9.11e7, 1.37e102, 8.32e3.21e8
            if (power is >= 15 or < 0)
            {
                return mantissa.ToString(format) + "e" + FormatExponent(number.Exponent);
            }
            
            long exp = Mathf.FloorToInt(power / 3f) * 3;
            long diff = power - exp;
            mantissa = number.Mantissa * BigDouble.Pow10(diff).ToDouble();
            
            //0 - 999.99
            if (power is < 3 and >= 0 )
            {
                return mantissa.ToString(optionalDigitsFormat);
            }

            //1K - 999.99T
            string magnitudePrefix = exp switch
            {
                < 3 => string.Empty,
                < 6 => "K",
                < 9 => "M",
                < 12 => "B",
                < 15 => "T",
                _ => throw new ArgumentOutOfRangeException(),
            };
                
            return mantissa.ToString(format) + magnitudePrefix;
        }


        private static string FormatExponent(double exponent)
        {
            if (exponent >= 1e9)
            {
                long power = Mathf.FloorToInt(Mathf.Log10((float) exponent));
                double mantissa = exponent / Math.Pow(10, power);
                
                return mantissa.ToString("f2") + "e" + power;
            }
            
            if (exponent >= 1e4)
            {
                return exponent.ToString("N0");
            }

            return exponent.ToString("f0");
        }


        public static string SplitPascalCase(string text)
        {
            return Regex.Replace(
                text,
                @"(\p{Ll})(\P{Ll})",
                "$1 $2"
            );
        }
        

        public static string Sign(float value)
        {
            return value > 0 ? "+" : "-";
        }
        
        
        public static string Sign(BigDouble value)
        {
            return value > 0 ? "+" : "-";
        }


        public static string RomanFormat(int value)
        {
            switch (value)
            {
                case < 0:
                case > 3999:
                    throw new ArgumentOutOfRangeException(nameof(value), "insert value between 1 and 3999");
                case < 1:
                    return string.Empty;
                case >= 1000:
                    return "M" + RomanFormat(value - 1000);
                case >= 900:
                    return "CM" + RomanFormat(value - 900);
                case >= 500:
                    return "D" + RomanFormat(value - 500);
                case >= 400:
                    return "CD" + RomanFormat(value - 400);
                case >= 100:
                    return "C" + RomanFormat(value - 100);
                case >= 90:
                    return "XC" + RomanFormat(value - 90);
                case >= 50:
                    return "L" + RomanFormat(value - 50);
                case >= 40:
                    return "XL" + RomanFormat(value - 40);
                case >= 10:
                    return "X" + RomanFormat(value - 10);
                case >= 9:
                    return "IX" + RomanFormat(value - 9);
                case >= 5:
                    return "V" + RomanFormat(value - 5);
                case >= 4:
                    return "IV" + RomanFormat(value - 4);
                case >= 1:
                    return "I" + RomanFormat(value - 1);
                default:
                    throw new Exception("Impossible state reached");
            }
        }
    }
}