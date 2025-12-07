using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Controllers")]
    [SerializeField] private Player player;
    [SerializeField] private ClockController clockController;
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private CropsGrowController growController;
    [SerializeField] private ObjectsPlacement objectsPlacement;
    [SerializeField] private HungerSystem hungerSystem;

    [Header("Tilemaps")]
    [SerializeField] private TilemapLoader tilemapLoader;
    [SerializeField] private List<Tilemap> tileMaps = new();

    [Header("First Initialization")]
    [SerializeField] private UnityEvent onFirstLoad;

    [Header("Loading Animation")]
    [SerializeField] private Animator loadingAnim;

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
        PlayerHealth health = player.ReceiveHealth();
        SaveData data = new SaveData()
        {
            Name = name,
            InventorySlotsInfo = inventoryController.Save(),
            ClockContext = clockController.Save(),
            PlayerData = new PlayerData() { 
                Health = Mathf.RoundToInt(health.Health), 
                MaxHealth = Mathf.RoundToInt(health.MaxHealth), 
                MaxSaturation = Mathf.RoundToInt(HungerSystem.Instance.MaxSaturation), 
                Saturation = Mathf.RoundToInt(HungerSystem.Instance.HungerAmount), 
                PlayerPosition = PlayerPosition.GetPosition()},
            ObjectPositions = ObjectsPositions.Objects.ToList(),
            WorldSaveData = TilemapSaver.SaveAllTilemaps(tileMaps),
            EventHappens = EventsStorage.Save(),
            ActiveQuests = CurrentQuests.Get(),
            EnemyPositions = SpawnedEnemies.Get()
        };
        Debug.Log(data.WorldSaveData.saveTime);
        return data;
    }

    public void Load(SaveData saveData)
    {
        if (isLoaded) return;

        DefaultLoad();

        if (saveData == null || saveData.WorldSaveData == null)
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
        hungerSystem.Initialize();
        growController.Initialize();
        objectsPlacement.Initialize();
        NamesHelper.ReloadStorage();
        loadingAnim.speed = 0;
    }

    private void FirstLoad()
    {
        inventoryController.Initialize();
        clockController.Initialize(new ClockContext());
        onFirstLoad.Invoke();
        
        PlayerData playerData = new() { Health = 100, MaxHealth = 100, MaxSaturation = 100, Saturation = 100 };
        hungerSystem.Load(playerData);

        FinishLoading();
    }

    private void Reload(SaveData saveData)
    {
        ObjectsPositions.Objects = saveData.ObjectPositions.ToHashSet();
        LoadObjects(saveData.ObjectPositions);
        inventoryController.Initialize(saveData);
        tilemapLoader.LoadWorld(saveData.WorldSaveData);
        clockController.Initialize(saveData.ClockContext);
        player.Load(saveData.PlayerData);
        hungerSystem.Load(saveData.PlayerData);
        EventsStorage.Load(saveData);

        CurrentQuests.Load(saveData);

        LoadEnemies(saveData.EnemyPositions);

        FinishLoading();
    }

    private void FinishLoading()
    {
        isLoaded = true;
        loadingAnim.speed = 1;
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

    private void LoadEnemies(List<EnemyPosition> enemies)
    {
        try
        {
            foreach (EnemyPosition enemy in enemies)
            {
                EnemyData data = ResourcesHelper.FindEnemyResource(enemy.EnemyUID);
                GameObject enemyInstance = Instantiate(data.EnemyPrefab, enemy.Position, Quaternion.identity);
                SpawnedEnemies.AddEnemy(data, enemyInstance, enemy.Position);
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }
}
