using UnityEngine;
using System.Collections.Generic;

public class AutoOpenDoor : MonoBehaviour {

    Animator anim;
    public GameObject character;
    public float distanceToOpen = 5;
    int characterNearbyHash = Animator.StringToHash("character_nearby");
    public bool isLocked = false;
    public List<GameObject> enemies = new List<GameObject>();

	void Start () {
        anim = GetComponent<Animator>();
	}
	
	void Update () {
        var distance = Vector3.Distance(transform.position, character.transform.position);

        if(name != "Puerta Final")
        {
            isLocked = false;
            foreach (var i in enemies)
                if (i != null)
                    isLocked = true;
        }

        if (distanceToOpen >= distance && !isLocked)
            anim.SetBool(characterNearbyHash, true);
        else
            anim.SetBool(characterNearbyHash, false);
    }
}
