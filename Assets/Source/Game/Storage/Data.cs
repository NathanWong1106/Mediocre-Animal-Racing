using Racing.Util.Serialization;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Racing.Game.Storage
{
    /// <summary>
    /// DTO to transfer data across scenes
    /// </summary>
    public class Data
    {
        public static Data Default { get { return new Data(8, 1, 5); } }
        public static readonly string CustomDataKey = "CustomRaceData";
        public static readonly string FilePath = Path.Combine(Application.persistentDataPath, CustomDataKey);
        public static List<Data> CustomRaceData = new List<Data>();

        public int AIPlayerCount { get; set; }
        public int PlayerCount { get; set; }
        public int TotalPlayerCount { get { return AIPlayerCount + PlayerCount; } }
        public int PlayerStartPosition { get; set; }
        public string Name { get; set; }

        public Data()
        {

        }

        public Data(int AIPlayerCount, int PlayerCount, int PlayerStartPosition)
        {
            this.AIPlayerCount = AIPlayerCount;
            this.PlayerCount = PlayerCount;
            this.PlayerStartPosition = PlayerStartPosition;
        }

        [RuntimeInitializeOnLoadMethod]
        public static void Init()
        {
            CustomRaceData = GetDataFromPlayerPrefs();
        }

        public static List<Data> GetDataFromPlayerPrefs()
        {
            List<Data> customRaceData = Deserializer.DeserializeFromFile<List<Data>>(FilePath);
            return customRaceData != null ? customRaceData : new List<Data>();
        }

        //Operator overloading
        public override bool Equals(object obj)
        {
            return obj is Data data && Name == data.Name;
        }

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
        }

        public static bool operator ==(Data x, Data y)
        {
            return x.Name == y.Name;
        }

        public static bool operator !=(Data x, Data y)
        {
            return x.Name != y.Name;
        }
    }
}