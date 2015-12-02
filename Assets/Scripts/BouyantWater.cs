using UnityEngine;
using System.Collections;




public class BouyantWater : MonoBehaviour {
	public float waterLevel = 4.0f;
	public float floatHeight = 4.0f;
	public float bounceDamp = .05f;
	public Vector3 buoyancyCentreOffset;

	private float forceFactor;
	private Vector3 actionPoint;
	private Vector3 uplift;

	public bool isActive;

	public Rigidbody rbody;
	// Use this for initialization
	void Start () {
		rbody = GetComponent<Rigidbody> ();
		isActive = false;
		//bouyancyCentreOffset = new Vector3 (0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		if (isActive == true) {
			actionPoint = transform.position + transform.TransformDirection(buoyancyCentreOffset);
			forceFactor = 1f - ((actionPoint.y - waterLevel) / floatHeight);
			
			if (forceFactor > 0f) 
			{
				uplift = -Physics.gravity * (forceFactor - rbody.velocity.y * bounceDamp);
				rbody.AddForceAtPosition(uplift, actionPoint);
			}
			//print ("Object is floating");
		}

	}
}

/*var waterLevel : float = 4;
var floatHeight : float = 2;
var bounceDamp : float = 0.05;
var buoyancyCentreOffset : Vector3;


private var forceFactor : float;
private var actionPoint : Vector3;
private var uplift : Vector3;

function Update() 
{
	actionPoint = transform.position + transform.TransformDirection(buoyancyCentreOffset);
	forceFactor = 1f - ((actionPoint.y - waterLevel) / floatHeight);
	
	if (forceFactor > 0f) 
	{
		uplift = -Physics.gravity * (forceFactor - rigidbody.velocity.y * bounceDamp);
		rigidbody.AddForceAtPosition(uplift, actionPoint);
	}
}*/