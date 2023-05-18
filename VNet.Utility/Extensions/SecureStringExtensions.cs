using System;
using System.Runtime.InteropServices;
using System.Security;

namespace VNet.Utility.Extensions
{
	public static class SecureStringExtensions
    {
        public static string ToUnsecureString(this SecureString secureText)
        {
            if (secureText is null)
                throw new ArgumentNullException();

            var unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureText);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}
