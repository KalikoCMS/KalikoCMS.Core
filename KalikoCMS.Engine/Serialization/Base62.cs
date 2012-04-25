//
// Based on work by Michael Stum. http://www.stum.de/2008/10/20/base36-encoderdecoder-in-c/
//

namespace KalikoCMS.Serialization {
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class Base62 {
        private const string CharList = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
        private const int BaseLength = 62;
        private static readonly char[] CharListArray = CharList.ToCharArray();

        public static string Encode(int input) {
            if (input < 0) {
                throw new ArgumentOutOfRangeException("input", input, "Input cannot be negative.");
            }

            var result = new Stack<char>();

            while (input != 0) {
                result.Push(CharListArray[input % BaseLength]);
                input /= BaseLength;
            }

            return new string(result.ToArray());
        }

        public static int Decode(string input) {
            var reversed = input.Reverse();
            int result = 0;
            int pos = 0;
            foreach (char c in reversed) {
                result += CharList.IndexOf(c) * (int)Math.Pow(BaseLength, pos);
                pos++;
            }
            return result;
        }
    }
}
