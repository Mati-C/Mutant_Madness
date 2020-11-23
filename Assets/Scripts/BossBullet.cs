using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float speed;
    public float damage;

    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.GetComponent<Player>())
            c.gameObject.GetComponent<Player>().GetDamage(damage);
        Destroy(gameObject);
    }
}
