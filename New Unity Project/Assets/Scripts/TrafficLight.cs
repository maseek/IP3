using UnityEngine;
using System.Collections;

public class TrafficLight : MonoBehaviour {
	
	public GameObject lightGreen;
	public GameObject lightYellow;
	public GameObject lightRed;
	public float syncDelayOutOfThirty = 0.0f;
	public GameObject block;
	
	private float timeScale = 1.0f;
	private Light lightGreenLight;
	private Light lightYellowLight;
	private Light lightRedLight;
	private BoxCollider blockCollider;

	// Use this for initialization
	void Start () {
		lightGreenLight = lightGreen.GetComponent<Light>();	
		lightYellowLight = lightYellow.GetComponent<Light>();	
		lightRedLight = lightRed.GetComponent<Light>();
		if(block)
			blockCollider = block.GetComponent<BoxCollider>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		lightGreenLight.enabled = false;
		lightYellowLight.enabled = false;
		lightRedLight.enabled = false;
		float timeSpan = (Time.time + syncDelayOutOfThirty) % (30.0f / timeScale);
		if(timeSpan <= 12.0f / timeScale) {
			lightGreenLight.enabled = true;
			if(blockCollider)
				blockCollider.enabled = false;
		}
		else if(timeSpan > 12.0f / timeScale && timeSpan < 14.0f / timeScale) {
			lightYellowLight.enabled = true;
			if(blockCollider)
				blockCollider.enabled = true;
		}
		else if(timeSpan <= 28.0f / timeScale) {
			lightRedLight.enabled = true;
			if(blockCollider)
				blockCollider.enabled = true;
		}
		else {
			lightRedLight.enabled = true;
			lightYellowLight.enabled = true;
			if(blockCollider)
				blockCollider.enabled = false;
		}
	}
}
