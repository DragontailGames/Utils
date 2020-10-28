using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class CSVDownloader
{
    internal static IEnumerator DownloadData(string googleSheetDocId, System.Action<string, bool> onCompleted)
    {
        yield return new WaitForEndOfFrame();

        string url = "https://docs.google.com/spreadsheets/d/" + googleSheetDocId + "/export?format=csv";

        string downloadData = null;
            bool newVersion = false;

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log("Download Error: " + webRequest.error);
                downloadData = PlayerPrefs.GetString("LastDataDownloaded", null);
                string versionText = PlayerPrefs.GetString("LastDataDownloaded", null);
                Debug.Log("Using stale data version: " + versionText);
            }
            else
            {
                Debug.Log("Download success");

                // First term will be preceeded by version number, e.g. "100=English"
                string versionSection = webRequest.downloadHandler.text.Substring(0, 5);
                int equalsIndex = versionSection.IndexOf('=');
                UnityEngine.Assertions.Assert.IsFalse(equalsIndex == -1, "Could not find a '=' at the start of the CVS");

                string versionText = webRequest.downloadHandler.text.Substring(0, equalsIndex);

                if (!string.IsNullOrEmpty(PlayerPrefs.GetString("LastDataDownloaded")) && PlayerPrefs.GetString("LastDataDownloaded") != versionText)
                {
                    newVersion = true;
                }

                PlayerPrefs.SetString("LastDataDownloaded", webRequest.downloadHandler.text);
                PlayerPrefs.SetString("LastDataDownloadedVersion", versionText);

                downloadData = webRequest.downloadHandler.text.Substring(equalsIndex + 1);
            }
        }

        onCompleted(downloadData, newVersion);
    }
}