using System.Text.RegularExpressions;

namespace Pluto.BlogCore.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static bool IsUrl(this string @this)
        {
            string pattern = @"^(http|https|ftp)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&$%\$#\=~])*$";
            return Regex.IsMatch(@this, pattern);
        }
    }
}