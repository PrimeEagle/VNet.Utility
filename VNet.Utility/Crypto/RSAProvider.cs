using System;
using global::System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace VNet.Utility.Crypto
{
    public sealed class RSAProvider : IRSAProvider, IDisposable
    {
        RSACryptoServiceProvider _rsa;
        IKeyContainerParameters _keyContainerParameters;

        public RSAProvider()
        {
            _keyContainerParameters = new KeyContainerParameters();
            _rsa = new RSACryptoServiceProvider(_keyContainerParameters.CspParameters);
        }

        private void RefreshKeyContainerParameters()
        {
            _rsa = new RSACryptoServiceProvider(_keyContainerParameters.CspParameters);
        }

        public void FromXmlString(string xml)
        {
            this.RefreshKeyContainerParameters();
            _rsa.FromXmlString(xml);
        }

        public string ToXmlString(bool includePrivate)
        {
            return _rsa.ToXmlString(includePrivate);
        }

        public void Clear()
        {
            this.RefreshKeyContainerParameters();
            _rsa.Clear();
        }

        public bool PersistKeyInCsp
        {
            get
            {
                return _rsa.PersistKeyInCsp;
            }
            set
            {
                _rsa.PersistKeyInCsp = value;
            }
        }

        [ExcludeFromCodeCoverage]
        public IKeyContainerParameters KeyContainerParameters
        {
            get
            {
                return _keyContainerParameters;
            }
            set
            {
                _keyContainerParameters = value;
            }
        }

        public void Dispose()
        {
            _rsa.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
