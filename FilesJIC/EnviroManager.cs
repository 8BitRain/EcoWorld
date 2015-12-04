using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EnviroManager : MonoBehaviour {

	public GameObject enviroHazardMeter;
	RectTransform enviroHazardMeterTransform;
	Slider enviroHazardSlider;
	GameObject[] containerObjs;
	GameObject[] switchObjs;

	//WeatherEffects
	//MagmaRain
	public GameObject magmaRain;
	public Transform magmaRainSpawn;
	private bool magmaRainSpawned;

	//FireMist
	public GameObject fireMist;
	public Transform fireMistSpawn;
	private bool fireMistSpawned;

	private int numObjsInWater;
	private int prevNumObjsInWater;
	// Use this for initialization
	void Start () {
		enviroHazardMeterTransform = enviroHazardMeter.GetComponent<RectTransform> (); 
		enviroHazardSlider = enviroHazardMeter.GetComponent<Slider> ();
		//enviroHazardMeterTransform.transform.
		enviroHazardMeterTransform.sizeDelta = new Vector2 (Screen.width, 40);
		containerObjs = GameObject.FindGameObjectsWithTag ("CONTAINER"); 
		switchObjs = GameObject.FindGameObjectsWithTag("SWITCH");

		numObjsInWater = 0;
		prevNumObjsInWater = 0; 

		//Weather Initialization
		magmaRainSpawned = false;
		fireMistSpawned = false;
	}

	// Update is called once per frame
	void Update () {
		//CountWaterObjs and check to see if water needs to be incremented
		countWaterObjs ();
		if (prevNumObjsInWater < numObjsInWater) {
			print (prevNumObjsInWater);
			increaseEnviromentMeter((numObjsInWater - prevNumObjsInWater)*15);
		}

	}

	int countWaterObjs(){
		//GameObject.FindGameObjectsWithTag ("CONTAINER").Length;
		prevNumObjsInWater = numObjsInWater;
		numObjsInWater = 0;
		for (int i = 0; i < containerObjs.Length; i++) {
			numObjsInWater += containerObjs[i].GetComponent<InteractiveObj>().numObjs;
			checkWeatherEffect();
		}
		return numObjsInWater;
	}

	void increaseEnviromentMeter(int incrementAmnt){
		//Need a value for increment maybe by 5?
		enviroHazardSlider.value += incrementAmnt;
	}

	void checkWeatherEffect(){
		if (enviroHazardSlider.value > 50 && !magmaRainSpawned) {
			Instantiate(magmaRain, magmaRainSpawn.position, magmaRainSpawn.rotation);
			magmaRainSpawned = true;
		}

		if (enviroHazardSlider.value >= 75 && !fireMistSpawned) {
			Instantiate(fireMist, fireMistSpawn.position, fireMistSpawn.rotation);
			fireMistSpawned = true;
		}
	}

	void getContainers(){

	}
}
