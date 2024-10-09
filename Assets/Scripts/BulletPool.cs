using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class BulletPool : MonoBehaviour
{
    public GameObject bulletPrefab;
    private Queue<NetworkObject> bulletPool = new Queue<NetworkObject>();
    public int initialPoolSize = 200;

    public void InitializePool(NetworkRunner runner)
    {
        // Tạo trước một số viên đạn và đưa vào pool
        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateBullet(runner);
        }
    }

    public NetworkObject GetBulletFromPool(NetworkRunner runner, Vector3 position, Quaternion rotation)
    {
        if (bulletPool.Count <= 0)
        {
            CreateBullet(runner);
        }
        NetworkObject bullet = bulletPool.Dequeue();
        bullet.transform.position = position;
        bullet.transform.rotation = rotation;
        bullet.gameObject.SetActive(true);
        return bullet;
    }

    private void CreateBullet(NetworkRunner runner)
    {
        NetworkObject bullet = runner.Spawn(bulletPrefab, Vector3.zero, Quaternion.identity, null);
        bullet.gameObject.SetActive(false);
        bullet.transform.SetParent(transform);
        bulletPool.Enqueue(bullet);
    }

    public void ReturnBulletToPool(NetworkObject bullet)
    {
        bullet.gameObject.SetActive(false);
        bulletPool.Enqueue(bullet);
    }
}
