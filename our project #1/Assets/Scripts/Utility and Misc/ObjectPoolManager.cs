using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPoolManager : MonoBehaviour
{
    public static List<pooledObjectInfo> objeectPools = new List<pooledObjectInfo>();
    [SerializeField] public static Transform perent;

    public void Awake()
    {
        perent = transform;
    }
    public static GameObject spawnObject(GameObject objectToSpawn, Vector2 spawnPosition, Quaternion spawnRotation)
    {
        pooledObjectInfo pool = objeectPools.Find(p => p.lookUpString == objectToSpawn.name);

        if(pool == null)
        {
            pool = new pooledObjectInfo() { lookUpString = objectToSpawn.name };
            objeectPools.Add(pool);
        }

        GameObject spawnableObj = pool.inactiveObjects.FirstOrDefault();

        if(spawnableObj == null)
        {
            spawnableObj = Instantiate(objectToSpawn, spawnPosition, spawnRotation, perent);
        }
        else
        {
            spawnableObj.transform.position = spawnPosition;
            spawnableObj.transform.rotation = spawnRotation;
            pool.inactiveObjects.Remove(spawnableObj);
            spawnableObj.SetActive(true);
        }

        return spawnableObj;
    }

    public static void ReturnObjectToPool(GameObject obj)
    {
        string goName = obj.name.Substring(0, obj.name.Length - 7);
        pooledObjectInfo pool = objeectPools.Find(p => p.lookUpString == goName);
        if(pool == null)
        {
            Debug.LogWarning("Trying To Release An Object That Is Not Pooled:" + obj.name);
        }
        else
        {
            obj.SetActive(false);
            pool.inactiveObjects.Add(obj);
        }
    }
}

public class pooledObjectInfo
{
    public string lookUpString;
    public List<GameObject> inactiveObjects = new List<GameObject>();
}