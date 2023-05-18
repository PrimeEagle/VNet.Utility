using System.Security.Cryptography;

namespace VNet.Utility.Crypto
{
    public interface IKeyContainerParameters
    {
        string KeyContainerName { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flags")]
        CspProviderFlags Flags { get; set; }
        int KeyNumber { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Csp")]
        CspParameters CspParameters { get; }
    }
}
