using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AS.Utility.Helpers
{
    public static class NumberHelper
    {
        public static int ToInt32(this string number)
        {
            int result;
            int.TryParse(number, out result);
            return result;
        }

        public static long ToInt64(this string number)
        {
            long result;
            long.TryParse(number, out result);
            return result;
        }

        public static long ToInt64(this double number)
        {
            return Convert.ToInt64(number);
        }

        public static double ToDouble(this string number)
        {
            double result;
            double.TryParse(number, out result);
            return result;
        }

        public static long RialToToman(this long price)
        {
            return price / 10;
        }

        public static double RialToToman(this double price)
        {
            return price / 10;
        }

        public static double ToPrice(this string price)
        {
            price = price.Replace(",", "");
            return price.ToDouble();
        }

        public static double DivisionBy6Zero(this string quant)
        {
            return ToDouble(quant) / 1000000;
        }

        public static double DivisionBy6Zero(this double amount)
        {
            return amount / 1000000;
        }

        public static long RemoveDecimalNumber(this double number)
        {
            return (long)number;
        }
    }
}
