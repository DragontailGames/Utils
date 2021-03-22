using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Dragontailgames.Utils
{
    public class TranslationText : MonoBehaviour
    {
        public string tagId;

        public void Start()
        {
            if (Translation.instance)
            {
                if (Translation.instance.IsDownloaded())
                {
                    SetText();
                }
                else
                {
                    Translation.instance.ListenerAfterDownload(this);
                }
            }
        }

        public void SetText()
        {
            if (Translation.instance != null)
            {
                if (!this.transform.GetComponent<Text>() && !this.transform.GetComponent<TextMeshProUGUI>())
                {
                    Debug.LogError("No text element exists in: " + this.transform.name);
                    return;
                }

                if (this.tagId == null && string.IsNullOrEmpty(this.tagId) &&
                    string.IsNullOrEmpty(this.transform.GetComponent<Text>().text) &&
                    string.IsNullOrEmpty(this.transform.GetComponent<TextMeshProUGUI>().text))
                {
                    Debug.LogError("No id or text defined in: " + this.transform.name);
                }

                if (this.transform.GetComponent<Text>())
                {
                    string code = string.IsNullOrEmpty(this.tagId) ? this.transform.GetComponent<Text>().text : this.tagId;
                    this.transform.GetComponent<Text>().text = Translation.instance.GetTerm(code);
                }

                if (this.transform.GetComponent<TextMeshProUGUI>())
                {
                    string code = string.IsNullOrEmpty(this.tagId) ? this.transform.GetComponent<TextMeshProUGUI>().text : this.tagId;

                    string term = Translation.instance.GetTerm(code);
                    if (!string.IsNullOrEmpty(term))
                        this.transform.GetComponent<TextMeshProUGUI>().text = Translation.instance.GetTerm(code);
                }
            }
        }
    }
}
