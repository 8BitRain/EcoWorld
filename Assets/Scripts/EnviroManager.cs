using UnityEngine;
using System.Collections;

public class EnviroManager : MonoBehaviour {

	public GameObject enviroHazardMeter;
	RectTransform enviroHazardMeterTransform;
	// Use this for initialization
	void Start () {
		enviroHazardMeterTransform = enviroHazardMeter.GetComponent<RectTransform> (); 
		//enviroHazardMeterTransform.transform.
		enviroHazardMeterTransform.sizeDelta = new Vector2 (Screen.width, 40);
	}

	// Update is called once per frame
	void Update () {
	 
	}
}
