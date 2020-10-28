using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Dragontailgames.Utils
{

    public static class SaveAndLoad
    {

        /// <summary>
        /// Serializa um objeto baseado na classe
        /// </summary>
        /// <typeparam name="T">Classe</typeparam>
        /// <param name="obj">Objeto da classe</param>
        /// <returns>Retorna json do objeto</returns>
        public static string SerializeObject<T>(T obj)
        {
            if (obj == null)
            {
                Debug.LogError("Objeto nulo");
                return null;
            }
            return JsonUtility.ToJson(obj);
        }

        /// <summary>
        /// Deserializada um objeto na classe relacionada
        /// </summary>
        /// <typeparam name="T">Classe</typeparam>
        /// <param name="jsonValue">Json da classe</param>
        /// <returns>Retorna um objeto da classe</returns>
        public static T DeserializeObject<T>(string jsonValue)
        {
            if(jsonValue == "" || jsonValue == null)
            {
                Debug.LogError("Json vazio");
                return default(T);
            }
            return JsonUtility.FromJson<T>(jsonValue);
        }

        /// <summary>
        /// Salvar json serializado na pasta Streaming Assets
        /// </summary>
        /// <param name="filename">Nome do arquivo a ser salvo</param>
        /// <param name="json">Objeto serializado</param>
        /// <param name="extension">Extensão opcional</param>
        public static void Save(string filename, string json, string extension = ".json")
        {
            Directory.CreateDirectory(Application.streamingAssetsPath);
            File.WriteAllText(Application.streamingAssetsPath + "/" + filename + extension, json);
        }

        /// <summary>
        /// Salvar objeto na pasta Streaming Assets
        /// </summary>
        /// <typeparam name="T">Classe</typeparam>
        /// <param name="filename">Nome do arquivo a ser salvo</param>
        /// <param name="obj">Objeto a ser salvo</param>
        /// <param name="extension">Extensão opcional</param>
        public static void Save<T>(string filename, T obj, string extension = ".json")
        {
            Directory.CreateDirectory(Application.streamingAssetsPath);
            File.WriteAllText(Application.streamingAssetsPath + "/" + filename + extension, SerializeObject(obj));
        }

        /// <summary>
        /// Carrega o arquivo da pasta Streaming Assets
        /// </summary>
        /// <param name="filename">Nome do arquivo</param>
        /// <param name="extension">Extensão se for diferente de .json</param>
        /// <returns>Retorna o objeto da classe</returns>
        public static string Load(string filename, string extension = ".json")
        {
            string url = Application.streamingAssetsPath + "/" + filename + extension;

            if (!File.Exists(url))
            {
                Debug.LogError("Arquivo nao encontrado");
                return null;
            }
            return File.ReadAllText(url);
        }

        /// <summary>
        /// Carrega o arquivo de qualquer pasta
        /// </summary>
        /// <param name="filename">Nome do arquivo</param>
        /// <returns>Retorna o objeto da classe</returns>
        public static string LoadSimple(string filename)
        {
            if (!File.Exists(filename))
            {
                Debug.LogError("Arquivo nao encontrado");
                return null;
            }
            return File.ReadAllText(filename);
        }
    }
}
