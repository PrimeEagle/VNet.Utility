using System;

namespace VNet.Utility
{
	public static class StringUtility
    {
       public static string CreateRandomDigits(int length)
        {
            var random = new Random();
            var s = string.Empty;
            for (var i = 0; i < length; i++)
                s = string.Concat(s, random.Next(10).ToString());
            
            return s;
        }
    }
}
