using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseObject;
    [SerializeField] private Animator loadingAnimator;
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

    public async void OpenMenu()
    {
        loadingAnimator.SetTrigger("Loading");

        Resume();

        await Task.Delay(1500);

        await SceneManager.LoadSceneAsync("Menu");
    }

    public void Resume()
    {
        pauseObject.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = false;
    }

    public void Pause()
    {
        pauseObject.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }
}
