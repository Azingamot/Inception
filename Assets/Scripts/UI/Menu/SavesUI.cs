using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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

    public async void AddSaveFile()
    {
        await SaveSystem.AddSaveFile();
        LoadSaves();
    }

    public void RemoveSaveFile(int index)
    {
        SaveSystem.DeleteSaveFile(index);
        descriptionUI.gameObject.SetActive(false);
        LoadSaves();
    }

    public void LoadSaves()
    {
        Clear();

        foreach (string path in SaveSystem.ReceiveSaveFiles())
        {
            LoadSave(PathToIndex(path), SaveSystem.ReceiveSaveData(path));
        }
    }

    private int PathToIndex(string path)
    {
        string number = Regex.Match(path, @"\d+").Value;
        return Convert.ToInt32(number);
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
