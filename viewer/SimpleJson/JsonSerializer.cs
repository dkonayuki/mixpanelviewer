// --------------------------------
// <copyright file="JsonSerializer.cs" company="Thuzi LLC (www.thuzi.com)">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com), Jim Zimmerman (jimzimmerman.com) and Prabir Shrestha (prabir.me)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

namespace MixPanelViewer
{
    using System;

    /// <summary>
    /// Represents the json serializer class.
    /// </summary>
    public class JsonSerializer
    {
        public static string SerializeObject(object obj)
        {
            IJsonSerializer current = new SimpleJsonSerializer();
            return current.SerializeObject(obj);
        }

        public static object DeserializeObject(string json, Type type)
        {
            IJsonSerializer current = new SimpleJsonSerializer();
            return current.DeserializeObject(json, type);
        }

        public static T DeserializeObject<T>(string json)
        {
            IJsonSerializer current = new SimpleJsonSerializer();
            return current.DeserializeObject<T>(json);
        }

        public static object DeserializeObject(string json)
        {
            IJsonSerializer current = new SimpleJsonSerializer();
            return current.DeserializeObject(json);
        }

        private class SimpleJsonSerializer : IJsonSerializer
        {
            public string SerializeObject(object obj)
            {
                return SimpleJson.SimpleJson.SerializeObject(obj);
            }

            public object DeserializeObject(string json, Type type)
            {
                return SimpleJson.SimpleJson.DeserializeObject(json, type);
            }

            public T DeserializeObject<T>(string json)
            {
                return SimpleJson.SimpleJson.DeserializeObject<T>(json);
            }

            public object DeserializeObject(string json)
            {
                return SimpleJson.SimpleJson.DeserializeObject(json);
            }
        }
    }
}
