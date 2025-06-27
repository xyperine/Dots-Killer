using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace DotsKiller.SaveSystem
{
    public class SaveSerializer<T>
        where T : new()
    {
        private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            Formatting = Newtonsoft.Json.Formatting.Indented,
        };

        private const string FILE_NAME = "save.json";
        private readonly string _fullPath = string.Join('/', Application.persistentDataPath, FILE_NAME);


        public T ReadFile()
        {
            if (!File.Exists(_fullPath))
            {
                return default;
            }

            string json = File.ReadAllText(_fullPath);
            return JsonConvert.DeserializeObject<T>(json, _serializerSettings);
        }

        
        public void SaveFile(T gameState)
        {
            if (!Directory.Exists(Application.persistentDataPath))
            {
                Directory.CreateDirectory(Application.persistentDataPath);
            }

            string json = JsonConvert.SerializeObject(gameState, _serializerSettings);
            File.WriteAllText(_fullPath, json);
            PlayerPrefs.SetInt("ForceSave", 0);
            PlayerPrefs.Save();
        }


        public void DeleteFile()
        {
            if (File.Exists(_fullPath))
            {
                File.Delete(_fullPath);
            }
        }
    }
}