using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Dragontailgames.Utils
{
    [RequireComponent(typeof(Loader))]
    public class Translation : MonoBehaviour
    {
        public static Translation instance = null;

        public string language;

        public bool downloadedData = false;

        public UnityAction afterTranslate;

        void Awake()
        {
            if (instance == null) instance = this;
            else if (instance != this) Destroy(gameObject);
            DontDestroyOnLoad(gameObject);

            StartSaveTerms();
        }

        private List<TranslationText> txts = new List<TranslationText>();

        public void ListenerAfterDownload(TranslationText text)
        {
            if (!txts.Contains(text))
            {
                txts.Add(text);
            }
        }

        public void SetupAllTexts()
        {
            foreach(var aux in txts)
            {
                aux.SetText();
            }
            downloadedData = true;
            StartCoroutine(EndDelayed());
        }

        public IEnumerator EndDelayed()
        {
            yield return new WaitForSeconds(1.0f);
            afterTranslate?.Invoke();
        }

        public void StartSaveTerms()
        {
            this.transform.GetComponent<Loader>().Load(SetupAllTexts);
        }

        public string GetTerm(string code)
        {
            TermData.Terms term = this.transform.GetComponent<Loader>().termData;
            int languageIndex = term.languageIndicies.Find(n => n.key == language).value;
            if (!term.termTranslations.Exists(n => n.key == code))
            {
                Debug.LogError("Code: " + code + " not finded in database");
                return code;
            }
            string[] translations = term.termTranslations.Find(n => n.key == code).value;
            if(translations.Length==0)
            {
                Debug.LogError("Code: " + code + " don't have translations");
                    return code;
            }
            return translations[languageIndex-1];
        }

        public bool IsDownloaded()
        {
            return downloadedData;
        }
    }
}
