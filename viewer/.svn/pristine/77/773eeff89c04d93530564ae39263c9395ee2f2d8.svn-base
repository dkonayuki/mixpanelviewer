using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Web;

namespace MixPanelViewer
{
    class MixPanel
    {
        #region Properties and constants

        private const string API_KEY = "653609dbdf7d59acf9ea66e66533ba0d";
        private const string API_SECRET = "987b88d82bfce238b7b7660427c9b543";

        private string apiKey;
        public string ApiKey
        {
            get { return apiKey; }
            set { apiKey = value; }
        }

        private string apiSecret;
        public string ApiSecret
        {
            get { return apiSecret; }
            set { apiSecret = value; }
        }

        #endregion

        private MixPanel()
        {
            createApiKeys();
        }

        private void createApiKeys()
        {
            string filePath = Utilities.GetSettingPath();
            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string jsonStr = reader.ReadToEnd();
                    if (string.IsNullOrEmpty(jsonStr))
                    {
                        apiKey = API_KEY;
                        apiSecret = API_SECRET;
                    }
                    else
                    {
                        IDictionary<string, object> apiList = JsonSerializer.DeserializeObject(jsonStr) as IDictionary<string, object>;
                        apiKey = apiList["api_key"] as string;
                        apiSecret = apiList["api_secret"] as string;
                    }
                }
            }
            else
            {
                apiKey = API_KEY;
                apiSecret = API_SECRET;
            }
        }

        public void HttpGet(string uri, DownloadStringCompletedEventHandler downloadStringCompleted)
        {
            WebClient webClient = new WebClient();
            using (webClient)
            {
                webClient.DownloadStringCompleted += downloadStringCompleted;
                webClient.DownloadStringAsync(new Uri(uri));
            }
        }

        public string GetUri(Dictionary<string, string> parameters, string desUri)
        {
            string uri = desUri;
            string signature = string.Empty;
            parameters.Add("api_key", apiKey);

            //Create parameter 'sig'
            List<string> list = parameters.Keys.ToList<string>();
            list.Sort();
            foreach (string key in list)
            {
                signature += key + '=' + parameters[key];
            }
            string sig = Utilities.Md5(signature + apiSecret);

            //Final parameters list               
            string uriTemp = string.Empty;
            foreach (string key in list)
            {
                if (!String.IsNullOrEmpty(uriTemp))
                {
                    uriTemp += '&';
                }
                else
                {
                    uriTemp += '?';
                }
                // Url encoder
                uriTemp += key + '=' + Uri.EscapeDataString(parameters[key]);
            }
            uri = uri + uriTemp;
            uri += "&sig=" + sig;
            return uri;
        }

        #region Singleton implement

        private static MixPanel m_MixPanel;
        public static MixPanel Default
        {
            get
            {
                if (m_MixPanel == null) m_MixPanel = new MixPanel();
                return m_MixPanel;
            }
        }

        #endregion
    }
}
