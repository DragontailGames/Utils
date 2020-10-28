﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Obtains the ( audio / picture / translation ) that corresponds to a given term
public static class TermData {

    [System.Serializable]
    public class Terms {
        // < language, index of language's term within a value of termTranslations >
        public List<LanguageIndicie> languageIndicies;

        // < englishText, [spanishText, germanText, ...] >
        public List<TermTranslations> termTranslations;

        public Terms() {
            languageIndicies = new List<LanguageIndicie>();
            termTranslations = new List<TermTranslations>();
        }
    }

    [System.Serializable]
    public class LanguageIndicie
    {
        public string key;

        public int value;

        public LanguageIndicie()
        {
            key = "";
            value = 0;
        }
    }

    [System.Serializable]
    public class TermTranslations
    {
        public string key;

        public string[] value;

        public TermTranslations()
        {
            key = "";
        }
    }
}