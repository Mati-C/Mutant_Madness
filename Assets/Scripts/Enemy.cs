using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float life;
    Rigidbody rb;
    public float speed;
    public float attack;
    public float pursuitDistance;
    public float attackDistance;
    public GameObject deadPrefab;
    AudioSource audioPlayer;
    Transform player;
    Animator anim;
    bool isAttacking;
    bool isHit;
    bool isCountingDamage;
    float damageCounted = 0f;
    public int enemyType;
    GameObject ps;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        audioPlayer = GetComponent<AudioSource>();
        player = GameObject.Find("FPSController").transform;
    }

    void FixedUpdate()
    {
        var distance = Vector3.Distance(player.position, transform.position);

        if (!isAttacking)
        {
            if (distance <= attackDistance)
            {
                if(enemyType == 1)
                    StartCoroutine(Attack(0.3f, 3));
                else if(enemyType == 2)
                    StartCoroutine(Attack(0.5f, 4));
                else if(enemyType == 3)
                    StartCoroutine(Attack(2, 5));
            }
            else
            {
                if (distance <= pursuitDistance)
                    Pursuit();
                else
                    anim.Play("Idle");
            }
        }

        if (life <= 0)
        {
            GameObject d = Instantiate(deadPrefab);
            d.transform.position = transform.position;
            d.transform.forward = transform.forward;
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == LayerMask.NameToLayer("Chainsaw"))
            ps = c.transform.parent.GetComponent<Weapon>().ps;
    }

    void OnTriggerStay(Collider c)
    {
        if (c.gameObject.layer == LayerMask.NameToLayer("Chainsaw"))
            GetDamage(c.transform.parent.GetComponent<Weapon>().chainsawDamage);
    }

    void OnTriggerExit(Collider c)
    {
        if (c.gameObject.layer == LayerMask.NameToLayer("Chainsaw") && ps)
            ps.SetActive(false);
    }

    void OnDestroy()
    {
        if (ps)
            ps.SetActive(false);
    }

    public void GetDamage(float dmg)
    {
        if (ps)
            if (Input.GetMouseButton(0))
                ps.SetActive(true);
            else
                ps.SetActive(false);
        if (!isCountingDamage)
            StartCoroutine(CountDamage());
        else
            damageCounted += dmg;
        life -= dmg;
        if (damageCounted > life / 3 && !isHit)
            StartCoroutine(Hit());
        audioPlayer.Play();
    }

    public void Explosion(Vector3 pos, float rad, float pwr, float dmg)
    {
        rb.AddExplosionForce(pwr / 2, pos, rad);
        GetDamage(dmg * (Vector3.Distance(pos, transform.position) / rad));
    }

    void Pursuit()
    {
        anim.Play("Move");
        Vector3 direction = player.position - transform.position;
        direction.y = 0;
        rb.transform.forward = direction;
        rb.transform.position += rb.transform.forward * Time.fixedDeltaTime * speed;
    }

    IEnumerator Hit()
    {
        isHit = true;
        anim.Play("Hit");
        yield return new WaitForSeconds(GetAnimation("Hit").length);
        anim.Play("Idle");
        isHit = false;
    }

    IEnumerator Attack(float delay, float cooldown)
    {
        isAttacking = true;
        anim.Play("Attack");
        yield return new WaitForSeconds(delay);
        var col = Physics.OverlapSphere(transform.position + (transform.forward * 2) + Vector3.up, 1);
        foreach (var i in col)
            if (i.GetComponent<Player>())
                i.GetComponent<Player>().GetDamage(attack);
        yield return new WaitForSeconds(GetAnimation("Attack").length - delay);
        anim.Play("Idle");
        yield return new WaitForSeconds(cooldown);
        isAttacking = false;
    }

    AnimationClip GetAnimation(string name)
    {
        foreach (var i in anim.runtimeAnimatorController.animationClips)
            if (i.name == name)
                return i;
        return null;
    }

    IEnumerator CountDamage()
    {
        isCountingDamage = true;
        yield return new WaitForSeconds(2);
        isCountingDamage = false;
        damageCounted = 0;
    }
}
