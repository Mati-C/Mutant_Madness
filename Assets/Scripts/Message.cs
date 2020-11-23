using System.Collections;
using UnityEngine;

public class Message : MonoBehaviour {

    public GameObject message;
    public float messageTime;

    void OnTriggerEnter (Collider c)
    {
        if (c.gameObject.GetComponent<Player>())
            StartCoroutine(ShowMessage(messageTime));
    }

    public IEnumerator ShowMessage(float t)
    {
        message.SetActive(true);
        yield return new WaitForSeconds(t);
        message.SetActive(false);
    }
}
