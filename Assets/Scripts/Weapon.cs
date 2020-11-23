using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    [HideInInspector]
    public short bulletAmount;
    public short maxBullets;
    public Transform bulletPH;
    public float fireRate;
    float fireTimer = 0;
    public string weaponType;
    public float accuracy;

    public AudioClip weaponSound;
    AudioSource audioPlayer;
    public AudioClip activateChainsaw;

    Animator chainsawAnim;
    GameObject chainsawCollider;

    Transform idleRot;
    Transform attackRot;

    public float chainsawDamage;

    bool isRunning;

    public Transform cam;
    public LayerMask lm;

    public GameObject ps;

    void Awake()
    {
        weaponType = gameObject.name;
        fireTimer = fireRate;
        audioPlayer = GetComponent<AudioSource>();
        bulletAmount = maxBullets;
        lm = ~lm;
        accuracy /= 100;

        if (weaponType == "Chainsaw")
        {
            chainsawAnim = GetComponent<Animator>();
            chainsawCollider = transform.GetChild(0).gameObject;
            chainsawCollider.SetActive(false);
            idleRot = transform.parent.GetChild(0);
            attackRot = transform.parent.GetChild(1);
            transform.rotation = idleRot.rotation;
            transform.position = idleRot.position;

            audioPlayer.loop = true;
            audioPlayer.Play();
        }
        else
            audioPlayer.clip = weaponSound;
    }

    void Update()
    {
        if (weaponType == "Chainsaw")
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(ActivateChainsawSound());
                chainsawCollider.SetActive(true);
                chainsawAnim.SetBool("isActive", true);
                StartCoroutine(RotateChainsaw(attackRot));
            }
            if (Input.GetMouseButtonUp(0))
            {
                audioPlayer.Stop();
                chainsawCollider.SetActive(false);
                chainsawAnim.SetBool("isActive", false);
                StartCoroutine(RotateChainsaw(idleRot));
            }
        }
        else
        {
            fireTimer += Time.deltaTime;

            if (Input.GetMouseButton(0) && fireTimer >= fireRate)
                Shoot();
        }
    }

    void Shoot()
    {
        fireTimer = 0;
        bulletAmount--;
        audioPlayer.Play();

        RaycastHit hit;
        Physics.Raycast(transform.parent.position, transform.parent.forward, out hit, 90, lm);

        if (weaponType == "Shotgun")
        {
            for (short i = 0; i < 10; i++)
            {
                GameObject b = Instantiate(bulletPrefab);
                b.transform.position = transform.parent.position;
                b.transform.forward = transform.parent.forward;

                Quaternion bulletRotation = Quaternion.Euler(Random.Range(b.transform.eulerAngles.x - (12 * accuracy), b.transform.eulerAngles.x + (12 * accuracy)), Random.Range(b.transform.eulerAngles.y - (10 * accuracy), b.transform.eulerAngles.y + (10 * accuracy)), bulletPH.transform.rotation.z);
                b.transform.rotation = bulletRotation;
            }
        }
        else
        {
            GameObject b = Instantiate(bulletPrefab);
            b.transform.position = transform.parent.position;
            b.transform.forward = transform.parent.forward;

            Quaternion bulletRotation = Quaternion.Euler(Random.Range(b.transform.eulerAngles.x - (12 * accuracy), b.transform.eulerAngles.x + (12 * accuracy)), Random.Range(b.transform.eulerAngles.y - (10 * accuracy), b.transform.eulerAngles.y + (10 * accuracy)), bulletPH.transform.rotation.z);
            b.transform.rotation = bulletRotation;
        }
    }

    IEnumerator ActivateChainsawSound()
    {
        audioPlayer.volume = 0.3f;
        audioPlayer.clip = activateChainsaw;
        audioPlayer.loop = false;
        audioPlayer.Play();
        if (isRunning)
            yield break;
        isRunning = true;
        yield return new WaitForSeconds(activateChainsaw.length - Time.deltaTime);
        if (audioPlayer.clip == activateChainsaw && audioPlayer.isPlaying)
        {
            audioPlayer.clip = weaponSound;
            audioPlayer.loop = true;
            audioPlayer.Play();
        }
        isRunning = false;
    }

    IEnumerator StopChainsawSound()
    {
        while (audioPlayer.volume > 0)
        {
            audioPlayer.volume -= Time.deltaTime * 2;
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator RotateChainsaw(Transform target)
    {
        var t = 0f;
        while (transform.rotation != target.rotation || transform.position != target.position)
        {
            t += Time.fixedDeltaTime * 5;
            transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, t);
            transform.position = Vector3.Lerp(transform.position, target.position, t);
            yield return new WaitForFixedUpdate();
        }
    }
}
