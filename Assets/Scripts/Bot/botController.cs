using System.Collections;
using Fusion;
using UnityEngine;

[OrderBefore(typeof(NetworkTransform))]
[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
public class BotController : NetworkTransform
{
    [HideInInspector]
    public Transform target;
    public float speed = 5f;
    public float rotationSpeed = 5f;
    private Rigidbody rb;
    public int maxHp = 10;
    public int hp = 10;
    [HideInInspector]
    public float targetUpdateInterval = 3f;
    [HideInInspector]
    public float hpUpdateInterval = 3f;
    [HideInInspector]
    public float lastTargetUpdateTime;
    [HideInInspector]
    public float lastHpUpdateTime;
    private GameManager gameManager;

    public override void Spawned()
    {
        base.Spawned();
        rb = GetComponent<Rigidbody>();
        if (Object.HasStateAuthority)
        {
            StartCoroutine(WaitForNetworkReady());
        }
        gameManager = FindObjectOfType<GameManager>();
    }

    IEnumerator WaitForNetworkReady()
    {
        yield return new WaitForSeconds(5.0f);

        lastTargetUpdateTime = Time.time;
        lastHpUpdateTime = Time.time;
    }

    public void FindTarget()
    {
        if (gameManager != null) // Kiểm tra nếu GameManager đã được tìm thấy
        {
            target = gameManager.GetRandomInGameObject().transform;
        }
    }

    public void FleeFromTarget()
    {
        Vector3 direction = (transform.position - target.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        rb.MoveRotation(Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed));
        rb.velocity = transform.forward * speed;
    }

    public void ChaseTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        rb.MoveRotation(Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed));

        rb.AddForce(transform.forward * speed - rb.velocity, ForceMode.VelocityChange);
    }
}
