using UnityEngine;
using UnityEngine.InputSystem;

public class PauseUIControl : MonoBehaviour
{
    [SerializeField] private InputActionReference pauseAction;
    [SerializeField] private PauseMenu pauseMenu;


    private void OnEnable()
    {
        pauseAction.action.started += SwitchPause;
    }

    private void OnDisable()
    {
        pauseAction.action.started -= SwitchPause;
    }


    public void SwitchPause(InputAction.CallbackContext context)
    {
        pauseMenu.SwitchPause();
    }
}
