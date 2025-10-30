using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseObject;
    private bool isPaused = false;

    private void Start()
    {
        Resume();
    }

    public void SwitchPause()
    {
        isPaused = !isPaused;
        if (isPaused)
            Pause();
        else 
            Resume();
    }

    public void OpenMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Resume()
    {
        pauseObject.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void Pause()
    {
        pauseObject.SetActive(true);
        Time.timeScale = 0;
    }
}
