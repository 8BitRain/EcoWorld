using UnityEngine;
using System.Collections;

public class Demo : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp(KeyCode.Alpha1)){
			//animation.Play("Run",AnimationPlayMode.Stop,AnimationBlendMode.Blend);
			GetComponent<Animation>().CrossFade("Run");
		}
		else if(Input.GetKeyUp(KeyCode.Alpha2)){
			GetComponent<Animation>().CrossFade("Walk");
		}
		else if(Input.GetKeyUp(KeyCode.Alpha3)){
			GetComponent<Animation>().CrossFade("Attack");
		}
		else if(Input.GetKeyUp(KeyCode.Alpha4)){
			GetComponent<Animation>().CrossFade("AttackHorns");
		}
		else if(Input.GetKeyUp(KeyCode.Alpha7)){
			GetComponent<Animation>().CrossFade("Die");
		}
		else if(Input.GetKeyUp(KeyCode.Alpha6)){
			GetComponent<Animation>().CrossFade("Eat");
		}
	}
}
