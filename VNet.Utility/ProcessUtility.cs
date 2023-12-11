using global::System.Diagnostics;

namespace VNet.Utility
{
    public static class ProcessUtility
    {
        public static string ExecuteProcess(string filePath, string arguments, bool readLine)
        {
            var process = new Process();

            var processStartInfo = new ProcessStartInfo(filePath, arguments)
            {
                UseShellExecute = false,
                RedirectStandardOutput = true
            };

            process.StartInfo = processStartInfo;
            process.Start();

            var streamReader = process.StandardOutput;

            var content = readLine ? streamReader.ReadLine() : streamReader.ReadToEnd();

            process.WaitForExit();
            process.Close();

            return content;
        }

        public static string ExecuteProcess(string filePath, string arguments, string input)
        {
            var process = new Process();

            var processStartInfo = new ProcessStartInfo(filePath, arguments)
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardInput = true
            };

            process.StartInfo = processStartInfo;
            process.Start();

            var streamReader = process.StandardOutput;
            var streamWriter = process.StandardInput;
            streamWriter.Write(input);
            streamWriter.Close();

            var content = streamReader.ReadToEnd();

            process.WaitForExit();
            process.Close();

            return content;
        }
    }
}
