using System;
using Fusion;
using UnityEngine;

[OrderBefore(typeof(NetworkTransform))]
[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
public class NetworkPlayerController : NetworkTransform
{
  public float speed = 5.0f;
  public float turnSpeed = 0f;
  public float turnBackSpeed = 0.5f;
  // public ParticleSystem boom;
  public BulletPool bulletPool;
  public float spin = 1000f;
  public float fireRate = 0.1f;

  private float nextFireTime = 0f;
  public Transform bulletSpawnPos;
  public Transform bulletSpawnPos2;

  [Networked]
  public Vector3 Velocity { get; set; }

  [Networked]
  public bool IsGrounded { get; set; }

  public Rigidbody Rb { get; private set; }

  protected override void Awake()
  {
    base.Awake();
    Rb = GetComponent<Rigidbody>();
    bulletPool = GameObject.Find("bulletPool").GetComponent<BulletPool>();
  }

  public override void Spawned()
  {
    base.Spawned();
    transform.GetComponentInChildren<Camera>().gameObject.SetActive(Object.HasInputAuthority);
  }

  public void Rotate(float verticalInput, float horizontalInput)
  {
    Vector3 currentEulerAngles = Rb.rotation.eulerAngles;

    if (currentEulerAngles.x > 180) currentEulerAngles.x -= 360;
    if (currentEulerAngles.y > 180) currentEulerAngles.y -= 360;
    if (currentEulerAngles.z > 180) currentEulerAngles.z -= 360;

    float newRotationX = Mathf.Clamp(currentEulerAngles.x - Time.deltaTime * turnSpeed * verticalInput, -90f, 90f);
    float newRotationY = currentEulerAngles.y - Time.deltaTime * turnSpeed * horizontalInput;
    float newRotationZ = currentEulerAngles.z + Time.deltaTime * turnSpeed * horizontalInput;

    Quaternion newRotation = Quaternion.Euler(newRotationX, newRotationY, Mathf.Clamp(newRotationZ, -90f, 90f));
    Rb.MoveRotation(newRotation);
  }

  public void RotateBack()
  {
    float rotationZ = Rb.rotation.eulerAngles.z;
    if (rotationZ > 180f) rotationZ -= 360f;
    if (rotationZ == 0f)
      return;

    if (Mathf.Abs(rotationZ) <= turnBackSpeed)
    {
      rotationZ = 0f; // Gần 0 thì quay về 0
    }
    else
    {
      rotationZ = rotationZ > 0f ? rotationZ - turnBackSpeed : rotationZ + turnBackSpeed;
    }

    Rb.MoveRotation(Quaternion.Euler(Rb.rotation.eulerAngles.x, Rb.rotation.eulerAngles.y, rotationZ));
  }

  public void Fire()
  {
    if (Time.time >= nextFireTime)
    {
      nextFireTime = Time.time + fireRate;

      bulletPool.GetBulletFromPool(Runner, bulletSpawnPos.position, bulletSpawnPos.rotation);
      bulletPool.GetBulletFromPool(Runner, bulletSpawnPos2.position, bulletSpawnPos2.rotation);
    }
  }
  private void OnCollisionEnter(Collision collision)
  {
    // Destroy(gameObject);
    // boom.Play();
  }
}
