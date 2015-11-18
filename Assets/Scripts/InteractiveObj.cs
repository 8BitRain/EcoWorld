using UnityEngine;
using System.Collections;

public class InteractiveObj : MonoBehaviour {
	public bool isSwitch;
	public bool switchOn;

	public bool canHoldObjs;
	public bool isHoldingObj;

	public bool playerCanHold;
	public bool playerIsHolding;

	public InteractiveObjType objType;

	public enum InteractiveObjType{SWITCH, CONTAINER, EQUIPPABLE};

	public GameObject player;
	public bool playerCanInteract;
	public PlayerController playerController;
	
	public bool objectHit;
	Renderer rend;
	Color initRendColor;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player");
		playerController = player.GetComponent<PlayerController> ();
		objectHit = false;
		rend = this.GetComponent<Renderer>();
		initRendColor = rend.sharedMaterial.color;

		if (objType == InteractiveObjType.EQUIPPABLE) {
			this.playerCanHold = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		//Check Distance of Player
		float distance = Vector3.Distance (this.transform.position, player.transform.position);
		//print (distance);
		//Is the Player within a distance where she can interact with the object?
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
				if(Input.GetKeyDown(KeyCode.Space)){
					this.transform.parent = GameObject.Find("FrontVision").transform;
					this.transform.localScale += new Vector3(-0.5F, -0.5F, -0.5F);
					print ("Script is running");
					this.playerCanInteract =false;
					//playerController.
					playerController.playerState = PlayerController.PlayerState.HOLDING;
					playerIsHolding = true;
				}
			}
		}
		//Highlight Object being viewed by reticle 
		//Based on if the playerIsHolding something. If she is change this value

			if (objectHit && !playerIsHolding) {
				rend.sharedMaterial.color = Color.yellow;
			}
			if(!objectHit && !playerIsHolding){
				rend.sharedMaterial.color = initRendColor;
			}
			if (playerIsHolding) {
				rend.sharedMaterial.color = initRendColor;
			}
		//How should this object act if the player is carrying it?
		if (playerController.playerState == PlayerController.PlayerState.HOLDING && playerIsHolding == true) {
			if(Input.GetKeyDown(KeyCode.F)){
				print (this.transform.parent.name);
				this.transform.parent = null;
				print ("Player droped item");
				//this.playerCanInteract = true;
				//playerController.
				playerController.playerState = PlayerController.PlayerState.NEUTRAL;
				this.transform.localScale += new Vector3(0.5F, 0.5F, 0.5F);
				playerIsHolding = false;
			}
		}
		
		
	}

	void OnDrawGizmosSelected() {
		//Gizmos.color = Color.blue;
		//Gizmos.DrawWireCube(transform.position, new Vector3(1.5f, 1.5f, 1.5f));
	}
}
