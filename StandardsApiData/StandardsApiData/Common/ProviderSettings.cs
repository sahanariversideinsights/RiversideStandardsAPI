using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;


namespace StandardsApiData.Common
{
    public class ProviderSettings
    {
        private Dictionary<string, string> _providerSettingsDictionary = new Dictionary<string, string>();
        private void LoadSettings(string providerName)
        {
            Common commonData = new Common();
            DataTable dt = commonData.GetProviderSettings(providerName);
            foreach (DataRow dr in dt.Rows)
            {
                _providerSettingsDictionary.Add(dr["ConfigKey"].ToString(), dr["ConfigValue"].ToString());
            }
        }

        public ProviderSettings()
        {
            AppConfiguration appConfig = new AppConfiguration();
            //currently certica is the only provider - scalable in the future
            LoadSettings(appConfig.DefaultProvider);
        }
        
        public Dictionary<string, string> getProviderSettings()
        {
            return _providerSettingsDictionary;
        }

        public string getConfigValue(string configKey)
        {
            return _providerSettingsDictionary[configKey];
        }
    }
}