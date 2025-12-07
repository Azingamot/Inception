using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

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

        string value = storage.Get(key);

        return value == null ? key : value;
    }

    public static StringsStorage LoadStorage(string language = "")
    {
        string path = TranslationHandler.ConvertToPath(persistentPath, "strings", language);
        TextAsset translationData = TranslationHandler.LoadTextFile(path);
        return JsonUtility.FromJson<StringsStorage>(translationData.text);
    }

    public static void CreateFile(StringsStorage storage)
    {
        foreach (string language in TranslationHandler.LanguagesPathMap.Keys)
        {
            CreateFile(language, storage);
        }  
    }

    public static void CreateFile(string language, StringsStorage storage)
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

    public static void WriteToFile(List<NameString> nameStrings)
    {
        foreach (string language in TranslationHandler.LanguagesPathMap.Keys)
        {
            string path = Path.Combine("Assets", "Resources", TranslationHandler.ConvertToPath(persistentPath, "strings", language));

            string storageJson;

            using (StreamReader sr = new StreamReader(path + ".json"))
            {
                string jsonString;
                jsonString = sr.ReadToEnd();
                StringsStorage storage = JsonUtility.FromJson<StringsStorage>(jsonString);
                storage.NameStrings.AddRange(nameStrings);
                storageJson = JsonUtility.ToJson(storage, true);
            }

            using (StreamWriter sw = new StreamWriter(path + ".json"))
            {
                sw.Write(storageJson);
            }
        }
    }
}
