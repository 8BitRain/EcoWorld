using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {
	
	public GameObject gazelle;
	public float mSpeed;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += transform.right * mSpeed * Time.deltaTime;
		transform.LookAt(gazelle.transform.position);	
	}
}
