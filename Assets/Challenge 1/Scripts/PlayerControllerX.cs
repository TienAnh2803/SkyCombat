using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float turnSpeed = 0f;
    private float verticalInput;
    private float horizontalInput;
    private GameObject gun;
    private GameObject gun2;
    public GameObject bulletPre;
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

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        Rotate();
        if (Input.GetKey(KeyCode.Space))
        {
            gun.transform.Rotate(Vector3.up * spin);
            gun2.transform.Rotate(Vector3.up * spin);
        }
        if (Input.GetKey(KeyCode.Space) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Instantiate(bulletPre, bulletPos.transform.position, bulletPos.transform.rotation);
            bulletPos.transform.rotation = transform.rotation;
            Instantiate(bulletPre, bulletPos2.transform.position, bulletPos2.transform.rotation);
            bulletPos2.transform.rotation = transform.rotation;
        }
        if(gameObject.CompareTag("Mountain"))
        {
            Debug.Log("asdasd");
        }
    }

    void Rotate()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

        Vector3 currentEulerAngles = transform.eulerAngles;

        if (currentEulerAngles.x > 180) currentEulerAngles.x -= 360;
        if (currentEulerAngles.y > 180) currentEulerAngles.y -= 360;
        if (currentEulerAngles.z > 180) currentEulerAngles.z -= 360;

        float newRotationX = currentEulerAngles.x - Time.deltaTime * turnSpeed * verticalInput;
        newRotationX = Mathf.Clamp(newRotationX, -60f, 60f);


        float newRotationY = currentEulerAngles.y - Time.deltaTime * turnSpeed * horizontalInput;
        float newRotationZ = currentEulerAngles.z + Time.deltaTime * turnSpeed * horizontalInput;
        newRotationZ = Mathf.Clamp(newRotationZ, -30f, 30f);

        transform.eulerAngles = new Vector3(newRotationX, newRotationY, newRotationZ);

    }

}
