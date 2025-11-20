using UnityEngine;
using System.IO;
using System.Threading.Tasks;

public static class SaveSystem
{
    private static SaveData saveData = new SaveData();
    private static string debugPath = "/debug";

    public static string SaveFileName()
    {
        string saveFile = Application.persistentDataPath + PathToSave();
        return saveFile;
    }

    public static string SaveFileName(int index)
    {
        string saveFile = Application.persistentDataPath + PathToSave(index);
        return saveFile;
    }

    public static string DebugSaveFileName()
    {
        string debugDirectoryPath = Application.persistentDataPath + debugPath;
        if (!Directory.Exists(debugDirectoryPath))
        {
            Directory.CreateDirectory(debugDirectoryPath);
        }
        string saveFile = debugDirectoryPath + PathToSave();
        return saveFile;
    }

    public static async Task Save()
    {
        HandleSaveData();
        SaveFilePath();

        await File.WriteAllTextAsync(SaveFileName(), JsonUtility.ToJson(saveData, false), encoding: System.Text.Encoding.UTF8);
        await File.WriteAllTextAsync(DebugSaveFileName(), JsonUtility.ToJson(saveData, true), encoding: System.Text.Encoding.UTF8);
    }

    public static void DeleteSaveFile(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    private static void SaveFilePath()
    {
        PlayerPrefs.SetString($"Save{CurrentSaveFile()}", SaveFileName());
    }

    private static void HandleSaveData()
    {
        saveData = GameManager.Instance.Save($"Save №{CurrentSaveFile()}");
    }

    public static async Task LoadAsync(string name)
    {
        await SceneLoader.LoadScene(name);

        await Task.Delay(500);

        await GameManager.Instance.WaitForSceneToBeFullyLoaded();

        string saveContent = ReceiveDataFromSaveFile();

        if (!string.IsNullOrEmpty(saveContent))
            saveData = JsonUtility.FromJson<SaveData>(saveContent);
        else
            saveData = null;

        HandleLoadData();
    }

    public static void Load()
    {
        string saveContent = ReceiveDataFromSaveFile();

        if (!string.IsNullOrEmpty(saveContent))
            saveData = JsonUtility.FromJson<SaveData>(saveContent);
        else
            saveData = null;
        
        HandleLoadData();
    }

    public static SaveData ReceiveSaveData(string path)
    {
        Debug.Log(path);
        if (File.Exists(path))
        {
            return JsonUtility.FromJson<SaveData>(SaveFileContent(path));
        }
        return null;
    }

    private static string SaveFileContent(string path)
    {
        return File.ReadAllText(path);
    }

    private static string ReceiveDataFromSaveFile()
    {
        if (File.Exists(SaveFileName()))
        {
            return File.ReadAllText(SaveFileName());
        }
        return "";
    }

    private static void HandleLoadData()
    {
        GameManager.Instance.Load(saveData);
    }

    private static string PathToSave()
    {
        return $"/save{CurrentSaveFile()}.save";
    }

    private static string PathToSave(int index)
    {
        return $"/save{index}.save";
    }

    private static int CurrentSaveFile()
    {
        if (!PlayerPrefs.HasKey("CurrentSaveFile"))
            PlayerPrefs.SetInt("CurrentSaveFile", 1);
        return PlayerPrefs.GetInt("CurrentSaveFile");
    }
}
