using UnityEngine;
using UnityEngine.InputSystem;

public class MousePosition : MonoBehaviour
{
    [SerializeField] private InputActionReference mouseInputReference;
    private static InputActionReference mouseInput = null;

    private void Start()
    {
        mouseInput = mouseInputReference;
    }

    public static Vector2 GetData()
    {
        return mouseInput != null ? mouseInput.action.ReadValue<Vector2>() : Vector2.zero;
    }
}
