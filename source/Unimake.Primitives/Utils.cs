using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Unimake.Primitives
{
    internal static class Utils
    {
        #region Internal Methods

        internal static string Format(string value, string mask)
        {
            var mascara = new MaskedTextProvider(mask);

            mascara.Set(value, out _, out var resultHint);

            if(resultHint == MaskedTextResultHint.Success)
            {
                return mascara.ToDisplayString();
            }

            return value;
        }

        internal static string OnlyNumbers(object text)
        {
            var regexObj = new Regex(@"[^\d]");
            var result = regexObj.Replace(text.ToString(), "");
            return result ?? "";
        }

        #endregion Internal Methods
    }
}