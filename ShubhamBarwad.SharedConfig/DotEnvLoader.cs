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
                    {
                        continue;
                    }

                    var parts = line.Split(new[] { '=' }, 2);

                    if (parts.Length == 2)
                    {
                        var key = parts[0].Trim();
                        var value = parts[1].Trim();

                        if (!configValues.ContainsKey(key))
                        {
                            configValues[key] = value;
                        }
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
