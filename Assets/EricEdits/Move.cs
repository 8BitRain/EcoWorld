using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {

	public Transform goal;
	public NavMeshAgent agent;
	private float distance;

	void Start () {
		agent = GetComponent<NavMeshAgent>();
		agent.destination = goal.position; 
	}
	
	// Update is called once per frame
	void Update () {
		if (this.tag != "GAZELLE") {
			distance = Vector3.Distance (this.transform.position, goal.position);
			print (distance);
			if (distance <= 3) {
				//print ("Reached Grazing Position");
			} else {
				//print ("Gazelle hasn't reached its necessary positon");
			}
		}

	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "GRAZE") {
			//print ("Reached Destination");
		}
	}
}
