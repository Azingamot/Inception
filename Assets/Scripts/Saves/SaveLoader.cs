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

    public async Task LoadSaveFile(int index)
    {
        loadingAnimator.SetTrigger("Loading");
        PlayerPrefs.SetInt("CurrentSaveFile", index);

        await Task.Delay(1500);

        await SaveSystem.LoadAsync(mainSceneName);
    }
}
