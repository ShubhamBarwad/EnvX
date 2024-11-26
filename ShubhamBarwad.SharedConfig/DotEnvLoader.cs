using System.Text.RegularExpressions;

namespace ShubhamBarwad.SharedConfig
{
    /// <summary>
    /// Provides functionality to load and parse environment variables from a `.env` file located in the solution root.
    /// </summary>
    public static class DotEnvLoader
    {
        /// <summary>
        /// Precompiled regex pattern to match key-value pairs in the .env file
        /// </summary>
        private static readonly Regex KeyValueRegex = new Regex(
            @"^\s*(?<key>[A-Za-z_][A-Za-z0-9_]*)\s*=\s*(?<value>.+?)\s*$",
            RegexOptions.Compiled);

        /// <summary>
        /// Loads environment variables from a `.env` file in the solution root directory.
        /// </summary>
        /// <returns>
        /// A dictionary containing the key-value pairs of the environment variables.
        /// </returns>
        /// <exception cref="FileNotFoundException">
        /// Thrown if the `.env` file is not found in the solution root.
        /// </exception>
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
                    if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#") || line.StartsWith("//"))
                    {
                        continue;
                    }

                    var match = KeyValueRegex.Match(line);
                    if (match.Success)
                    {
                        var key = match.Groups["key"].Value;
                        var rawValue = match.Groups["value"].Value;

                        string value = ParseValue(rawValue);

                        configValues[key] = value;
                    }

                }
            }
            else
            {
                throw new FileNotFoundException(".env file not found at solution root.");
            }

            return configValues;
        }

        /// <summary>
        /// Gets the root directory of the solution by searching for a `.sln` file in parent directories.
        /// </summary>
        /// <returns>
        /// The full path of the solution root directory.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the solution root directory is not found.
        /// </exception>
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

        /// <summary>
        /// Parses a raw value string from the `.env` file.
        /// Handles quotes, escape sequences, and other special characters.
        /// </summary>
        /// <param name="rawValue">The raw value string from the `.env` file.</param>
        /// <returns>The parsed and cleaned value string.</returns>
        private static string ParseValue(string rawValue)
        {
            if ((rawValue.StartsWith("\"") && rawValue.EndsWith("\"")) ||
                (rawValue.StartsWith("'") && rawValue.EndsWith("'")))
            {
#if NET6_0_OR_GREATER
                rawValue = rawValue[1..^1];
#else
                rawValue = rawValue.Substring(1, rawValue.Length - 2);
#endif
            }

            return rawValue.Replace("\\n", "\n")
                           .Replace("\\t", "\t")
                           .Replace("\\r", "\r")
                           .Replace("\\\"", "\"")
                           .Replace("\\'", "'");
        }
    }
}
