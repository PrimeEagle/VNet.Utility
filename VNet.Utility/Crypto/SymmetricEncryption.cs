using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using VNet.Utility.Extensions;

namespace VNet.Utility.Crypto
{
    public static class SymmetricEncryption
    {
        const int saltIterations = 4096;

        /// <summary>
        /// Encrypt the given string using AES.  The string can be decrypted using 
        /// DecryptStringAes().  The cipherKey parameters must match.
        /// </summary>
        /// <param name="plaintext">The text to encrypt.</param>
        /// <param name="cipherKey">A password used to generate a key for encryption.</param>
        /// <param name="salt">A random salt, encoded as a Base64 string.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "plainSalt"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "plainCipher"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public static string EncryptString(SecureString plaintext, SecureString cipherKey, SecureString salt)
        {
            if (plaintext == null)
                throw new ArgumentNullException();
            if (cipherKey == null)
                throw new ArgumentNullException();
            if (salt == null)
                throw new ArgumentNullException();

            string outStr = null;                       // Encrypted string to return
            AesManaged aesAlg = null;              // AesManaged object used to encrypt the data.

            try
            {
                // generate the key from the shared secret and the salt
                using (Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(cipherKey.ToUnsecureString(), salt.ToUnsecureString().HexStringToBytes(), saltIterations))
                {
                    // Create a AesManaged object with the specified key and IV.
                    using (aesAlg = new AesManaged())
                    {
                        aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                        aesAlg.IV = key.GetBytes(aesAlg.BlockSize / 8);

                        // Create an encryptor to perform the stream transform.
                        ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                        // Create the streams used for encryption.
                        using (MemoryStream msEncrypt = new MemoryStream())
                        {
                            using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                            {
                                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                                {
                                    //Write all data to the stream.
                                    swEncrypt.Write(plaintext.ToUnsecureString());
                                }
                            }
                            outStr = msEncrypt.ToArray().ToHexString();
                        }
                    }
                }
            }
            finally
            {
                // Clear the AesManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            // Return the encrypted bytes from the memory stream.
            return outStr;
        }

        /// <summary>
        /// Decrypt the given string.  Assumes the string was encrypted using 
        /// EncryptStringAes(), using an identical cipherKey.
        /// </summary>
        /// <param name="cipherText">The text to decrypt.</param>
        /// <param name="cipherKey">A password used to generate a key for decryption.</param>
        /// <param name="salt">A random salt, encoded as a Base64 string.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public static SecureString DecryptString(string cipherText, SecureString cipherKey, SecureString salt)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentNullException("cipherText");
            if (cipherKey == null)
                throw new ArgumentNullException("cipherKey");
            if (salt == null)
                throw new ArgumentNullException("salt");

            // Declare the AesManaged object used to decrypt the data.
            AesManaged aesAlg = null;

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            try
            {
                // generate the key from the shared secret and the salt
                using (Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(cipherKey.ToUnsecureString(), salt.ToUnsecureString().HexStringToBytes(), saltIterations))
                {

                    // Create a AesManaged object with the specified key and IV.
                    using (aesAlg = new AesManaged())
                    {
                        aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                        aesAlg.IV = key.GetBytes(aesAlg.BlockSize / 8);

                        // Create a decrytor to perform the stream transform.
                        ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                        // Create the streams used for decryption.                
                        byte[] bytes = cipherText.HexStringToBytes();
                        using (MemoryStream msDecrypt = new MemoryStream(bytes))
                        {
                            using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                            {
                                using (StreamReader srDecrypt = new StreamReader(csDecrypt))

                                    // Read the decrypted bytes from the decrypting stream and place them in a string.
                                    plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
            }
            finally
            {
                // Clear the AesManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            return plaintext.ToSecureString();
        }

        public static SecureString GenerateSalt(int length)
        {
            //Create and populate random byte array
            byte[] randomArray = new byte[length];

            //Create random salt and convert to string
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomArray);

                return randomArray.ToHexString().ToSecureString();
            }
        }

        public static SecureString GenerateRandomKey(int length)
        {
            byte[] buffer = new byte[length];

            using (RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider())
            {

                provider.GetBytes(buffer);

                return buffer.ToHexString().ToSecureString();
            }
        }

        public static SecureString GenerateRandomKey(int minLength, int maxLength)
        {
            CryptoRandom cr = new CryptoRandom();
            int length = cr.Next(minLength, maxLength);

            return GenerateRandomKey(length);
        }

        public static SecureString ReintegrateSplitCipherKey(SplitCipherKey splitKey)
        {
            if (splitKey == null)
                throw new ArgumentNullException();

            byte[] padBuffer = splitKey.Pad.ToUnsecureString().HexStringToBytes();
            byte[] keyFragmentBuffer = splitKey.KeyFragment.ToUnsecureString().HexStringToBytes();

            byte[] integratedKey = new byte[keyFragmentBuffer.Length];

            for (int i = 0; i < keyFragmentBuffer.Length; i++)
                integratedKey[i] = (byte)(padBuffer[i] ^ keyFragmentBuffer[i]);

            return integratedKey.ToHexString().ToSecureString();
        }

        public static SplitCipherKey SplitCipherKey(SecureString key)
        {
            byte[] keyBuffer = key.ToUnsecureString().HexStringToBytes();
            
            string pad = SymmetricEncryption.GenerateRandomKey(keyBuffer.Length).ToUnsecureString();
            byte[] padBuffer = pad.HexStringToBytes();

            byte[] keyFragmentBuffer = new byte[keyBuffer.Length];

            for (int i = 0; i < keyBuffer.Length; i++)
                keyFragmentBuffer[i] = (byte)(keyBuffer[i] ^ padBuffer[i]);

            SplitCipherKey splitKey = new SplitCipherKey();

            splitKey.KeyFragment = keyFragmentBuffer.ToHexString().ToSecureString();
            splitKey.Pad = pad.ToSecureString();

            return splitKey;
        }
    }
}
