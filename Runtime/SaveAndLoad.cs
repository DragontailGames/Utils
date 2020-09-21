using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Dragontailgames.Utils
{

    public static class SaveAndLoad
    {

        public static string SerializeObject<T>(T obj)
        {
            return JsonUtility.ToJson(obj);
        }

        public static T DeserializeObject<T>(string jsonValue)
        {
            return JsonUtility.FromJson<T>(jsonValue);
        }

        public static void Save(string fileName, string json, string extension = ".json")
        {
            Directory.CreateDirectory(Application.streamingAssetsPath);
            File.WriteAllText(Application.streamingAssetsPath + "/" + fileName + extension, json);
        }

        public static string Load(string fileName, string extension = ".json")
        {
            string url = Application.streamingAssetsPath + "/" + fileName + extension;

            if (!File.Exists(url))
            {
                Debug.LogError("Arquivo nao encontrado");
                return null;
            }
            return File.ReadAllText(url);
        }

        public static string LoadSimple(string fileName)
        {
            if (!File.Exists(fileName))
            {
                Debug.LogError("Arquivo nao encontrado");
                return null;
            }
            return File.ReadAllText(fileName);
        }
    }
}
