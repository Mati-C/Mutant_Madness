using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float maxLife = 100;
    float life;
    float armor = 0;
    public Image lifeDisplay;
    public Image armorDisplay;

    AudioSource audioPlayer;
    Rigidbody rb;

    public GameObject allahuAkbarVideo;
    string[] allahuAkbarCode = new string[] { "a", "l", "l", "a", "h", "u", "a", "k", "b", "a", "r" };
    short allahuAkbarIndex = 0;

    int currentWeaponIndex = 4;
    public GameObject[] allWeapons;

    public Text remainingBullets;
    public GameObject[] crosshairs;

    void Start()
    {
        audioPlayer = transform.GetChild(1).GetComponent<AudioSource>();
        var allAudios = FindObjectsOfType<AudioSource>();
        foreach (var i in allAudios)
            if (i != audioPlayer && i != GetComponent<AudioSource>())
                i.volume = 0.7f;
        life = maxLife;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        lifeDisplay.fillAmount = life / maxLife;
        if (currentWeaponIndex != 4 && currentWeaponIndex != 0)
        {
            if (!remainingBullets.gameObject.activeSelf)
                remainingBullets.gameObject.SetActive(true);
            remainingBullets.text = allWeapons[currentWeaponIndex].GetComponent<Weapon>().bulletAmount.ToString();
        }
        else
        {
            if (remainingBullets.gameObject.activeSelf)
                remainingBullets.gameObject.SetActive(false);
        }

        if (currentWeaponIndex != 4)
            if (allWeapons[currentWeaponIndex].GetComponent<Weapon>().bulletAmount <= 0)
                ChangeWeapon(currentWeaponIndex - 1);

        if (Input.anyKeyDown)
            if (Input.GetKeyDown(allahuAkbarCode[allahuAkbarIndex]))
                allahuAkbarIndex++;
            else
                allahuAkbarIndex = 0;
        if (allahuAkbarIndex == allahuAkbarCode.Length)
            allahuAkbarVideo.SetActive(true);
    }

    public void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == LayerMask.NameToLayer("Fin"))
            SceneManager.LoadScene(1);

        if (c.gameObject.layer == LayerMask.NameToLayer("Pozo"))
            SceneManager.LoadScene(2);

        if (c.gameObject.GetComponent<Pickable>())
        {
            PickUp(c.gameObject);
            Destroy(c.gameObject);
        }
    }

    void PickUp(GameObject go)
    {
        if (go.GetComponent<CapsuleCollider>())
        {
            if (currentWeaponIndex != 4)
            {
                allWeapons[currentWeaponIndex].SetActive(false);
                if(currentWeaponIndex != 0)
                    crosshairs[currentWeaponIndex].SetActive(false);
            }

            for (short i = 0; i < allWeapons.Length; i++)
                if (allWeapons[i] && allWeapons[i].GetComponent<Weapon>().weaponType == go.name)
                {
                    allWeapons[i].SetActive(true);
                    currentWeaponIndex = i;
                    if (i != 0)
                        crosshairs[i].SetActive(true);
                }
        }
        else
        {
            if (go.name == "Ammo")
            {
                foreach (var i in allWeapons)
                    i.GetComponent<Weapon>().bulletAmount = i.GetComponent<Weapon>().maxBullets;
                ChangeWeapon(3);
            }

            if (go.name == "Health")
            {
                life = life > 100 ? life + 50 : maxLife;
                lifeDisplay.fillAmount = life / maxLife;
            }

            if (go.name == "Armor")
            {
                armor = armor > 100 ? armor + 50 : 100;
                armorDisplay.fillAmount = armor / 100;
            }
        }
    }

    void ChangeWeapon(int i)
    {
        if (i == currentWeaponIndex) return;
        allWeapons[currentWeaponIndex].gameObject.SetActive(false);
        if (i != 0)
            crosshairs[currentWeaponIndex].SetActive(false);
        allWeapons[i].gameObject.SetActive(true);
        if (i != 0)
            crosshairs[i].SetActive(true);
        currentWeaponIndex = i;
    }

    public void GetDamage(float dmg)
    {
        if (dmg <= 0) return;

        var lastArmor = armor;
        armor -= dmg;
        if (lastArmor > dmg / 2)
            life -= dmg / 3;
        else
            life -= dmg;

        lifeDisplay.fillAmount = life / maxLife;
        armorDisplay.fillAmount = armor / 100;
        if (life <= 0)
            SceneManager.LoadScene(2);
        StartCoroutine(ShakeCamera(Mathf.Clamp(dmg, 5, 50), 0.75f));
        audioPlayer.Play();
    }

    public void Explosion(Vector3 pos, float rad, float pwr, float dmg)
    {
        rb.AddExplosionForce(pwr, pos, rad);
        float effect = 1 - (Vector3.Distance(transform.position, pos) / rad);
        GetDamage(dmg * effect);
    }

    IEnumerator ShakeCamera(float magnitude, float time)
    {
        magnitude *= (0.3f / 50);
        while (time > 0)
        {
            Camera.main.transform.localPosition = (Random.insideUnitSphere * magnitude) + (Vector3.up * 0.8f);
            time -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
}
