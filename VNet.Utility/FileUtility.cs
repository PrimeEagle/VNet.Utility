using System;
using global::System.IO;
using System.Security.Cryptography;

namespace VNet.Utility
{
    public static class FileUtility
    {
        // Adapted and modified from: http://www.codeproject.com/Articles/22736/Securely-Delete-a-File-using-NET
        public static void WipeFile(string fileName, int timesToWrite)
        {
            if (global::System.IO.File.Exists(fileName))
            {
                // Set the files attributes to normal in case it's read-only.

                global::System.IO.File.SetAttributes(fileName, FileAttributes.Normal);

                // Calculate the total number of sectors in the file.
                double sectors = Math.Ceiling(new FileInfo(fileName).Length / 512.0);

                // Create a dummy-buffer the size of a sector.

                byte[] dummyBuffer = new byte[512];

                // Create a cryptographic Random Number Generator.
                // This is what I use to create the garbage data.

                using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                {
                    // Open a FileStream to the file.
                    using (FileStream inputStream = new FileStream(fileName, FileMode.Open))
                    {
                        for (int currentPass = 0; currentPass < timesToWrite; currentPass++)
                        {
                            // Go to the beginning of the stream

                            inputStream.Position = 0;

                            // Loop all sectors
                            for (int sectorsWritten = 0; sectorsWritten < sectors; sectorsWritten++)
                            {
                                // Fill the dummy-buffer with random data

                                rng.GetBytes(dummyBuffer);

                                // Write it to the stream
                                inputStream.Write(dummyBuffer, 0, dummyBuffer.Length);
                            }
                        }

                        // Truncate the file to 0 bytes.
                        // This will hide the original file-length if you try to recover the file.

                        inputStream.SetLength(0);

                        // Close the stream.
                        inputStream.Close();
                    }
                }

                // As an extra precaution I change the dates of the file so the
                // original dates are hidden if you try to recover the file.

                Random random = new Random();
                DateTime dt = new DateTime(random.Next(2000, 3000),
                                           random.Next(1, 12),
                                           random.Next(1, 28),
                                           random.Next(0, 23),
                                           random.Next(0, 59),
                                           random.Next(0, 59));

                global::System.IO.File.SetCreationTime(fileName, dt);
                global::System.IO.File.SetLastAccessTime(fileName, dt);
                global::System.IO.File.SetLastWriteTime(fileName, dt);

                // Finally, delete the file

                global::System.IO.File.Delete(fileName);
            }
        }
    }
}
