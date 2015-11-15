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

	public bool objectHit;
	Renderer rend;
	Color initRendColor;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player");
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
		print (distance);
		//Is the Player within a distance where she can interact with the object?
		if (objectHit) {
			if (distance <= 3) {
				playerCanInteract = true;
				//print ("Player Can Interact");
			} else{
				playerCanInteract = false;
			}
		} else {
			playerCanInteract = false;
		}

		//If the Player can interact can she Equip the Item/Press the Item/
		if (playerCanInteract) {
			if(objType == InteractiveObjType.EQUIPPABLE){
				if(Input.GetKey(KeyCode.Space)){
					this.transform.parent = GameObject.Find("Player").transform;
					print ("This occured");
					this.playerCanInteract =false;
					playerIsHolding = true;
				}
			}
		}
		//Highlight Object being viewed by reticle
		if (objectHit && !playerIsHolding) {
			rend.sharedMaterial.color = Color.yellow;
		} else {
			rend.sharedMaterial.color = initRendColor;
		}

	}

	void OnDrawGizmosSelected() {
		//Gizmos.color = Color.blue;
		//Gizmos.DrawWireCube(transform.position, new Vector3(1.5f, 1.5f, 1.5f));
	}
}
