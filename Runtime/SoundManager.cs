using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dragontailgames.Utils
{
    public class SoundManager : MonoBehaviour
    {
        // Componentes de Audio
        public AudioSource EffectsSource;
        public AudioSource MusicSource;
        public AudioClip[] FXCliplist;
    

        // Intancia do Singleton.
        public static SoundManager Instance = null;

        // Iniciar instancia.
        private void Awake()
        {
            // Se não houver instancia
            if (Instance == null)
            {
                Instance = this;
            }
            //Se já houver instancia.
            else if (Instance != this)
            {
                Destroy(gameObject);
            }

            //Setar como dontdestroyonload
            DontDestroyOnLoad(gameObject);

            FXCliplist = Resources.LoadAll<AudioClip>("Audio/GAME_SFX");
        }

        // Toca SFX
        public void Play(string clipname)
        {
            foreach ( AudioClip searchlip in FXCliplist)
            {
                if (searchlip.name == clipname)
                {
                    EffectsSource.clip = searchlip;
                    EffectsSource.Play();
                }
            }
        
        }

        // Toca a musica
        public void PlayMusic(AudioClip clip)
        {
            MusicSource.clip = clip;
            MusicSource.Play();
        }
    }
}