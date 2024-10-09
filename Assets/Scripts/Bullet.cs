using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Bullet : NetworkBehaviour
{
    public float speed = 500f;
    public Rigidbody rb;
    public ObjectPool pool;
    void Awake()
    {
        if (pool == null)
        {
            pool = GameObject.Find("bulletPool").GetComponent<ObjectPool>();
        }

    }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority)
        {
            rb.velocity = transform.forward * speed;
        }
    }
    public override void Spawned()
    {
        Invoke(nameof(DespawnBullet), 2f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        DespawnBullet();
    }

    void DespawnBullet()
    {
        if (Object != null && Object.HasStateAuthority)
        {
            Debug.Log(gameObject);
            Debug.Log(pool);
            pool.ReturnBullet(gameObject);
            Runner.Despawn(Object);
        }
    }
}
