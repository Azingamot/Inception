using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseObject;
    [SerializeField] private Volume volume;
    [SerializeField] private UnityEvent onOpen;
    [SerializeField] private Animator loadingAnimator;
    private bool isPaused = false;
    private DepthOfField depthOfField;
   
    private void Start()
    {
        volume.profile.TryGet<DepthOfField>(out depthOfField);
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
        if (depthOfField != null)
            depthOfField.focusDistance.value = 10;
        pauseObject.SetActive(false);
        Time.timeScale = 1.0f;
        isPaused = false;
    }

    public void Pause()
    {
        if (depthOfField != null)
            depthOfField.focusDistance.value = 2;
        onOpen.Invoke();
        pauseObject.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }
}
