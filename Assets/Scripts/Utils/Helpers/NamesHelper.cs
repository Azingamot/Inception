using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using static Unity.VisualScripting.Icons;

public static class NamesHelper
{
    private static string persistentPath = "Strings";
    private static StringsStorage storage;

    public static void ReloadStorage()
    {
        storage = LoadStorage();
    }

    public static string ReceiveName(string key)
    {
        if (storage == null)
            storage = LoadStorage();

        return storage.Get(key);
    }

    public static StringsStorage LoadStorage()
    {
        string path = TranslationHandler.ConvertToPath(persistentPath, "strings");
        TextAsset translationData = TranslationHandler.LoadTextFile(path);
        return JsonUtility.FromJson<StringsStorage>(translationData.text);
    }

    public static void CreateFile(StringsStorage storage)
    {
        foreach (string language in TranslationHandler.LanguagesPathMap.Keys)
        {
            string path = Path.Combine("Assets", "Resources", TranslationHandler.ConvertToPath(persistentPath, "strings", language));
            using (FileStream fs = new FileStream(path + ".json", 
                FileMode.OpenOrCreate, 
                FileAccess.ReadWrite))
            {
                string storageJson = JsonUtility.ToJson(storage, true);
                byte[] storageBytes = Encoding.UTF8.GetBytes(storageJson);
                fs.Write(storageBytes);
            }
        }  
    }
}
