using System.Security.Cryptography;

namespace VNet.Utility.Crypto
{
    public interface IKeyContainerParameters
    {
        string KeyContainerName { get; set; }
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flags")]
        CspProviderFlags Flags { get; set; }
        int KeyNumber { get; set; }
        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Csp")]
        CspParameters CspParameters { get; }
    }
}
