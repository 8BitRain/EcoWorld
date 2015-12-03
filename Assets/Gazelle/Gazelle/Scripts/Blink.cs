using UnityEngine;
using System.Collections;

public class Blink : MonoBehaviour {

	public Material eye;
	public float blinkspeed;
	// Use this for initialization
	void Start () {
		StartBlink ();
	}
	
	public int blinking = 0;
	public float blink;
	// Update is called once per frame
	void Update () {
		if(blinking == 1){
			blink -= blinkspeed * Time.deltaTime;
			if(blink>0){
				blinking=0;
				blink =0;
				Invoke ("StartBlink",3);
			}
			eye.SetFloat("_Clip",0.4f);
		}
		else if(blinking == -1){
			blink += blinkspeed * Time.deltaTime;
			if(blink>1){
				blinking=1;
				blink =1;
			}
			eye.SetFloat("_Clip",1f);
		}
	}
	
	void StartBlink(){
		blinking = -1;
	}
}
