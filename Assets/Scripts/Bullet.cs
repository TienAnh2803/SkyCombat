using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    public float bulletSpeed;
    public ObjectPool pool;
    void OnEnable()
    {
        Invoke("Deactivate", 2f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
    }

    void Deactivate()
    {
        pool.ReturnBullet(gameObject);
    }

    void OnDisable()
    {
        CancelInvoke(); // Hủy mọi lời gọi Invoke khi đạn bị tắt
    }
}
