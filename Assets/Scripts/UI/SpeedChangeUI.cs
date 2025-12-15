using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedChangeUI : MonoBehaviour
{
    [SerializeField] private ClockController clockController;
    [SerializeField] public float value;
    [SerializeField] private bool isOn = false;
    private Image image;

    private void Start()
    {
        image = GetComponentInChildren<Image>();
        SpeedChange(isOn);
    }

    public void SpeedChange(bool selected)
    {
        if (selected)
            clockController.ChangeClockSpeed(value);
        image.color = selected ? Color.gray : Color.white;
    }
}
