using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    [SerializeField] private CinemachineCamera mainCamera;
    [SerializeField] private float angle;
    [SerializeField] private float speed;
    public static CameraEffects Instance { get; private set; }
    private Coroutine shakeCoroutine;
    private bool alternate = false;
   
    void Start()
    {
        Instance = this;
    }

    public void ApplyShake()
    {
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);
        shakeCoroutine = StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        float currentAngle = 0;
        float desiredAngle = angle * (alternate ? -1 : 1);

        if (alternate)
        {
            
            while (currentAngle > desiredAngle)
            {
                currentAngle -= Time.deltaTime * speed;
                mainCamera.Lens.Dutch = currentAngle;
                yield return null;
            }
            while (currentAngle < 0)
            {
                currentAngle += Time.deltaTime * speed;
                mainCamera.Lens.Dutch = currentAngle;
                yield return null;
            }
        }
        else
        {
            while (currentAngle < desiredAngle)
            {
                currentAngle += Time.deltaTime * speed;
                mainCamera.Lens.Dutch = currentAngle;
                yield return null;
            }
            while (currentAngle > 0)
            {
                currentAngle -= Time.deltaTime * speed;
                mainCamera.Lens.Dutch = currentAngle;
                yield return null;
            }
        }
       
        mainCamera.Lens.Dutch = 0;
        alternate = !alternate;
    }
}
