using System;
using System.Text;

namespace IndeedClone.Modules.Shared.RefNo
{

    /* 
     * # Static class to generate Referance Number
     * # Ex. - 8RCL-174C-UCNM-50AA-P185-
    */
    public static class ReferenceNumber
    {
        private static readonly Random _random = new();

        public static string GenerateRefNo()
        {
            string part1 = RandomAlphaNumeric(4);
            string part2 = RandomAlphaNumeric(4);
            string part3 = RandomAlphaNumeric(4);
            string part4 = RandomAlphaNumeric(4);
            string part5 = RandomAlphaNumeric(4);
            string part6 = RandomAlphaNumeric(4);

            return $"{part1}-{part2}-{part3}-{part4}-{part5}-{part6}".ToUpper();
        }

     // #  Generates random alphanumeric string of given length
        private static string RandomAlphaNumeric(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                sb.Append(chars[_random.Next(chars.Length)]);
            }
            return sb.ToString();
        }
    }
}
