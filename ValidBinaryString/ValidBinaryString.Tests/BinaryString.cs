using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ValidBinaryString.Tests
{
    public class BinaryString
    {
        public bool IsGood(string input)
        {
            ValidateInputIsValidBinary(input);

            var number = Convert.ToInt32(input, fromBase: 2);

            if (number == 0) return false;

            var result = CountOnesAndZeros(number);

            if (result.Ones != result.Zeros) return false;

            foreach (var prefix in result.Prefixes)
            {
                result = CountOnesAndZeros(prefix);
                if (result.Ones < result.Zeros) return false;
            }

            return true;
        }

        private static void ValidateInputIsValidBinary(string input)
        {
            var regex = new Regex("[01]+");
            
            if (!regex.IsMatch(input))
            {
                throw new ArgumentException("The string is not a valid binary");
            }
        }

        private static Result CountOnesAndZeros(int number)
        {
            var zeros = 0;
            var ones = 0;
            var prefixes = new List<int>();

            while (number != 0)
            {
                prefixes.Add(number);

                if ((number & 1) == 1) // if odd the bit is 1
                    ones++;
                else
                    zeros++;

                number >>= 1;
            }

            return new Result
            {
                Ones = ones,
                Zeros = zeros,
                Prefixes = prefixes
            };
        }

        private class Result
        {
            public int Ones { get; set; }

            public int Zeros { get; set; }

            public List<int> Prefixes { get; set; }
        }
    }


}