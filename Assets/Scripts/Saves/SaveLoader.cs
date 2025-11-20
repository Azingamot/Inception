using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoader : MonoBehaviour
{
    [SerializeField] private string mainSceneName = "Main";
    [SerializeField] private Animator loadingAnimator;

    public void AddSaveFile()
    {
        EnsureSaveCounterExisting();

        int newIndex = PlayerPrefs.GetInt("SaveFileCount") + 1;

        PlayerPrefs.SetInt("SaveFileCount", newIndex);
    }

    public void RemoveSaveFile(int index)
    {
        SaveSystem.DeleteSaveFile(PlayerPrefs.GetString($"Save{index}"));
        EnsureSaveCounterExisting();

        int saveFileCount = PlayerPrefs.GetInt("SaveFileCount");

        PlayerPrefs.SetInt("SaveFileCount", saveFileCount - 1);
        PlayerPrefs.DeleteKey($"Save{index}");
    }

    public async Task LoadSaveFile(int index)
    {
        loadingAnimator.SetTrigger("Loading");
        PlayerPrefs.SetInt("CurrentSaveFile", index);

        await Task.Delay(1500);

        await SaveSystem.LoadAsync(mainSceneName);
    }

    private bool SaveFileExisting(int index)
    {
        return PlayerPrefs.HasKey($"Save{index}");
    }

    public bool SaveFileHasData(int index)
    {
        return (SaveFileExisting(index) && File.Exists(SaveSystem.SaveFileName(index))) || !SaveFileExisting(index);
    }

    public int ReceiveSavesCount()
    {
        EnsureSaveCounterExisting();

        return PlayerPrefs.GetInt("SaveFileCount");
    }

    private void EnsureSaveCounterExisting()
    {
        if (!PlayerPrefs.HasKey("SaveFileCount"))
            PlayerPrefs.SetInt("SaveFileCount", 0);
    }

    public SaveData ReceiveSaveData(int index)
    {
        if (SaveFileExisting(index))
        {
            return SaveSystem.ReceiveSaveData(PathToSave(index));
        }
        return null;
    }

    private string PathToSave(int index)
    {
        return PlayerPrefs.GetString($"Save{index}");
    }
}
