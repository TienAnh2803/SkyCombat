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

    if (Object.HasInputAuthority)
      transform.GetComponentInChildren<Camera>().gameObject.SetActive(true);
    else
      transform.GetComponentInChildren<Camera>().gameObject.SetActive(false);
  }

  public void Rotate(float verticalInput, float horizontalInput)
  {
    Vector3 currentEulerAngles = Rb.rotation.eulerAngles;

    if (currentEulerAngles.x > 180) currentEulerAngles.x -= 360;
    if (currentEulerAngles.y > 180) currentEulerAngles.y -= 360;
    if (currentEulerAngles.z > 180) currentEulerAngles.z -= 360;

    float newRotationX = currentEulerAngles.x - Time.deltaTime * turnSpeed * verticalInput;
    newRotationX = Mathf.Clamp(newRotationX, -90f, 90f);

    float newRotationY = currentEulerAngles.y - Time.deltaTime * turnSpeed * horizontalInput;
    float newRotationZ = currentEulerAngles.z + Time.deltaTime * turnSpeed * horizontalInput;
    newRotationZ = Mathf.Clamp(newRotationZ, -90f, 90f);

    Quaternion newRotation = Quaternion.Euler(newRotationX, newRotationY, newRotationZ);

    Rb.MoveRotation(newRotation);
  }

  public void RotateBack()
  {
    Vector3 currentEulerAngles = Rb.rotation.eulerAngles;
    float rotationZ = currentEulerAngles.z;
    if (rotationZ == 0f)
    {
      return;
    }

    if (rotationZ > 180) rotationZ -= 360;

    if (rotationZ != 0f)
    {
      rotationZ = rotationZ > 0 ? rotationZ - turnBackSpeed : rotationZ + turnBackSpeed;
      if (MathF.Abs(rotationZ) <= turnBackSpeed) rotationZ = 0;
      Rb.MoveRotation(Quaternion.Euler(currentEulerAngles.x, currentEulerAngles.y, rotationZ));
    }
  }

  public void Fire()
  {
    if (Time.time >= nextFireTime)
    {
      nextFireTime = Time.time + fireRate;

      bulletPool.GetBulletFromPool(Runner, transform.position, transform.rotation);
      bulletPool.GetBulletFromPool(Runner, transform.position, transform.rotation);
    }
  }

  private void OnCollisionEnter(Collision collision)
  {
    // Destroy(gameObject);
    // boom.Play();
  }
}
