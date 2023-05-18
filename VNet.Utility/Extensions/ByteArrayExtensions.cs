using System;
using System.Globalization;
using System.Text;

namespace VNet.Utility.Extensions
{
	public static class ByteArrayExtensions
    {
        // based on: http://stackoverflow.com/questions/1660870/writing-values-to-the-registry-with-c-sharp
        public static string ToHexString(this byte[] value)
        {
            if (value is null)
                throw new ArgumentNullException();

            var sb = new StringBuilder();
            foreach (var t in value)
                sb.Append(t.ToString("x2", CultureInfo.InvariantCulture));

            return sb.ToString();
        }

       public static string ToString(this byte[] array)
        {
            if (array is null)
                throw new ArgumentNullException();

            var chars = new char[array.Length / sizeof(char)];
            System.Buffer.BlockCopy(array, 0, chars, 0, array.Length);
            return new string(chars);
        }
    }
}
