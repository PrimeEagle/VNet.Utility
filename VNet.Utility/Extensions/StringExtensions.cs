using System;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Text;

namespace VNet.Utility.Extensions
{
	public static class StringExtensions
    {
        public static string After(this string str, string searchTerm)
        {
            var idx = str.IndexOf(searchTerm, StringComparison.Ordinal);
            var result = idx >= 0 ? str[(idx + searchTerm.Length)..] : str;

            return result;
        }

        public static string Before(this string str, string searchTerm)
        {
            var idx = str.IndexOf(searchTerm, StringComparison.Ordinal);
            var result = idx >= 0 ? str[..idx] : str;

            return result;
        }

        public static bool IsNumber(this string s)
        {
            int tempInt;
            var result = int.TryParse(s, out tempInt);

            return result;
        }

        public static SecureString ToSecureString(this string text)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException();

            var securePassword = new SecureString();
            foreach (var c in text)
                securePassword.AppendChar(c);
            securePassword.MakeReadOnly();
            
            return securePassword;
        }
        
        // based on: http://stackoverflow.com/questions/1660870/writing-values-to-the-registry-with-c-sharp
        public static byte[] HexStringToBytes(this string value)
        {
            var sb = new StringBuilder(value);

            var s = sb.ToString();
            var bytes = new byte[s.Length / 2];
            for (var i = 0; i < s.Length; i += 2)
                bytes[i / 2] = byte.Parse(s.Substring(i, 2), System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture);

            return bytes;
        }

        public static byte[] ToBytes(this string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException();

            var bytes = new byte[value.Length * sizeof(char)];
            System.Buffer.BlockCopy(value.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static bool IsValidPropertyName<T>(this string propertyName)
        {
	        return typeof(T).GetProperties().Select(p => p.Name).Contains(propertyName);
        }

        public static bool IsValidMethodName<T>(this string methodName)
        {
	        return typeof(T).GetMethods().Select(p => p.Name).Contains(methodName);
        }
    }
}