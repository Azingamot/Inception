using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class TranslationHandler
{
    public static Dictionary<string, string> LanguagesPathMap = new()
    {
        {"Russian", "RU" },
        {"English", "EN" }
    };

    public static string ConvertToPath(string persistentPath, string fileName, string language = "")
    {
        if (string.IsNullOrEmpty(language))
            language = GetLanguageFromPlayerPrefs();
        return Path.Combine(persistentPath, LanguagesPathMap[language], fileName);
    }

    private static string GetLanguageFromPlayerPrefs()
    {
        if (PlayerPrefs.HasKey("Language"))
            return PlayerPrefs.GetString("Language");
        else
            return "English";
    }

    public static TextAsset LoadTextFile(string path)
    {
        TextAsset fileData = Resources.Load(path) as TextAsset;
        return fileData;
    }
}
