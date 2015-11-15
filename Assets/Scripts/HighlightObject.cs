using UnityEngine;
using System.Collections;

public class HighlightObject : MonoBehaviour {

	public bool objectHit;
	Renderer rend;
	Color initRendColor;
	// Use this for initialization
	void Start () {
		objectHit = false;
		rend = this.GetComponent<Renderer>();
		initRendColor = rend.sharedMaterial.color;
		//Use a Game Manager to hold on to initialMaterial

	}
	
	// Update is called once per frame
	void Update () {
		if (objectHit) {
			rend.sharedMaterial.color = Color.yellow;
		} else {
			rend.sharedMaterial.color = initRendColor;
		}

	}
}
