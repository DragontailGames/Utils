using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace Dragontailgames.Utils
{
    public class Loader : MonoBehaviour
    {
        private int progress = 0;
        List<string> languages = new List<string>();

        public string googleSheetId = "";

#if UNITY_EDITOR
        [ReadOnly]
#endif
        public TermData.Terms termData;

        public System.Action afterDownloadSetupTexts;

        public void Load(System.Action setupTexts)
        {
            afterDownloadSetupTexts = setupTexts;
            StartCoroutine(CSVDownloader.DownloadData(googleSheetId, AfterDownload));
        }

        public void AfterDownload(string data, bool changeVersion)
        {
            if (null == data)
            {
                Debug.LogError("Was not able to download data or retrieve stale data.");
                // TODO: Display a notification that this is likely due to poor internet connectivity
                //       Maybe ask them about if they want to report a bug over this, though if there's no internet I guess they can't
            }
            else
            {
                if (changeVersion)
                {
                    StartCoroutine(ProcessData(data, AfterProcessData));
                }
                else
                {
                    ProcessLocalData();
                }
            }
        }

        private void ProcessLocalData()
        {
            termData = SaveAndLoad.DeserializeObject<TermData.Terms>(SaveAndLoad.Load("Terms"));
            afterDownloadSetupTexts?.Invoke();
            Debug.Log("Old version local loaded");
        }

        private void AfterProcessData(string errorMessage)
        {
            if (null != errorMessage)
            {
                Debug.LogError("Was not able to process data: " + errorMessage);
                // TODO: 
            }
            else
            {
                SaveAndLoad.Save("Terms", SaveAndLoad.SerializeObject(termData));
                afterDownloadSetupTexts?.Invoke();
            }
        }

        public IEnumerator ProcessData(string data, System.Action<string> onCompleted)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            termData = new TermData.Terms();

            // Line level
            int currLineIndex = 0;
            bool inQuote = false;
            int linesSinceUpdate = 0;
            int kLinesBetweenUpdate = 15;

            // Entry level
            string currEntry = "";
            int currCharIndex = 0;
            bool currEntryContainedQuote = false;
            List<string> currLineEntries = new List<string>();

            // "\r\n" means end of line and should be only occurence of '\r' (unless on macOS/iOS in which case lines ends with just \n)
            char lineEnding = Application.platform == RuntimePlatform.IPhonePlayer ? '\n' : '\r';
            int lineEndingLength = Application.platform == RuntimePlatform.IPhonePlayer ? 1 : 2;

            data += lineEnding;

            while (currCharIndex < data.Length)
            {
                if (!inQuote && (data[currCharIndex] == lineEnding))
                {
                    // Skip the line ending
                    currCharIndex += lineEndingLength;

                    // Wrap up the last entry
                    // If we were in a quote, trim bordering quotation marks
                    if (currEntryContainedQuote)
                    {
                        currEntry = currEntry.Substring(1, currEntry.Length - 2);
                    }

                    currLineEntries.Add(currEntry);
                    currEntry = "";
                    currEntryContainedQuote = false;

                    // Line ended
                    ProcessLineFromCSV(currLineEntries, currLineIndex);
                    currLineIndex++;
                    currLineEntries = new List<string>();

                    linesSinceUpdate++;
                    if (linesSinceUpdate > kLinesBetweenUpdate)
                    {
                        linesSinceUpdate = 0;
                        yield return new WaitForEndOfFrame();
                    }
                }
                else
                {
                    if (data[currCharIndex] == '"')
                    {
                        inQuote = !inQuote;
                        currEntryContainedQuote = true;
                    }

                    // Entry level stuff
                    {
                        if (data[currCharIndex] == ',')
                        {
                            if (inQuote)
                            {
                                currEntry += data[currCharIndex];
                            }
                            else
                            {
                                // If we were in a quote, trim bordering quotation marks
                                if (currEntryContainedQuote)
                                {
                                    currEntry = currEntry.Substring(1, currEntry.Length - 2);
                                }

                                currLineEntries.Add(currEntry);
                                currEntry = "";
                                currEntryContainedQuote = false;
                            }
                        }
                        else
                        {
                            currEntry += data[currCharIndex];
                        }
                    }
                    currCharIndex++;
                }

                progress = (int)((float)currCharIndex / data.Length * 100.0f);
            }

            onCompleted(null);
        }

        private void ProcessLineFromCSV(List<string> currLineElements, int currLineIndex)
        {

            // This line contains the column headers, telling us which languages are in which column
            if (currLineIndex == 0)
            {
                languages = new List<string>();
                for (int columnIndex = 0; columnIndex < currLineElements.Count; columnIndex++)
                {
                    string currLanguage = currLineElements[columnIndex];
                    Assert.IsTrue((columnIndex == 0 || currLanguage != "English"), "First column first row was:" + currLanguage);
                    Assert.IsFalse(termData.languageIndicies.Exists(n => n.key == currLanguage));
                    termData.languageIndicies.Add(new TermData.LanguageIndicie(){
                        key = currLanguage, 
                        value = columnIndex });
                    languages.Add(currLanguage);
                }
                UnityEngine.Assertions.Assert.IsFalse(languages.Count == 0);
            }
            // This is a normal node
            else if (currLineElements.Count > 1)
            {
                string englishSpelling = null;
                string[] nonEnglishSpellings = new string[languages.Count - 1];

                for (int columnIndex = 0; columnIndex < currLineElements.Count; columnIndex++)
                {
                    string currentTerm = currLineElements[columnIndex];
                    if (columnIndex == 0)
                    {
                        Assert.IsFalse(termData.termTranslations.Exists(n => n.key == currentTerm), "Saw this term twice: " + currentTerm);
                        englishSpelling = currentTerm;
                    }
                    else
                    {
                        nonEnglishSpellings[columnIndex - 1] = currentTerm;
                    }
                }
                if(termData.termTranslations.Find(n => n.key == englishSpelling) != null)
                {
                    termData.termTranslations.Find(n => n.key == englishSpelling).value = nonEnglishSpellings;
                }
                else
                {
                    termData.termTranslations.Add(new TermData.TermTranslations() {
                        key = englishSpelling,
                        value = nonEnglishSpellings
                    });
                }
                
                //print( "englishSpelling: >" + englishSpelling + "<" );
            }
            else
            {
                Debug.LogError("Database line did not fall into one of the expected categories.");
            }
        }
    }


#if UNITY_EDITOR

    public class ReadOnlyAttribute : PropertyAttribute
    {

    }

    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property,
                                                GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position,
                                    SerializedProperty property,
                                    GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
#endif
}
