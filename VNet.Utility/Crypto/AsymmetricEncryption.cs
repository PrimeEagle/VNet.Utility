using System;
using System.Security;
using System.Security.Cryptography;
using VNet.Utility.Extensions;

namespace VNet.Utility.Crypto
{
    public static class AsymmetricEncryption
    {
        public static AsymmetricKeyInfo GenerateAsymmetricKeys()
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                AsymmetricKeyInfo keyInfo = new AsymmetricKeyInfo();
                keyInfo.FromXml(rsa.ToXmlString(true));

                return keyInfo;
            }
        }

        public static string EncryptString(SecureString plaintext, AsymmetricKeyInfo key)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(key.ToXml());

                byte[] encryptedData = rsa.Encrypt(plaintext.ToUnsecureString().ToBytes(), true);

                return ByteArrayExtensions.ToString(encryptedData);
            }
        }

        public static SecureString DecryptString(string encryptedData, AsymmetricKeyInfo key)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(key.ToXml());

                byte[] decryptedData = rsa.Decrypt(encryptedData.ToBytes(), true);

                return ByteArrayExtensions.ToString(decryptedData).ToSecureString();
            }
        }

        public static void SaveKeyInContainer(IRSAProvider provider, string containerName, AsymmetricKeyInfo key)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            if (provider == null)
                throw new ArgumentNullException("provider");

            provider.KeyContainerParameters.KeyContainerName = containerName;
            provider.KeyContainerParameters.Flags = CspProviderFlags.UseMachineKeyStore;
            provider.KeyContainerParameters.KeyNumber = (int)KeyNumber.Exchange;

            provider.FromXmlString(key.ToXml());
            provider.PersistKeyInCsp = true;
        }

        public static AsymmetricKeyInfo LoadKeyFromContainer(IRSAProvider provider, string containerName)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");

            provider.KeyContainerParameters.KeyContainerName = containerName;
            provider.KeyContainerParameters.Flags = CspProviderFlags.UseMachineKeyStore | CspProviderFlags.UseExistingKey;

            AsymmetricKeyInfo keyInfo = new AsymmetricKeyInfo();
            keyInfo.FromXml(provider.ToXmlString(true));

            return keyInfo;
        }

        public static bool KeyContainerExists(IRSAProvider provider, string containerName)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");

            bool exists = true;

            provider.KeyContainerParameters.KeyContainerName = containerName;
            provider.KeyContainerParameters.Flags = CspProviderFlags.UseMachineKeyStore | CspProviderFlags.UseExistingKey;

            try
            {
                provider.FromXmlString(string.Empty);
            }
            catch (CryptographicException)
            {
                exists = false;
            }

            return exists;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.GC.Collect")]
        public static void DeleteKeyFromContainer(IRSAProvider provider, string containerName)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");

            provider.KeyContainerParameters.KeyContainerName = containerName;
            provider.KeyContainerParameters.Flags = CspProviderFlags.UseMachineKeyStore;
            provider.PersistKeyInCsp = false;

            provider.Clear();

            GC.Collect();
        }
    }
}
