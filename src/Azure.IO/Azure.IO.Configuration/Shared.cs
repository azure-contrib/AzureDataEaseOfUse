using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Azure.IO.Configuration
{
    internal static class Shared
    {

        public static string AppSetting(string key)
        {
            return Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings[key]);
        }

        public static string ConnectionString(string key)
        {
            return Environment.ExpandEnvironmentVariables(ConfigurationManager.ConnectionStrings[key]?.ConnectionString);
        }



    }
}
