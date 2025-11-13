using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject savesMenu, settingsMenu;

    private void Start()
    {
        CloseSavesMenu();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SwitchSaveMenu()
    {
        if (savesMenu.activeInHierarchy)
            CloseSavesMenu();
        else 
            OpenSavesMenu();
    }

    public void SwitchSettingsMenu()
    {
        if (settingsMenu.activeInHierarchy)
            CloseSettingsMenu();
        else
            OpenSettingsMenu();
        
    }

    public void OpenSavesMenu()
    {
        settingsMenu.SetActive(false);
        savesMenu.SetActive(true);
    }

    public void OpenSettingsMenu()
    {
        savesMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void CloseSavesMenu()
    {
        savesMenu.SetActive(false);
    }

    public void CloseSettingsMenu()
    {
        settingsMenu.SetActive(false);
    }
}
