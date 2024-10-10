
using UnityEngine;
using Fusion;

[OrderBefore(typeof(NetworkTransform))]
[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
public class Bullet : NetworkTransform
{
    private BulletPool bulletPool;
    public float speed = 10f;
    public Rigidbody rb;

    public override void Spawned()
    {
        if (bulletPool == null)
        {
            bulletPool = GameObject.Find("bulletPool").GetComponent<BulletPool>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        ReturnToPool();
    }

    public void ReturnToPool()
    {
        if (bulletPool != null)
        {
            bulletPool.ReturnBulletToPool(Object);
        }
        else if (Object.HasStateAuthority)
        {
            Runner.Despawn(Object);
        }
    }

}
