using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FishingCircleUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private RectTransform arrow;
    [SerializeField] private RectTransform greenZone;

    [Header("Input Reference")]
    [SerializeField] private InputActionReference finishKey;

    private bool isSpinning = false;
    private float currentAngle = 0f;
    private float greenZoneStartAngle = 60f;
    private UnityEvent<bool> onFinished = new();
    private float rotationSpeed = 180f;
    private float greenZoneSize = 36f;

    private void OnEnable()
    {
        finishKey.action.started += CheckResultOnInput;
    }

    private void OnDisable()
    {
        finishKey.action.started -= CheckResultOnInput;
    }

    private void FixedUpdate()
    {
        if (isSpinning)
        {
            Spin();
        }
    }

    public void SetRandomGreenZone(float size)
    {
        greenZoneSize = size;
        greenZoneStartAngle = Random.Range(0f, 360f - greenZoneSize);

        if (greenZone != null)
        {
            greenZone.rotation = Quaternion.Euler(0, 0, -greenZoneStartAngle);

            Image greenImage = greenZone.GetComponent<Image>();
            greenImage.fillAmount = greenZoneSize / 360f;
        }
    }

    private void Spin()
    {
        currentAngle += rotationSpeed * Time.deltaTime;
        if (currentAngle >= 360f)
            currentAngle -= 360f;

        arrow.rotation = Quaternion.Euler(0, 0, -currentAngle);
    }

    public void CheckResultOnInput(InputAction.CallbackContext callbackContext)
    {
        CheckResult();
        isSpinning = false;
    }

    public void StartSpinning(float speed, params UnityAction<bool>[] finishedActions)
    {
        rotationSpeed = speed;
        foreach (var action in finishedActions)
            onFinished.AddListener(action);
        
        StartSpinning();
    }

    public void StartSpinning()
    {
        isSpinning = true;
        currentAngle = 0f;
    }

    private void CheckResult()
    {
        float angle = currentAngle % 360f;

        float greenEnd = greenZoneStartAngle + greenZoneSize;

        bool inGreen = false;
        if (greenEnd <= 360f)
        {
            inGreen = (angle >= greenZoneStartAngle && angle <= greenEnd);
        }
        else
        {
            inGreen = (angle >= greenZoneStartAngle || angle <= (greenEnd - 360f));
        }

        onFinished.Invoke(inGreen);
        onFinished.RemoveAllListeners();
    }
}