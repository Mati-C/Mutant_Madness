using UnityEngine;

public class Pickable : MonoBehaviour {

    public Vector3 rotation;
	
	void Update () {
        transform.Rotate(rotation.x * Time.deltaTime, rotation.y * Time.deltaTime, rotation.z * Time.deltaTime);
	}
}
