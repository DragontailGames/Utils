using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dragontailgames.Utils
{
    public class SceneLoadManager : MonoBehaviour
    {
        public static SceneLoadManager instance = null;
        
        void Awake()
        {
            if (instance == null)instance = this;
            else if (instance != this)Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }

        public void gotoMenu()
        {
            var objects = FindObjectsOfType<GameObject>();

            foreach (var aux in objects)
            {
                if(aux.gameObject != this.gameObject)
                Destroy(aux.gameObject);
            }
            SceneManager.LoadScene(0);
        }

        public void gotoNext()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void LoadScene(int scene)
        {
            SceneManager.LoadScene(scene);
        }
        
        public void LoadScene(string scene)
        {
            SceneManager.LoadScene(scene);
        }

        public void Reload()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public string GetCurrentScene()
        {
            return SceneManager.GetActiveScene().name;
        }

        public void Close()
        {
            Application.Quit();
        }
    }
}
