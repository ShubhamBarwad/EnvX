using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShubhamBarwad.SharedConfig
{
    public static class DotEnvLoader
    {
        public static Dictionary<string, string> LoadEnvVariables()
        {
            var configValues = new Dictionary<string, string>();
            string solutionRoot = GetSolutionRootDirectory();
            string envFilePath = Path.Combine(solutionRoot, ".env");

            if (File.Exists(envFilePath))
            {
                var lines = File.ReadAllLines(envFilePath);

                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                        continue;

#if NET6_0_OR_GREATER
                    var keyValuePair = line.Split('=', 2);
#else
                    var keyValuePair = line.Split(new[] { '=' }, 2, StringSplitOptions.None);
#endif
                    if (keyValuePair.Length == 2)
                    {
                        configValues[keyValuePair[0].Trim()] = keyValuePair[1].Trim();
                    }
                }
            }
            else
            {
                throw new FileNotFoundException(".env file not found at solution root.");
            }

            return configValues;
        }

        private static string GetSolutionRootDirectory()
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            while (currentDirectory != null)
            {
                if (Directory.GetFiles(currentDirectory, "*.sln").Length > 0)
                {
                    return currentDirectory;
                }
                currentDirectory = Directory.GetParent(currentDirectory)?.FullName;
            }

            throw new InvalidOperationException("Solution root not found.");
        }
    }
}
