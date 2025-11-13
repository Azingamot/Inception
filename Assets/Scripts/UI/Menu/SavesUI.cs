using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavesUI : MonoBehaviour
{
    [SerializeField] private SaveDescriptionUI descriptionUI;
    [SerializeField] private SaveUI saveUI;
    [SerializeField] private LayoutGroup savesLayout;
    [SerializeField] private SaveLoader saveLoader;
    private List<SaveUI> saveUIs = new();

    private void Awake()
    {
        descriptionUI.gameObject.SetActive(false);
        LoadSaves();
    }

    public void AddSaveFile()
    {
        saveLoader.AddSaveFile();
        LoadSaves();
    }

    public void RemoveSaveFile(int index)
    {
        saveLoader.RemoveSaveFile(index);
        descriptionUI.gameObject.SetActive(false);
        LoadSaves();
    }

    private void LoadSaves()
    {
        Clear();
        int savesCount = saveLoader.ReceiveSavesCount();

        for (int i = 1; i <= savesCount; i++)
        {
            if (saveLoader.SaveFileHasData(i))
            {
                LoadSave(i, saveLoader.ReceiveSaveData(i));
            }
            else
            {
                saveLoader.RemoveSaveFile(i);
            }
        }
    }

    private void Clear()
    {
        saveUIs.Clear();
        for (int i = 0; i < savesLayout.transform.childCount; i++)
        {
            Destroy(savesLayout.transform.GetChild(i).gameObject);
        }
    }

    private void LoadSave(int index, SaveData saveData)
    {
        SaveUI newSaveUI = Instantiate<SaveUI>(saveUI, savesLayout.transform, false);

        if (saveData != null)
            newSaveUI.Initialize(index, saveData.Name, this);
        else
            newSaveUI.Initialize(index, "Empty Save", this);
        saveUIs.Add(newSaveUI);
    }

    public void OpenSave(int index)
    {
        if (!descriptionUI.gameObject.activeInHierarchy)  
            descriptionUI.gameObject.SetActive(true);

        descriptionUI.SetDescriptionForSave(index, saveLoader, this);
    }

    public void ChangeSelection(SaveUI saveUI)
    {
        saveUI.Select();
        foreach (var save in saveUIs)
        {
            if (save != saveUI)
                save.Deselect();
        }
    }
}
