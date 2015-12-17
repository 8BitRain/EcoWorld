using UnityEngine;
using System.Collections;

public class InteractiveObj : MonoBehaviour {
	public bool switchOn;
	
	private bool playerCanHold;
	private bool playerIsHolding;

	public InteractiveObjType objType;
	public enum InteractiveObjType{SWITCH, CONTAINER, EQUIPPABLE};

	private GameObject player;
	private bool playerCanInteract;
	private PlayerController playerController;
	private BouyantWater waterScript;


	public bool objectHit;
	public int numObjs;
	private bool itemInContainer;

	Renderer rend;
	private Rigidbody rigbody;
	Color initRendColor;
	private EnviroManager enviroManager;
	

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");

		playerController = player.GetComponent<PlayerController> ();
		waterScript = GetComponent<BouyantWater> ();
		objectHit = false;

		if (this.GetComponent<Renderer> () != null) {
			rend = this.GetComponent<Renderer>();
			initRendColor = rend.material.color;
		}
		rigbody = this.GetComponent<Rigidbody> ();

		if (objType == InteractiveObjType.EQUIPPABLE) {
			this.playerCanHold = true;
			itemInContainer = false;
		}
		if (objType == InteractiveObjType.SWITCH) {
			this.playerCanHold = false;
			itemInContainer = false;
		}
		enviroManager = GameObject.FindGameObjectWithTag ("ENVMANAGER").GetComponent<EnviroManager> ();
		numObjs = 0;
	}
	
	// Update is called once per frame
	void Update () {
		//Check Distance of Player
		float distance = Vector3.Distance (this.transform.position, player.transform.position);
		//print (distance);
		//Is the Player within a distance where she can interact with the object?
		//if()
		if (objectHit && playerController.playerState == PlayerController.PlayerState.NEUTRAL) {
			if (distance <= 3) {
				playerCanInteract = true;
				//print ("Player Can Interact");
			} else{
				playerCanInteract = false;
			}
		} else {
			playerCanInteract = false;
		}

		//If the Player can interact can she Equip the Item/Press the Item/ //Needs to be decided by
		//each individual object
		if (playerCanInteract) {
			if(objType == InteractiveObjType.EQUIPPABLE){
				if(Input.GetKeyDown(KeyCode.G)){
					transform.localRotation = Quaternion.identity;
					//rigbody.rotation = Quaternion.identity;
					rigbody.detectCollisions = false;
					rigbody.isKinematic = true;

					//TODO uncomment this code if instantiating object doesn't reset it
					this.transform.parent = GameObject.FindGameObjectWithTag("ItemHolder").transform;

					//this.transform.localPosition = GameObject.FindGameObjectWithTag("ItemHolder").transform.position;
					//this.transform.localScale = new Vector3(transform.localScale.x/2, transform.localScale.y/2, transform.localScale.z/2);
					//this.transform.eulerAngles = new Vector3(0,0,0);

					transform.localRotation = Quaternion.identity;

					//print ("Script is running");
					this.playerCanInteract =false;
					//playerController.
					playerController.playerState = PlayerController.PlayerState.HOLDING;
					playerIsHolding = true;
				
				}
			}
		}

		//How should this object act if the player is carrying it?
		if (playerController.playerState == PlayerController.PlayerState.HOLDING && playerIsHolding == true) {
			if(Input.GetKeyDown(KeyCode.F)){
				//print (this.transform.parent.name);
				//TODO Update this code
				this.transform.parent = null;
				//print ("Player droped item");

				//this.playerCanInteract = true;
				//playerController.
				playerController.playerState = PlayerController.PlayerState.NEUTRAL;
				//this.transform.localScale = new Vector3(transform.localScale.x*2, transform.localScale.y*2, transform.localScale.z*2);
				//Rigidbody gameObjectsRigidBody = this.gameObject.AddComponent<Rigidbody>(); // Add the rigidbody.
				rigbody.detectCollisions = true;
				rigbody.isKinematic = false;

				//rigbody.AddForce(Vector3.forward*1000);
				rigbody.AddRelativeForce(new Vector3(0,0,1000));
				playerIsHolding = false;

			}
		}

		//How should this object act if it is a switch?
		if(playerCanInteract && objType == InteractiveObjType.SWITCH){
			if (Input.GetKeyDown (KeyCode.G) && !switchOn) {
				switchOn = true;
				enviroManager.powerplantHealthMeter.value += 15;
				print ("UP");

			}
			else if (Input.GetKeyDown (KeyCode.G) && switchOn) {
				switchOn = false;
				enviroManager.powerplantHealthMeter.value -= 15;
				print("DOWN");
			}
		}
		//Highlight Object being viewed by reticle 
		//Based on if the playerIsHolding something. If she is change this value
		if (objType == InteractiveObjType.EQUIPPABLE || objType == InteractiveObjType.SWITCH) {
			if (objectHit && !playerIsHolding && playerController.playerState == PlayerController.PlayerState.NEUTRAL) {
				rend.sharedMaterial.color = Color.yellow;
			} else {
				rend.material.color = initRendColor;
			}
			if (!objectHit && !playerIsHolding) {
				rend.material.color = initRendColor;
			}
			if (playerIsHolding) {
				rend.material.color = initRendColor;
			}
		}
		
		objectHit = false;
	}

	void OnDrawGizmosSelected() {
		//Gizmos.color = Color.blue;
		//Gizmos.DrawWireCube(transform.position, new Vector3(1.5f, 1.5f, 1.5f));
	}

	void OnTriggerEnter(Collider other){
		//Only do this logic if this object is Equippable
		if(objType == InteractiveObjType.EQUIPPABLE){
			if(itemInContainer == false){
				if (other.gameObject.layer == LayerMask.NameToLayer("Water")) {
					waterScript.isActive = true;
					//print ("Object Collided With Water");
					itemInContainer = true;
					other.GetComponent<InteractiveObj>().numObjs += 1;
				}
			}

		}

	}

	int countObjs(){
		return numObjs;
	}
}
