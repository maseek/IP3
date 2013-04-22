using UnityEngine;
using System.Collections;

public class VehicleTrigger : MonoBehaviour {
	public GameObject vehicle;
	private VehicleScript vehicleScript;
	public string collisionTag;
	// Use this for initialization
	void Start () {
		vehicleScript = vehicle.GetComponent<VehicleScript>();
	}
	
	// Update is called once per frame
	void Update () {
		rigidbody.WakeUp();
	}
	
	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "VehicleDespawn")
			vehicleScript.OnVehicleDestroy();
		else if(other.gameObject.tag == collisionTag)
			vehicleScript.OnVehicleTriggerEnter(other);
	}
	
	void OnTriggerStay(Collider other) {
		if(other.gameObject.tag == collisionTag)
			vehicleScript.OnVehicleTriggerStay(other);
	}
	
	void OnTriggerExit(Collider other) {
		vehicleScript.OnVehicleTriggerExit(other);
	}
}
