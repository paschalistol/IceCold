using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SurvivalPool : MonoBehaviour
{

    [System.Serializable]
    public class Pool
    {
        public PoolObject poolObject;
        public GameObject prefab;
        public int initialSize;
    }
    private Dictionary<PoolObject, GameObject> poolObjectTranslation = new Dictionary<PoolObject, GameObject>();
    private Dictionary<PoolObject, Queue<GameObject>> poolDictionary = new Dictionary<PoolObject, Queue<GameObject>>();
    GameObject tempPoolObject;
    [SerializeField] private List<Pool> pools;
    [Header("Hole Settings")]
    [SerializeField] private float minScale = 0.4f;
    [SerializeField] private float maxScale = 1;
    [FormerlySerializedAs("clockSize")]
    [Header("Clock Settings")]
    [SerializeField] private float clockScale = 0.65f;
    private void Start()
    {
        PopulateTranslation();
        // for (int i = 0; i < initialPoolSize; i++)
        // {
        //     GrowPool(PoolObject.BIG_HOLE);
        // }

        foreach (Pool pool in pools)
        {
            for (int i = 0; i < pool.initialSize; i++)
            {
                GrowPool(pool.poolObject);
            }
        }
    }

    private void PopulateTranslation()
    {
        foreach (Pool pool in pools)
        {
            poolObjectTranslation.Add(pool.poolObject, pool.prefab);
            Queue<GameObject> objectPool = new Queue<GameObject>();
            poolDictionary.Add(pool.poolObject, objectPool);
        }
    }
    private void GrowPool(PoolObject poolObject)
    {
        tempPoolObject = Instantiate(poolObjectTranslation[poolObject]);
        tempPoolObject.transform.SetParent(transform);
        DifferentObjectHandler(poolObject);
        AddToPool(poolObject, tempPoolObject);
    }

    private void DifferentObjectHandler(PoolObject poolObject)
    {
        if (poolObject == PoolObject.BIG_HOLE)
        {
            tempPoolObject.GetComponent<SurvivalHole>().SurvivalHoleInit(this);
            tempPoolObject.transform.GetChild(0).transform.localScale = Vector3.one * Random.Range(minScale, maxScale) / tempPoolObject.transform.localScale.x;
        }
        else if (poolObject == PoolObject.CLOCK)
        {
            tempPoolObject.transform.localScale= Vector3.one * clockScale;
        }
    }

    public void AddToPool(PoolObject poolObject, GameObject instanceToAdd)
    {
        instanceToAdd.SetActive(false);
        poolDictionary[poolObject].Enqueue(instanceToAdd);
    }
    public GameObject GetFromPool(PoolObject poolObject)
    {


        if (!poolDictionary.ContainsKey(poolObject) || poolDictionary[poolObject].Count == 0)
        {
            GrowPool(poolObject);
        }
        tempPoolObject = poolDictionary[poolObject].Dequeue();
        tempPoolObject.SetActive(true);
        // tempPoolObject.transform.GetComponentInChildren<Transform>().localScale = Vector3.one * Random.Range(minScale, maxScale);
        return tempPoolObject;
    }

    public GameObject GetFromPool(PoolObject poolObject, Vector3 position)
    {
        tempPoolObject = GetFromPool(poolObject);
        tempPoolObject.transform.position = position;
        return tempPoolObject;
    }

}
public enum PoolObject
{
    BIG_HOLE, CLOCK
}