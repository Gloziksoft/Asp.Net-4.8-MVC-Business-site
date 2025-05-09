using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace dufeksoft.lib.Text
{
    public class StringHelper
    {
        /// <summary>
        /// Removes diacritics from input string
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>Output string without diacritics</returns>
        public static string RemoveDiacritics(string input)
        {
            string normalizedString = input.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < normalizedString.Length; i++)
            {
                char c = normalizedString[i];
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    stringBuilder.Append(c);
            }

            return stringBuilder.ToString();
        }

        public static string KeepOnlyValidCharacters(string input, string regex)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            StringBuilder str = new StringBuilder();
            Regex r = new Regex(regex);
            for (int i = 0; i < input.Length; i++)
            {
                string s = input.Substring(i, 1);
                if (r.IsMatch(s))
                {
                    str.Append(s);
                }
            }

            return str.ToString();
        }
    }
}
