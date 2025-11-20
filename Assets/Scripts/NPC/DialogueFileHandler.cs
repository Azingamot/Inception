using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public static class DialogueFileHandler
{
    private static string persistentDialoguePath = "Dialogues";

    public static DialogueData ReceiveDialogueData(string dialogueName)
    {
        string path = TranslationHandler.ConvertToPath(persistentDialoguePath, dialogueName);
        TextAsset textAsset = TranslationHandler.LoadTextFile(path);
        DialogueData result = JsonUtility.FromJson<DialogueData>(textAsset.text);
        return result;
    }

    public static void GenerateDialogueFile(string dialogueName)
    {
        DialogueData emptyData = new DialogueData()
        {
            DialogueLines = new() { 
                new()
                {
                    Title = "Test",
                    Content = "Test!",
                    Emotion = CharacterEmotion.Happy,
                },
                new()
                {
                    Title = "Test",
                    Content = "Test!",
                    Emotion = CharacterEmotion.Sad,
                }
            }
        };
        WriteDialogueData(dialogueName, emptyData);
    }

    public static void GenerateDialogueFile(string dialogueName, DialogueData data)
    {
        WriteDialogueData(dialogueName, data);
    }

    public static void GenerateDialogueFile(string dialogueName, string language, DialogueData data)
    {
        WriteDialogueData(dialogueName, language, data);
    }

    private static void WriteDialogueData(string dialogueName, DialogueData data)
    {
        foreach (var key in TranslationHandler.LanguagesPathMap.Keys)
        {
            string path = Path.Combine("Assets", "Resources", TranslationHandler.ConvertToPath(persistentDialoguePath, dialogueName, key));
            using (FileStream fs = File.Create(path + ".json"))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data, true));
                fs.Write(bytes);
            }
        }
    }

    private static void WriteDialogueData(string dialogueName, string language, DialogueData data)
    {
        string path = Path.Combine("Assets", "Resources", TranslationHandler.ConvertToPath(persistentDialoguePath, dialogueName, language));
        using (FileStream fs = File.Create(path + ".json"))
        {
            byte[] bytes = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data, true));
            fs.Write(bytes);
        }
    }
}
