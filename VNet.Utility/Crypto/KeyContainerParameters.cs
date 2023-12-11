using global::System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace VNet.Utility.Crypto
{
    public class KeyContainerParameters : IKeyContainerParameters
    {
        CspParameters _cspParameters;

        public KeyContainerParameters()
        {
            _cspParameters = new CspParameters();
        }

        //Excluded because the KeyContainerName field does not show up under Moles.
        [ExcludeFromCodeCoverage]
        public string KeyContainerName
        {
            get
            {
                return _cspParameters.KeyContainerName;
            }
            set
            {
                _cspParameters.KeyContainerName = value;
            }
        }

        public global::System.Security.Cryptography.CspProviderFlags Flags
        {
            get
            {
                return _cspParameters.Flags;
            }
            set
            {
                _cspParameters.Flags = value;
            }
        }

        //Excluded because the KeyNumber field does not show up under Moles.
        [ExcludeFromCodeCoverage]
        public int KeyNumber
        {
            get
            {
                return _cspParameters.KeyNumber;
            }
            set
            {
                _cspParameters.KeyNumber = value;
            }
        }

        public CspParameters CspParameters
        {
            get { return _cspParameters; }
        }
    }
}
