using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class botController : MonoBehaviour
{
    private Transform target;
    public float speed = 5f;
    public float rotationSpeed = 5f;
    private Rigidbody rb;
    private int maxHp = 10;
    private int hp = 10;

    void Start()
    {
        StartCoroutine(AutoFindTarget());
        StartCoroutine(AutoHp());
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (hp >= maxHp / 2) {
            ChaseTarget();
        } else {
            FleeFromTarget();
        } 
    }

    IEnumerator AutoFindTarget()
    {
        while (true)
        {
            FindTarget();
            yield return new WaitForSeconds(3f);
        }
    }

    IEnumerator AutoHp() {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            hp = Random.Range(0, 10);
        }
    }


    void FindTarget()
    {
        target = FindObjectOfType<GameManager>().GetRandomBot().transform;
    }

    void FleeFromTarget() {
        Vector3 direction = (transform.position - target.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        rb.MoveRotation(Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed));
        rb.velocity = transform.forward * speed;
    }

    void ChaseTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        rb.MoveRotation(Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed));
        rb.velocity = transform.forward * speed;
    }
}
