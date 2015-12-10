using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EnviroManager : MonoBehaviour {

	public GameObject enviroHazardMeter;
	RectTransform enviroHazardMeterTransform;
	//Sliders for EnviromentHealthBars
	[Header("Enviroment Meters")]
	public Slider waterHealthSlider;
	public Slider forestHealthSlider;
	public Slider powerplantHealthMeter;

	[Header("Enviroment Meter Text")]
	//Enviroment Health Text
	public GameObject waterMeterText;
	public GameObject forestMeterText;
	public GameObject powerplantMeterText;

	Slider enviroHazardSlider;

	GameObject[] containerObjs;
	GameObject[] switchObjs;




	//WeatherEffects
	[Header("Enviroment Effects")]
	//MagmaRain
	public GameObject magmaRain;
	public Transform magmaRainSpawn;
	private bool magmaRainSpawned;
	
	//FireMist
	public GameObject fireMist;
	public Transform fireMistSpawn;
	private bool fireMistSpawned;

	//HealthBarText


	private int numObjsInWater;
	private int prevNumObjsInWater;
	//Missions
	[Header("Current Mission")]
	public Mission mission;
	public enum Mission{MISSION_LAKEPOLLUTION, MISSION_POWERPLANT, MISSION_FORESTHARM};
	// Use this for initialization
	void Start () {
		/*enviroHazardMeterTransform = enviroHazardMeter.GetComponent<RectTransform> (); 
		enviroHazardSlider = enviroHazardMeter.GetComponent<Slider> ();
		//enviroHazardMeterTransform.transform.
		enviroHazardMeterTransform.sizeDelta = new Vector2 (Screen.width, 40);*/
		containerObjs = GameObject.FindGameObjectsWithTag ("CONTAINER"); 
		switchObjs = GameObject.FindGameObjectsWithTag("SWITCH");
		mission = Mission.MISSION_LAKEPOLLUTION;
		numObjsInWater = 0;
		prevNumObjsInWater = 0; 

		//Meter Init
		waterHealthSlider.gameObject.SetActive (false);
		forestHealthSlider.gameObject.SetActive (false);
		powerplantHealthMeter.gameObject.SetActive (false);

		//Text Init
		waterMeterText.gameObject.SetActive (false);
		forestMeterText.gameObject.SetActive (false);
		powerplantMeterText.gameObject.SetActive (false);

		//Weather Initialization
		magmaRainSpawned = false;
		fireMistSpawned = false;
	}

	// Update is called once per frame
	void Update () {
		switch (mission) {
		case Mission.MISSION_LAKEPOLLUTION:
			//Turn on health for 
			deactivateMetersAndText();
			waterHealthSlider.gameObject.SetActive(true);
			waterMeterText.gameObject.SetActive(true);
			//CountWaterObjs and check to see if water needs to be incremented
			countWaterObjs ();
			if (prevNumObjsInWater < numObjsInWater) {
				print (prevNumObjsInWater);
				increaseEnviromentMeter((numObjsInWater - prevNumObjsInWater)*15);
			}
			break;
		case Mission.MISSION_POWERPLANT:
			deactivateMetersAndText();
			powerplantHealthMeter.gameObject.SetActive(true);
			powerplantMeterText.gameObject.SetActive(true);
			break;
		case Mission.MISSION_FORESTHARM:
			deactivateMetersAndText();
			forestHealthSlider.gameObject.SetActive(true);
			forestMeterText.gameObject.SetActive(true);
			break;
		}

	}

	//Logic to determine how many trash items are in the water
	int countWaterObjs(){
		//GameObject.FindGameObjectsWithTag ("CONTAINER").Length;
		prevNumObjsInWater = numObjsInWater;
		numObjsInWater = 0;
		for (int i = 0; i < containerObjs.Length; i++) {
			numObjsInWater += containerObjs[i].GetComponent<InteractiveObj>().numObjs;
			checkElementalEffect();
		}
		return numObjsInWater;
	}

	void increaseEnviromentMeter(int incrementAmnt){
		//Need a value for increment maybe by 5?
		//enviroHazardSlider.value += incrementAmnt;
	}

	void deactivateMetersAndText(){
		//Meter Init
		waterHealthSlider.gameObject.SetActive (false);
		forestHealthSlider.gameObject.SetActive (false);
		powerplantHealthMeter.gameObject.SetActive (false);
		
		//Text Init
		waterMeterText.gameObject.SetActive (false);
		forestMeterText.gameObject.SetActive (false);
		powerplantMeterText.gameObject.SetActive (false);
	}

	void checkElementalEffect(){
		//TODO Do a switch case here on the type of enviroment
		switch (mission) {
		case Mission.MISSION_LAKEPOLLUTION:
			//print ("Lake Pollution");
			if (waterHealthSlider.value > 50 && !magmaRainSpawned) {
				Instantiate(magmaRain, magmaRainSpawn.position, magmaRainSpawn.rotation);
				magmaRainSpawned = true;
			}
			if (waterHealthSlider.value >= 75 && !fireMistSpawned) {
				Instantiate(fireMist, fireMistSpawn.position, fireMistSpawn.rotation);
				fireMistSpawned = true;
			}
			break;
		case Mission.MISSION_POWERPLANT:
			print ("Powerplant");
			break;
		case Mission.MISSION_FORESTHARM:
			print ("ForestHarm");
			break;
		}

	}

	void getContainers(){

	}
}
