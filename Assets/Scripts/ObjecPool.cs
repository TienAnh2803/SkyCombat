using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab;
    public int poolSize = 10;

    private List<GameObject> pool;
     public Transform poolFolder;

    void Start()
    {
        pool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject go = Instantiate(prefab, transform);
            go.SetActive(false);
            go.GetComponent<Bullet>().pool = this;
            pool.Add(go);
        }
    }

    public GameObject GetBullet()
    {
        foreach (GameObject go in pool)
        {
            if (!go.activeInHierarchy)
            {
                return go;
            }
        }

        GameObject moreGO = Instantiate(prefab, transform);
        moreGO.SetActive(false);
        pool.Add(moreGO);
        return moreGO;
    }

    public void ReturnBullet(GameObject go)
    {
        go.SetActive(false);
    }
}
