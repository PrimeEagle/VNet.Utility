using System.Security;

namespace VNet.Utility.Crypto
{
    public class SplitCipherKey
    {
        public SecureString KeyFragment { get; set; }
        public SecureString Pad { get; set; }
    }
}
