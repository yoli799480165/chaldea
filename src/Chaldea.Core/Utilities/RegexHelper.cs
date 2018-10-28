using System.Text.RegularExpressions;

namespace Chaldea.Core.Utilities
{
    public static class RegexHelper
    {
        public static bool IsTelephone(string telephone)
        {
            const string regex = "^((13[0-9])|(14[5|7])|(15([0-3]|[5-9]))|(18[0,5-9]))\\d{8}$";
            return Regex.IsMatch(telephone, regex);
        }
    }
}