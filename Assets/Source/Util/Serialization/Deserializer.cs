using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace Racing.Util.Serialization
{
    /// <summary>
    /// Utility class for Xml Deserialization
    /// </summary>
    class Deserializer
    {
        /// <summary>
        /// Deserializes from an Xml string in PlayerPrefs and returns the object
        /// </summary>
        public static T DeserializeFromPlayerPrefs<T>(string key)
        {
            if (!PlayerPrefs.HasKey(key))
            {
                return default;
            }

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            StringReader reader = new StringReader(PlayerPrefs.GetString(key));
            return (T)serializer.Deserialize(reader);
        }

        public static T DeserializeFromFile<T>(string path)
        {
            path += ".xml";

            if (!File.Exists(path))
                throw new System.Exception("~Deserializer - File does not exist");

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            StringReader reader = new StringReader(File.ReadAllText(path));
            
            return (T)serializer.Deserialize(reader);
           
        }
    }
}
