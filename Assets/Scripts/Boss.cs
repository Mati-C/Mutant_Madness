using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public GameObject deadPrefab;
    public GameObject bulletPrefab;
    public Transform riflePH;
    public float fireRate;
    float currentFireRate = -4.2f;

    public AudioClip bossTheme;
    public AudioClip[] damageClips;
    AudioSource musicPlayer;
    AudioSource audioPlayer;
    Animator anim;

    Rigidbody rb;
    bool battleBegun = false;

    public AutoOpenDoor returnDoor;
    public AutoOpenDoor finalDoor;
    Transform player;

    public float maxLife;
    float life;
    public Slider lifeBar;

    public GameObject rifle1;
    public GameObject rifle2;

    void Start()
    {
        player = GameObject.Find("FPSController").transform;
        musicPlayer = GameObject.Find("Directional Light").GetComponent<AudioSource>();
        audioPlayer = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        life = maxLife;
    }

    void Update()
    {
        print(Vector3.Distance(transform.position, player.position));
        if (battleBegun)
        {
            Vector3 direction = player.position - transform.position;
            direction.y = 0;
            rb.transform.forward = direction;

            currentFireRate += Time.deltaTime;
            if(currentFireRate >= fireRate)
            {
                currentFireRate = 0;
                Shoot();
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, player.position) < 23)
                StartBattle();
        }
    }

    public void Shoot()
    {
        GameObject b = Instantiate(bulletPrefab);
        b.transform.position = riflePH.position;
        b.transform.forward = (player.position + (Vector3.up * 0.5f)) - b.transform.position;
    }

    public void GetDamage(float dmg)
    {
        life -= dmg;
        lifeBar.value = life / maxLife;
        audioPlayer.clip = damageClips[Random.Range(0, damageClips.Length - 1)];
        audioPlayer.Play();

        if(life <= 0)
        {
            GameObject d = Instantiate(deadPrefab);
            d.transform.position = transform.position;
            d.transform.rotation = transform.rotation;
            Destroy(lifeBar.gameObject);
            Destroy(gameObject);
        }
    }

    public void Explosion(Vector3 pos, float rad, float pwr, float dmg)
    {
        GetDamage(dmg * (Vector3.Distance(pos, transform.position) / rad));
    }

    void StartBattle()
    {
        musicPlayer.clip = bossTheme;
        musicPlayer.Play();
        anim.Play("Grab Rifle");
        battleBegun = true;
        lifeBar.gameObject.SetActive(true);
        returnDoor.isLocked = true;
        finalDoor.isLocked = true;
        StartCoroutine(RifleChange());
    }

    AnimationClip GetAnimation(string name)
    {
        foreach (var i in anim.runtimeAnimatorController.animationClips)
            if (i.name == name)
                return i;
        return null;
    }

    void OnDestroy()
    {
        returnDoor.isLocked = false;
        finalDoor.isLocked = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 22);
    }

    IEnumerator RifleChange()
    {
        yield return new WaitForSeconds(1.5f);
        rifle1.SetActive(false);
        rifle2.SetActive(true);
    }
}
