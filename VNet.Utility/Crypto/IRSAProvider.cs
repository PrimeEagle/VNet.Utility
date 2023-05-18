namespace VNet.Utility.Crypto
{
    public interface IRSAProvider
    {
        void FromXmlString(string xml);
        string ToXmlString(bool includePrivate);
        bool PersistKeyInCsp { get; set; }
        IKeyContainerParameters KeyContainerParameters { get; set; }
        void Clear();
    }
}
