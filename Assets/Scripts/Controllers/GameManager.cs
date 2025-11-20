using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System.Linq;
using UnityEngine.Events;
using System;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Controllers")]
    [SerializeField] private Player player;
    [SerializeField] private ClockController clockController;
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private CropsGrowController growController;
    [SerializeField] private ObjectsPlacement objectsPlacement;

    [Header("Tilemaps")]
    [SerializeField] private TilemapLoader tilemapLoader;
    [SerializeField] private List<Tilemap> tileMaps = new();

    [Header("First Initialization")]
    [SerializeField] private UnityEvent onFirstLoad;

    private bool isLoaded = false;

    private void OnEnable()
    {
        onFirstLoad.AddListener(EventsStorage.InitialLoad);
    }

    private void OnDisable()
    {
        onFirstLoad.RemoveListener(EventsStorage.InitialLoad);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Initialize();
        }       
    }

    private void Initialize()
    {
        SaveSystem.Load();
    }

    public SaveData Save(string name = "save")
    {
        SaveData data = new SaveData()
        {
            Name = name,
            InventorySlotsInfo = inventoryController.Save(),
            ClockContext = clockController.Save(),
            PlayerData = new PlayerData() { Health = 100, MaxHealth = 100, MaxSaturation = 20, Saturation = 20, PlayerPosition = PlayerPosition.GetPosition()},
            ObjectPositions = ObjectsPositions.Objects.ToList(),
            WorldSaveData = TilemapSaver.SaveAllTilemaps(tileMaps),
            EventHappens = EventsStorage.Save()
        };
        Debug.Log(data.WorldSaveData.saveTime);
        return data;
    }

    public void Load(SaveData saveData)
    {
        if (isLoaded) return;

        DefaultLoad();

        if (saveData == null)
            FirstLoad();
        else
            Reload(saveData);    
    }

    public Task WaitForSceneToBeFullyLoaded()
    {
        TaskCompletionSource<bool> taskCompletion = new TaskCompletionSource<bool>();

        UnityAction<Scene, LoadSceneMode> sceneLoaderHandler = null;

        sceneLoaderHandler = (scene, mode) =>
        {
            taskCompletion.SetResult(true);
            SceneManager.sceneLoaded -= sceneLoaderHandler;
        };

        SceneManager.sceneLoaded += sceneLoaderHandler;

        return taskCompletion.Task;
    }

    private void DefaultLoad()
    {
        growController.Initialize();
        objectsPlacement.Initialize();
        NamesHelper.ReloadStorage();
    }

    private void FirstLoad()
    {
        inventoryController.Initialize();
        clockController.Initialize(new ClockContext());
        onFirstLoad.Invoke();

        isLoaded = true;
    }

    private void Reload(SaveData saveData)
    {
        ObjectsPositions.Objects = saveData.ObjectPositions.ToHashSet();

        LoadObjects(saveData.ObjectPositions);

        inventoryController.Initialize(saveData);

        tilemapLoader.LoadWorld(saveData.WorldSaveData);

        clockController.Initialize(saveData.ClockContext);

        player.transform.position = saveData.PlayerData.PlayerPosition;

        EventsStorage.Load(saveData);

        isLoaded = true;
    }

    private void LoadObjects(List<ObjectPosition> objects)
    {
        try
        {
            foreach (ObjectPosition obj in objects)
            {
                if (obj.ObjectReference != null && !ObjectsPositions.Objects.Contains(obj))
                {
                    objectsPlacement.PlaceObject((BuildingItem)obj.ObjectReference.ReceiveItemData(), obj.Position);
                }                
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }   
    }
}
