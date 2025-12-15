using UnityEngine;
using UnityEngine.UI;

public class SpeedChangeToggle : MonoBehaviour
{
    public float Value;
    private Image image;

    private void Start()
    {
        image = GetComponentInChildren<Image>();
    }

    public void SetActive(bool active)
    {
        image.color = active ? Color.gray : Color.white;
    }
}
