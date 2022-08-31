using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;
    private List<GameObject> pooledObjects = new List<GameObject>();
    private int amoutToPool = 4;

    [SerializeField] private GameObject tomePrefab;
    [SerializeField] private GameObject spearPrefab;
    [SerializeField] private GameObject parent;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < amoutToPool; i++)
        {
            GameObject obj1 = Instantiate(tomePrefab, parent.transform);
            GameObject obj2 = Instantiate(spearPrefab, parent.transform);

            obj1.SetActive(false);
            pooledObjects.Add(obj1);

            obj2.SetActive(false);
            pooledObjects.Add(obj2);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if(!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }
}
