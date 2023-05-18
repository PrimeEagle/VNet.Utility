namespace VNet.Utility.Crypto
{
    public class AsymmetricPrivateKey
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "P")]
        public string P { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Q")]
        public string Q { get; set; }
        public string DP { get; set; }
        public string DQ { get; set; }
        public string InverseQ { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "D")]
        public string D { get; set; }
    }
}
