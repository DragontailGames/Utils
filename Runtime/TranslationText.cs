using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TranslationText : MonoBehaviour
{
    public string tagId;

    public void Start()
    {
        if(!this.transform.GetComponent<Text>() && !this.transform.GetComponent<TextMeshProUGUI>())
        {
            Debug.LogError("No text element exists in: " + this.transform.name);
            return;
        }

        if(string.IsNullOrEmpty(this.tagId))
        {
            Debug.LogWarning("No id defined in: " + this.transform.name);
        }
    }
}
