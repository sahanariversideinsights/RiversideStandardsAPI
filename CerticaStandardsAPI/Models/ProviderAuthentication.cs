using StandardsApiData.Common;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace CerticaStandardsAPI.Models
{
    public class ProviderAuthentication
    {
        Authentication authentication = new Authentication();
        protected Authentication MapAllProviderSettings()
        {
            ProviderSettings providerSettings = new ProviderSettings();

            foreach (KeyValuePair<string, string> pair in providerSettings.getProviderSettings())
            {
                AssignAuthentication(pair);
            }

            return authentication;
        }

        private void AssignAuthentication(KeyValuePair<string, string> pair)
        {
            if (pair.Key.ToLower().Contains("partnerid"))
            {
                authentication.partnerID = pair.Value;
            }

            if (pair.Key.ToLower().Contains("partnerkey"))
            {
                authentication.partnerKey = pair.Value;
            }

            if (pair.Key.ToLower().Contains("userid"))
            {
                if (!string.IsNullOrEmpty(pair.Value))
                {
                    authentication.userId = pair.Value;
                }
                else
                {
                    authentication.userId = " ";
                }

            }
        }


        protected internal UriBuilder AuthenticateProvider(string facet,string Guid=null,string app=null, bool paging = false, string next = null)
        {
            authentication = MapAllProviderSettings();

            var expires = (long)Math.Floor((DateTime.UtcNow.AddHours(24) - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds);

            var message = string.Format("{0}\n{1}", expires, authentication.userId);

            var keyBytes = Encoding.UTF8.GetBytes(authentication.partnerKey);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            string signature;
            using (var hmac = new HMACSHA256(keyBytes))
            {
                signature = Convert.ToBase64String(hmac.ComputeHash(messageBytes));
            }
            Helper helper = new Helper();
            UriBuilder requestBuilder = helper.BuildApiUrl(facet, signature, expires, authentication,Guid,app,paging,next);

            return requestBuilder;
        }
    }
}