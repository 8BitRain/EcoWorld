using UnityEngine;
using System.Collections;

public class InteractionRange : MonoBehaviour {


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){
		if (other.transform.tag == "Player") {
			InteractiveObj parentScript = this.GetComponentInParent<InteractiveObj>();
			print ("Player can interact w/ object");
		}
	}

	void onTriggerExit(Collider other){
		if (other.transform.tag == "Player") {
			print("Player left");
		}
	}
}
