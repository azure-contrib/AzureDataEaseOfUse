using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Azure.IO
{
    public static class Configuration
    {

        public static string AppSetting(string key)
        {
            var value = ConfigurationManager.AppSettings[key];

            return string.IsNullOrWhiteSpace(value) ? string.Empty : Environment.ExpandEnvironmentVariables(value);
        }

        public static string ConnectionString(string key)
        {
            var value = ConfigurationManager.ConnectionStrings[key]?.ConnectionString;

            return string.IsNullOrWhiteSpace(value) ? string.Empty : Environment.ExpandEnvironmentVariables(value);
        }

    }
}
