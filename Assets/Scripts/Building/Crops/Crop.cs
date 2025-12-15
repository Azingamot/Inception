using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Crop : MonoBehaviour, IObserver, IInteractable
{
    [SerializeField] private bool initializeOnStart = false;
    [SerializeField] private CropData data;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private InteractionUI interactionUI;
    [SerializeField] private UnityEvent onStateChanged;
    [SerializeField] private UnityEvent<LootTable> onGather;
    [SerializeField] private float interactionShowTime = 1;
    private float currentTime;
    private int currentStateIndex;
    private bool canGrow = true;
    private bool canInteract = true;
    private int randomValue;

    public Transform Transform => transform;

    private void Awake()
    {
        if (initializeOnStart)
            Initialize(data);
    }

    public void Initialize(CropData cropData, CropState currentState = null)
    {
        data = cropData;
        randomValue = Random.Range(0, 20);
        ParticleSystem particles = GetComponentInChildren<ParticleSystem>();
        if (particles != null)
        {
            ParticleSystem.MainModule main = particles.main;
            main.startColor = cropData.particlesColor;
        }
        CropStateChange(currentState ?? data.cropStates[0]);
    }

    public void OnUpdate(object context)
    {
        if (context is CropGrowContext contextCrop)
        {
            currentTime += contextCrop.timeToAdd;
            CheckStateChange();
        }
    }

    private void CheckStateChange()
    {
        if (canGrow && currentTime > data.cropStates[currentStateIndex + 1].TimeToChange + randomValue)
        {
            currentStateIndex++;

            CropStateChange(data.cropStates[currentStateIndex]);
            
            if (currentStateIndex == (data.cropStates.Count - 1))
            {
                canGrow = false;
                EndOfGrowth();
            }
        }
    }

    private void CropStateChange(CropState state)
    {
        spriteRenderer.sprite = state.StateSprite;
        onStateChanged.Invoke();
    }

    private void EndOfGrowth()
    {
        Unsubscribe();
        if (data.completionObject != null)
        {
            onGather.Invoke(data.lootTable);
            Instantiate(data.completionObject, Transform.position, Quaternion.identity);
        }
    }

    private void Subscribe()
    {
        CropsGrowController.Instance.AddObserver(this);
    }

    private void Unsubscribe()
    {
        CropsGrowController.Instance.RemoveObserver(this);
    }

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDisable()
    {
        Unsubscribe();
    }

    public void OnInteract(GameObject interactor)
    {
        if (!canInteract) return;
        if (canGrow)
        {
            ShowTimeToGrow();
            return;
        }

        onGather.Invoke(data.lootTable);
        canInteract = false;
    }

    private void ShowTimeToGrow()
    {
        interactionUI.TemporaryShow($"{currentTime}/{data.cropStates[data.cropStates.Count - 1].TimeToChange + randomValue}", interactionShowTime);
    }
}
