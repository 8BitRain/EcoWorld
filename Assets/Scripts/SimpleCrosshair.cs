using UnityEngine;
using System.Collections;

public class SimpleCrosshair : MonoBehaviour {

	public Camera cameraFacing;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.LookAt (cameraFacing.transform.position);
		transform.Rotate (0.0f, 180.0f, 0.0f);
		transform.position = cameraFacing.transform.position + cameraFacing.transform.rotation * Vector3.forward;
	}
}
