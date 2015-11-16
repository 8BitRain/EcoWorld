using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public enum PlayerState{HOLDING, NEUTRAL};
	public PlayerState playerState;

	// Use this for initialization
	void Start () {
		this.playerState = PlayerState.NEUTRAL;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
