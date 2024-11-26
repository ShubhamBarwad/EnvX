namespace ShubhamBarwad.SharedConfig
{
    /// <summary>
    /// Provides a centralized manager for accessing configuration values.
    /// </summary>
    public class ConfigManager
    {
        private static readonly Dictionary<string, string> _configValues;

        /// <summary>
        /// Static constructor to initialize the configuration values by loading them from the .env file.
        /// </summary>
        static ConfigManager()
        {
            _configValues = DotEnvLoader.LoadEnvVariables();
        }

        /// <summary>
        /// Retrieves the configuration value for the specified key.
        /// </summary>
        /// <param name="key">The key of the configuration value to retrieve.</param>
        /// <returns>The value associated with the specified key.</returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown if the specified key is not found in the configuration.
        /// </exception>
        public static string GetConfigValue(string key)
        {
            if (_configValues.TryGetValue(key, out var value))
            {
                return value;
            }
            else
            {
                throw new KeyNotFoundException($"Configuration key '{key}' not found.");
            }
        }
    }

}
