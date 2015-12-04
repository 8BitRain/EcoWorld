using UnityEngine;
using System.Collections;


public class SimpleCrosshair : MonoBehaviour {

	public Camera CameraFacing;
	public float[] values;
	private Vector3 originalScale;
	private InteractiveObj interactiveObj;
	public GameObject objectViewing;
	
	// Use this for initialization
	void Start () {
		originalScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		float distance;
		if (Physics.Raycast (new Ray (CameraFacing.transform.position,
		                              CameraFacing.transform.rotation * Vector3.forward),
		                     out hit)) {
			distance = hit.distance;
			if (hit.transform.GetComponent<InteractiveObj> () != null && hit.transform.tag != "Background") {
				interactiveObj = hit.transform.GetComponent<InteractiveObj> ();
				interactiveObj.objectHit = true;
				objectViewing = hit.transform.gameObject;
				//print ("Hitting Something");
			} else{
				if(interactiveObj != null){
					//print ("Hitting Nothing");
					interactiveObj.objectHit = false;
					objectViewing = null;
				}

			}
		} else {
			distance = CameraFacing.farClipPlane * 0.95f;
		}
		transform.position = CameraFacing.transform.position +
			CameraFacing.transform.rotation * Vector3.forward;
		transform.LookAt (CameraFacing.transform.position);
		transform.Rotate (0.0f, 180.0f, 0.0f);
		if (distance < 10.0f) {
			distance *= 1 + 5*Mathf.Exp (-distance);
		}
		//transform.localScale = originalScale * distance;
	}
}


