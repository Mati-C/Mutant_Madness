using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float damage;
    public GameObject explosion;
    public bool isExplosive;

    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
    }

    void OnTriggerEnter(Collider c)
    {
        if (isExplosive)
        {
            var col = Physics.OverlapSphere(transform.position, 6);
            foreach (var i in col)
            {
                if (i.GetComponent<Enemy>())
                    i.gameObject.GetComponent<Enemy>().Explosion(transform.position, 6, 500, 150);
                if (i.GetComponent<Player>())
                    i.gameObject.GetComponent<Player>().Explosion(transform.position, 5, 500, 30);
                if (i.GetComponent<Boss>())
                    i.gameObject.GetComponent<Boss>().Explosion(transform.position, 6, 500, 150);
            }

            GameObject b = Instantiate(explosion);
            b.transform.position = transform.position;
        }
        else
        {
            if (c.gameObject.GetComponent<Enemy>())
                c.gameObject.GetComponent<Enemy>().GetDamage(damage);
            if (c.gameObject.GetComponent<Boss>())
                c.gameObject.GetComponent<Boss>().GetDamage(damage);
        }
        Destroy(gameObject);
    }
}
