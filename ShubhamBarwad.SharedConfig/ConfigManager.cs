namespace ShubhamBarwad.SharedConfig
{
    public class ConfigManager
    {
        private static readonly Dictionary<string, string> _configValues;

        static ConfigManager()
        {
            _configValues = DotEnvLoader.LoadEnvVariables();
        }

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
