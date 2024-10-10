using Fusion;

public class BulletNetwork : NetworkBehaviour
{
    Bullet bullet;

    private void Awake()
    {
        bullet = GetComponent<Bullet>();
    }
    void OnEnable()
    {
        Invoke(nameof(Deactivate), 2f);
    }

    public override void FixedUpdateNetwork()
    {
        bullet.rb.velocity = transform.forward * bullet.speed;
    }

    void Deactivate()
    {
        bullet.ReturnToPool();
    }


    void OnDisable()
    {
        CancelInvoke();
    }
}
