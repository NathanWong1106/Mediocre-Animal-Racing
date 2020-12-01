using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace Racing.Util.Serialization
{
    public static class Serializer
    {
        /// <summary>
        /// Serializes an object to an Xml string and saves it to the PlayerPrefs
        /// </summary>
        public static string SerializeToPlayerPrefs<T>(string key, T data)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            StringWriter writer = new StringWriter();
            serializer.Serialize(writer, data);

            PlayerPrefs.SetString(key, writer.ToString());
            PlayerPrefs.Save();

            return PlayerPrefs.GetString(key);
        }
    }
}
