using UnityEngine;
using System.Collections;

public class colliderTest : MonoBehaviour {

	public GameObject fire;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnParticleCollision(GameObject other){
		print (this.name + " was hit");
		//if (other.name == "MagmaCollision") {
			Instantiate(fire, this.transform.position, this.transform.rotation);
		//}
	}
}
