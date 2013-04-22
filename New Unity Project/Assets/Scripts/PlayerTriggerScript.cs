using UnityEngine;
using System.Collections;

public class PlayerTriggerScript : MonoBehaviour {
	
	private bool isTriggered = false;
	public bool IsTriggered() { return isTriggered; }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag.Equals("Player")) {
			isTriggered = true;	
		}
	}
	
	void OnTriggerExit(Collider other) {
		if(other.gameObject.tag.Equals("Player")) {
			isTriggered = false;	
		}
	}
}
