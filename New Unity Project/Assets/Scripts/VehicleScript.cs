using UnityEngine;
using System.Collections;

public class VehicleScript : MonoBehaviour {
	
	public float speed = 0.0f;
	public float maxSpeed = 10.0f;
	public float acceleration = 0.1f;
	private bool move = true;
	private bool stay = false;
	private float nextStayCheck;
	private float fixedTime;
	private bool destroyThis = false;
	
	// Use this for initialization
	void Start () {
		nextStayCheck = 0;
		fixedTime = 0;
		maxSpeed += Random.Range(-2.0f, 2.0f);
		
		Transform carColourPart = transform.Find("Cylinder002");
		if(carColourPart) {
			Material[] mats = carColourPart.renderer.materials;
			mats[0].color = new Color(Random.Range(0.0f, 1.0f),Random.Range(0.0f, 1.0f),Random.Range(0.0f, 1.0f));
			carColourPart.renderer.materials = mats;
		}
		Transform vanColourPart1 = transform.Find("Cylinder001van");
		Transform vanColourPart2 = transform.Find("Cylinder002van");
		if(vanColourPart1 && vanColourPart2) {
			Material[] mats = vanColourPart1.renderer.materials;
			Color paint = new Color(Random.Range(0.0f, 1.0f),Random.Range(0.0f, 1.0f),Random.Range(0.0f, 1.0f));
			mats[0].color = paint;
			vanColourPart1.renderer.materials = mats;
			Material[] mats2 = vanColourPart2.renderer.materials;
			mats2[0].color = new Color(Mathf.Max(0, paint.r - 0.15f),Mathf.Max(0, paint.g - 0.15f),Mathf.Max(0, paint.b - 0.15f));
			vanColourPart2.renderer.materials = mats2;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(destroyThis)
			Destroy(gameObject);
		fixedTime = Time.fixedTime;
		if(move) {
			if(speed < maxSpeed)
				speed += acceleration;
			if(speed > maxSpeed)
				speed = maxSpeed;
		}
		else {
			if(speed > 0)	
				speed -= acceleration*10.0f;
			if(speed < 0)
				speed = 0;
			//transform.Translate(0, 0, speed*Time.fixedDeltaTime, Space.Self);
		}
		transform.Translate(0, 0, speed*Time.fixedDeltaTime, Space.Self);
		if(stay) {
			if(Time.fixedTime >= nextStayCheck) {
				//nextStayCheck = Time.fixedTime + 1;
				stay = false;
				move = true;
			}
		}
	}
	
	public void OnVehicleTriggerEnter(Collider other) {
		move = false;
		stay = true;
	}
	
	public void OnVehicleTriggerStay(Collider other) {
		stay = true;
		move = false;
		nextStayCheck = fixedTime + 1;
	}
	
	public void OnVehicleTriggerExit(Collider other) {
		move = true;
	}
	
	public void OnVehicleDestroy() {
		destroyThis = true;
	}
}
