using UnityEngine;
using System.Collections;

public class CameraBehaviour : MonoBehaviour {
	
	public GameObject player;
	public GUIStyle backStyle;
	public GUIStyle frontStyle;
	public Vector3 cameraOffset;
	public Camera minimapCamera;
	
	private bool rotateLeft;
	private bool rotateRight;
	private bool obstructed;
	private int layerMask;

	// Use this for initialization
	void Start () {
		rotateLeft = false;
		rotateRight = false;
		obstructed = false;
		// ignore vehicles layer
		layerMask = 3 << (13);
		layerMask = ~layerMask;
		
		UpdatePosition();
	}
	
	public void UpdatePosition() {
		Vector3 playerPos = player.transform.position;
		transform.position = new Vector3(playerPos.x + cameraOffset.x, playerPos.y + cameraOffset.y, playerPos.z + cameraOffset.z);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 playerPos = player.transform.position;
		
		Quaternion rot = Quaternion.LookRotation(playerPos - transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.fixedDeltaTime);
		if(Vector3.Distance(playerPos,transform.position) > cameraOffset.y*1.11f) {
			Vector3 dir = new Vector3(playerPos.x - transform.position.x, 0, playerPos.z - transform.position.z);
			dir.Normalize();
			transform.Translate(dir*5.0f*Time.fixedDeltaTime, Space.World);
		}
		else if(Vector3.Distance(playerPos,transform.position) < cameraOffset.y*1.0f) {
			Vector3 dir = new Vector3(transform.position.x - playerPos.x, 0, transform.position.z - playerPos.z);
			dir.Normalize();
			transform.Translate(dir*5.0f*Time.fixedDeltaTime, Space.World);
		}
		
		Ray ray1 = new Ray(transform.position*2-player.transform.position-Vector3.up*0.5f, player.transform.position+Vector3.up*0.5f-transform.position);
		Debug.DrawLine(transform.position*2-player.transform.position-Vector3.up*0.5f, player.transform.position*30+Vector3.up*15-transform.position*29);
		RaycastHit hit1;
	 
	    if (Physics.Raycast(ray1, out hit1, Mathf.Infinity, layerMask)) {
	        if (hit1.collider.gameObject.tag.Equals("Player")) {
	            // enemy can see the player!
				//Debug.Log("SEE1");
				obstructed = false;
	        } else {
	            // there is something obstructing the view
				//Debug.Log("NAE SEE1");
				obstructed = true;
			}
	    }
		bool left = false;
		bool right = false;
		if(obstructed) {
			Vector3 cameraPos = transform.position;
			int i = 1;
			while(!left && !right && i < 180) {
				Vector3 leftPos = RotateAroundPoint(cameraPos, playerPos, Vector3.up, -1*i);
				Vector3 rightPos = RotateAroundPoint(cameraPos, playerPos, Vector3.up, 1*i);
				Ray leftRay = new Ray(leftPos*2-player.transform.position-Vector3.up*0.5f, player.transform.position+Vector3.up*0.5f-leftPos);
				RaycastHit leftHit;
			    if (Physics.Raycast(leftRay, out leftHit, Mathf.Infinity, layerMask)) {
			        if (leftHit.collider.gameObject.tag.Equals("Player")) {
						left = true;
			        }
			    }
				Ray rightRay = new Ray(rightPos*2-player.transform.position-Vector3.up*0.5f, player.transform.position+Vector3.up*0.5f-rightPos);
				RaycastHit rightHit;
			    if (Physics.Raycast(rightRay, out rightHit, Mathf.Infinity, layerMask)) {
			        if (rightHit.collider.gameObject.tag.Equals("Player")) {
						right = true;
			        }
			    }
				i++;
			}
			if(left)
				transform.RotateAround(player.transform.position, Vector3.up, -i/2);
			else if(right)
				transform.RotateAround(player.transform.position, Vector3.up, i/2);
			//Debug.Log(left+" "+right+" "+i);
		}
		
		//minimapCamera.transform.Rotate(new Vector3(0, 0, minimapCamera.transform.rotation.eulerAngles.z-transform.rotation.eulerAngles.z),Space.Self);
	}
	
	static Vector3 RotateAroundPoint(Vector3 point, Vector3 pivot, Vector3 axis, float angle) {
		Quaternion rotation = Quaternion.AngleAxis(angle, axis);
	    Vector3 finalPos = point - pivot;
	    //Center the point around the origin
	    finalPos = rotation * finalPos;
	    //Rotate the point.
	 	finalPos += pivot;
	    //Move the point back to its original offset. 
	 	return finalPos;
	}
}

