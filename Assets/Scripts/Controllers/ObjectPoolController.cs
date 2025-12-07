using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolController : MonoBehaviour
{
    private GameObject emptyHolder;

    private static GameObject particleSystemsEmpty;
    private static GameObject gameObjectsEmpty;
    private static GameObject soundFxEmpty;

    private static Dictionary<GameObject, ObjectPool<GameObject>> objectPools;
    private static Dictionary<GameObject, GameObject> cloneToPrefabMap;

    public enum PoolType
    {
        GameObject,
        ParticleSystem,
        SoundFX
    }
    public static PoolType PoolingType;

    private void Awake()
    {
        objectPools = new();
        cloneToPrefabMap = new();

        SetupEmpty();
    }

    private void SetupEmpty()
    {
        emptyHolder = new GameObject("Object Pools");

        particleSystemsEmpty = new GameObject("Particle Systems");
        particleSystemsEmpty.transform.SetParent(emptyHolder.transform, false);

        gameObjectsEmpty = new GameObject("Game Objects");
        gameObjectsEmpty.transform.SetParent(emptyHolder.transform, false);

        soundFxEmpty = new GameObject("Sound FX");
        soundFxEmpty.transform.SetParent(emptyHolder.transform, false);
    }

    private static void CreatePool(GameObject prefab, Vector3 position, Quaternion rotation, PoolType poolType = PoolType.GameObject)
    {
        ObjectPool<GameObject> pool = new(
            createFunc: () => CreateObject(prefab, position, rotation, poolType),
            actionOnGet: OnGetObject,
            actionOnRelease: OnReleaseObject,
            actionOnDestroy: OnDestroyObject
            );

        objectPools.Add(prefab, pool);
    }

    private static void CreatePool(GameObject prefab, Transform parent, Quaternion rotation, PoolType poolType = PoolType.GameObject)
    {
        ObjectPool<GameObject> pool = new(
            createFunc: () => CreateObject(prefab, parent, rotation, poolType),
            actionOnGet: OnGetObject,
            actionOnRelease: OnReleaseObject,
            actionOnDestroy: OnDestroyObject
            );

        objectPools.Add(prefab, pool);
    }

    private static GameObject CreateObject(GameObject prefab, Vector3 position, Quaternion rotation, PoolType poolType = PoolType.GameObject)
    {
        prefab.SetActive(false);

        GameObject obj = Instantiate(prefab, position, rotation);

        prefab.SetActive(true);

        GameObject parentObject = SetParentObject(poolType);
        obj.transform.SetParent(parentObject.transform, false);

        return obj;
    }

    private static GameObject CreateObject(GameObject prefab, Transform parent, Quaternion rotation, PoolType poolType = PoolType.GameObject)
    {
        prefab.SetActive(false);

        GameObject obj = Instantiate(prefab, parent);

        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = rotation;
        obj.transform.localScale = Vector3.one;

        prefab.SetActive(true);

        GameObject parentObject = SetParentObject(poolType);
        obj.transform.SetParent(parentObject.transform, false);

        return obj;
    }

    private static void OnGetObject(GameObject obj)
    {
        
    }

    private static void OnReleaseObject(GameObject obj)
    {
        obj.SetActive(false);
    }

    private static void OnDestroyObject(GameObject obj)
    {
        if (cloneToPrefabMap.ContainsKey(obj))
            cloneToPrefabMap.Remove(obj);
    }

    private static T SpawnObject<T>(GameObject objectToSpawn, Vector3 position, Quaternion spawnRotation, PoolType poolType = PoolType.GameObject) where T: Object
    {
        if (!objectPools.ContainsKey(objectToSpawn))
            CreatePool(objectToSpawn, position, spawnRotation, poolType);

        GameObject obj = objectPools[objectToSpawn].Get();

        if (obj != null)
        {
            if (!cloneToPrefabMap.ContainsKey(obj))
                cloneToPrefabMap.Add(obj, objectToSpawn);

            obj.transform.position = position;
            obj.transform.rotation = spawnRotation;

            obj.SetActive(true);

            if (typeof(T) == typeof(GameObject))
            {
                return obj as T;
            }

            T component = obj.GetComponent<T>();    

            if (component != null)
            {
                Debug.LogError($"Object doesn't have component of type {typeof(T)}");
                return null;
            }

            return component;
        }

        return null;
    }

    private static T SpawnObject<T>(GameObject objectToSpawn, Transform parent, Quaternion spawnRotation, PoolType poolType = PoolType.GameObject) where T : Object
    {
        if (!objectPools.ContainsKey(objectToSpawn))
            CreatePool(objectToSpawn, parent, spawnRotation, poolType);

        GameObject obj = objectPools[objectToSpawn].Get();

        if (obj != null)
        {
            if (!cloneToPrefabMap.ContainsKey(obj))
                cloneToPrefabMap.Add(obj, objectToSpawn);

            obj.transform.SetParent(parent, false);
            obj.transform.localRotation = spawnRotation;

            obj.SetActive(true);

            if (typeof(T) == typeof(GameObject))
            {
                return obj as T;
            }

            T component = obj.GetComponent<T>();

            if (component != null)
            {
                Debug.LogError($"Object doesn't have component of type {typeof(T)}");
                return null;
            }

            return component;
        }

        return null;
    }

    public static T SpawnObject<T>(T typePrefab, Vector3 position, Quaternion spawnRotation, PoolType poolType = PoolType.GameObject) where T: Component
    {
        return SpawnObject<T>(typePrefab, position, spawnRotation, poolType);
    }

    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 position, Quaternion spawnRotation, PoolType poolType = PoolType.GameObject)
    {
        return SpawnObject<GameObject>(objectToSpawn, position, spawnRotation, poolType);
    }

    public static T SpawnObject<T>(T typePrefab, Transform parent, Quaternion spawnRotation, PoolType poolType = PoolType.GameObject) where T : Component
    {
        return SpawnObject<T>(typePrefab, parent, spawnRotation, poolType);
    }

    public static GameObject SpawnObject(GameObject objectToSpawn, Transform parent, Quaternion spawnRotation, PoolType poolType = PoolType.GameObject)
    {
        return SpawnObject<GameObject>(objectToSpawn, parent, spawnRotation, poolType);
    }

    public static void ReturnObjectToPool(GameObject obj, PoolType poolType = PoolType.GameObject)
    {
        if (cloneToPrefabMap.TryGetValue(obj, out GameObject prefab))
        {
            GameObject parent = SetParentObject(poolType);

            if (obj.transform.parent != parent.transform)
            {
                obj.transform.SetParent(parent.transform);
            }

            if (objectPools.TryGetValue(prefab, out ObjectPool<GameObject> pool))
            {
                pool.Release(obj);
            }
            else
            {
                Debug.LogWarning($"Object {obj.name} can not be found in pool");
            }
        }
        else
        {
            Debug.LogWarning($"Object {obj.name} can not be found in prefab list");
        }
    }

    public static bool ExistInPool(GameObject obj)
    {
        return cloneToPrefabMap.TryGetValue(obj, out GameObject prefab);
    }

    private static GameObject SetParentObject(PoolType poolType)
    {
        switch (poolType)
        { 
            case PoolType.GameObject:
                return gameObjectsEmpty;
            case PoolType.ParticleSystem:
                return particleSystemsEmpty;
            case PoolType.SoundFX:
                return soundFxEmpty;
            default:
                return null;
        }
    }
}
