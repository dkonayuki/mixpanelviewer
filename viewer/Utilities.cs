using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.IO;
using System.Security.Cryptography;

namespace MixPanelViewer
{
    static class Utilities
    {
        public static List<Report> GetReports(string list)
        {
            List<Report> reportsList = new List<Report>();               
            List<object> objList = JsonSerializer.DeserializeObject(list) as List<object>;
            
            foreach (object obj in objList)
            {
                IDictionary<string, object> iobj = obj as IDictionary<string, object>;

                Report report = new Report();
                report.Event = iobj["event"] as string;
                report.Properties = iobj["properties"] as IDictionary<string, object>;
                report.Properties.Add("No.", reportsList.Count + 1);
                reportsList.Add(report);
            }                       
            return reportsList;
        }

        public static List<object> GetEventsName(string list)
        {
            object obj = JsonSerializer.DeserializeObject(list);
            List<object> eventsList = obj as List<object>;
            return eventsList;
        }

        public static string GetAppDir()
        {
            //string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            string path = Path.GetFullPath(string.Format("/{0}", PublicResource.PROGRAM_DATA));
            return Path.Combine(path, PublicResource.APP_PATH);
        }

        public static string GetSettingPath()
        {
            return Path.Combine(GetAppDir(), PublicResource.SETTING_FILE);
        }

        public static int ConvertToTimestamp(DateTime value)
        {
            //create Timespan by subtracting the value provided from
            //the Unix Epoch
            TimeSpan span = (value - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());

            //return the total seconds (which is a UNIX timestamp)
            return (int)span.TotalSeconds;
        }

        // Hash an input string and return the hash as
        // a 32 character hexadecimal string.
        public static string Md5(string input)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            MD5 md5Hasher = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public static Stream StringToStreamReader(string content)
        {
            // convert string to stream
            byte[] byteArray = Encoding.UTF8.GetBytes(content);
            //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
            MemoryStream stream = new MemoryStream(byteArray);
            return stream;
        }
    }
}
