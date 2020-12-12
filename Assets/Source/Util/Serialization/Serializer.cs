using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace Racing.Util.Serialization
{
    /// <summary>
    /// Utility class for Xml Serialization
    /// </summary>
    public static class Serializer
    {
        /// <summary>
        /// Serializes an object to an Xml string and saves it to the PlayerPrefs
        /// </summary>
        public static void SerializeToPlayerPrefs<T>(string key, T data)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            StringWriter writer = new StringWriter();
            serializer.Serialize(writer, data);

            PlayerPrefs.SetString(key, writer.ToString());
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Serializes an object to an xml file
        /// </summary>
        /// <param name="filename">Name of the file (don't put any extensions - e.g. '.xml')</param>
        /// <param name="path">Optional custom path - Else saves to persistent data path</param>
        public static void SerializeToFile<T>(string filename, T data, string path = null)
        {
            string filePath = (path != null) ? Path.Combine(path, filename) : Path.Combine(Application.persistentDataPath, filename + ".xml");

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            FileStream file = File.Exists(filePath) ? File.OpenWrite(filePath) : File.Create(filePath);

            serializer.Serialize(file, data);
            file.Close();
        }
    }
}
