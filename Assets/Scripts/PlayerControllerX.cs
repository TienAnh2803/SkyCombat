using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public Rigidbody rb;
    public float turnSpeed = 0f;
    public float turnBackSpeed = 0.5f;
    private float verticalInput;
    private float horizontalInput;
    private GameObject gun;
    private GameObject gun2;
    public ParticleSystem boom;
    public ObjectPool bulletPool;
    private GameObject bulletPos;
    private GameObject bulletPos2;
    private float spin = 1000;
    private float fireRate = 0.1f;
    private float nextFireTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        bulletPos = GameObject.Find("SpawnPos");
        bulletPos2 = GameObject.Find("SpawnPos2");
        gun = GameObject.Find("Gun");
        gun2 = GameObject.Find("Gun2");
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.forward * speed;
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        if (verticalInput != 0 || horizontalInput != 0)
        {
            Rotate(verticalInput, horizontalInput);
        }
        else
        {
            RotateBack();
        }
        boom.Stop();
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            gun.transform.Rotate(Vector3.up * spin);
            gun2.transform.Rotate(Vector3.up * spin);
        }

        if (Input.GetKey(KeyCode.Space) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;

            GameObject bullet = bulletPool.GetBullet();
            bullet.transform.position = bulletPos.transform.position;
            bullet.transform.rotation = transform.rotation;
            bullet.SetActive(true);

            GameObject bullet2 = bulletPool.GetBullet();
            bullet2.transform.position = bulletPos2.transform.position;
            bullet2.transform.rotation = transform.rotation;
            bullet2.SetActive(true);
            
      
            // bulletPos.transform.rotation = transform.rotation;
            // bulletPos2.transform.rotation = transform.rotation;
        }
    }

    void Rotate(float verticalInput, float horizontalInput)
    {
        Vector3 currentEulerAngles = rb.rotation.eulerAngles;

        if (currentEulerAngles.x > 180) currentEulerAngles.x -= 360;
        if (currentEulerAngles.y > 180) currentEulerAngles.y -= 360;
        if (currentEulerAngles.z > 180) currentEulerAngles.z -= 360;

        float newRotationX = currentEulerAngles.x - Time.deltaTime * turnSpeed * verticalInput;
        newRotationX = Mathf.Clamp(newRotationX, -90f, 90f);

        float newRotationY = currentEulerAngles.y - Time.deltaTime * turnSpeed * horizontalInput;
        float newRotationZ = currentEulerAngles.z + Time.deltaTime * turnSpeed * horizontalInput;
        newRotationZ = Mathf.Clamp(newRotationZ, -90f, 90f);

        Quaternion newRotation = Quaternion.Euler(newRotationX, newRotationY, newRotationZ);

        rb.MoveRotation(newRotation);
    }

    void RotateBack()
    {
        Vector3 currentEulerAngles = rb.rotation.eulerAngles;
        float rotationZ = currentEulerAngles.z;
        if (rotationZ > 180) rotationZ -= 360;

        if (rotationZ != 0f)
        {
            rotationZ = rotationZ > 0 ? rotationZ -= turnBackSpeed : rotationZ += turnBackSpeed;
            if (MathF.Abs(rotationZ) <= turnBackSpeed) rotationZ = 0;
            rb.MoveRotation(Quaternion.Euler(currentEulerAngles.x, currentEulerAngles.y, rotationZ));
        }
    }
    private void OnCollisionEnter(Collision collision)
    {   
          Destroy(gameObject);
          boom.Play();
    }
}
