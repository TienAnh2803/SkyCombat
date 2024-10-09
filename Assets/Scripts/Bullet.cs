using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Bullet : NetworkBehaviour
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

    void OnEnable()
    {
        Invoke("Deactivate", 2f);
    }


    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority)
        {
            rb.velocity = transform.forward * speed;
        }
    }

    void Deactivate()
    {
        ReturnToPool();
    }

    private void OnCollisionEnter(Collision collision)
    {
        ReturnToPool();
    }

    private void ReturnToPool()
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

    void OnDisable()
    {
        CancelInvoke();
    }
}
