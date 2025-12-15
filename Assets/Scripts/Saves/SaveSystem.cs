using UnityEngine;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

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

    public static SaveData ReceiveSaveData(string path)
    {
        if (File.Exists(path))
        {
            return JsonUtility.FromJson<SaveData>(SaveFileContent(path));
        }
        return null;
    }

    public static SaveData ReceiveSaveData(int index)
    {
        string path = SaveFileName(index);
        if (File.Exists(path))
        {
            return JsonUtility.FromJson<SaveData>(SaveFileContent(path));
        }
        return null;
    }

    public static async Task SetSaveFileName(SaveData saveData, int index, string name)
    {
        saveData.Name = name;
        await File.WriteAllTextAsync(SaveFileName(index), JsonUtility.ToJson(saveData, false), encoding: System.Text.Encoding.UTF8);
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

        await File.WriteAllTextAsync(SaveFileName(), JsonUtility.ToJson(saveData, false), encoding: System.Text.Encoding.UTF8);
        await File.WriteAllTextAsync(DebugSaveFileName(), JsonUtility.ToJson(saveData, true), encoding: System.Text.Encoding.UTF8);
    }

    private static void HandleSaveData()
    {
        string name = saveData != null ? saveData.Name : $"Save �{CurrentSaveFile()}";
        saveData = GameManager.Instance.Save(name);
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

    public static string[] ReceiveSaveFiles()
    {
        return Directory.GetFiles(Application.persistentDataPath, "*.save").OrderBy(u => File.GetCreationTime(u)).ToArray();
    }

    public static int ReceiveSaveFilesCount()
    {
        return Directory.GetFiles(Application.persistentDataPath, "*.save").Length;
    }

    public static async Task AddSaveFile()
    {
        int index = ReceiveSaveFilesCount() + 1;
        string path = SaveFileName(index);

        while (File.Exists(path))
            path = SaveFileName(index + 1);

        FileStream fs = new FileStream(path, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.ReadWrite);
        await fs.DisposeAsync();
    }

    public static void DeleteSaveFile(int index)
    {
        string path = SaveFileName(index);
        File.Delete(path);
    }
}
